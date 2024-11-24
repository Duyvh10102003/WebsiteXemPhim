using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface IPhimLeRepository
    {
        Task<IEnumerable<PhimLe>> GetAllAsync();
        Task<PhimLe> GetByIdAsync(int id);
        Task AddAsync(PhimLe phimLe);
        Task UpdateAsync(PhimLe phimLe);
        Task DeleteAsync(int id);
    }
}
