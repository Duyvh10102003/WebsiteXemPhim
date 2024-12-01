using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteXemPhim.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebsiteXemPhim.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Trang chỉnh sửa thông tin người dùng
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var model = new EditProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email, 
                avatar = user.avatar,
                sex = user.sex,
            };

            return View(model);
        }


        // Xử lý khi người dùng cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model, IFormFile avatar)
        {
            ModelState.Remove("avatar");
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });

                }
                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên            
                if (avatar == null)
                {
                    model.avatar = user.avatar;
                }
                else
                {
                    // Lưu hình ảnh mới
                    model.avatar = await SaveImage(avatar);
                }
                user.UserName = model.UserName;
                user.sex = model.sex;
                user.avatar = model.avatar;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("EditProfile", "User");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/frontend/img/avatar", image.FileName); // Thay   đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/frontend/img/avatar/" + image.FileName; // Trả về đường dẫn tương đối
        }

    }
}
