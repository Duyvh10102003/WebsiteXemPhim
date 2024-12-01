using WebsiteXemPhim.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebsiteXemPhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        //----------------------Lock/Unlock User--------------------

        [HttpGet]
        public async Task<IActionResult> LockUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Set LockoutEnd to a future date, which effectively locks the user
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddYears(100)); // Lock user for a long time

            if (result.Succeeded)
            {
                return RedirectToAction("Index"); // Hoặc trang quản lý users
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UnlockUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Set LockoutEnd to null, which unlocks the user
            var result = await _userManager.SetLockoutEndDateAsync(user, null);

            if (result.Succeeded)
            {
                return RedirectToAction("Index"); // Quay lại trang quản lý users
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Index");
        }

        //----------------------Role--------------------

        public async Task<IActionResult> Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Role name is required.");
                return View();
            }

            var role = new IdentityRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required.");
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                CurrentName = role.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            // Kiểm tra nếu tên mới đã tồn tại
            var existingRole = await _roleManager.FindByNameAsync(model.NewName);
            if (existingRole != null && existingRole.Id != role.Id)
            {
                ModelState.AddModelError("", "Role name already exists.");
                return View(model);
            }

            // Cập nhật tên role
            role.Name = model.NewName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public class EditRoleViewModel
        {
            public string Id { get; set; } // Role ID
            [Required]
            [Display(Name = "Current Role Name")]
            public string CurrentName { get; set; }

            [Required]
            [Display(Name = "New Role Name")]
            public string NewName { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required.");
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var model = new DeleteRoleViewModel
            {
                RoleName = role.Name
            };

            return View(model);
        }

        public class DeleteRoleViewModel
        {
            public string RoleName { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(DeleteRoleViewModel model)
        {
            if (string.IsNullOrEmpty(model.RoleName))
            {
                return BadRequest("Role name is required.");
            }

            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            // Xóa role
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Roles"); // Quay lại trang quản lý roles
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        //----------------------Role User--------------------


        [HttpGet]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                //AssignedRoles = userRoles,
                SelectedRole = userRoles.FirstOrDefault(),
                AvailableRoles = roles.Select(r => r.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy danh sách role hiện tại của user
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Xóa các role không còn được chọn
            //var rolesToRemove = currentRoles.Except(model.SelectedRoles);
            //var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            
            // Xóa tất cả các role cũ
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Error removing roles.");
                return View("ManageRoles", model);
            }

            // Thêm các role mới được chọn
            //var rolesToAdd = model.SelectedRoles.Except(currentRoles);
            //var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);

            //if (!addResult.Succeeded)
            //{
            //    ModelState.AddModelError("", "Error adding roles.");
            //    return View("ManageRoles", model);
            //}

            // Gán role mới nếu người dùng đã chọn
            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                var addResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Error assigning new role.");
                    return View("ManageRoles", model);
                }
            }

            return RedirectToAction("Index");
        }

        public class ManageUserRolesViewModel
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            //public IList<string> AssignedRoles { get; set; } = new List<string>();
            //public IList<string> SelectedRoles { get; set; } = new List<string>();
            public string? SelectedRole { get; set; }
            public IList<string> AvailableRoles { get; set; } = new List<string>();
        }

    }
}
