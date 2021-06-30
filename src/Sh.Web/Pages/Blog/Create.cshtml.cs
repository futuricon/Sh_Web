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

namespace Sh.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IBlogRepository _blogRepository;
        private readonly UserManager<AppUser> _userManager;

        public CreateModel(ImageHelper imageHelper, IBlogRepository blogRepository,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _imageHelper = imageHelper;
            _blogRepository = blogRepository;
        }

        public class InputModel
        {
            public Domain.Entities.BlogModel.Blog Blog { get; set; }
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
            Input.Blog.Slug = Input.Blog.Title.Slugify();
            Input.Blog.Author = currentUser;

            if (Input.UploadCoverPhoto != null && Input.UploadCoverPhoto.Length > 0)
            {
                Input.Blog.CoverPhotoPath = await _imageHelper.UploadImageAsync(Input.UploadCoverPhoto, $"{Input.Blog.Id}_blog_cover", "post_imgs");
            }

            await _blogRepository.AddBlogAsync(Input.Blog);
            await _blogRepository.UpdateTagsAsync(Input.Blog, tags);

            return RedirectToPage("./Index", new { slug = Input.Blog.Slug });
        }
    }
}
