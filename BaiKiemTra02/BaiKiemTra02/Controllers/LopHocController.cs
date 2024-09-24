using BaiKiemTra02.Data;
using BaiKiemTra02.Data.Migrations;
using BaiKiemTra02.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiKiemTra02.Controllers
{
    public class LopHocController : Controller
    {
        private readonly ApplicationDbContext _db;

        public LopHocController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var lophoc = _db.LopHoc.ToList();
            ViewBag.Lophoc = lophoc;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(LopHoc lopHoc)
        {
            if (ModelState.IsValid)
            {
                //them thong tin
                _db.LopHoc.Add(lopHoc);
                //luu lai thong tin
                _db.SaveChanges();
                //chuyen ve trang index
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var theloai = _db.LopHoc.Find(id);
            return View();
        }

        [HttpPost]
        public IActionResult Edit(LopHoc lopHoc)
        {
            if (ModelState.IsValid)
            {
                //them thong tin
                _db.LopHoc.Update(lopHoc);
                //luu lai thong tin
                _db.SaveChanges();
                //chuyen ve trang index
                return RedirectToAction("Index");
            }
            return View(lopHoc);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var theloai = _db.LopHoc.Find(id);
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            var lophoc = _db.LopHoc.Find(id);
            if (lophoc == null)
            {
                return NotFound();
            }
            _db.LopHoc.Remove(lophoc);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var lophoc = _db.LopHoc.Find(id);
            return View(lophoc);
        }
    }
}
