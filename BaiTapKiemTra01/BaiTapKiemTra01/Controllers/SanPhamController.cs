using BaiTapKiemTra01.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTapKiemTra01.Controllers
{
    public class SanPhamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SanPham(SanPhamViewModel SanPham)
        {
            ViewBag.NamePD = "Laptop Asus FX105";
            ViewBag.Price = "12 599 000";
            return View(SanPham);
        }
    }
}
