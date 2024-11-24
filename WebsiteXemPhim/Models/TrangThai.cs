using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class TrangThai
    {
        public int TrangThaiId { get; set; }
        [Required, StringLength(20)]
        public string TrangThaiPhim { get; set; }
        public List<PhimBo>? phimBos { get; set; }
        public List<PhimLe>? phimLes { get; set; }
    }
}
