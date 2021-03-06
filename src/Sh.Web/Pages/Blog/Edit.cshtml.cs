using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;
using SuxrobGM.Sdk.Extensions;

namespace Sh.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IBlogRepository _blogRepository;

        public EditModel(ImageHelper imageHelper, IBlogRepository blogRepository)
        {
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

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var blog = await _blogRepository.GetByIdAsync<Domain.Entities.BlogModel.Blog>(id);

            if (blog == null)
            {
                return NotFound();
            }

            Input = new InputModel()
            {
                Blog = blog,
                Tags = Tag.ConvertTagsToString(blog.Tags)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blog = await _blogRepository.GetByIdAsync<Domain.Entities.BlogModel.Blog>(Input.Blog.Id);
            blog.Title = Input.Blog.Title;
            blog.TitleRu = Input.Blog.TitleRu;
            blog.TitleUz = Input.Blog.TitleUz;
            blog.Content = Input.Blog.Content;
            blog.ContentRu = Input.Blog.ContentRu;
            blog.ContentUz = Input.Blog.ContentUz;
            blog.Slug = Input.Blog.Title.Slugify();
            var tags = Tag.ParseTags(Input.Tags);

            if (Input.UploadCoverPhoto != null && Input.UploadCoverPhoto.Length > 0)
            {
                _imageHelper.RemoveImage(blog.CoverPhotoPath, "post_imgs");
                blog.CoverPhotoPath = await _imageHelper.UploadImageAsync(Input.UploadCoverPhoto, $"{Input.Blog.Id}_blog_cover", "post_imgs");
            }

            await _blogRepository.UpdateTagsAsync(blog, tags);
            await _blogRepository.UpdateBlogAsync(blog);

            return RedirectToPage("./Index", new { slug = blog.Slug });
        }
    }
}
