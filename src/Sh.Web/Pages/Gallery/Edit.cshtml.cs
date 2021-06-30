using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.GalleryModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;

namespace Sh.Web.Pages.Gallery
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IGalleryRepository _galleryRepository;

        public EditModel(ImageHelper imageHelper, IGalleryRepository galleryRepository)
        {
            _imageHelper = imageHelper;
            _galleryRepository = galleryRepository;
        }

        public class InputModel
        {
            public Media Media { get; set; }
            public IFormFile UploadPhoto { get; set; }
            public string Categories { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var media = await _galleryRepository.GetByIdAsync<Media>(id);

            Input = new InputModel()
            {
                Media = media,
                Categories = Category.ConvertCategoriesToString(media.Categories,',')
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var media = await _galleryRepository.GetByIdAsync<Media>(Input.Media.Id);
            var categories = Category.ParseCategories(Input.Categories);

            if (Input.UploadPhoto != null)
            {
                _imageHelper.RemoveImage(media.MediaPath, "gallery_imgs");
                media.MediaPath = await _imageHelper.UploadImageAsync(Input.UploadPhoto, $"{Input.Media.Id}_media", "gallery_imgs");
                media.MediaType = MediaType.Image;
            }
            else if (!String.IsNullOrWhiteSpace(Input.Media.MediaPath))
            {
                if (Input.Media.MediaPath.Contains("https://youtu.be/")) // YouTube Video
                {
                    _imageHelper.RemoveImage(media.MediaPath, "gallery_imgs");
                    media.MediaPath = Input.Media.MediaPath.Split('/').TakeLast(1).FirstOrDefault();
                    media.MediaType = MediaType.Video;
                }
                else if (Input.Media.MediaPath.Contains("https://live.staticflickr.com/")) // Flickr Image
                {
                    _imageHelper.RemoveImage(media.MediaPath, "gallery_imgs");
                    var halfString = Input.Media.MediaPath.Split("https://live.staticflickr.com/").TakeLast(1).FirstOrDefault();
                    media.MediaPath = "https://live.staticflickr.com/" + halfString.Split("\"").FirstOrDefault();
                    media.MediaType = MediaType.Image;
                }
                else
                {
                    media.MediaPath = Input.Media.MediaPath;
                    media.MediaType = MediaType.Image;
                }
            }
            else
            {
                return Page();
            }

            await _galleryRepository.UpdateCategoriesAsync(media, categories);
            return RedirectToPage("/Gallery/List");
        }
    }
}
