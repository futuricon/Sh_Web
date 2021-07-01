using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Interfaces.Repositories;
using Sh.Web.Pages.Utils;

namespace Sh.Web.Pages.Dossier
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IDossierRepository _dossierRepository;

        public DeleteModel(ImageHelper imageHelper, IDossierRepository dossierRepository)
        {
            _imageHelper = imageHelper;
            _dossierRepository = dossierRepository;
        }

        [BindProperty]
        public Domain.Entities.DossierModel.Dossier Dossier { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dossier = await _dossierRepository.GetByIdAsync<Domain.Entities.DossierModel.Dossier>(id);

            if (Dossier == null)
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

            Dossier = await _dossierRepository.GetByIdAsync<Domain.Entities.DossierModel.Dossier>(id);

            if (Dossier != null)
            {
                await _dossierRepository.DeleteDossierAsync(Dossier);
                _imageHelper.RemoveImage(Dossier.CoverPhotoPath, "dossier_imgs");
            }
            return RedirectToPage("/About");
        }
    }
}
