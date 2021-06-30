using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.GalleryModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;

namespace Sh.Web.Pages.Gallery
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IGalleryRepository _galleryRepository;

        public DeleteModel(ImageHelper imageHelper, IGalleryRepository galleryRepository)
        {
            _imageHelper = imageHelper;
            _galleryRepository = galleryRepository;
        }

        [BindProperty]
        public Media Media { get; set; }

        public string Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Media = await _galleryRepository.GetByIdAsync<Media>(id);
            Categories = Category.ConvertCategoriesToString(Media.Categories);

            if (Media == null)
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

            Media = await _galleryRepository.GetByIdAsync<Media>(id);

            if (Media != null)
            {
                await _galleryRepository.DeleteMediaAsync(Media);
                _imageHelper.RemoveImage(Media.MediaPath, "gallery_imgs");
            }

            return RedirectToPage("/Gallery/List");
        }
    }
}
