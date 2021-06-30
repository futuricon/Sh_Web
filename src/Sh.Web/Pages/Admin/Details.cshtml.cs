using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sh.Domain.Entities.UserModel;

namespace Sh.Web.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
    
        public DetailsModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
    
        public AppUser AppUser { get; set; }
        public bool IsAdmin { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
    
            AppUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
    
            if (AppUser == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(AppUser);

            if (userRoles.Contains(Role.Admin.ToString()))
            {
                IsAdmin = true;
            }

            return Page();
        }
    }
}
