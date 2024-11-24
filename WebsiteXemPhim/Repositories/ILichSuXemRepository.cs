using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface ILichSuXemRepository
    {
        Task<IEnumerable<LichSuXem>> GetAllAsync();
        Task<LichSuXem> GetByIdAsync(int id);
        Task AddAsync(LichSuXem lichSuXem);
        Task UpdateAsync(LichSuXem lichSuXem);
        Task DeleteAsync(int id);
    }
}
