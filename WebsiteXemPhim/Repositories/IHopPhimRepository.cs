using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface IHopPhimRepository
    {
        Task<IEnumerable<HopPhim>> GetAllAsync();
        Task<HopPhim> GetByIdAsync(int id);
        Task AddAsync(HopPhim hopPhim);
        Task UpdateAsync(HopPhim hopPhim);
        Task DeleteAsync(int id);
    }
}
