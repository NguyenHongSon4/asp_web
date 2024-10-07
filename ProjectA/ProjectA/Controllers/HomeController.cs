using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectA.Data;
using ProjectA.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ProjectA.Controllers
{
    [Area("Customers")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<SanPham> sanpham = _db.SanPham.Include("TheLoai").ToList();
            return View(sanpham);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Details(int? id, int? sanphamId)
        {
            // Chap nhan ca 2 ten parameter: id (asp-route-id) va sanphamId
            int productId = id ?? sanphamId ?? 0;

            var sanpham = _db.SanPham.Include("TheLoai").FirstOrDefault(sp => sp.Id == productId);

            if (sanpham == null)
            {
                return NotFound();
            }

            GioHang gioHang = new GioHang()
            {
                SanPhamId = productId,
                SanPham = sanpham,
                Quantity = 1
            };

            ViewBag.DanhGias = _db.DanhGias
                .Where(d => d.SanPhamId == productId)
                .OrderByDescending(d => d.NgayDang)
                .ToList();

            return View(gioHang);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(GioHang giohang)
        {
            // Lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            giohang.ApplicationUserId = claim.Value;


            // Kiểm tra sản phẩm đã có trong cơ sở dữ liệu hay chưa?
            var giohangdb = _db.GioHang.FirstOrDefault(gh => gh.SanPhamId == giohang.SanPhamId
       && gh.ApplicationUserId == giohang.ApplicationUserId);

            if (giohangdb == null)
            {
                _db.GioHang.Add(giohang); // Them san pham vao gio hang
            }
            else
            {
                giohangdb.Quantity += giohang.Quantity;
            }
            // Them san pham vao gio hang
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult FilterByTheLoai(int id)
        {
            IEnumerable<SanPham> sanpham = _db.SanPham.Include("TheLoai")
                .Where(sp=>sp.TheLoai.Id==id)
                .ToList();

            return View("Index", sanpham);
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View(); 
        }

        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            var products = _db.SanPham
                .Include("TheLoai")
                .Where(sp => sp.Name.Contains(searchTerm))
                .ToList();

            return PartialView("SearchPage", products); // Render một partial view với kết quả tìm kiếm
        }

        [HttpGet]
        public IActionResult SearchPage(string q)
        {
            q = q?.Trim() ?? "";

            var products = string.IsNullOrEmpty(q)
                ? new List<SanPham>()
                : _db.SanPham
                    .Include("TheLoai")
                    .Where(sp => sp.Name.Contains(q))
                    .ToList();

            ViewBag.Keyword = q;
            return View(products);
        }
    }
}
