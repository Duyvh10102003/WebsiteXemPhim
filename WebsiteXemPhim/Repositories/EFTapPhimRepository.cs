using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public class EFTapPhimRepository : ITapPhimRepository
    {
        private readonly ApplicationDbContext _context;
        public EFTapPhimRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TapPhim>> GetAllAsync()
        {
            return await _context.TapPhim.Include(p => p.PhimBo).ToListAsync();
        }

        public async Task<TapPhim> GetByIdAsync(int id)
        {
            return await _context.TapPhim.Include(p => p.PhimBo).FirstOrDefaultAsync(p => p.TapPhimId == id);
        }
        public async Task<IEnumerable<TapPhim>> GetByPhimBoIdAsync(int phimId)
        {
            List<TapPhim> tapPhims = await _context.TapPhim.Include(p => p.PhimBo).Where(p => p.TapPhimId == phimId).ToListAsync();
            return tapPhims;
        }
        public async Task AddAsync(TapPhim tapPhim)
        {
            _context.TapPhim.Add(tapPhim);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TapPhim tapPhim)
        {
            _context.TapPhim.Update(tapPhim);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tapPhim = await _context.TapPhim.FindAsync(id);
            _context.TapPhim.Remove(tapPhim);
            await _context.SaveChangesAsync();
        }
    }
}
