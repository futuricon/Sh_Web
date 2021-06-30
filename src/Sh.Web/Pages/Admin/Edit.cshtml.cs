using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.UserModel;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public class EditModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public EditModel(UserManager<AppUser> userManager,
            RoleManager<UserRole> roleManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        [BindProperty]
        public AppUser AppUser { get; set; }

        [BindProperty]
        public bool IsAdmin { get; set; }

        public UserManager<AppUser> UserManager => _userManager;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser = await UserManager.FindByIdAsync(id);

            if (AppUser == null)
            {
                return NotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(AppUser);
            if (userRoles.Contains(Role.Admin.ToString()))
            {
                IsAdmin = true;
            }
            else
            {
                IsAdmin = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await UserManager.FindByIdAsync(AppUser.Id);

            if (user == null)
            {
                return NotFound();
            }

            var UserRolesName = new List<string>();

            user.UserName = AppUser.UserName;
            user.Email = AppUser.Email;
            user.IsBlocked = AppUser.IsBlocked;

            if (user.IsBlocked)
            {
                var endDate = DateTime.Now.AddMinutes(5);
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, endDate);
                await _userManager.UpdateSecurityStampAsync(user);
            }
            if (IsAdmin)
            {
                UserRolesName.Add("Admin");
            }

            await _userRepository.UpdateUserRolesAsync(user, UserRolesName);
            await UserManager.UpdateAsync(user);

            return RedirectToPage("./Details", new { id = AppUser.Id });
        }
    }
}
