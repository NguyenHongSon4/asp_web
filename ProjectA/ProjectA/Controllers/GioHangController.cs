using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectA.Data;
using ProjectA.Models;
using System.Security.Claims;

namespace ProjectA.Controllers
{
    [Area("Customers")]
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GioHangController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            GioHangViewModel giohang = new GioHangViewModel()
            {
                DsGioHang = _db.GioHang.Include("SanPham")
                    .Where(gh => gh.ApplicationUserId == claim.Value)
                    .ToList(),
                HoaDon = new HoaDon()
            };
            foreach (var item in giohang.DsGioHang)
            {
                giohang.HoaDon.Total += item.Quantity * item.SanPham.price;
            }

            return View(giohang);
        }

        [Authorize]
        public IActionResult ThanhToan()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            GioHangViewModel giohang = new GioHangViewModel()
            {
                DsGioHang = _db.GioHang.Include("SanPham")
                    .Where(gh => gh.ApplicationUserId == claim.Value)
                    .ToList(),
                HoaDon = new HoaDon()
            };
            giohang.HoaDon.ApplicationUser = _db.ApplicationUser
                .FirstOrDefault(u => u.Id == claim.Value);

            giohang.HoaDon.Name = giohang.HoaDon.ApplicationUser.Name;
            giohang.HoaDon.Address = giohang.HoaDon.ApplicationUser.Address;
            giohang.HoaDon.PhoneNumber = giohang.HoaDon.ApplicationUser.PhoneNumber;

            foreach (var item in giohang.DsGioHang)
            {
                giohang.HoaDon.Total += item.Quantity * item.SanPham.price;
            }

            return View(giohang);
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public IActionResult ThanhToan(GioHangViewModel giohang)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            giohang.DsGioHang = _db.GioHang.Include("SanPham")
                .Where(gh => gh.ApplicationUserId == claim.Value)
                .ToList();

            giohang.HoaDon.ApplicationUserId = claim.Value;
            giohang.HoaDon.OrderDate = DateTime.Now;
            giohang.HoaDon.OrderStatus = "Dang xac nhan...";

            foreach (var item in giohang.DsGioHang)
            {
                giohang.HoaDon.Total += item.Quantity * item.SanPham.price;
            }

            _db.HoaDon.Add(giohang.HoaDon);
            _db.SaveChanges();

            foreach (var item in giohang.DsGioHang)
            {
                ChiTietHoaDon chitiet = new ChiTietHoaDon()
                {
                    SanPhamId = item.SanPhamId,
                    HoaDonId = giohang.HoaDon.Id,
                    ProductPrice = item.SanPham.price * item.Quantity,
                    Quantity = item.Quantity
                };
                _db.ChiTietHoaDon.Add(chitiet);
            }
            _db.SaveChanges();

            _db.GioHang.RemoveRange(giohang.DsGioHang);
            _db.SaveChanges();

            // Lưu thông tin để hiện modal cảm ơn
            TempData["OrderSuccess"] = "true";
            TempData["OrderId"] = "DH-" + giohang.HoaDon.Id.ToString("D6");
            TempData["OrderTotal"] = giohang.HoaDon.Total.ToString("N0").Replace(",", ".");
          

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Xoa(int giohangId)
        {
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);
            _db.GioHang.Remove(giohang);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Tang(int giohangId)
        {
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);
            giohang.Quantity += 1;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Giam(int giohangId)
        {
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);
            giohang.Quantity -= 1;
            if (giohang.Quantity == 0)
                _db.GioHang.Remove(giohang);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}