using Microsoft.AspNetCore.Identity;

namespace WebsiteXemPhim.Models
{
    public class HopPhim
    {
        public int HopPhimId {  get; set; }
        public int? PhimBoId { get; set; }
        public int? PhimLeId { get; set; }
        public string UserId { get; set; }
        public PhimBo? PhimBo { get; set; }
        public PhimLe? PhimLe { get; set; }
        public IdentityUser User { get; set; }
    }
    public class HopPhimViewModel
    {
        public int HopPhimId { get; set; }
        public int? PhimBoId { get; set; }
        public int? PhimLeId { get; set; }
        public string UserId { get; set; }
        public string TenPhim { get; set; } // Tên của PhimBo hoặc PhimLe
        public string LoaiPhim { get; set; } // "PhimBo" hoặc "PhimLe"
        public string Anh { get; set; } // Đường dẫn ảnh
        public string TrangThai { get; set; } // Trạng thái phim
    }
}
