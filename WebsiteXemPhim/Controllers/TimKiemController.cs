using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebsiteXemPhim.Controllers
{
    public class TimKiemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimKiemController(ApplicationDbContext context)
        {
            _context = context;
        }
        public class PhimWithLoai
        {
            public int Id { get; set; }
            public string Loai { get; set; } // Bộ hoặc Lẻ
            public string TenPhim { get; set; }
            public string Anh { get; set; }
            public string TrangThai { get; set; }
        }
        public IQueryable<PhimWithLoai> GetPhims(IQueryable<PhimWithLoai> dsPhim, string sortOrder)
        {
            // Xử lý sắp xếp
            switch (sortOrder)
            {
                case "AZ":
                    dsPhim = dsPhim.OrderBy(p => p.TenPhim);
                    break;
                case "ZA":
                    dsPhim = dsPhim.OrderByDescending(p => p.TenPhim);
                    break;
                default:
                    dsPhim = dsPhim.OrderByDescending(p => p.Id);
                    break;
            }

            return dsPhim;
        }
        public async Task<IActionResult> TimKiemTheLoai(int id, int pageNumber = 1, string sortOrder = "")
        {
            ViewData["idTheLoai"] = id;
            int pageSize = 12;
            var TopPhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(3).ToList();
            var TopPhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(2).ToList();
            var TopLikePhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(3).ToList();
            var TopLikePhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(2).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            IQueryable<PhimBo> DSPhimBo = _context.PhimBo.Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.TheLoai).OrderByDescending(p => p.PhimBoId).Where(p => p.ChiTietTheLoaiPhimBos.Any(ct => ct.TheLoaiId == id));

            
            IQueryable<PhimWithLoai> DSPhims = _context.PhimBo
                                                    .Include(p => p.TrangThai)
                                                    .Include(p => p.ChiTietTheLoaiPhimBos)
                                                    .ThenInclude(p => p.TheLoai)
                                                    .Where(p => p.ChiTietTheLoaiPhimBos.Any(ct => ct.TheLoaiId == id))
                                                    .Select(p => new PhimWithLoai { Id = p.PhimBoId, Loai = "Bo", TenPhim = p.TenPhim, Anh = p.Anh , TrangThai = p.TrangThai.TrangThaiPhim})
                                                    .Union(
                                               _context.PhimLe
                                                    .Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.TheLoai)
                                                    .Include(p => p.TrangThai)
                                                    .Where(p => p.ChiTietTheLoaiPhimLes.Any(ct => ct.TheLoaiId == id))
                                                    .Select(p => new PhimWithLoai { Id = p.PhimLeId, Loai = "Le", TenPhim = p.TenPhim, Anh = p.Anh, TrangThai = p.TrangThai.TrangThaiPhim }))
                                                    .OrderByDescending(p => p.Id);
            var tenTheLoai = _context.TheLoai.FirstOrDefault(q => q.TheLoaiId == id)?.TenTheLoai;

            DSPhims = GetPhims(DSPhims, sortOrder);

            var paginatedPhims = await PaginatedList<PhimWithLoai>.CreateAsync(DSPhims, pageNumber, pageSize);

            ViewData["TopPhimBo"] = TopPhimBo;
            ViewData["TopPhimLe"] = TopPhimLe;
            ViewData["TopLikePhimBo"] = TopLikePhimBo;
            ViewData["TopLikePhimLe"] = TopLikePhimLe;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewBag.TenTheLoai = tenTheLoai;
            ViewData["PaginatedPhims"] = paginatedPhims;
            ViewData["sortOrder"] = sortOrder;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(paginatedPhims);
        }
        public async Task<IActionResult> TimKiemQuocGia(int id, int pageNumber = 1, string sortOrder = "")
        {
            ViewData["idQuocGia"] = id;
            int pageSize = 12;
            var TopPhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(3).ToList();
            var TopPhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(2).ToList();
            var TopLikePhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(3).ToList();
            var TopLikePhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(2).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            IQueryable<PhimBo> DSPhimBo = _context.PhimBo.Include(p => p.QuocGia).OrderByDescending(p => p.PhimBoId).Where(p => p.QuocGiaId == id);

            IQueryable<PhimWithLoai> DSPhims = _context.PhimBo.Include(p => p.TrangThai).Include(p => p.QuocGia)
                                                    .Where(p => p.QuocGiaId == id)
                                                    .Select(p => new PhimWithLoai { Id = p.PhimBoId, Loai = "Bo", TenPhim = p.TenPhim, Anh = p.Anh, TrangThai = p.TrangThai.TrangThaiPhim })
                                                    .Union(_context.PhimLe.Include(p => p.QuocGia).Include(p => p.TrangThai)
                                                                           .Where(p => p.QuocGiaId == id)
                                                                           .Select(p => new PhimWithLoai { Id = p.PhimLeId, Loai = "Le", TenPhim = p.TenPhim, Anh = p.Anh, TrangThai = p.TrangThai.TrangThaiPhim }))
                                                    .OrderByDescending(p => p.Id);
            var tenQuocGia = _context.QuocGia.FirstOrDefault(q => q.QuocGiaId == id)?.TenQuocGia;

            DSPhims = GetPhims(DSPhims, sortOrder);

            var paginatedPhims = await PaginatedList<PhimWithLoai>.CreateAsync(DSPhims, pageNumber, pageSize);
            ViewData["TopPhimBo"] = TopPhimBo;
            ViewData["TopPhimLe"] = TopPhimLe;
            ViewData["TopLikePhimBo"] = TopLikePhimBo;
            ViewData["TopLikePhimLe"] = TopLikePhimLe;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewBag.TenQuocGia = tenQuocGia;
            ViewData["PaginatedPhims"] = paginatedPhims;
            ViewData["sortOrder"] = sortOrder;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(paginatedPhims);
        }
        public async Task<IActionResult> TimKiemNam(int id, int pageNumber = 1, string sortOrder = "")
        {
            ViewData["idNam"] = id;
            int pageSize = 12;
            var TopPhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(3).ToList();
            var TopPhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.LuotXem).Take(2).ToList();
            var TopLikePhimBo = _context.PhimBo.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(3).ToList();
            var TopLikePhimLe = _context.PhimLe.Include(p => p.TrangThai).OrderByDescending(p => p.Like).Take(2).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            IQueryable<PhimBo> DSPhimBo = _context.PhimBo.Include(p => p.Nam).OrderByDescending(p => p.PhimBoId).Where(p => p.NamID == id);

            IQueryable<PhimWithLoai> DSPhims = _context.PhimBo.Include(p => p.Nam).Include(p => p.TrangThai)
                                                    .Where(p => p.NamID == id)
                                                    .Select(p => new PhimWithLoai { Id = p.PhimBoId, Loai = "Bo", TenPhim = p.TenPhim, Anh = p.Anh, TrangThai = p.TrangThai.TrangThaiPhim })
                                                    .Union(_context.PhimLe.Include(p => p.Nam)
                                                                           .Where(p => p.NamID == id)
                                                                           .Select(p => new PhimWithLoai { Id = p.PhimLeId, Loai = "Le", TenPhim = p.TenPhim, Anh = p.Anh, TrangThai = p.TrangThai.TrangThaiPhim }))
                                                    .OrderByDescending(p => p.Id);
            var tenNam = _context.Nam.FirstOrDefault(q => q.NamID == id)?.TenNam;

            DSPhims = GetPhims(DSPhims, sortOrder);
            var paginatedPhims = await PaginatedList<PhimWithLoai>.CreateAsync(DSPhims, pageNumber, pageSize);
            ViewData["TopPhimBo"] = TopPhimBo;
            ViewData["TopPhimLe"] = TopPhimLe;
            ViewData["TopLikePhimBo"] = TopLikePhimBo;
            ViewData["TopLikePhimLe"] = TopLikePhimLe;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewBag.TenNam = tenNam;
            ViewData["PaginatedPhims"] = paginatedPhims;
            ViewData["sortOrder"] = sortOrder;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(paginatedPhims);
        }

    }
}
