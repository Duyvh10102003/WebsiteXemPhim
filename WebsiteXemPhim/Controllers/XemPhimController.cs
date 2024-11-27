using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Controllers
{
    public class XemPhimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public XemPhimController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> XemPhimBo(int id,int tap)
        {
            var BinhLuan = await _context.BinhLuan
            .Include(b => b.User) 
            .Where(b => b.PhimBoId == id).OrderByDescending(p=>p.BinhLuanId)
            .ToListAsync(); 
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            var TapPhim = _context.TapPhim.Include(p => p.PhimBo).Where(p => p.PhimBoId == id).OrderByDescending(p=>p.Tap).ToList();
            var temp = _context.TapPhim.Include(p => p.PhimBo).Where(p => p.PhimBoId == id && p.Tap == 1);
            // Lấy thông tin phim hiện tại, bao gồm thể loại qua bảng ChiTietTheLoaiPhimBos
            var phimBo = await _context.PhimBo
                .Include(p => p.ChiTietTheLoaiPhimBos)
                .ThenInclude(ct => ct.TheLoai) // Lấy dữ liệu thể loại
                .FirstOrDefaultAsync(p => p.PhimBoId == id);

            if (phimBo == null)
            {
                return NotFound();
            }

            // Lấy danh sách các thể loại của phim hiện tại
            var theLoaiIds = phimBo.ChiTietTheLoaiPhimBos
                .Select(ct => ct.TheLoaiId)
                .ToList();

            // Tìm các phim có cùng thể loại, loại trừ phim hiện tại
            var phimGoiY = await _context.PhimBo
                .Include(p => p.ChiTietTheLoaiPhimBos)
                .Where(p => p.PhimBoId != id &&
                            p.ChiTietTheLoaiPhimBos.Any(ct => theLoaiIds.Contains(ct.TheLoaiId))) // Có ít nhất một thể loại trùng
                .OrderByDescending(p => p.LuotXem)
                .Take(10) // Giới hạn 10 phim
                .ToListAsync();
            ViewBag.BinhLuanThoiGian = BinhLuan.ToDictionary(b => b.BinhLuanId, b => ThoiGianTruoc(b.NgayTao));
            ViewData["PhimGoiY"] = phimGoiY;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;
            ViewData["TapPhim"] = TapPhim;
            ViewData["Tap"] = tap;
            ViewData["BinhLuan"] = BinhLuan;
            var PhimBoesQuery = _context.TapPhim.Include(p => p.PhimBo).Where(p => p.PhimBoId == id && p.Tap==tap);
            return View(PhimBoesQuery);
        }
        public async Task<IActionResult> XemPhimLe(int id)
        {
            var TheLoai = _context.TheLoai.ToList();
            var QuocGia = _context.QuocGia.ToList();
            var Nam = _context.Nam.ToList();
            var BinhLuan = await _context.BinhLuan
                .Include(b => b.User)
                .Where(b => b.PhimLeId == id)
                .OrderByDescending(p => p.BinhLuanId)
                .ToListAsync();

            // Lấy thông tin phim hiện tại, bao gồm thể loại qua bảng ChiTietTheLoaiPhimLes
            var phimLe = await _context.PhimLe
                .Include(p => p.ChiTietTheLoaiPhimLes)
                .ThenInclude(ct => ct.TheLoai) // Lấy dữ liệu thể loại
                .FirstOrDefaultAsync(p => p.PhimLeId == id);

            if (phimLe == null)
            {
                return NotFound();
            }

            // Lấy danh sách các thể loại của phim hiện tại
            var theLoaiIds = phimLe.ChiTietTheLoaiPhimLes
                .Select(ct => ct.TheLoaiId)
                .ToList();

            // Tìm các phim có cùng thể loại, loại trừ phim hiện tại
            var phimGoiY = await _context.PhimLe
                .Include(p => p.ChiTietTheLoaiPhimLes)
                .Where(p => p.PhimLeId != id &&
                            p.ChiTietTheLoaiPhimLes.Any(ct => theLoaiIds.Contains(ct.TheLoaiId))) // Có ít nhất một thể loại trùng
                .OrderByDescending(p => p.LuotXem)
                .Take(8) // Giới hạn 8 phim
                .ToListAsync();
            ViewBag.BinhLuanThoiGian = BinhLuan.ToDictionary(b => b.BinhLuanId, b => ThoiGianTruoc(b.NgayTao));
            ViewData["PhimGoiY"] = phimGoiY;
            ViewData["BinhLuan"] = BinhLuan;
            ViewData["TheLoai"] = TheLoai;
            ViewData["QuocGia"] = QuocGia;
            ViewData["Nam"] = Nam;

            return View(phimLe);
        }
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
