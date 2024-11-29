using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải bao gồm đúng 10 chữ số.")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

    }

}
