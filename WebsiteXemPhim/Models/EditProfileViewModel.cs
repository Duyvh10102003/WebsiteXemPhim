using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        public string UserName { get; set; }
        public string Email { get; set; }

        [StringLength(200)]
        public string? avatar { get; set; }
        [StringLength(5)]
        public string? sex { get; set; }
        public bool IsGoogleLogin { get; set; }
    }

}
