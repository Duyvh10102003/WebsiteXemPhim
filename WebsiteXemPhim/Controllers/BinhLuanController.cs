using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Controllers
{
    public class BinhLuanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public BinhLuanController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddBinhLuanPB(int phimboid, string text)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimBo.Where(p => p.PhimBoId == phimboid).FirstOrDefault();
            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var existingBinhLuan = await _context.BinhLuan
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.PhimBoId == phimboid);

            var binhluan = new BinhLuan
            {
                UserId = user.Id,
                PhimBoId = phimboid,
                NoiDungBinhLuan = text,
                NgayTao = DateTime.Now
            };
            _context.BinhLuan.Add(binhluan);
            await _context.SaveChangesAsync();

            return RedirectToAction("XemPhimBo", "XemPhim", new { id = phimboid, tap = 1 });
        }
        public async Task<IActionResult> AddBinhLuanPB2(int phimboid, string text)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimBo.Where(p => p.PhimBoId == phimboid).FirstOrDefault();
            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var existingBinhLuan = await _context.BinhLuan
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.PhimBoId == phimboid);

            var binhluan = new BinhLuan
            {
                UserId = user.Id,
                PhimBoId = phimboid,
                NoiDungBinhLuan = text,
                NgayTao = DateTime.Now
            };
            _context.BinhLuan.Add(binhluan);
            await _context.SaveChangesAsync();

            return RedirectToAction("ChiTietPhimBo", "ChiTietPhim", new { id = phimboid});
        }
        public async Task<IActionResult> AddBinhLuanPL(int phimLeid, string text)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimLe.Where(p => p.PhimLeId == phimLeid).FirstOrDefault();
            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var existingBinhLuan = await _context.BinhLuan
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.PhimLeId == phimLeid);

            var binhluan = new BinhLuan
            {
                UserId = user.Id,
                PhimLeId = phimLeid,
                NoiDungBinhLuan = text,
                NgayTao = DateTime.Now
            };
            _context.BinhLuan.Add(binhluan);
            await _context.SaveChangesAsync();

            return RedirectToAction("XemPhimLe", "XemPhim", new { id = phimLeid});
        }
        public async Task<IActionResult> AddBinhLuanPL2(int phimLeid, string text)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimLe.Where(p => p.PhimLeId == phimLeid).FirstOrDefault();
            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            var existingBinhLuan = await _context.BinhLuan
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.PhimLeId == phimLeid);

            var binhluan = new BinhLuan
            {
                UserId = user.Id,
                PhimLeId = phimLeid,
                NoiDungBinhLuan = text,
                NgayTao = DateTime.Now
            };
            _context.BinhLuan.Add(binhluan);
            await _context.SaveChangesAsync();

            return RedirectToAction("ChiTietPhimLe", "ChiTietPhim", new { id = phimLeid });
        }
    }
}
