using Microsoft.AspNetCore.Mvc;

namespace BaiTap05.Models
{
    public class TheLoaiViewModel:Controller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IActionResult Index()
        {
            return View();
        }
    }
}