using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;

namespace Sh.Web.Pages.Dossier
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IDossierRepository _dossierRepository;

        public CreateModel(ImageHelper imageHelper, 
            IDossierRepository dossierRepository)
        {
            _imageHelper = imageHelper;
            _dossierRepository = dossierRepository;
        }

        public class InputModel
        {
            public Domain.Entities.DossierModel.Dossier Dossier { get; set; }
            public IFormFile UploadCoverPhoto { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.UploadCoverPhoto != null && Input.UploadCoverPhoto.Length > 0)
            {
                Input.Dossier.CoverPhotoPath = await _imageHelper.UploadImageAsync(Input.UploadCoverPhoto, $"{Input.Dossier.Id}_dossier_cover", "dossier_imgs");
            }
            await _dossierRepository.AddDossierAsync(Input.Dossier);

            return RedirectToPage("/About");
        }
    }
}
