using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectA.Data;
using ProjectA.Models;

namespace ProjectA.ViewComponents
{
    public class TheLoaiViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public TheLoaiViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var theloai = await _db.TheLoai.ToListAsync(); // Lấy dữ liệu một cách bất đồng bộ
            return View(theloai);
        }

    }
}
