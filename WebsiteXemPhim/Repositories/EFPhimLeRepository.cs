using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public class EFPhimLeRepository : IPhimLeRepository
    {
        private readonly ApplicationDbContext _context;
        public EFPhimLeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PhimLe>> GetAllAsync()
        {
            return await _context.PhimLe.Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai).ToListAsync();
        }
        public async Task<PhimLe> GetByIdAsync(int id)
        {
            return await _context.PhimLe.Include(p => p.ChiTietTheLoaiPhimLes).ThenInclude(p => p.TheLoai).Include(p => p.Nam).Include(p => p.QuocGia).Include(p => p.TrangThai).FirstOrDefaultAsync(p => p.PhimLeId == id);
        }
        public async Task AddAsync(PhimLe phimLe)
        {
            _context.PhimLe.Add(phimLe);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(PhimLe phimLe)
        {
            _context.PhimLe.Update(phimLe);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var phimLe = await _context.PhimLe.FindAsync(id);
            if (phimLe == null)
            {
                throw new Exception("Không tìm thấy phim lẻ.");
            }
            var hopPhims = _context.HopPhim.Where(h => h.PhimLeId == phimLe.PhimLeId);
            var lichSuXems = _context.LichSuXem.Where(l => l.PhimLeId == phimLe.PhimLeId);
            var binhLuans = _context.BinhLuan.Where(b => b.PhimLeId == phimLe.PhimLeId);
            var danhGias = _context.DanhGia.Where(a => a.PhimLeId == phimLe.PhimLeId);
            var chitiettheloai = _context.ChiTietTheLoaiPhimLe.Where(b => b.ChiTietTheLoaiPhimLeId == phimLe.PhimLeId);
            _context.HopPhim.RemoveRange(hopPhims);
            _context.LichSuXem.RemoveRange(lichSuXems);
            _context.BinhLuan.RemoveRange(binhLuans);
            _context.DanhGia.RemoveRange(danhGias);
            _context.ChiTietTheLoaiPhimLe.RemoveRange(chitiettheloai);
            _context.PhimLe.Remove(phimLe);
            await _context.SaveChangesAsync();
        }
    }
}
