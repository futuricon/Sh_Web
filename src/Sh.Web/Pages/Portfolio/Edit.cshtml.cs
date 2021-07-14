using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;
using SuxrobGM.Sdk.Extensions;

namespace Sh.Web.Pages.Portfolio
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IPortfolioRepository _portfolioRepository;

        public EditModel(ImageHelper imageHelper, IPortfolioRepository portfolioRepository)
        {
            _imageHelper = imageHelper;
            _portfolioRepository = portfolioRepository;
        }

        public class InputModel
        {
            public Domain.Entities.PortfolioModel.Project Project { get; set; }
            public IFormFile UploadCoverPhoto { get; set; }
            public string Tags { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var project = await _portfolioRepository.GetByIdAsync<Domain.Entities.PortfolioModel.Project>(id);

            if (project == null)
            {
                return NotFound();
            }

            Input = new InputModel()
            {
                Project = project,
                Tags = Tag.ConvertTagsToString(project.Tags)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var project = await _portfolioRepository.GetByIdAsync<Domain.Entities.PortfolioModel.Project>(Input.Project.Id);
            project.Title = Input.Project.Title;
            project.TitleRu = Input.Project.TitleRu;
            project.TitleUz = Input.Project.TitleUz;
            project.Content = Input.Project.Content;
            project.ContentRu = Input.Project.ContentRu;
            project.ContentUz = Input.Project.ContentUz;
            project.Slug = Input.Project.Title.Slugify();
            var tags = Tag.ParseTags(Input.Tags);

            if (Input.UploadCoverPhoto != null && Input.UploadCoverPhoto.Length > 0)
            {
                _imageHelper.RemoveImage(project.CoverPhotoPath, "project_imgs");
                project.CoverPhotoPath = await _imageHelper.UploadImageAsync(Input.UploadCoverPhoto, $"{Input.Project.Id}_project_cover", "project_imgs");
            }

            await _portfolioRepository.UpdateTagsAsync(project, tags);
            await _portfolioRepository.UpdateProjectAsync(project);

            return RedirectToPage("./Index", new { slug = project.Slug });
        }
    }
}
