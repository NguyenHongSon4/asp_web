using BaiTapKiemTra01.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTapKiemTra01.Controllers
{
    public class TaiKhoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DangKy(TaiKhoanViewModel TaiKhoan)
        {
            if (TaiKhoan != null && TaiKhoan.Password != null && (TaiKhoan.Password).Length > 0)
            {
                TaiKhoan.Password = TaiKhoan.Password;
            }

            return View(TaiKhoan);
        }
    }
}
