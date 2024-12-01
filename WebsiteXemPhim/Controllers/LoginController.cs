using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebsiteXemPhim.Models;

namespace WebsiteXemPhim.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Login");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result == null || result.Principal == null)
            {
                return BadRequest("Authentication failed or user not authenticated.");
            }

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);

            // Tìm người dùng trong cơ sở dữ liệu
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Nếu người dùng chưa tồn tại, tạo mới
                user = new AppUser { UserName = email, Email = email };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    // Đăng nhập người dùng
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                {
                    return BadRequest("User creation failed.");
                }
            }
            else
            {
                // Nếu người dùng đã tồn tại, đăng nhập
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            // Trả về view với thông tin người dùng
            var userInfo = new UserInfo
            {
                Email = email,
                Name = name
            };

            return View("UserInfo", userInfo); // Trả về view với thông tin người dùng
        }

        public class UserInfo
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }
    }
}
