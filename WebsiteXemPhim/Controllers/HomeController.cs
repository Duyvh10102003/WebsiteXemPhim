using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Banner1 = _context.PhimBo.Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.TheLoai).OrderByDescending(p=>p.PhimBoId).Take(3).ToList();
            var Banner2 = _context.PhimLe.Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.TheLoai).OrderByDescending(p => p.PhimLeId).Take(2).ToList();
            var TopPhimBo =_context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p=>p.LuotXem).Take(3).ToList();
            var TopPhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(2).ToList();
            var TopLikePhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(3).ToList();
            var TopLikePhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(2).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            var DSPhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.PhimLeId).Take(6).ToList();
            var DSPhim =_context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p=>p.PhimBoId).Take(6).ToList();
            ViewData["Banner"] = Banner1;
            ViewData["Banner2"] = Banner2;
            ViewData["TopPhimBo"] = TopPhimBo;
            ViewData["TopPhimLe"] = TopPhimLe;
            ViewData["TopLikePhimBo"] = TopLikePhimBo;
            ViewData["TopLikePhimLe"] = TopLikePhimLe;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewData["DSPhim"] = DSPhim;
            ViewData["DSPhimLe"] = DSPhimLe;
            return View();
        }
        public async Task<IActionResult> TatCaPhimBo(int pageNumber = 1, string sortOrder = "")
        {
            int pageSize = 12;
            var TopPhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(3).ToList();
            var TopPhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(2).ToList();
            var TopLikePhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(3).ToList();
            var TopLikePhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(2).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            IQueryable<PhimBo> DSPhim = _context.PhimBo.OrderByDescending(p => p.PhimBoId);
            // Sắp xếp dựa trên kiểu sắp xếp
            switch (sortOrder)
            {
                case "AZ":
                    DSPhim = DSPhim.OrderBy(p => p.TenPhim);
                    break;
                case "ZA":
                    DSPhim = DSPhim.OrderByDescending(p => p.TenPhim);
                    break;
                default:
                    DSPhim = DSPhim.OrderByDescending(p => p.PhimBoId);
                    break;
            }
            var paginatedPhimBos = await PaginatedList<PhimBo>.CreateAsync(DSPhim, pageNumber, pageSize);
            ViewData["TopPhimBo"] = TopPhimBo;
            ViewData["TopPhimLe"] = TopPhimLe;
            ViewData["TopLikePhimBo"] = TopLikePhimBo;
            ViewData["TopLikePhimLe"] = TopLikePhimLe;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewData["sortOrder"] = sortOrder;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(paginatedPhimBos);
        }

        public async Task<IActionResult> TatCaPhimLe(int pageNumber = 1, string sortOrder = "")
        {
            int pageSize = 12;
            var TopPhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(3).ToList();
            var TopPhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(2).ToList();
            var TopLikePhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(3).ToList();
            var TopLikePhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(2).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            IQueryable<PhimLe> DSPhim = _context.PhimLe.OrderByDescending(p => p.PhimLeId);
            // Sắp xếp dựa trên kiểu sắp xếp
            switch (sortOrder)
            {
                case "AZ":
                    DSPhim = DSPhim.OrderBy(p => p.TenPhim);
                    break;
                case "ZA":
                    DSPhim = DSPhim.OrderByDescending(p => p.TenPhim);
                    break;
                default:
                    DSPhim = DSPhim.OrderByDescending(p => p.PhimLeId);
                    break;
            }
            var paginatedPhimLes = await PaginatedList<PhimLe>.CreateAsync(DSPhim, pageNumber, pageSize);
            ViewData["TopPhimBo"] = TopPhimBo;
            ViewData["TopPhimLe"] = TopPhimLe;
            ViewData["TopLikePhimBo"] = TopLikePhimBo;
            ViewData["TopLikePhimLe"] = TopLikePhimLe;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewData["sortOrder"] = sortOrder;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(paginatedPhimLes);
        }

        public List<string> SearchSuggestions(string query)
        {
            var phimBoSuggestions = _context.PhimBo
                .Where(p => p.TenPhim.Contains(query))
                .Select(p => p.TenPhim + "|" + p.Anh)
                .ToList();

            var phimLeSuggestions = _context.PhimLe
                .Where(p => p.TenPhim.Contains(query))
                .Select(p => p.TenPhim + "|" + p.Anh)
                .ToList();

            var suggestions = phimBoSuggestions.Union(phimLeSuggestions).ToList();

            return suggestions;
        }
        public async Task<IActionResult> LuotXemPhimBo(int phimboid)
        {
            var Phim = _context.PhimBo.Where(p => p.PhimBoId == phimboid).FirstOrDefault();
            Phim.LuotXem += 1;
            _context.SaveChanges();

            return RedirectToAction("XemPhimBo", "XemPhim", new { id = phimboid, tap = 1 });
        }
        public async Task<IActionResult> LuotXemPhimLe(int phimleid)
        {
            var Phim = _context.PhimLe.Where(p => p.PhimLeId == phimleid).FirstOrDefault();
            Phim.LuotXem += 1;
            _context.SaveChanges();

            return RedirectToAction("XemPhimLe", "XemPhim", new { id = phimleid });
        }
    }
}
