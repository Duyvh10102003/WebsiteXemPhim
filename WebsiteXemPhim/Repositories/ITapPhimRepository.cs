using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface ITapPhimRepository
    {
        Task<IEnumerable<TapPhim>> GetAllAsync();
        Task<TapPhim> GetByIdAsync(int id);
        Task<IEnumerable<TapPhim>> GetByPhimBoIdAsync(int id);
        Task AddAsync(TapPhim tapPhim);
        Task UpdateAsync(TapPhim tapPhim);
        Task DeleteAsync(int id);
    }
}
