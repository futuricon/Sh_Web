using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.PortfolioModel;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages.Portfolio
{
    public class ListModel : PageModel
    {
        private readonly IPortfolioRepository _portfolioRepository;
        public ListModel(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public List<Project> Projects { get; set; }
        public string RCName { get; set; }
        public async Task OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            Projects = (await _portfolioRepository.GetListAsync<Project>()).OrderByDescending(i => i.PostedDate).Take(10).ToList();

            ViewData["Amount"] = Projects.Count;
        }

        public async Task<IActionResult> OnPostProjectAsync(int amount)
        {
            var projectList = (await _portfolioRepository.GetListAsync<Project>()).OrderByDescending(i => i.PostedDate).Skip(amount).Take(5).ToList();

            if (projectList == null || projectList.Count == 0)
            {
                return BadRequest();
            }

            return new JsonResult(projectList);
        }
    }
}
