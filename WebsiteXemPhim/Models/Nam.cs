using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class Nam
    {
        public int NamID { get; set; }
        [StringLength(5)]
        public string TenNam { get; set; }
        public List<PhimBo>? phimBos { get; set; }
        public List<PhimLe>? phimLes { get; set; }
    }
}
