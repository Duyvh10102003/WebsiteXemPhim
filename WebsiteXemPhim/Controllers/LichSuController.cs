using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class LichSuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;       
        public LichSuController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string sortOrder = "", int pageNumber = 1)
        {
            int pageSize = 12;
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }
            IQueryable<LichSuPhimViewModel> lichSuXemList = _context.LichSuXem
                .Where(h => h.UserId == user.Id)
                .Select(h => new LichSuPhimViewModel
                {
                    LichSuXemId = h.LichSuXemId,
                    PhimBoId = h.PhimBoId,
                    PhimLeId = h.PhimLeId,
                    UserId = h.UserId,
                    TenPhim = h.PhimBoId != null ? h.PhimBo.TenPhim : h.PhimLe.TenPhim,
                    LoaiPhim = h.PhimBoId != null ? "PhimBo" : "PhimLe",
                    Anh = h.PhimBoId != null ? h.PhimBo.Anh : h.PhimLe.Anh,
                    TrangThai = h.PhimBoId != null ? h.PhimBo.TrangThai.TrangThaiPhim : h.PhimLe.TrangThai.TrangThaiPhim
                });
            IQueryable<PhimWithLoai> DSPhims = _context.PhimBo.Include(p => p.TrangThai)
                                                   .Select(p => new PhimWithLoai { Id = p.PhimBoId, Loai = "Bo", TenPhim = p.TenPhim, Anh = p.Anh, AnhNen = p.AnhNen, TrangThai = p.TrangThai.TrangThaiPhim, Like = p.Like, LuotXem = p.LuotXem })
                                                   .Union(_context.PhimLe.Include(p => p.TrangThai)
                                                                         .Select(p => new PhimWithLoai { Id = p.PhimLeId, Loai = "Le", TenPhim = p.TenPhim, Anh = p.Anh, AnhNen = p.AnhNen, TrangThai = p.TrangThai.TrangThaiPhim, Like = p.Like, LuotXem = p.LuotXem }))
                                                   .OrderByDescending(p => p.Id);
            var TopLuotXem = DSPhims.OrderByDescending(p => p.LuotXem).Take(5).ToList();
            var TopLikePhim = DSPhims.OrderByDescending(p => p.Like).Take(5).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            ViewData["TopLuotXem"] = TopLuotXem;
            ViewData["TopLikePhim"] = TopLikePhim;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
        
            switch (sortOrder)
            {
                case "AZ":
                    lichSuXemList = lichSuXemList.OrderBy(p => p.TenPhim);
                    break;
                case "ZA":
                    lichSuXemList = lichSuXemList.OrderByDescending(p => p.TenPhim);
                    break;
                default:
                    lichSuXemList = lichSuXemList.OrderByDescending(p => p.LichSuXemId);
                    break;
            }
            var paginatedPhims = await PaginatedList<LichSuPhimViewModel>.CreateAsync(lichSuXemList, pageNumber, pageSize);

            ViewData["sortOrder"] = sortOrder;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(paginatedPhims);
        }

        public async Task<IActionResult> AddPhimBo(int phimboid)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimBo.Where(p => p.PhimBoId == phimboid).FirstOrDefault();
            Phim.LuotXem += 1;
            _context.SaveChanges();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingLichSu = await _context.LichSuXem
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.PhimBoId == phimboid);

            if (existingLichSu == null)
            {
                var lichSu = new LichSuXem
                {
                    UserId = user.Id,
                    PhimBoId = phimboid,
                };
                _context.LichSuXem.Add(lichSu);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("XemPhimBo", "XemPhim", new { id = phimboid, tap = 1 });
        }
        public async Task<IActionResult> RemovePhimBo(int phimboid)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingLichSu = await _context.LichSuXem
                .FirstOrDefaultAsync(h => h.UserId == user.Id && h.PhimBoId == phimboid);

            if (existingLichSu != null)
            {
                _context.LichSuXem.Remove(existingLichSu);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "LichSu");
        }
        public async Task<IActionResult> AddPhimLe(int phimleid)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimLe.Where(p => p.PhimLeId == phimleid).FirstOrDefault();
            Phim.LuotXem += 1;
            _context.SaveChanges();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingLichSu = await _context.LichSuXem
                .FirstOrDefaultAsync(l => l.UserId == user.Id && l.PhimLeId == phimleid);

            if (existingLichSu == null)
            {
                var lichSu = new LichSuXem
                {
                    UserId = user.Id,
                    PhimLeId = phimleid,
                };
                _context.LichSuXem.Add(lichSu);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("XemPhimLe", "XemPhim", new { id = phimleid });
        }
        public async Task<IActionResult> RemovePhimLe(int phimleid)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingLichSu = await _context.LichSuXem
                .FirstOrDefaultAsync(h => h.UserId == user.Id && h.PhimLeId == phimleid);

            if (existingLichSu != null)
            {
                _context.LichSuXem.Remove(existingLichSu);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "LichSu");
        }

        public async Task<IActionResult> RemoveAll()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingLichSus = await _context.LichSuXem
                .Where(h => h.UserId == user.Id)
                .ToListAsync();

            if (existingLichSus.Any())
            {
                _context.LichSuXem.RemoveRange(existingLichSus);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "LichSu");
        }

    }
}

