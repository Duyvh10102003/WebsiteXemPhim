using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface INamRepository
    {
        Task<IEnumerable<Nam>> GetAllAsync();
        Task<Nam> GetByIdAsync(int id);
        Task AddAsync(Nam nam);
        Task UpdateAsync(Nam nam);
        Task DeleteAsync(int id);
    }
}
