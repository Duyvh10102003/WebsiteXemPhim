using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;
using WebsiteXemPhim.Repositories;

namespace WebsiteXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TapPhimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITapPhimRepository _tapPhimRepository;
        private readonly IPhimBoRepository _phimBoRepository;
        private readonly IEmailSender _emailSender;
        public TapPhimsController(ApplicationDbContext context, IPhimBoRepository phimBoRepository, ITapPhimRepository tapPhimRepository, IEmailSender emailSender) // Cập nhật constructor
        {
            _context = context;
            _phimBoRepository = phimBoRepository;
            _tapPhimRepository = tapPhimRepository;
            _emailSender = emailSender; // Khởi tạo trường
        }

        public async Task<IActionResult> Index(int id, int pageNumber = 1)
        {
            TempData["PhimBoId"] = id;
            int pageSize = 10;
            IQueryable<TapPhim> PhimBoesQuery = _context.TapPhim.Include(p => p.PhimBo).Where(p => p.PhimBoId == id);
            var paginatedPhimBoes = await PaginatedList<TapPhim>.CreateAsync(PhimBoesQuery, pageNumber, pageSize);
            return View(paginatedPhimBoes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tapPhims = await _tapPhimRepository.GetByIdAsync(id);
            if (tapPhims == null)
            {
                return NotFound();
            }
            ViewBag.Id = tapPhims.PhimBoId;
            return View(tapPhims);
        }

        public IActionResult Create()
        {
            int tapPhimId = (int)TempData["PhimBoId"];
            TempData["PhimBoId"] = tapPhimId;
            ViewBag.PhimBoId = tapPhimId;
            return View();
        }
        private async Task NotifyUsersAboutNewEpisode(int phimBoId, TapPhim tapPhim)
        {
            var users = await _context.HopPhim
                .Where(h => h.PhimBoId == phimBoId)
                .Select(h => h.UserId)
                .Distinct()
                .ToListAsync();

            var phim = await _context.TapPhim
                .Include(tp => tp.PhimBo)
                .FirstOrDefaultAsync(tp => tp.PhimBoId == phimBoId && tp.Tap == tapPhim.Tap);

            var callbackUrl = Url.Action(
                "XemPhimBo",
                "XemPhim",
                new { area = "", id = phimBoId, tap = tapPhim.Tap },
                protocol: Request.Scheme);

            string message = $"Bộ phim {phim.PhimBo.TenPhim} đã có tập mới {tapPhim.Tap}.";

            foreach (var userId in users)
            {
                // Lưu thông báo
                var notification = new ThongBao
                {
                    UserId = userId,
                    Message = $"{phim.PhimBo.TenPhim} tập {tapPhim.Tap}.",
                    Url = callbackUrl
                };
                _context.ThongBaos.Add(notification);

                // Gửi email nếu cần
                var userEmail = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => u.Email)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(userEmail))
                {
                    string subject = "Tập phim mới đã được cập nhật!";
                    string htmlMessage = $"{message} <a href='{callbackUrl}'>Bấm vào đây để xem ngay</a>";
                    await _emailSender.SendEmailAsync(userEmail, subject, htmlMessage);
                }
            }

            await _context.SaveChangesAsync();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TapPhim tapPhim)
        {
            int tapPhimId = (int)TempData["PhimBoId"];
            if (ModelState.IsValid)
            {
                tapPhim.PhimBoId = tapPhimId;
                await _tapPhimRepository.AddAsync(tapPhim);

                // Gửi email thông báo cho tất cả tài khoản đã thích bộ phim
                await NotifyUsersAboutNewEpisode(tapPhimId, tapPhim);

                return RedirectToAction("Index", new { id = tapPhim.PhimBoId });
            }
            ViewBag.PhimBoId = tapPhimId;
            return View(tapPhim);
        }


        // GET: Admin/TapPhims/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tapPhims = await _tapPhimRepository.GetByIdAsync(id);
            if (tapPhims == null)
            {
                return NotFound();
            }
            TempData["PhimBoId"] = tapPhims.PhimBoId;
            ViewBag.Id = tapPhims.PhimBoId;
            var truyen = await _phimBoRepository.GetAllAsync();
            return View(tapPhims);
        }

        // POST: Admin/TapPhims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TapPhim tapPhim)
        {
            int PhimBoId = (int)TempData["PhimBoId"];
            if (id != tapPhim.TapPhimId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingPhim = await _tapPhimRepository.GetByIdAsync(id);
                
                existingPhim.Tap = tapPhim.Tap;
                existingPhim.Link = tapPhim.Link;
                existingPhim.Linkdb = tapPhim.Linkdb;
                existingPhim.ThoiLuong = tapPhim.ThoiLuong;

                existingPhim.PhimBoId = PhimBoId;

                await _tapPhimRepository.UpdateAsync(existingPhim);

                return RedirectToAction("Index", new { id = PhimBoId });
            }
            var truyen = await _phimBoRepository.GetAllAsync();
            ViewBag.PhimBoId = PhimBoId;
            return View(tapPhim);
        }

        // GET: Admin/TapPhims/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tapPhims = await _tapPhimRepository.GetByIdAsync(id);
            if (tapPhims == null)
            {
                return NotFound();
            }
            TempData["PhimBoID"] = tapPhims.PhimBoId;
            ViewBag.Id = tapPhims.PhimBoId;
            return View(tapPhims);
        }

        // POST: Admin/TapPhims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var tap = await _context.TapPhim.Where(p => p.TapPhimId == id).FirstOrDefaultAsync();

            if (tap == null)
            {
                return NotFound(); 
            }

            if (TempData["PhimBoId"] == null)
            {
                return BadRequest("PhimBoId is missing.");
            }

            var notifications = await _context.ThongBaos
                .Where(tb => !string.IsNullOrEmpty(tb.Url) &&
                            tb.Url.Contains($"/{(int)TempData["PhimBoId"]}?tap={tap.Tap}"))
                .ToListAsync();

            _context.ThongBaos.RemoveRange(notifications);
            await _tapPhimRepository.DeleteAsync(id);
            await _context.SaveChangesAsync(); 

            return RedirectToAction("Index", new { id = (int)TempData["PhimBoId"] });
        }




    }
}
