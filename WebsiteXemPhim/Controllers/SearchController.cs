using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> SearchPhims(string query, int pageNumber = 1, string sortOrder = "")
        {
            int pageSize = 12;
            ViewData["Query"] = query;
            // Lấy danh sách phim bộ và phim lẻ theo từ khóa tìm kiếm
            IQueryable<PhimWithLoai> DSPhims = _context.PhimBo.Include(p => p.TrangThai).Where(p => p.TenPhim.Contains(query) )
                                                               .Select(p => new PhimWithLoai { Id = p.PhimBoId, Loai = "Bo", TenPhim = p.TenPhim , Anh = p.Anh, TrangThai = p.TrangThai.TrangThaiPhim})
                                                               .Union(_context.PhimLe.Include(p => p.TrangThai).Where(p => p.TenPhim.Contains(query))
                                                                                     .Select(p => new PhimWithLoai { Id = p.PhimLeId, Loai = "Le", TenPhim = p.TenPhim, Anh = p.Anh , TrangThai = p.TrangThai.TrangThaiPhim }))
                                                               .OrderByDescending(p => p.Id);
            switch (sortOrder)
            {
                case "AZ":
                    DSPhims = DSPhims.OrderBy(p => p.TenPhim);
                    break;
                case "ZA":
                    DSPhims = DSPhims.OrderByDescending(p => p.TenPhim);
                    break;
                default:
                    DSPhims = DSPhims.OrderByDescending(p => p.Id);
                    break;
            }

            // Tạo phân trang cho danh sách chung
            var paginatedPhims = await PaginatedList<PhimWithLoai>.CreateAsync(DSPhims, pageNumber, pageSize);

            // Lấy danh sách các thông tin khác cần thiết
            IQueryable<PhimWithLoai> DSPhims2 = _context.PhimBo.Include(p => p.TrangThai)
                                                   .Select(p => new PhimWithLoai { Id = p.PhimBoId, Loai = "Bo", TenPhim = p.TenPhim, Anh = p.Anh, AnhNen = p.AnhNen, TrangThai = p.TrangThai.TrangThaiPhim, Like = p.Like, LuotXem = p.LuotXem })
                                                   .Union(_context.PhimLe.Include(p => p.TrangThai)
                                                                         .Select(p => new PhimWithLoai { Id = p.PhimLeId, Loai = "Le", TenPhim = p.TenPhim, Anh = p.Anh, AnhNen = p.AnhNen, TrangThai = p.TrangThai.TrangThaiPhim, Like = p.Like, LuotXem = p.LuotXem }))
                                                   .OrderByDescending(p => p.Id);
            var TopLuotXem = DSPhims2.OrderByDescending(p => p.LuotXem).Take(5).ToList();
            var TopLikePhim = DSPhims2.OrderByDescending(p => p.Like).Take(5).ToList();
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();

            // Đưa dữ liệu vào ViewData
            ViewData["TopLuotXem"] = TopLuotXem;
            ViewData["TopLikePhim"] = TopLikePhim;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewData["PaginatedPhims"] = paginatedPhims;
            ViewData["sortOrder"] = sortOrder;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(paginatedPhims);
        }
    }
}
