using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages.Portfolio
{
    public class IndexModel : PageModel
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public IndexModel(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public Domain.Entities.PortfolioModel.Project Project { get; set; }

        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var projects = _portfolioRepository.GetAll<Domain.Entities.PortfolioModel.Project>();
            Project = projects.Where(i => i.Slug == slug).FirstOrDefault();

            if (Project == null)
            {
                return NotFound();
            }
            if (!Request.Headers["User-Agent"].ToString().ToLower().Contains("bot"))
            {
                Project.ViewCount++;
            }
            await _portfolioRepository.UpdateProjectAsync(Project, false);

            switch (RCName.ToLower())
            {
                case "ru":
                    ViewData["ProjectTitle"] = Project.TitleRu;
                    ViewData["ProjectDesription"] = Domain.Entities.BlogModel.Blog.GetShortContent(Project.ContentRu, 250);
                    break;
                case "uz":
                    ViewData["ProjectTitle"] = Project.TitleUz;
                    ViewData["ProjectDesription"] = Domain.Entities.BlogModel.Blog.GetShortContent(Project.ContentUz, 250);
                    break;
                default:
                    ViewData["ProjectTitle"] = Project.Title;
                    ViewData["ProjectDesription"] = Domain.Entities.BlogModel.Blog.GetShortContent(Project.Content, 250);
                    break;
            }

            return Page();
        }
    }
}
