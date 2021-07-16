using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.GalleryModel;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages.Gallery
{
    public class IndexModel : PageModel
    {
        private readonly IGalleryRepository _galleryRepository;

        public IndexModel(IGalleryRepository galleryRepository)
        {
            _galleryRepository = galleryRepository;
        }

        public List<Media> Medias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Medias = (await _galleryRepository.GetListAsync<Media>()).OrderByDescending(i=>i.PostedDate).Take(10).ToList();

            ViewData["Amount"] = Medias.Count;

            return Page();
        }

        public async Task<IActionResult> OnPostMediaAsync(int amount)
        {
            var mediaList = (await _galleryRepository.GetListAsync<Media>()).OrderByDescending(i => i.PostedDate).Skip(amount).Take(5).ToList();

            if (mediaList == null || mediaList.Count == 0)
            {
                return BadRequest();
            }

            return new JsonResult(mediaList);
        }
    }
}
