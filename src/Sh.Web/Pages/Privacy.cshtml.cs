using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sh.Web.Pages
{
    public class PrivacyModel : PageModel
    {
        public PrivacyModel()
        {
        }

        public void OnGet()
        {
            ViewData["PageName"] = "PrivacyPage";
        }
    }
}
