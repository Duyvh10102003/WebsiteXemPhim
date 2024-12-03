using Microsoft.AspNetCore.Mvc;
using WebsiteXemPhim.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteXemPhim.DataAccess;
using Microsoft.AspNetCore.Identity;

namespace WebsiteXemPhim.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public NotificationsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lấy số lượng thông báo chưa đọc
        [HttpGet]
        public async Task<IActionResult> GetUnreadNotificationsCount()
        {
            var user = await _userManager.GetUserAsync(User);
            var count = await _context.ThongBaos
                .Where(tb => tb.UserId == user.Id)
                .CountAsync();

            return Json(new { count });
        }

        // Lấy danh sách thông báo
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            var notifications = await _context.ThongBaos
                .Where(tb => tb.UserId == user.Id)
                .OrderByDescending(tb => tb.CreatedAt)
                .Select(tb => new
                {
                    tb.Id,
                    tb.Message,
                    tb.Url,
                    Anh = tb.PhimBo != null ? tb.PhimBo.Anh : null // Lấy URL ảnh nếu có
                })
                .ToListAsync();

            return Json(notifications);
        }


        // Xóa một thông báo
        [HttpPost]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var notification = await _context.ThongBaos
                .FirstOrDefaultAsync(tb => tb.Id == id && tb.UserId == user.Id);

            if (notification == null)
            {
                return Json(new { success = false, message = "Thông báo không tồn tại." });
            }

            _context.ThongBaos.Remove(notification);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // Xóa tất cả thông báo
        [HttpPost]
        public async Task<IActionResult> DeleteAllNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            var notifications = _context.ThongBaos
                .Where(tb => tb.UserId == user.Id);

            _context.ThongBaos.RemoveRange(notifications);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
