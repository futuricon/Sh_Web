using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Entities.UserModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;
using SuxrobGM.Sdk.Extensions;

namespace Sh.Web.Pages.Portfolio
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly UserManager<AppUser> _userManager;

        public CreateModel(ImageHelper imageHelper, IPortfolioRepository portfolioRepository,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            AppUser currentUser = await _userManager.GetUserAsync(User);
            Tag[] tags = Tag.ParseTags(Input.Tags);
            Input.Project.Slug = Input.Project.Title.Slugify();
            Input.Project.Author = currentUser;

            if (Input.UploadCoverPhoto != null && Input.UploadCoverPhoto.Length > 0)
            {
                Input.Project.CoverPhotoPath = await _imageHelper.UploadImageAsync(Input.UploadCoverPhoto, $"{Input.Project.Id}_project_cover", "project_imgs");
            }

            await _portfolioRepository.AddProjectAsync(Input.Project);
            await _portfolioRepository.UpdateTagsAsync(Input.Project, tags);

            return RedirectToPage("./Index", new { slug = Input.Project.Slug });
        }
    }
}
