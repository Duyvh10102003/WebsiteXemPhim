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
        public class BinhLuanViewModel
        {
            public int BinhLuanId { get; set; }
            public int? PhimBoId { get; set; }
            public int? PhimLeId { get; set; }
            public string UserId { get; set; }
            public string NoiDungBinhLuan { get; set; } // Nội dung bình luận
            public DateTime NgayTao { get; set; } // Ngày tạo bình luận
            public string TenNguoiDung { get; set; } // Tên người dùng

            public BinhLuanViewModel()
            {
                NgayTao = DateTime.Now; // Gán ngày hiện tại
            }
            // Thuộc tính tính toán để hiển thị thời gian đã trôi qua
            public string ThoiGianTruoc
            {
                get
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
