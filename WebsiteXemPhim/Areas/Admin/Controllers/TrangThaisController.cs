using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsiteXemPhim.Models;
using WebsiteXemPhim.Repositories;

namespace WebsiteXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TrangThaisController : Controller
    {
        private readonly ITrangThaiRepository _trangThaiRepository;
        public TrangThaisController(ITrangThaiRepository trangThaiRepository)
        {
            _trangThaiRepository = trangThaiRepository;
        }
        public async Task<IActionResult> Index()
        {
            var trangThai = await _trangThaiRepository.GetAllAsync();
            return View(trangThai);
        }
        public async Task<IActionResult> Details(int id)
        {
            var trangThai = await _trangThaiRepository.GetByIdAsync(id);
            if (trangThai == null)
            {
                return NotFound();
            }
            return View(trangThai);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrangThai trangThai)
        {
            if (ModelState.IsValid)
            {
                await _trangThaiRepository.AddAsync(trangThai);
                return RedirectToAction(nameof(Index));
            }
            return View(trangThai);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var trangThai = await _trangThaiRepository.GetByIdAsync(id);
            if (trangThai == null)
            {
                return NotFound();
            }
            return View(trangThai);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrangThai trangThai)
        {
            if (id != trangThai.TrangThaiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingTrangThai = await _trangThaiRepository.GetByIdAsync(id);
                // Cập nhật các thông tin khác của sản phẩm
                existingTrangThai.TrangThaiPhim = trangThai.TrangThaiPhim;

                await _trangThaiRepository.UpdateAsync(existingTrangThai);

                return RedirectToAction(nameof(Index));
            }
            return View(trangThai);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var trangThai = await _trangThaiRepository.GetByIdAsync(id);
            if (trangThai == null)
            {
                return NotFound();
            }
            return View(trangThai);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _trangThaiRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
