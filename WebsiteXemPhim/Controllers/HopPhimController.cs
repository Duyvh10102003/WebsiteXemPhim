using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;
using static WebsiteXemPhim.Controllers.LichSuController;

namespace WebsiteXemPhim.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class HopPhimController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public class HopPhimViewModel
        {
            public int HopPhimId { get; set; }
            public int? PhimBoId { get; set; }
            public int? PhimLeId { get; set; }
            public string UserId { get; set; }
            public string TenPhim { get; set; } // Tên của PhimBo hoặc PhimLe
            public string LoaiPhim { get; set; } // "PhimBo" hoặc "PhimLe"
            public string Anh { get; set; } // Đường dẫn ảnh
            public string TrangThai { get; set; } // Trạng thái phim
        }
        public HopPhimController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string sortOrder = "")
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }
            int pageNumber = 1;
            int pageSize = 12;
            IQueryable<HopPhimViewModel> hopPhimList = _context.HopPhim
                .Where(h => h.UserId == user.Id)
                .Select(h => new HopPhimViewModel
                {
                    HopPhimId = h.HopPhimId,
                    PhimBoId = h.PhimBoId,
                    PhimLeId = h.PhimLeId,
                    UserId = h.UserId,
                    TenPhim = h.PhimBoId != null ? h.PhimBo.TenPhim : h.PhimLe.TenPhim,
                    LoaiPhim = h.PhimBoId != null ? "PhimBo" : "PhimLe",
                    Anh = h.PhimBoId != null ? h.PhimBo.Anh : h.PhimLe.Anh,
                   
                    TrangThai = h.PhimBoId != null ? h.PhimBo.TrangThai.TrangThaiPhim : h.PhimLe.TrangThai.TrangThaiPhim
                });
            var TopPhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(3).ToList();
            var TopPhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(2).ToList();
            var TopLikePhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(3).ToList();
            var TopLikePhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(2).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            ViewData["TopPhimBo"] = TopPhimBo;
            ViewData["TopPhimLe"] = TopPhimLe;
            ViewData["TopLikePhimBo"] = TopLikePhimBo;
            ViewData["TopLikePhimLe"] = TopLikePhimLe;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            

            switch (sortOrder)
            {
                case "AZ":
                    hopPhimList = hopPhimList.OrderBy(p => p.TenPhim);
                    break;
                case "ZA":
                    hopPhimList = hopPhimList.OrderByDescending(p => p.TenPhim);
                    break;
                default:
                    hopPhimList = hopPhimList.OrderByDescending(p => p.HopPhimId);
                    break;
            }

            var paginatedPhims = await PaginatedList<HopPhimViewModel>.CreateAsync(hopPhimList, pageNumber, pageSize);
            ViewData["sortOrder"] = sortOrder;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(paginatedPhims);
        }

        public async Task<IActionResult> AddPhimBo(int phimboid)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimBo.Where(p => p.PhimBoId == phimboid).FirstOrDefault();
            Phim.Like += 1;
            _context.SaveChanges();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingHopPhim = await _context.HopPhim
                .FirstOrDefaultAsync(h => h.UserId == user.Id && h.PhimBoId == phimboid);

            if (existingHopPhim == null)
            {
                var hopPhim = new HopPhim
                {
                    UserId = user.Id,
                    PhimBoId = phimboid,
                };
                _context.HopPhim.Add(hopPhim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ChiTietPhimBo", "ChiTietPhim", new { id = phimboid });
        }
        public async Task<IActionResult> RemovePhimBo(int phimboid)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimBo.Where(p => p.PhimBoId == phimboid).FirstOrDefault();
            Phim.Like -= 1;
            _context.SaveChanges();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingHopPhim = await _context.HopPhim
                .FirstOrDefaultAsync(h => h.UserId == user.Id && h.PhimBoId == phimboid);

            if (existingHopPhim != null)
            {
                _context.HopPhim.Remove(existingHopPhim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ChiTietPhimBo", "ChiTietPhim", new { id = phimboid });
        }

        public async Task<IActionResult> RemovePhimBo2(int phimboid)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimBo.Where(p => p.PhimBoId == phimboid).FirstOrDefault();
            Phim.Like -= 1;
            _context.SaveChanges();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingHopPhim = await _context.HopPhim
                .FirstOrDefaultAsync(h => h.UserId == user.Id && h.PhimBoId == phimboid);

            if (existingHopPhim != null)
            {
                _context.HopPhim.Remove(existingHopPhim);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "HopPhim");
        }
        public async Task<IActionResult> AddPhimLe(int phimleid)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimLe.Where(p => p.PhimLeId == phimleid).FirstOrDefault();
            Phim.Like += 1;
            _context.SaveChanges();
            if (user == null)
            {
                // Xử lý trường hợp người dùng chưa xác thực, nếu cần thiết.
                return RedirectToAction("Login", "Account");
            }

            var existingHopPhim = await _context.HopPhim
                .FirstOrDefaultAsync(h => h.UserId == user.Id && h.PhimLeId == phimleid);

            if (existingHopPhim == null)
            {
                var hopPhim = new HopPhim
                {
                    UserId = user.Id,
                    PhimLeId = phimleid,
                };
                _context.HopPhim.Add(hopPhim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ChiTietPhimLe", "ChiTietPhim", new { id = phimleid });
        }

        public async Task<IActionResult> RemovePhimLe(int phimleid)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimLe.Where(p => p.PhimLeId == phimleid).FirstOrDefault();
            Phim.Like -= 1;
            _context.SaveChanges();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingHopPhim = await _context.HopPhim
                .FirstOrDefaultAsync(h => h.UserId == user.Id && h.PhimLeId == phimleid);

            if (existingHopPhim != null)
            {
                _context.HopPhim.Remove(existingHopPhim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ChiTietPhimLe", "ChiTietPhim", new { id = phimleid });
        }
        public async Task<IActionResult> RemovePhimLe2(int phimleid)
        {
            var user = await _userManager.GetUserAsync(User);
            var Phim = _context.PhimLe.Where(p => p.PhimLeId == phimleid).FirstOrDefault();
            Phim.Like -= 1;
            _context.SaveChanges();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingHopPhim = await _context.HopPhim
                .FirstOrDefaultAsync(h => h.UserId == user.Id && h.PhimLeId == phimleid);

            if (existingHopPhim != null)
            {
                _context.HopPhim.Remove(existingHopPhim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "HopPhim");
        }

        public async Task<IActionResult> RemoveAll()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingHopPhims = await _context.HopPhim
                .Where(h => h.UserId == user.Id)
                .ToListAsync();

            if (existingHopPhims.Any())
            {
                _context.HopPhim.RemoveRange(existingHopPhims);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "HopPhim");
        }

    }
}
