using Microsoft.AspNetCore.Mvc;
using WebsiteXemPhim.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using WebsiteXemPhim.DataAccess;

namespace WebsiteXemPhim.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lấy số lượng thông báo chưa đọc
        [HttpGet]
        public async Task<IActionResult> GetUnreadNotificationsCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await _context.ThongBaos
                .Where(tb => tb.UserId == userId && !tb.IsRead)
                .CountAsync();

            return Json(new { count });
        }
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _context.ThongBaos
                .Where(tb => tb.UserId == userId)
                .OrderByDescending(tb => tb.CreatedAt)
                .Select(tb => new
                {
                    tb.Message,
                    tb.Url
                })
                .ToListAsync();

            return Json(notifications);
        }


    }
}
