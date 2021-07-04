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
    public class EditModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly PDFFileHelper _pDFFileHelper;
        private readonly IDossierRepository _dossiersRepository;

        public EditModel(PDFFileHelper pDFFileHelper, ImageHelper imageHelper, 
            IDossierRepository dossiersRepository)
        {
            _pDFFileHelper = pDFFileHelper;
            _imageHelper = imageHelper;
            _dossiersRepository = dossiersRepository;
        }

        public class InputModel
        {
            public Domain.Entities.DossierModel.Dossier Dossier { get; set; }
            public IFormFile UploadCoverPhoto { get; set; }
            public IFormFile UploadCVFile { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var dossier = await _dossiersRepository.GetByIdAsync<Domain.Entities.DossierModel.Dossier>(id);
            if (dossier == null)
            {
                return NotFound();
            }

            Input = new InputModel()
            {
                Dossier = dossier,
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dossier = await _dossiersRepository.GetByIdAsync<Domain.Entities.DossierModel.Dossier>(Input.Dossier.Id);
            dossier.FirstName = Input.Dossier.FirstName;
            dossier.LastName = Input.Dossier.LastName;
            dossier.FirstNameRu = Input.Dossier.FirstNameRu;
            dossier.LastNameRu = Input.Dossier.LastNameRu;
            dossier.FirstNameUz = Input.Dossier.FirstNameUz;
            dossier.LastNameUz = Input.Dossier.LastNameUz;
            dossier.Position = Input.Dossier.Position;
            dossier.PositionRu = Input.Dossier.PositionRu;
            dossier.PositionUz = Input.Dossier.PositionUz;
            dossier.Description = Input.Dossier.Description;
            dossier.DescriptionRu = Input.Dossier.DescriptionRu;
            dossier.DescriptionUz = Input.Dossier.DescriptionUz;

            if (Input.UploadCoverPhoto != null && Input.UploadCoverPhoto.Length > 0)
            {
                _imageHelper.RemoveImage(dossier.CoverPhotoPath, "dossier_imgs");
                dossier.CoverPhotoPath = await _imageHelper.UploadImageAsync(Input.UploadCoverPhoto, $"{Input.Dossier.Id}_dossier_cover", "dossier_imgs");
            }

            if (dossier.IsAboutInfo)
            {
                dossier.Address = Input.Dossier.Address;
                dossier.PhoneNumber = Input.Dossier.PhoneNumber;
                dossier.Email = Input.Dossier.Email;
                if (Input.UploadCVFile != null && Input.UploadCVFile.Length > 0)
                {
                    _pDFFileHelper.DeleteFile(dossier.CVFilePath, "CVFile");
                    await _pDFFileHelper.SaveFile(Input.UploadCVFile, $"{Input.Dossier.Id}_dossier_file", "CVFile");
                }
            }
            await _dossiersRepository.UpdateDossierAsync(dossier);

            return RedirectToPage("/About");
        }
    }
}
