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
using WebsiteXemPhim.Repository;

namespace WebsiteXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PhimBoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhimBoRepository _phimBoRepository;
        private readonly ITheLoaiRepository _theLoaiRepository;
        private readonly INamRepository _namRepository;
        private readonly IQuocGiaRepository _quocGiaRepository;
        private readonly ITrangThaiRepository _trangThaiRepository;
        public PhimBoesController(ApplicationDbContext context, INamRepository namRepository, IQuocGiaRepository quocGiaRepository, ITheLoaiRepository theLoaiRepository,ITrangThaiRepository trangThaiRepository, IPhimBoRepository phimBoRepository)
        {
            _context = context;
            _namRepository = namRepository;
            _quocGiaRepository = quocGiaRepository;
            _theLoaiRepository = theLoaiRepository;
            _phimBoRepository = phimBoRepository;
            _trangThaiRepository = trangThaiRepository;
        }

        // GET: Admin/PhimLes
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            IQueryable<PhimBo> phimBosQuery = _context.PhimBo.Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai).OrderByDescending(p=>p.PhimBoId);

            var paginatedPhimBos = await PaginatedList<PhimBo>.CreateAsync(phimBosQuery, pageNumber, pageSize);
            // Tạo một danh sách chứa số lượng bình luận cho từng bộ phim
            var soLuongBinhLuan = new Dictionary<int, int>();
            // Lấy số lượng bình luận cho mỗi phim
            foreach (var phimBo in paginatedPhimBos)
            {
                var binhLuanCount = await _context.BinhLuan.CountAsync(b => b.PhimBoId == phimBo.PhimBoId);
                soLuongBinhLuan[phimBo.PhimBoId] = binhLuanCount;
            }
            ViewData["SoLuongBinhLuan"] = soLuongBinhLuan;
            return View(paginatedPhimBos);
        }

        // GET: Admin/PhimLes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var phimBo = await _phimBoRepository.GetByIdAsync(id);

            if (phimBo == null)
            {
                return NotFound();
            }

            return View(phimBo);
        }

        // GET: Admin/PhimLes/Create
        public async Task<IActionResult> Create()
        {
            var nams = await _namRepository.GetAllAsync();
            var theLoais = await _theLoaiRepository.GetAllAsync();
            var quocGias = await _quocGiaRepository.GetAllAsync();
            var trangThais = await _trangThaiRepository.GetAllAsync();
            ViewBag.Nams = new SelectList(nams, "NamID", "TenNam");
            ViewBag.QuocGias = new SelectList(quocGias, "QuocGiaId", "TenQuocGia");
            ViewBag.TheLoais = theLoais;
            ViewBag.TrangThais = new SelectList(trangThais, "TrangThaiId", "TrangThaiPhim");
            return View();
        }

        // POST: Admin/PhimLes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhimBo phimBo, IFormFile anh, IFormFile anhnen, List<int> theLoais)
        {
            if (ModelState.IsValid)
            {
                if (anh != null && anhnen != null)
                {
                    phimBo.Anh = await SaveImage(anh);
                    phimBo.AnhNen = await SaveImage(anhnen);
                    phimBo.LuotXem = 0;
                }

                await _phimBoRepository.AddAsync(phimBo);

                // Lưu các thể loại được chọn vào bảng chi tiết
                foreach (var theLoaiId in theLoais)
                {
                    var phimTheLoai = new ChiTietTheLoaiPhimBo { PhimBoId = phimBo.PhimBoId, TheLoaiId = theLoaiId };
                    _context.ChiTietTheLoaiPhimBo.Add(phimTheLoai);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var nams = await _namRepository.GetAllAsync();
            var theLoai = await _theLoaiRepository.GetAllAsync();
            var quocGias = await _quocGiaRepository.GetAllAsync();
            ViewBag.Nams = new SelectList(nams, "NamID", "TenNam");
            ViewBag.QuocGias = new SelectList(quocGias, "QuocGiaId", "TenQuocGia");
            ViewBag.TheLoais = theLoai;
            return View(phimBo);
        }


        // GET: Admin/PhimLes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var phimBo = await _phimBoRepository.GetByIdAsync(id);
            if (phimBo == null)
            {
                return NotFound();
            }
            var nams = await _namRepository.GetAllAsync();
            var theLoais = await _theLoaiRepository.GetAllAsync();
            var quocGias = await _quocGiaRepository.GetAllAsync();
            var trangThais = await _trangThaiRepository.GetAllAsync();
            ViewBag.Nams = new SelectList(nams, "NamID", "TenNam");
            ViewBag.QuocGias = new SelectList(quocGias, "QuocGiaId", "TenQuocGia");
            ViewBag.TheLoais = theLoais;
            ViewBag.TrangThais = new SelectList(trangThais, "TrangThaiId", "TrangThaiPhim");
            return View(phimBo);
        }

        // POST: Admin/PhimLes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PhimBo phimBo, IFormFile anh, IFormFile anhnen, List<int> theLoais)
        {
            ModelState.Remove("Anh");
            ModelState.Remove("AnhNen");
            // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != phimBo.PhimBoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingPhimBo = await _phimBoRepository.GetByIdAsync(id);
                if (existingPhimBo == null)
                {
                    return NotFound();
                }

                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên            
                if (anh == null)
                {
                    phimBo.Anh = existingPhimBo.Anh;
                }
                else
                {
                    // Lưu hình ảnh mới
                    phimBo.Anh = await SaveImage(anh);
                }
                if (anhnen == null)
                {
                    phimBo.AnhNen = existingPhimBo.AnhNen;
                }
                else
                {
                    // Lưu hình ảnh mới
                    phimBo.AnhNen = await SaveImage(anhnen);
                }

                // Xóa các mối quan hệ thể loại cũ
                var oldTheLoais = _context.ChiTietTheLoaiPhimBo.Where(ct => ct.PhimBoId == id).ToList();
                _context.ChiTietTheLoaiPhimBo.RemoveRange(oldTheLoais);

                // Thêm các mối quan hệ thể loại mới
                foreach (var theLoaiId in theLoais)
                {
                    var phimTheLoai = new ChiTietTheLoaiPhimBo { PhimBoId = phimBo.PhimBoId, TheLoaiId = theLoaiId };
                    _context.ChiTietTheLoaiPhimBo.Add(phimTheLoai);
                }

                // Cập nhật các thông tin khác của sản phẩm
                existingPhimBo.TenPhim = phimBo.TenPhim;
                existingPhimBo.NoiDung = phimBo.NoiDung;
                existingPhimBo.Anh = phimBo.Anh;
                existingPhimBo.AnhNen = phimBo.AnhNen;
                existingPhimBo.LuotXem = phimBo.LuotXem;
                existingPhimBo.Like = phimBo.Like;
                existingPhimBo.TrangThaiId = phimBo.TrangThaiId;
                existingPhimBo.QuocGiaId = phimBo.QuocGiaId;
                existingPhimBo.NamID = phimBo.NamID;

                await _phimBoRepository.UpdateAsync(existingPhimBo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var nams = await _namRepository.GetAllAsync();
            var theLoai = await _theLoaiRepository.GetAllAsync();
            var quocGias = await _quocGiaRepository.GetAllAsync();
            var trangThais = await _trangThaiRepository.GetAllAsync();
            ViewBag.Nams = new SelectList(nams, "NamID", "TenNam");
            ViewBag.QuocGias = new SelectList(quocGias, "QuocGiaId", "TenQuocGia");
            ViewBag.TheLoais = new SelectList(theLoai, "TheLoaiId", "TenTheLoai");
            ViewBag.TrangThais = new SelectList(trangThais, "TrangThaiId", "TrangThaiPhim");
            ViewBag.TheLoai = theLoai;
            return View(phimBo);
        }

        // GET: Admin/PhimLes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var phimBo = await _phimBoRepository.GetByIdAsync(id);
            if (phimBo == null)
            {
                return NotFound();
            }
            return View(phimBo);
        }

        // POST: Admin/PhimLes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chitiet = _context.ChiTietTheLoaiPhimBo.Where(ct => ct.PhimBoId == id).ToList();
            _context.ChiTietTheLoaiPhimBo.RemoveRange(chitiet);
            await _phimBoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/frontend/img/anime", image.FileName); // Thay   đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/frontend/img/anime/" + image.FileName; // Trả về đường dẫn tương đối
        }

        public async Task<IActionResult> SearchPhims(string query, int pageNumber = 1)
        {
            IQueryable<PhimBo> phimBoesQuery = _context.PhimBo.Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai).Where(p => p.TenPhim.Contains(query));

            var paginatedPhimBoes = await PaginatedList<PhimBo>.CreateAsync(phimBoesQuery, pageNumber, 2);
            return PartialView(paginatedPhimBoes);
        }
        public async Task<IActionResult> PagingNoLibrary(int pageNumber)
        {
            int pageSize = 10;
            IQueryable<PhimBo> PhimBosQuery = _context.PhimBo.Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai); 
            var pagedPhimBos = await PhimBosQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return View(pagedPhimBos);
        }
        public List<string> SearchSuggestions(string query)
        {
            return _context.PhimBo
                .Where(p => p.TenPhim.Contains(query))
                .Select(p => p.TenPhim + "|" + p.Anh)
                .ToList();
        }
    }
}
