using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.GalleryModel;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages.Gallery
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ListModel : PageModel
    {
        private readonly IGalleryRepository _galleryRepository;

        public ListModel(IGalleryRepository galleryRepository)
        {
            _galleryRepository = galleryRepository;
        }

        public List<Media> Medias { get; set; }

        public async Task OnGetAsync()
        {
            Medias = (await _galleryRepository.GetListAsync<Media>()).OrderByDescending(i=>i.PostedDate).ToList();
        }
    }
}
