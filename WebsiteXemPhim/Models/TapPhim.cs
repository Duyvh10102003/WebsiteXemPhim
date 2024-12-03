using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class TapPhim
    {
        public int TapPhimId { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "TapPhim không phải là số không âm.")]
        public int Tap { get; set; }
        [StringLength(200)]
        public string Link { get; set; }
        [StringLength(10)]
        public string? ThoiLuong { get; set; }
        public int? PhimBoId { get; set; }
        public PhimBo? PhimBo { get; set; }

    }
}
