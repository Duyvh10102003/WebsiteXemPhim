namespace WebsiteXemPhim.Models
{
    public class ChiTietTheLoaiPhimBo
    {
        public int ChiTietTheLoaiPhimBoId { get; set; }
        public int? PhimBoId { get; set; }
        public PhimBo? PhimBo { get; set; }
        public int? TheLoaiId { get; set; }
        public TheLoai? TheLoai { get; set; }
    }
}
