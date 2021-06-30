using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;

namespace Sh.Web.Pages.Portfolio
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IPortfolioRepository _portfolioRepository;

        public DeleteModel(ImageHelper imageHelper, IPortfolioRepository portfolioRepository)
        {
            _imageHelper = imageHelper;
            _portfolioRepository = portfolioRepository;
        }

        [BindProperty]
        public Domain.Entities.PortfolioModel.Project Project { get; set; }

        public string Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = await _portfolioRepository.GetByIdAsync<Domain.Entities.PortfolioModel.Project>(id);
            Tags = Tag.ConvertTagsToString(Project.Tags);

            if (Project == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = await _portfolioRepository.GetByIdAsync<Domain.Entities.PortfolioModel.Project>(id);

            if (Project != null)
            {
                await _portfolioRepository.DeleteProjectAsync(Project);
                _imageHelper.RemoveImage(Project.CoverPhotoPath, "project_imgs");
            }

            return RedirectToPage("/Portfolio/List");
        }
    }
}
