using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface IPhimBoRepository
    {
        Task<IEnumerable<PhimBo>> GetAllAsync();
        Task<PhimBo> GetByIdAsync(int id);
        Task AddAsync(PhimBo phimBo);
        Task UpdateAsync(PhimBo phimBo);
        Task DeleteAsync(int id);
    }
}
