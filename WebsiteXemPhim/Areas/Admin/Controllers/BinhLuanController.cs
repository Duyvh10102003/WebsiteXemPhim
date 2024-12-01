using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;
using WebsiteXemPhim.Repositories;

namespace WebsiteXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BinhLuanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public BinhLuanController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> BinhLuanPhimBo(int id, int pageNumber = 1)
        {
            int pageSize = 20;
            IQueryable<BinhLuan> BinhLuanQuery = _context.BinhLuan.Include(p => p.PhimBo).Include(p=>p.User).Where(p => p.PhimBoId == id);
            var paginatedBinhLuans = await PaginatedList<BinhLuan>.CreateAsync(BinhLuanQuery, pageNumber, pageSize);
            return View(paginatedBinhLuans);
        }
        public async Task<IActionResult> BinhLuanPhimLe(int id, int pageNumber = 1)
        {
            int pageSize = 20;
            IQueryable<BinhLuan> BinhLuanQuery = _context.BinhLuan.Include(p => p.PhimLe).Include(p=>p.User).Where(p => p.PhimLeId == id);
            var paginatedBinhLuans = await PaginatedList<BinhLuan>.CreateAsync(BinhLuanQuery, pageNumber, pageSize);
            return View(paginatedBinhLuans);
        }
        public async Task<IActionResult> RemoveBinhLuanPhimBo(int id)
        {
            
            var binhLuan = await _context.BinhLuan
                .FirstOrDefaultAsync(p=>p.BinhLuanId == id);
            var temp = binhLuan.PhimBoId;
            if (binhLuan != null)
            {
                _context.BinhLuan.Remove(binhLuan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("BinhLuanPhimBo", new { id = temp });
        }
        public async Task<IActionResult> RemoveBinhLuanPhimLe(int id)
        {

            var binhLuan = await _context.BinhLuan.FirstOrDefaultAsync(p => p.BinhLuanId == id);
            var temp = binhLuan.PhimLeId;
            if (binhLuan != null)
            {
                _context.BinhLuan.Remove(binhLuan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("BinhLuanPhimLe", new { id = temp });
        }
    }

}
