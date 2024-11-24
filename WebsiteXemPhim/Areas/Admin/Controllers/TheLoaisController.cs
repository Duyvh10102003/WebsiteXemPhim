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
    public class TheLoaisController : Controller
    {
        private readonly ITheLoaiRepository _theLoaiRepository;
        public TheLoaisController(ITheLoaiRepository theLoaiRepository)
        {
            _theLoaiRepository = theLoaiRepository;
        }
        public async Task<IActionResult> Index()
        {
            var theLoai = await _theLoaiRepository.GetAllAsync();
            return View(theLoai);
        }
        public async Task<IActionResult> Details(int id)
        {
            var theLoai = await _theLoaiRepository.GetByIdAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }
            return View(theLoai);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TheLoai theLoai)
        {
            if (ModelState.IsValid)
            {
                await _theLoaiRepository.AddAsync(theLoai);
                return RedirectToAction(nameof(Index));
            }
            return View(theLoai);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var theLoai = await _theLoaiRepository.GetByIdAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }
            return View(theLoai);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TheLoai theLoai)
        {
            if (id != theLoai.TheLoaiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingTheLoai = await _theLoaiRepository.GetByIdAsync(id);
                // Cập nhật các thông tin khác của sản phẩm
                existingTheLoai.TenTheLoai = theLoai.TenTheLoai;

                await _theLoaiRepository.UpdateAsync(existingTheLoai);

                return RedirectToAction(nameof(Index));
            }
            return View(theLoai);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var theLoai = await _theLoaiRepository.GetByIdAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }
            return View(theLoai);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _theLoaiRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
