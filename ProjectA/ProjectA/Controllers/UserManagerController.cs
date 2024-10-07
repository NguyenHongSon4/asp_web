using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectA.Data;
using ProjectA.Models;
using System.Threading.Tasks;

namespace ProjectA.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ cho phép admin truy cập
    public class UserManagerController : Controller
    {
        private readonly ApplicationDbContext _db;
       
        public UserManagerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _db.ApplicationUser.ToListAsync();
            return View(users);
        }

       


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Name = model.Name,
                    Address = model.Address,
                    LockoutEnabled = true // Đặt người dùng là có thể khóa
                };

                user.NormalizedUserName = model.Username.ToUpper();
                user.NormalizedEmail = model.Email.ToUpper();

                var passwordHasher = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

                _db.ApplicationUser.Add(user);
                await _db.SaveChangesAsync();

                // Thiết lập quyền cho người dùng
                if (model.Role == "Admin")
                {
                   
                }
               
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        public async Task<IActionResult> Edit(string id)
        {
            var user = await _db.ApplicationUser.FindAsync(id);
            if (user == null) return NotFound();

            var model = new EditUserViewModel // Sử dụng EditUserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Address = user.Address
          
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.ApplicationUser.FindAsync(model.Id);
                if (user == null) return NotFound();

                user.UserName = model.Username;
                user.Email = model.Email;
                user.Name = model.Name;
                user.Address = model.Address;

                _db.ApplicationUser.Update(user);
                await _db.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cập nhật người dùng thành công!";

                // Redirect để tránh lỗi Confirm Form Resubmission
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }





        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _db.ApplicationUser.FindAsync(id);
            if (user == null) return NotFound();

            _db.ApplicationUser.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
