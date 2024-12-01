// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Nhập lại mật khẩu")]
            [Compare("Password", ErrorMessage = "Password phải giống nhau.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("Mã token không có.");
            }
            else
            {
                Input = new InputModel
                {
                    // Giải mã lại code từ code trong url (do mã này khi gửi mail
                    // đã thực hiện Encode bằng WebEncoders.Base64UrlEncode)
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Tìm User theo email
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Không thấy user
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            // Kiểm tra nếu mật khẩu mới trùng với mật khẩu hiện tại
            var passwordHasher = _userManager.PasswordHasher;
            var currentPasswordHash = user.PasswordHash;
            if (passwordHasher.VerifyHashedPassword(user, currentPasswordHash, Input.Password) == PasswordVerificationResult.Success)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu mới không được trùng với mật khẩu hiện tại.");
                return Page();
            }

            // Đặt lại mật khẩu cho user - có kiểm tra mã token khi đổi
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

            if (result.Succeeded)
            {
                // Chuyển đến trang thông báo đã reset thành công
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

    }
}
