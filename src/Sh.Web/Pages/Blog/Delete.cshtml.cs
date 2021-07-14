using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;

namespace Sh.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IBlogRepository _blogRepository;

        public DeleteModel(ImageHelper imageHelper, IBlogRepository blogRepository)
        {
            _imageHelper = imageHelper;
            _blogRepository = blogRepository;
        }

        [BindProperty]
        public Domain.Entities.BlogModel.Blog Blog { get; set; }

        public string Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _blogRepository.GetByIdAsync<Domain.Entities.BlogModel.Blog>(id);
            Tags = Tag.ConvertTagsToString(Blog.Tags);

            if (Blog == null)
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

            Blog = await _blogRepository.GetByIdAsync<Domain.Entities.BlogModel.Blog>(id);

            if (Blog != null)
            {
                await _blogRepository.DeleteBlogAsync(Blog);
                _imageHelper.RemoveImage(Blog.CoverPhotoPath, "post_imgs");
            }

            return RedirectToPage("./List");
        }
    }
}
