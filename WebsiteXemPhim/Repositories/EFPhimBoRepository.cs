using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public class EFPhimBoRepository : IPhimBoRepository
    {
        private readonly ApplicationDbContext _context;
        public EFPhimBoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PhimBo>> GetAllAsync()
        {
            return await _context.PhimBo.Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai).ToListAsync();
        }
        public async Task<PhimBo> GetByIdAsync(int id)
        {
            return await _context.PhimBo.Include(p => p.ChiTietTheLoaiPhimBos).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai).FirstOrDefaultAsync(p => p.PhimBoId == id);
        }
        public async Task AddAsync(PhimBo phimBo)
        {
            _context.PhimBo.Add(phimBo);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(PhimBo phimBo)
        {
            _context.PhimBo.Update(phimBo);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var phimBo = await _context.PhimBo.Include(c => c.TapPhims).FirstOrDefaultAsync(c => c.PhimBoId == id);
            if (phimBo == null)
            {
                throw new Exception("Không có phim cần tìm.");
            }

            foreach (var tapPhim in phimBo.TapPhims)
            {
                var hopPhims = _context.HopPhim.Where(h => h.PhimBoId == phimBo.PhimBoId);
                var lichSuXems = _context.LichSuXem.Where(l => l.PhimBoId == phimBo.PhimBoId);
                var binhLuans = _context.BinhLuan.Where(b => b.PhimBoId == phimBo.PhimBoId);
                var danhGias = _context.DanhGia.Where(a => a.PhimBoId == phimBo.PhimBoId);
                
                _context.HopPhim.RemoveRange(hopPhims);
                _context.LichSuXem.RemoveRange(lichSuXems);
                _context.BinhLuan.RemoveRange(binhLuans);
                _context.TapPhim.Remove(tapPhim);
                _context.DanhGia.RemoveRange(danhGias);
            }
            // Xóa chủ đề
            _context.PhimBo.Remove(phimBo);
            await _context.SaveChangesAsync();
        }
    }
}
