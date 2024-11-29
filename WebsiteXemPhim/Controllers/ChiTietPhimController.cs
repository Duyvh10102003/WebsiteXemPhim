using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Controllers
{
    public class ChiTietPhimController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ChiTietPhimController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ChiTietPhimBo(int id)
        {
            var BinhLuan = await _context.BinhLuan
                .Include(b => b.User)
                .Where(b => b.PhimBoId == id)
                .OrderByDescending(p => p.BinhLuanId)
                .ToListAsync();

            var TopPhim = await _context.PhimBo
                .Include(p => p.TrangThai)
                .OrderByDescending(p => p.LuotXem)
                .Take(5)
                .ToListAsync();

            var TheLoai = await _context.TheLoai.ToListAsync();
            var QuocGia = await _context.QuocGia.ToListAsync();
            var Nam = await _context.Nam.ToListAsync();
            var chiTietPhim = await _context.PhimBo
                .Include(p => p.TrangThai)
                .Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.TheLoai)
                .Include(p => p.QuocGia)
                .Include(p => p.Nam)
                .Where(p => p.PhimBoId == id)
                .FirstOrDefaultAsync();
            // Tính trung bình số sao cho PhimBoId
            var averageRating = await _context.DanhGia
                .Where(d => d.PhimBoId == id)
                .DefaultIfEmpty()  // Đảm bảo trả về một danh sách có phần tử mặc định nếu không có đánh giá nào
                .AverageAsync(d => d == null ? 0 : d.Star); // Nếu không có đánh giá nào, trả về 0
            int tongUser = _context.DanhGia.Count(dg => dg.PhimBoId == id);

            ViewBag.TongUser = tongUser;
            ViewBag.AverageRating = averageRating;
            var user = await _userManager.GetUserAsync(User);
            ViewBag.BinhLuanThoiGian = BinhLuan.ToDictionary(b => b.BinhLuanId, b => ThoiGianTruoc(b.NgayTao));
            ViewData["IsFavorite"] = await CheckIfFavoriteAsync(id, user);
            ViewData["TopPhim"] = TopPhim;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewData["BinhLuan"] = BinhLuan;
            return View(chiTietPhim);
        }


        public async Task<IActionResult> ChiTietPhimLe(int id)
        {
            var BinhLuan = await _context.BinhLuan
               .Include(b => b.User)
               .Where(b => b.PhimLeId == id)
               .OrderByDescending(p => p.BinhLuanId)
               .ToListAsync();

            var TopPhim = await _context.PhimLe
                .Include(p => p.TrangThai)
                .OrderByDescending(p => p.LuotXem)
                .Take(5)
                .ToListAsync();

            var TheLoai = await _context.TheLoai.ToListAsync();
            var QuocGia = await _context.QuocGia.ToListAsync();
            var Nam = await _context.Nam.ToListAsync();
            var chiTietPhimLe = await _context.PhimLe
                .Include(p => p.TrangThai)
                .Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.TheLoai)
                .Include(p => p.QuocGia)
                .Include(p => p.Nam)
                .Where(p => p.PhimLeId == id)
                .FirstOrDefaultAsync();
            // Tính trung bình số sao cho PhimBoId
            var averageRating = await _context.DanhGia
                 .Where(d => d.PhimLeId == id)
                 .DefaultIfEmpty()  // Đảm bảo trả về một danh sách có phần tử mặc định nếu không có đánh giá nào
                 .AverageAsync(d => d == null ? 0 : d.Star); // Nếu không có đánh giá nào, trả về 0
            int tongUser = _context.DanhGia.Count(dg => dg.PhimLeId == id);

            ViewBag.TongUser = tongUser;
            ViewBag.AverageRating = averageRating;
            var user = await _userManager.GetUserAsync(User);
            ViewBag.BinhLuanThoiGian = BinhLuan.ToDictionary(b => b.BinhLuanId, b => ThoiGianTruoc(b.NgayTao));
            ViewData["IsFavorite"] = await CheckIfFavoriteAsync(id, user);
            ViewData["TopPhim"] = TopPhim;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewData["BinhLuan"] = BinhLuan;
            return View(chiTietPhimLe);
        }

        // Kiểm tra xem phim có phải là yêu thích hay không
        public async Task<bool> CheckIfFavoriteAsync(int phimId, IdentityUser user)
        {
            if (user == null) return false;

            // Kiểm tra xem phim có thuộc danh mục phim lẻ hay phim bộ và có phải yêu thích hay không
            var hopPhimBo = await _context.HopPhim
               .AnyAsync(h => h.UserId == user.Id && h.PhimBoId == phimId);
            var hopPhimLe = await _context.HopPhim
                .AnyAsync(h => h.UserId == user.Id && h.PhimLeId == phimId);

            return hopPhimLe || hopPhimBo;
        }
        public async Task<IActionResult> XemPhimNgauNhien(string query = "")
        {
            // Kết hợp dữ liệu từ PhimBo và PhimLe
            IQueryable<PhimWithLoai> DSPhims = _context.PhimBo.Include(p => p.TrangThai)
                .Where(p => string.IsNullOrEmpty(query) || p.TenPhim.Contains(query))  // Kiểm tra query nếu không có
                .Select(p => new PhimWithLoai
                {
                    Id = p.PhimBoId,
                    Loai = "Bo",
                    TenPhim = p.TenPhim,
                    Anh = p.Anh,
                    TrangThai = p.TrangThai.TrangThaiPhim
                })
                .Union(
                    _context.PhimLe.Include(p => p.TrangThai)
                        .Where(p => string.IsNullOrEmpty(query) || p.TenPhim.Contains(query))  // Kiểm tra query nếu không có
                        .Select(p => new PhimWithLoai
                        {
                            Id = p.PhimLeId,
                            Loai = "Le",
                            TenPhim = p.TenPhim,
                            Anh = p.Anh,
                            TrangThai = p.TrangThai.TrangThaiPhim
                        })
                )
                .OrderBy(r => Guid.NewGuid())  // Sắp xếp ngẫu nhiên
                .Take(1);  // Lấy một bản ghi ngẫu nhiên

            var phimNgauNhien = await DSPhims.FirstOrDefaultAsync();

            if (phimNgauNhien != null)
            {
                if (phimNgauNhien.Loai == "Bo")
                {
                    return RedirectToAction("ChiTietPhimBo", new { id = phimNgauNhien.Id });
                }
                else
                {
                    return RedirectToAction("ChiTietPhimLe", new { id = phimNgauNhien.Id });
                }
            }

            // Nếu không có phim nào, quay về trang chủ
            return RedirectToAction("Index", "Home");
        }
        // Thuộc tính tính toán để hiển thị thời gian đã trôi qua
        public string ThoiGianTruoc(DateTime NgayTao)
        {
            TimeSpan timeSinceCreation = DateTime.Now - NgayTao;
            if (timeSinceCreation.TotalMinutes < 1)
            {
                return "vừa xong";
            }
            else if (timeSinceCreation.TotalMinutes < 60)
            {
                return $"{(int)timeSinceCreation.TotalMinutes} phút trước";
            }
            else if (timeSinceCreation.TotalHours < 24)
            {
                return $"{(int)timeSinceCreation.TotalHours} giờ trước";
            }
            else if (timeSinceCreation.TotalDays < 30)
            {
                return $"{(int)timeSinceCreation.TotalDays} ngày trước";
            }
            else if (timeSinceCreation.TotalDays < 365)
            {
                return $"{(int)(timeSinceCreation.TotalDays / 30)} tháng trước";
            }
            else
            {
                return $"{(int)(timeSinceCreation.TotalDays / 365)} năm trước";
            }
        }
    }
}
