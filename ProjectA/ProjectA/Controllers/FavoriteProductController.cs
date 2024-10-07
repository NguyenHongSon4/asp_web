using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectA.Data;
using ProjectA.Models;
using System.Security.Claims;

namespace ProjectA.Controllers
{
    [Area("Customers")]
    public class FavoriteProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FavoriteProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        private string GetCurrentUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToFavorites(int sanPhamId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            // Kiểm tra số lần gọi
            Console.WriteLine($"AddToFavorites called for product ID: {sanPhamId} by user ID: {userId}");

            // Kiểm tra xem sản phẩm đã tồn tại trong danh sách yêu thích chưa
            var existingFavorite = await _db.SanPhamYeuThich
                .FirstOrDefaultAsync(f => f.SanPhamId == sanPhamId && f.ApplicationUserId == userId);

            if (existingFavorite != null)
            {
                // Sản phẩm đã có trong danh sách yêu thích
                return Json(new { success = false, message = "Product already in favorites." });
            }

            // Nếu không có, tiến hành thêm sản phẩm vào danh sách yêu thích
            var favorite = new SanPhamYeuThich
            {
                SanPhamId = sanPhamId,
                ApplicationUserId = userId
            };
            _db.SanPhamYeuThich.Add(favorite);
            await _db.SaveChangesAsync();

            // Trả về thông báo thành công
            return Json(new { success = true, message = "Product added to favorites." });
        }



        [Authorize]
        // Action để hiển thị danh sách sản phẩm yêu thích của người dùng
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var favoriteProducts = await _db.SanPhamYeuThich
                .Include(f => f.SanPham)
                .Where(f => f.ApplicationUserId == userId)
                .ToListAsync();

            return View(favoriteProducts);
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromFavorites(int id)
        {
            var favorite = await _db.SanPhamYeuThich.FindAsync(id);
            if (favorite != null)
            {
                _db.SanPhamYeuThich.Remove(favorite);
                await _db.SaveChangesAsync();
            }

            // Điều hướng về trang chủ
            return RedirectToAction("Index", "Home");
        }

    }
}
