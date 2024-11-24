using Microsoft.AspNetCore.Identity;

namespace WebsiteXemPhim.Models
{
    public class LichSuXem
    {
        public int LichSuXemId { get; set; }
        public int? PhimBoId { get; set; }
        public int? PhimLeId { get; set; }
        public string UserId { get; set; }
        public PhimBo? PhimBo { get; set; }
        public PhimLe? PhimLe { get; set; }
        public IdentityUser User { get; set; }
    }
}
