using BaiKiemTra03_02.Data;
using BaiKiemTra03_02.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BaiKiemTra03_02.Controllers
{
    public class SachController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SachController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var sach = _db.Sach.ToList();
            ViewBag.Sach = sach;
            return View();
        }

		
		[HttpGet]
		public IActionResult Upsert(int id)
		{
			Sach sach = new Sach();

			IEnumerable<SelectListItem> dssach = _db.Sach.Select(
				item => new SelectListItem
				{
					Value = item.BookId.ToString(),
					Text = item.Title,
					
				});

			ViewBag.DSSach = dssach;

			if (id == 0) // Create / Insert
			{
				return View(sach);
			}
			else // Edit / Update
			{
				sach = _db.Sach.Include("TheLoai").FirstOrDefault(sp => sp.BookId == id);
				return View(sach);
			}
		}
		[HttpPost]
		public IActionResult Upsert(Sach sach)
		{
			if (ModelState.IsValid)
			{
				if (sach.BookId == 0)
				{
					_db.Sach.Add(sach);
				}
				else
				{
					_db.Sach.Update(sach);
				}
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();
		}

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var sach = _db.Sach.Find(id);
            return View(sach);
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            var sach = _db.Sach.Find(id);
            if (sach == null)
            {
                return NotFound();
            }
            _db.Sach.Remove(sach);
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
            var sach = _db.Sach.Find(id);
            return View(sach);
        }

		[HttpGet]
		public IActionResult Edit(int id)
		{
			if (id == 0)
			{
				return NotFound();
			}
			var sach = _db.Sach.Find(id);
			if (sach == null)
			{
				return NotFound();
			}
			return View(sach);

		}
		[HttpPost]
		public IActionResult Edit(Sach sach)
		{
			if (ModelState.IsValid)
			{
				_db.Sach.Update(sach);
				_db.SaveChanges();
				return RedirectToAction("Details", new { id = sach.BookId }); // Redirect to Details action
			}
			return View();
		}
	}
}
