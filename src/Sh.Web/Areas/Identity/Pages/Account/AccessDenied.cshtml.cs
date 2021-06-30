using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sh.Web.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {
            ViewData["PageName"] = "AccessDeniedPage";
        }
    }
}

