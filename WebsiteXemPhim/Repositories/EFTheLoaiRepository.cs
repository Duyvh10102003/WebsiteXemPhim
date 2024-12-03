using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;
using WebsiteXemPhim.Repositories;

namespace WebsiteXemPhim.Repository
{
    public class EFTheLoaiRepository : ITheLoaiRepository
    {
        private readonly ApplicationDbContext _context;

        public EFTheLoaiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TheLoai>> GetAllAsync()
        {
            return await _context.TheLoai.Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.PhimLe).Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.PhimBo).ToListAsync();
        }

        public async Task<TheLoai> GetByIdAsync(int id)
        {
            return await _context.TheLoai.Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.PhimLe).Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.PhimBo).FirstOrDefaultAsync(p => p.TheLoaiId == id);
        }
        public async Task AddAsync(TheLoai theLoai)
        {
            _context.TheLoai.Add(theLoai);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TheLoai theLoai)
        {
            _context.TheLoai.Update(theLoai);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var theLoai = await _context.TheLoai
                                        .Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(c => c.PhimBo).ThenInclude(b => b.TapPhims)
                                        .Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(c => c.PhimLe)
                                        .FirstOrDefaultAsync(c => c.TheLoaiId == id);
            if (theLoai == null)
            {
                throw new Exception("Không có thể loại cần tìm.");
            }

            // Xóa tất cả các phim bộ thuộc chủ đề này
            foreach (var phimBo in theLoai.ChiTietTheLoaiPhimBos)
            {
                var hopPhims = _context.HopPhim.Where(h => h.PhimBoId == phimBo.PhimBoId).ToList();
                _context.HopPhim.RemoveRange(hopPhims);

                // Xóa tất cả các LichSuXem thuộc phim bộ này
                var lichSuXems = _context.LichSuXem.Where(l => l.PhimBoId == phimBo.PhimBoId).ToList();
                _context.LichSuXem.RemoveRange(lichSuXems);

                // Xóa tất cả các BinhLuan thuộc phim bộ này
                var binhLuans = _context.BinhLuan.Where(b => b.PhimBoId == phimBo.PhimBoId).ToList();
                _context.BinhLuan.RemoveRange(binhLuans);

                var danhGias = _context.DanhGia.Where(a => a.PhimBoId == phimBo.PhimBoId);
                _context.DanhGia.RemoveRange(danhGias);

                var thongbaos = _context.ThongBaos.Where(a => a.PhimBoId == phimBo.PhimBoId);
                _context.ThongBaos.RemoveRange(thongbaos);

                foreach (var tapPhim in phimBo.PhimBo.TapPhims)
                {
                    _context.TapPhim.Remove(tapPhim);
                }
                _context.PhimBo.Remove(phimBo.PhimBo);
                _context.ChiTietTheLoaiPhimBo.Remove(phimBo);
            }

            // Xóa tất cả các phim lẻ thuộc chủ đề này
            foreach (var phimLe in theLoai.ChiTietTheLoaiPhimLes)
            {
                var hopPhims = _context.HopPhim.Where(h => h.PhimLeId == phimLe.PhimLeId).ToList();
                _context.HopPhim.RemoveRange(hopPhims);

                // Xóa tất cả các LichSuXem thuộc phim bộ này
                var lichSuXems = _context.LichSuXem.Where(l => l.PhimLeId == phimLe.PhimLeId).ToList();
                _context.LichSuXem.RemoveRange(lichSuXems);

                // Xóa tất cả các BinhLuan thuộc phim bộ này
                var binhLuans = _context.BinhLuan.Where(b => b.PhimLeId == phimLe.PhimLeId).ToList();
                _context.BinhLuan.RemoveRange(binhLuans);

                var danhGias = _context.DanhGia.Where(a => a.PhimLeId == phimLe.PhimLeId);
                _context.DanhGia.RemoveRange(danhGias);

                _context.PhimLe.Remove(phimLe.PhimLe);
                _context.ChiTietTheLoaiPhimLe.Remove(phimLe);
            }

            // Xóa chủ đề
            _context.TheLoai.Remove(theLoai);
            await _context.SaveChangesAsync();
        }
    }
}
