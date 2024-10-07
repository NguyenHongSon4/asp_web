using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectA.Data;
using System.Security.Claims;

namespace ProjectA.ViewComponents
{
    public class CartBadgeViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public CartBadgeViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            int count = 0;

            // Chỉ đếm nếu người dùng đã đăng nhập
            if (UserClaimsPrincipal.Identity?.IsAuthenticated == true)
            {
                var userId = ((ClaimsPrincipal)UserClaimsPrincipal)
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    count = _db.GioHang
                        .Where(g => g.ApplicationUserId == userId)
                        .Sum(g => (int?)g.Quantity) ?? 0;
                }
            }

            return View(count);
        }
    }
}