using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(200)]
        public string? avatar { get; set; }
        [StringLength(5)]
        public string? sex { get; set; }
    }
}
