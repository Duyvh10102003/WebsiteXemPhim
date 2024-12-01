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
    public class PhimLesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhimLeRepository _phimLeRepository;
        private readonly ITheLoaiRepository _theLoaiRepository;
        private readonly INamRepository _namRepository;
        private readonly IQuocGiaRepository _quocGiaRepository;
        private readonly ITrangThaiRepository _trangThaiRepository;
        public PhimLesController(ApplicationDbContext context, INamRepository namRepository, IQuocGiaRepository quocGiaRepository,ITrangThaiRepository trangThaiRepository, ITheLoaiRepository theLoaiRepository, IPhimLeRepository phimLeRepository)
        {
            _context = context;
            _namRepository = namRepository;
            _quocGiaRepository = quocGiaRepository;
            _theLoaiRepository = theLoaiRepository;
            _phimLeRepository = phimLeRepository;
            _trangThaiRepository = trangThaiRepository;
        }

        // GET: Admin/PhimLes
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;
            IQueryable<PhimLe> phimLesQuery = _context.PhimLe.Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai).OrderByDescending(p=>p.PhimLeId);
            var paginatedPhimLes = await PaginatedList<PhimLe>.CreateAsync(phimLesQuery, pageNumber, pageSize);
            // Tạo một danh sách chứa số lượng bình luận cho từng bộ phim
            var soLuongBinhLuan = new Dictionary<int, int>();
            // Lấy số lượng bình luận cho mỗi phim
            foreach (var phimLe in paginatedPhimLes)
            {
                var binhLuanCount = await _context.BinhLuan.CountAsync(b => b.PhimLeId == phimLe.PhimLeId);
                soLuongBinhLuan[phimLe.PhimLeId] = binhLuanCount;
            }
            ViewData["SoLuongBinhLuan"] = soLuongBinhLuan;
            return View(paginatedPhimLes);
        }

        // GET: Admin/PhimLes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var phimLe = await _phimLeRepository.GetByIdAsync(id);

            if (phimLe == null)
            {
                return NotFound();
            }

            return View(phimLe);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhimLe phimLe, IFormFile anh ,IFormFile anhnen, List<int> theLoais)
        {
            if (ModelState.IsValid)
            {
                if (anh != null && anhnen != null )
                {
                    phimLe.Anh = await SaveImage(anh);
                    phimLe.AnhNen = await SaveImage(anhnen);
                    phimLe.LuotXem = 0;
                    phimLe.Like = 0;
                }
                await _phimLeRepository.AddAsync(phimLe);
                // Lưu các thể loại được chọn vào bảng chi tiết
                foreach (var theLoaiId in theLoais)
                {
                    var phimTheLoai = new ChiTietTheLoaiPhimLe { PhimLeId = phimLe.PhimLeId, TheLoaiId = theLoaiId };
                    _context.ChiTietTheLoaiPhimLe.Add(phimTheLoai);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var nams = await _namRepository.GetAllAsync();
            var theLoai = await _theLoaiRepository.GetAllAsync();
            var quocGias = await _quocGiaRepository.GetAllAsync();
            var trangThais = await _trangThaiRepository.GetAllAsync();
            ViewBag.Nams = new SelectList(nams, "NamID", "TenNam");
            ViewBag.QuocGias = new SelectList(quocGias, "QuocGiaId", "TenQuocGia");
            ViewBag.TheLoais = theLoai;
            ViewBag.TrangThais = new SelectList(trangThais, "TrangThaiId", "TrangThaiPhim");
            return View(phimLe);
        }

        // GET: Admin/PhimLes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var phimLe = await _phimLeRepository.GetByIdAsync(id);
            if (phimLe == null)
            {
                return NotFound();
            }
            var selectedTheLoais = _context.ChiTietTheLoaiPhimLe.Where(ct => ct.PhimLeId == id).Select(ct => ct.TheLoaiId).ToList();
            var nams = await _namRepository.GetAllAsync();
            var theLoais = await _theLoaiRepository.GetAllAsync();
            var quocGias = await _quocGiaRepository.GetAllAsync();
            var trangThais = await _trangThaiRepository.GetAllAsync();
            ViewBag.SelectedTheLoais = selectedTheLoais;
            ViewBag.Nams = new SelectList(nams, "NamID", "TenNam");
            ViewBag.QuocGias = new SelectList(quocGias, "QuocGiaId", "TenQuocGia");
            ViewBag.TheLoais = theLoais;
            ViewBag.TrangThais = new SelectList(trangThais, "TrangThaiId", "TrangThaiPhim");
            return View(phimLe);
        }

        // POST: Admin/PhimLes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PhimLe phimLe, IFormFile anh, IFormFile anhnen, List<int> theLoais)
        {
            ModelState.Remove("Anh");
            ModelState.Remove("AnhNen");
            // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != phimLe.PhimLeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingPhimLe = await _phimLeRepository.GetByIdAsync(id);
                // Giả định có phương thức GetByIdAsync                                                     
                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên            
                if (anh == null)
                {
                    phimLe.Anh = existingPhimLe.Anh;
                }
                else
                {
                    // Lưu hình ảnh mới
                    phimLe.Anh = await SaveImage(anh);
                }
                if (anhnen == null)
                {
                    phimLe.AnhNen = existingPhimLe.AnhNen;
                }
                else
                {
                    // Lưu hình ảnh mới
                    phimLe.AnhNen= await SaveImage(anhnen);
                }
                // Xóa các mối quan hệ thể loại cũ
                var oldTheLoais = _context.ChiTietTheLoaiPhimLe.Where(ct => ct.PhimLeId == id).ToList();
                _context.ChiTietTheLoaiPhimLe.RemoveRange(oldTheLoais);

                // Thêm các mối quan hệ thể loại mới
                foreach (var theLoaiId in theLoais)
                {
                    var phimTheLoai = new ChiTietTheLoaiPhimLe { PhimLeId = phimLe.PhimLeId, TheLoaiId = theLoaiId };
                    _context.ChiTietTheLoaiPhimLe.Add(phimTheLoai);
                }
                // Cập nhật các thông tin khác của sản phẩm
                existingPhimLe.TenPhim = phimLe.TenPhim;
                existingPhimLe.NoiDung = phimLe.NoiDung;
                existingPhimLe.Anh = phimLe.Anh;
                existingPhimLe.AnhNen = phimLe.AnhNen;
                existingPhimLe.ThoiLuong = phimLe.ThoiLuong;
                existingPhimLe.LuotXem = phimLe.LuotXem;
                existingPhimLe.Link = phimLe.Link;
                existingPhimLe.Linkdb = phimLe.Linkdb;
                existingPhimLe.Like = phimLe.Like;

                existingPhimLe.TrangThaiId = phimLe.TrangThaiId;
                existingPhimLe.QuocGiaId = phimLe.QuocGiaId;
                existingPhimLe.NamID = phimLe.NamID;
                

                await _phimLeRepository.UpdateAsync(existingPhimLe);

                return RedirectToAction(nameof(Index));
            }
            var nams = await _namRepository.GetAllAsync();
            var theLoai = await _theLoaiRepository.GetAllAsync();
            var quocGias = await _quocGiaRepository.GetAllAsync();
            var trangThais = await _trangThaiRepository.GetAllAsync();
            ViewBag.Nams = new SelectList(nams, "NamID", "TenNam");
            ViewBag.QuocGias = new SelectList(quocGias, "QuocGiaId", "TenQuocGia");
            ViewBag.TheLoais = theLoai;
            ViewBag.TrangThais = new SelectList(trangThais, "TrangThaiId", "TrangThaiPhim");
            return View(phimLe);
        }

        // GET: Admin/PhimLes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var phimLe = await _phimLeRepository.GetByIdAsync(id);
            if (phimLe == null)
            {
                return NotFound();
            }
            return View(phimLe);
        }

        // POST: Admin/PhimLes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chitiet = _context.ChiTietTheLoaiPhimLe.Where(ct => ct.PhimLeId == id).ToList();
            _context.ChiTietTheLoaiPhimLe.RemoveRange(chitiet);
            await _phimLeRepository.DeleteAsync(id);
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
            IQueryable<PhimLe> phimLesQuery = _context.PhimLe.Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai).Where(p => p.TenPhim.Contains(query));

            var paginatedPhimLes = await PaginatedList<PhimLe>.CreateAsync(phimLesQuery, pageNumber, 10);
            // Tạo một danh sách chứa số lượng bình luận cho từng bộ phim
            var soLuongBinhLuan = new Dictionary<int, int>();
            // Lấy số lượng bình luận cho mỗi phim
            foreach (var phimLe in paginatedPhimLes)
            {
                var binhLuanCount = await _context.BinhLuan.CountAsync(b => b.PhimLeId == phimLe.PhimLeId);
                soLuongBinhLuan[phimLe.PhimLeId] = binhLuanCount;
            }
            ViewData["SoLuongBinhLuan"] = soLuongBinhLuan;
            return PartialView(paginatedPhimLes);
        }
        public async Task<IActionResult> PagingNoLibrary(int pageNumber)
        {
            int pageSize = 10;
            IQueryable<PhimLe> PhimLesQuery = _context.PhimLe.Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai); 
            var pagedPhimLes = await PhimLesQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return View(pagedPhimLes);
        }
        public List<string> SearchSuggestions(string query)
        {
            return _context.PhimLe
                .Where(p => p.TenPhim.Contains(query))
                .Select(p => p.TenPhim + "|" + p.Anh)
                .ToList();
        }
    }
}
