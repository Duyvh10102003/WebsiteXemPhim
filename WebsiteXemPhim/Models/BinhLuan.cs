using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class BinhLuan
    {
        public int BinhLuanId { get; set; }
        public int? PhimBoId { get; set; }
        public int? PhimLeId { get; set; }
        public string UserId { get; set; }
        [StringLength(500)] 
        public string NoiDungBinhLuan { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public PhimBo? PhimBo { get; set; }
        public PhimLe? PhimLe { get; set; }
        public AppUser User { get; set; }
    }
}
