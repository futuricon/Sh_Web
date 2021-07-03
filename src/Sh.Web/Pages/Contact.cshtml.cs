using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Interfaces;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IDossierRepository _dossierRepository;
        public ContactModel(IEmailService emailService,
            IDossierRepository dossierRepository)
        {
            _emailService = emailService;
            _dossierRepository = dossierRepository;
        }

        public Domain.Entities.DossierModel.Dossier Dossier { get; set; }

        public async Task OnGetAsync()
        {
            Dossier = await _dossierRepository.GetAsync<Domain.Entities.DossierModel.Dossier>(i => i.IsAboutInfo == true);
        }

        public async Task<IActionResult> OnPostMessageAsync(string authorName, string authorEmail, string authorPhone, string msgSubject, string msgContent)
        {
            if (string.IsNullOrWhiteSpace(msgContent) || string.IsNullOrWhiteSpace(authorEmail))
            {
                return BadRequest($"Something went wrong!");
            }

            var RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            string TGMsg = $"Hi. There is new message from {authorName}.\nE-mail:   {authorEmail}\nSubject: {msgSubject}\nContent:   {msgContent}";
            await _emailService.SendToAllTGAsync(TGMsg);

            var responce = RCName switch
            {
                "ru" => "Ваше письмо успешно отправлено!",
                "uz" => "Sizning xatingiz muvaffaqiyatli yuborildi!",
                _ => "Your email has been successfully sent!",
            };
            return new OkObjectResult(responce);
        }
    }
}
