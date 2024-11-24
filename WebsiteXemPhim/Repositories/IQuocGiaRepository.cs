using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Repositories
{
    public interface IQuocGiaRepository
    {
        Task<IEnumerable<QuocGia>> GetAllAsync();
        Task<QuocGia> GetByIdAsync(int id);
        Task AddAsync(QuocGia quocGia);
        Task UpdateAsync(QuocGia quocGia);
        Task DeleteAsync(int id);
    }
}
