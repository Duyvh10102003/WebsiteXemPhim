using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public class EFTrangThaiRepository : ITrangThaiRepository
    {
        private readonly ApplicationDbContext _context;

        public EFTrangThaiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrangThai>> GetAllAsync()
        {
            return await _context.TrangThai.Include(p => p.phimLes).Include(p => p.phimBos).ToListAsync();
        }

        public async Task<TrangThai> GetByIdAsync(int id)
        {
            return await _context.TrangThai.Include(p => p.phimLes).Include(p => p.phimBos).FirstOrDefaultAsync(p => p.TrangThaiId == id);
        }
        public async Task AddAsync(TrangThai trangThai)
        {
            _context.TrangThai.Add(trangThai);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrangThai trangThai)
        {
            _context.TrangThai.Update(trangThai);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trangThai = await _context.TrangThai
                                        .Include(c => c.phimBos).ThenInclude(b => b.TapPhims)
                                        .Include(c => c.phimLes)
                                        .FirstOrDefaultAsync(c => c.TrangThaiId == id);
            if (trangThai == null)
            {
                throw new Exception("Không có trạng thái cần tìm.");
            }

            // Xóa tất cả các phim bộ thuộc chủ đề này
            foreach (var phimBo in trangThai.phimBos)
            {
                var hopPhims = _context.HopPhim.Where(h => h.PhimBoId == phimBo.PhimBoId);
                var lichSuXems = _context.LichSuXem.Where(l => l.PhimBoId == phimBo.PhimBoId);
                var binhLuans = _context.BinhLuan.Where(b => b.PhimBoId == phimBo.PhimBoId);
                var chitiettheloai = _context.ChiTietTheLoaiPhimBo.Where(b => b.ChiTietTheLoaiPhimBoId == phimBo.PhimBoId);
                var danhGias = _context.DanhGia.Where(a => a.PhimBoId == phimBo.PhimBoId);
                var thongbaos = _context.ThongBaos.Where(a => a.PhimBoId == phimBo.PhimBoId);

                _context.ThongBaos.RemoveRange(thongbaos);
                _context.DanhGia.RemoveRange(danhGias);
                _context.HopPhim.RemoveRange(hopPhims);
                _context.LichSuXem.RemoveRange(lichSuXems);
                _context.BinhLuan.RemoveRange(binhLuans);
                _context.ChiTietTheLoaiPhimBo.RemoveRange(chitiettheloai);
                foreach (var tapPhim in phimBo.TapPhims)
                {
                    _context.TapPhim.Remove(tapPhim);
                }
                _context.PhimBo.Remove(phimBo);
            }

            // Xóa tất cả các phim lẻ thuộc chủ đề này
            foreach (var phimLe in trangThai.phimLes)
            {
                var hopPhims = _context.HopPhim.Where(h => h.PhimLeId == phimLe.PhimLeId);
                var lichSuXems = _context.LichSuXem.Where(l => l.PhimLeId == phimLe.PhimLeId);
                var binhLuans = _context.BinhLuan.Where(b => b.PhimLeId == phimLe.PhimLeId);
                var chitiettheloai = _context.ChiTietTheLoaiPhimLe.Where(b => b.ChiTietTheLoaiPhimLeId == phimLe.PhimLeId);
                var danhGias = _context.DanhGia.Where(a => a.PhimLeId == phimLe.PhimLeId);

                _context.DanhGia.RemoveRange(danhGias);
                _context.HopPhim.RemoveRange(hopPhims);
                _context.LichSuXem.RemoveRange(lichSuXems);
                _context.BinhLuan.RemoveRange(binhLuans);
                _context.ChiTietTheLoaiPhimLe.RemoveRange(chitiettheloai);
                _context.PhimLe.Remove(phimLe);
            }

            // Xóa chủ đề
            _context.TrangThai.Remove(trangThai);
            await _context.SaveChangesAsync();
        }
    }
}
