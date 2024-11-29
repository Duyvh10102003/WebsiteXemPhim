using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebsiteXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]


    public class ThongKeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ThongKeController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            // Kết hợp danh sách phim bộ và phim lẻ
            var DSPhims = _context.PhimBo
                .Include(p => p.TrangThai)
                .Select(p => new PhimWithLoai { Id = p.PhimBoId, Loai = "Bo", TenPhim = p.TenPhim, Anh = p.Anh, TrangThai = p.TrangThai.TrangThaiPhim, LuotXem = p.LuotXem })
                .Union(
                    _context.PhimLe
                        .Include(p => p.TrangThai)
                        .Select(p => new PhimWithLoai { Id = p.PhimLeId, Loai = "Le", TenPhim = p.TenPhim, Anh = p.Anh, TrangThai = p.TrangThai.TrangThaiPhim, LuotXem = p.LuotXem }))
                .OrderByDescending(p => p.Id)
                .Take(10)
                .ToList(); // Chuyển kết quả thành danh sách

            int demnd = _context.Users.Count();
            int demphimle = _context.PhimLe.Count();
            int demphimbo = _context.PhimBo.Count();

            ViewData["NguoiDung"] = demnd;
            ViewData["PhimLe"] = demphimle;
            ViewData["PhimBo"] = demphimbo;

            var DSPhimBo = _context.PhimBo.OrderByDescending(x => x.LuotXem).Take(10).ToList();
            ViewData["TopPhim"] = DSPhimBo;

            var DSPhimLe = _context.PhimLe.OrderByDescending(x => x.LuotXem).Take(10).ToList();
            ViewData["TopPhimLe"] = DSPhimLe;

            var DSPhimBoLike = _context.PhimBo.OrderByDescending(x => x.Like).Take(10).ToList();
            var DSPhimLeLike = _context.PhimLe.OrderByDescending(x => x.Like).Take(10).ToList();

            var phimBoMoi = _context.PhimBo.OrderByDescending(p => p.PhimBoId).Take(10).ToList();
            ViewData["PhimBoMoi"] = phimBoMoi;

            var phimLeMoi = _context.PhimLe.OrderByDescending(p => p.PhimLeId).Take(10).ToList();
            ViewData["PhimLeMoi"] = phimLeMoi;

            // Chuẩn bị dữ liệu cho biểu đồ với tên phim đã cắt ngắn
            ViewData["PhimBoLuotXem"] = DSPhimBo.Select(x => x.LuotXem).ToList();
            ViewData["PhimLeLuotXem"] = DSPhimLe.Select(x => x.LuotXem).ToList();
            ViewData["PhimBoLike"] = DSPhimBoLike.Select(x => x.Like).ToList();
            ViewData["PhimLeLike"] = DSPhimLeLike.Select(x => x.Like).ToList();
            ViewData["PhimBoTenDisplay"] = DSPhimBo.Select(x => ShortenTitle(x.TenPhim, 8)).ToList();
            ViewData["PhimLeTenDisplay"] = DSPhimLe.Select(x => ShortenTitle(x.TenPhim, 8)).ToList();
            ViewData["PhimBoTen"] = DSPhimBo.Select(x => x.TenPhim).ToList();
            ViewData["PhimLeTen"] = DSPhimLe.Select(x => x.TenPhim).ToList();

            return View(DSPhims); // Trả về danh sách DSPhims đã sắp xếp và chuyển thành danh sách
        }


        public string ShortenTitle(string title, int maxLength)
        {
            if (title.Length <= maxLength)
                return title;
            return title.Substring(0, maxLength) + "...";
        }





    }
}
