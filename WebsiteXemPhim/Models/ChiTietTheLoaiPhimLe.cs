namespace WebsiteXemPhim.Models
{
    public class ChiTietTheLoaiPhimLe
    {
        public int ChiTietTheLoaiPhimLeId { get; set; }
        public int? PhimLeId { get; set; }
        public PhimLe PhimLe { get; set; }
        public int TheLoaiId { get; set; }
        public TheLoai TheLoai { get; set; }
    }
}
