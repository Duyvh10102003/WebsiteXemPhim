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
    public class QuocGiasController : Controller
    {
        private readonly IQuocGiaRepository _quocGiaRepository;
        public QuocGiasController(IQuocGiaRepository quocGiaRepository)
        {
            _quocGiaRepository = quocGiaRepository;
        }
        public async Task<IActionResult> Index()
        {
            var quocGia = await _quocGiaRepository.GetAllAsync();
            return View(quocGia);
        }
        public async Task<IActionResult> Details(int id)
        {
            var quocGia = await _quocGiaRepository.GetByIdAsync(id);
            if (quocGia == null)
            {
                return NotFound();
            }
            return View(quocGia);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuocGia quocGia)
        {
            if (ModelState.IsValid)
            {
                await _quocGiaRepository.AddAsync(quocGia);
                return RedirectToAction(nameof(Index));
            }
            return View(quocGia);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var quocGia = await _quocGiaRepository.GetByIdAsync(id);
            if (quocGia == null)
            {
                return NotFound();
            }
            return View(quocGia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuocGia quocGia)
        {
            if (id != quocGia.QuocGiaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingQuocGia = await _quocGiaRepository.GetByIdAsync(id);
                // Cập nhật các thông tin khác của sản phẩm
                existingQuocGia.TenQuocGia = quocGia.TenQuocGia;

                await _quocGiaRepository.UpdateAsync(existingQuocGia);

                return RedirectToAction(nameof(Index));
            }
            return View(quocGia);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var quocGia = await _quocGiaRepository.GetByIdAsync(id);
            if (quocGia == null)
            {
                return NotFound();
            }
            return View(quocGia);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _quocGiaRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
