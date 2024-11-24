using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class TheLoai
    {
        public int TheLoaiId { get; set; }
        [StringLength(25)]
        public string TenTheLoai {  get; set; }
        public List<ChiTietTheLoaiPhimBo>? ChiTietTheLoaiPhimBos { get; set; }
        public List<ChiTietTheLoaiPhimLe>? ChiTietTheLoaiPhimLes { get; set; }

    }
}
