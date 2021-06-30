using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.GalleryModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Sh.Web.Pages.Gallery
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IGalleryRepository _galleryRepository;

        public CreateModel(ImageHelper imageHelper, IGalleryRepository galleryRepository)
        {
            _imageHelper = imageHelper;
            _galleryRepository = galleryRepository;
        }

        public class InputModel
        {
            public Media Media { get; set; }
            public IFormFile UploadPhoto { get; set; }
            [Required]
            public string Categories { get; set; }
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

            Category[] categories = Category.ParseCategories(Input.Categories);

            if (Input.UploadPhoto != null && Input.UploadPhoto.Length > 0)
            {
                Input.Media.MediaPath = await _imageHelper.UploadImageAsync(Input.UploadPhoto, $"{Input.Media.Id}_media", "gallery_imgs");
                Input.Media.MediaType = MediaType.Image;
            }
            else if (!String.IsNullOrWhiteSpace(Input.Media.MediaPath))
            {
                if (Input.Media.MediaPath.Contains("https://youtu.be/")) // YouTube Video
                {
                    Input.Media.MediaPath = Input.Media.MediaPath.Split('/').TakeLast(1).FirstOrDefault();
                    Input.Media.MediaType = MediaType.Video;
                }
                else if (Input.Media.MediaPath.Contains("https://live.staticflickr.com/")) // Flickr Image
                {
                    var halfString = Input.Media.MediaPath.Split("https://live.staticflickr.com/").TakeLast(1).FirstOrDefault();
                    Input.Media.MediaPath = "https://live.staticflickr.com/" + halfString.Split("\"").FirstOrDefault();
                    Input.Media.MediaType = MediaType.Image;
                }
                else
                {
                    Input.Media.MediaType = MediaType.Image;
                }
            }
            else
            {
                return Page();
            }

            await _galleryRepository.AddMediaAsync(Input.Media);
            await _galleryRepository.UpdateCategoriesAsync(Input.Media, categories);

            return RedirectToPage("/Gallery/Index");
        }
    }
}
