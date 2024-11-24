using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class QuocGia
    {
        public int QuocGiaId { get; set; }
        [StringLength(57)]
        public string TenQuocGia {  get; set; }
        public List<PhimBo>? phimBos { get; set; }
        public List<PhimLe>? phimLes { get; set; }
    }
}
