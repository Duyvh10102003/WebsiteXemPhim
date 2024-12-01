using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<LichSuXem> LichSuXem { get; set; }
        public DbSet<HopPhim> HopPhim { get; set; }
        public DbSet<TheLoai> TheLoai { get; set; }
        public DbSet<Nam> Nam { get; set; }
        public DbSet<QuocGia> QuocGia { get; set; }
        public DbSet<TrangThai> TrangThai { get; set; }
        public DbSet<PhimBo> PhimBo { get; set; }
        public DbSet<PhimLe> PhimLe { get; set; }
        public DbSet<TapPhim> TapPhim { get; set; }
        public DbSet<ChiTietTheLoaiPhimBo> ChiTietTheLoaiPhimBo { get; set; }
        public DbSet<ChiTietTheLoaiPhimLe> ChiTietTheLoaiPhimLe { get; set; }
        public DbSet<BinhLuan> BinhLuan { get; set; }
        public DbSet<DanhGia> DanhGia { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }


    }
}
