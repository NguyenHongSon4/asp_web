using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectA.Data;
using ProjectA.Models;
using System;
using System.IO;
using System.Linq;

namespace ProjectA.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class DanhGiaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public DanhGiaController(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _env = env;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult GuiDanhGia(DanhGia danhGia, IFormFile AnhMinhHoa)
        {
            // ✅ Xóa validation cho các field tự động set
            ModelState.Remove("TenNguoiDung");
            ModelState.Remove("UserId");
            ModelState.Remove("AnhUrl");
            ModelState.Remove("SanPham");  // navigation property
            ModelState.Remove("ApplicationUser"); // navigation property nếu có

            // ✅ Kiểm tra SoSao hợp lệ (1-5)
            if (danhGia.SoSao < 1 || danhGia.SoSao > 5)
            {
                TempData["DanhGiaError"] = "Vui lòng chọn số sao đánh giá (1-5 sao).";
                return RedirectToAction("Details", "Home",
                    new { area = "", id = danhGia.SanPhamId });
            }

            // ✅ Kiểm tra nội dung
            if (string.IsNullOrWhiteSpace(danhGia.NoiDung))
            {
                TempData["DanhGiaError"] = "Vui lòng nhập nội dung đánh giá.";
                return RedirectToAction("Details", "Home",
                    new { area = "", id = danhGia.SanPhamId });
            }

            // Set các field tự động
            danhGia.UserId = _userManager.GetUserId(User);
            danhGia.TenNguoiDung = User.Identity?.Name ?? "Ẩn danh";
            danhGia.NgayDang = DateTime.Now;

            // Xử lý upload ảnh nếu có
            if (AnhMinhHoa != null && AnhMinhHoa.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(AnhMinhHoa.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                {
                    TempData["DanhGiaError"] = "Chỉ chấp nhận ảnh JPG, PNG hoặc WebP.";
                    return RedirectToAction("Details", "Home",
                        new { area = "", id = danhGia.SanPhamId });
                }

                if (AnhMinhHoa.Length > 5 * 1024 * 1024)
                {
                    TempData["DanhGiaError"] = "Ảnh không được lớn hơn 5MB.";
                    return RedirectToAction("Details", "Home",
                        new { area = "", id = danhGia.SanPhamId });
                }

                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "reviews");
                Directory.CreateDirectory(uploadFolder);

                var fileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    AnhMinhHoa.CopyTo(stream);
                }

                danhGia.AnhUrl = "/uploads/reviews/" + fileName;
            }

            _db.DanhGias.Add(danhGia);
            _db.SaveChanges();

            TempData["DanhGiaSuccess"] = "Cảm ơn bạn đã đánh giá sản phẩm!";

            return RedirectToAction("Details", "Home",
                new { area = "", id = danhGia.SanPhamId });
        }
    }
}