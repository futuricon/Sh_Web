using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sh.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDossierRepository _dossierRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IBlogRepository _blogRepository;

        public IndexModel(IDossierRepository dossierRepository,
            IPortfolioRepository portfolioRepository, IBlogRepository blogRepository)
        {
            _dossierRepository = dossierRepository;
            _portfolioRepository = portfolioRepository;
            _blogRepository = blogRepository;
        }

        public Domain.Entities.DossierModel.Dossier AboutInfo { get; set; }
        public List<Domain.Entities.DossierModel.Dossier> Dossiers { get; set; }
        public List<Domain.Entities.PortfolioModel.Project> Projects { get; set; }
        public List<Domain.Entities.BlogModel.Blog> Blogs { get; set; }
        public string RCName { get; set; }

        public async Task OnGetAsync()
        {
            RCName = HttpContext.Features.
                Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var dossiers = await _dossierRepository.
                GetListAsync<Domain.Entities.DossierModel.Dossier>();
            AboutInfo = dossiers.
                Where(i => i.IsAboutInfo == true).
                FirstOrDefault();
            Dossiers = dossiers.
                Where(i => i.IsAboutInfo == false).
                ToList();
            Projects = (await _portfolioRepository.
                GetListAsync<Domain.Entities.PortfolioModel.Project>()).
                OrderByDescending(i => i.PostedDate).
                Take(4).ToList();
            Blogs = (await _blogRepository.
                GetListAsync<Domain.Entities.BlogModel.Blog>()).
                OrderByDescending(i => i.PostedDate).
                Take(3).ToList();

            ViewData["ShortDescription"] = RCName.ToLower() switch
            {
                "ru" => Domain.Entities.BlogModel.Blog.GetShortContent(AboutInfo.DescriptionRu, 250),
                "uz" => Domain.Entities.BlogModel.Blog.GetShortContent(AboutInfo.DescriptionUz, 250),
                _ => Domain.Entities.BlogModel.Blog.GetShortContent(AboutInfo.Description, 250),
            };

            ViewData["PageName"] = "HomePage";
        }
    }
}
