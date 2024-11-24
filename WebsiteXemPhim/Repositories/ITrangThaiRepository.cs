using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface ITrangThaiRepository
    {
        Task<IEnumerable<TrangThai>> GetAllAsync();
        Task<TrangThai> GetByIdAsync(int id);
        Task AddAsync(TrangThai trangThai);
        Task UpdateAsync(TrangThai trangThai);
        Task DeleteAsync(int id);
    }
}
