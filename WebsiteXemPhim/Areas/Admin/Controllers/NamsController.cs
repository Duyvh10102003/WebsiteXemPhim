using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class NamsController : Controller
    {
        private readonly INamRepository _namRepository;
        public NamsController(INamRepository namRepository)
        {
            _namRepository = namRepository;
        }
        public async Task<IActionResult> Index()
        {
            var nam = await _namRepository.GetAllAsync();
            return View(nam);
        }
        public async Task<IActionResult> Details(int id)
        {
            var nam = await _namRepository.GetByIdAsync(id);
            if (nam == null)
            {
                return NotFound();
            }
            return View(nam);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Nam nam)
        {
            if (ModelState.IsValid)
            {
                await _namRepository.AddAsync(nam);
                return RedirectToAction(nameof(Index));
            }
            return View(nam);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var nam = await _namRepository.GetByIdAsync(id);
            if (nam == null)
            {
                return NotFound();
            }
            return View(nam);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Nam nam)
        {
            if (id != nam.NamID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingNam = await _namRepository.GetByIdAsync(id);
                // Cập nhật các thông tin khác của sản phẩm
                existingNam.TenNam = nam.TenNam;

                await _namRepository.UpdateAsync(existingNam);

                return RedirectToAction(nameof(Index));
            }
            return View(nam);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var nam = await _namRepository.GetByIdAsync(id);
            if (nam == null)
            {
                return NotFound();
            }
            return View(nam);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _namRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
