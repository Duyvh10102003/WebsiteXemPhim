using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Controllers
{
    [Route("api/DanhGia")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public DanhGiaController(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }

        [HttpPost("AddRating")]
        [Authorize]
        public async Task<IActionResult> AddRating([FromBody] DanhGiaModel model)
        {
            if (model == null || model.Star < 1 || model.Star > 5)
            {
                return BadRequest(new { success = false, message = "Invalid rating data" });
            }

            // Lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { success = false, message = "User not authenticated" });
            }

            // Kiểm tra xem người dùng đã xem phim này chưa trong bảng LichSu
            var isViewed = await _context.LichSuXem
                .AnyAsync(ls => ls.UserId == user.Id &&
                                ((ls.PhimBoId == model.PhimBoId && model.PhimBoId != null) ||
                                 (ls.PhimLeId == model.PhimLeId && model.PhimLeId != null)));

            if (!isViewed)
            {
                return BadRequest(new { success = false, message = "Bạn phải xem phim trước mới được đánh giá." });
            }

            // Kiểm tra xem đã có đánh giá cho phim này từ người dùng chưa
            var existingRating = _context.DanhGia
                .FirstOrDefault(d => d.UserId == user.Id &&
                                    ((d.PhimBoId == model.PhimBoId && model.PhimBoId != null) ||
                                     (d.PhimLeId == model.PhimLeId && model.PhimLeId != null)));

            if (existingRating != null)
            {
                // Nếu đã tồn tại, cập nhật số sao mới
                existingRating.Star = model.Star;
                _context.DanhGia.Update(existingRating);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Đã cập nhật đánh giá!" });
            }

            // Tạo đối tượng DanhGia mới
            var danhGia = new DanhGia
            {
                UserId = user.Id,
                PhimBoId = model.PhimBoId,
                PhimLeId = model.PhimLeId,
                Star = model.Star,
                User = user
            };

            // Lưu đánh giá vào cơ sở dữ liệu
            _context.DanhGia.Add(danhGia);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Đã thêm đánh giá mới!" });
        }
    }
}
