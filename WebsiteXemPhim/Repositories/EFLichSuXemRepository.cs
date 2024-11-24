using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.DataAccess;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public class EFLichSuXemRepository : ILichSuXemRepository
    {
        private readonly ApplicationDbContext _context;

        public EFLichSuXemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LichSuXem>> GetAllAsync()
        {
            return await _context.LichSuXem.ToListAsync();
        }

        public async Task<LichSuXem> GetByIdAsync(int id)
        {
            return await _context.LichSuXem.FirstOrDefaultAsync(p => p.LichSuXemId == id);
        }
        public async Task AddAsync(LichSuXem lichSuXem)
        {
            _context.LichSuXem.Add(lichSuXem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LichSuXem lichSuXem)
        {
            _context.LichSuXem.Update(lichSuXem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var lichSuXem = await _context.LichSuXem.FindAsync(id);
            _context.LichSuXem.Remove(lichSuXem);
            await _context.SaveChangesAsync();
        }
    }
}
