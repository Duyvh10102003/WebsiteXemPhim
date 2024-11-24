using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public class EFHopPhimRepository : IHopPhimRepository
    {
        private readonly ApplicationDbContext _context;

        public EFHopPhimRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HopPhim>> GetAllAsync()
        {
            return await _context.HopPhim.ToListAsync();
        }

        public async Task<HopPhim> GetByIdAsync(int id)
        {
            return await _context.HopPhim.FirstOrDefaultAsync(p => p.HopPhimId == id);
        }
        public async Task AddAsync(HopPhim hopPhim)
        {
            _context.HopPhim.Add(hopPhim);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HopPhim hopPhim)
        {
            _context.HopPhim.Update(hopPhim);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hopPhim = await _context.HopPhim.FindAsync(id);
            _context.HopPhim.Remove(hopPhim);
            await _context.SaveChangesAsync();
        }
    }
}
