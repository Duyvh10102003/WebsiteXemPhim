using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface ITheLoaiRepository
    {
        Task<IEnumerable<TheLoai>> GetAllAsync();
        Task<TheLoai> GetByIdAsync(int id);
        Task AddAsync(TheLoai TheLoai);
        Task UpdateAsync(TheLoai TheLoai);
        Task DeleteAsync(int id);
    }
}
