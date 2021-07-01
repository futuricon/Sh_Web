using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace Sh.Web.Pages.Utils
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _env;
        public const string DefaultCoverPhotoPath = "/img/blog_image.jpg";
        public const string DefaultTestiPhotoPath = "/img/profile_image.jpg";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
        }
                
        public void DeleteFile(string filePath)
        {
            var absolutePath = Path.Combine(_env.WebRootPath, filePath);
            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }

        public async Task<string> UploadImageAsync(IFormFile image, string imageFileName, string subFolder)
        {
            try
            {
                var fileExtension = Path.GetExtension(image.FileName);
                var imagePath = $"{imageFileName}{fileExtension}";
                var absolutePath = Path.Combine(_env.WebRootPath, "img", subFolder, imagePath);

                using (Stream fileStream = new FileStream(absolutePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return $"/img/{subFolder}/{imagePath}";
            }
            catch (Exception)
            {
                if (subFolder == "dossier_imgs")
                {
                    return DefaultTestiPhotoPath;
                }
                return DefaultCoverPhotoPath;
            }
        }

        public void RemoveImage(string imgPath, string subFolder)
        {
            if (imgPath == DefaultCoverPhotoPath)
            {
                return;
            }

            var imgFileName = Path.GetFileName(imgPath);
            var imgFullPath = Path.Combine(_env.WebRootPath, "img", subFolder, imgFileName);

            if (File.Exists(imgFullPath))
            {
                File.Delete(imgFullPath);
            }
        }


        public async Task<bool> GetExtensionsAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var img = Image.FromStream(memoryStream))
                {
                    if (img.Height > img.Width)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}

