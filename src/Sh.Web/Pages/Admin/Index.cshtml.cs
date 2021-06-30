using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.UserModel;

namespace Sh.Web.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public List<AppUser> Users { get; set; }

        public void OnGet()
        {
           Users = _userManager.Users.ToList();
        }
    }
}
