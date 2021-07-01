using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages
{
    public class AboutModel : PageModel
    {
        private readonly IDossierRepository _dossiersRepository;
        public AboutModel(IDossierRepository dossiersRepository)
        {
            _dossiersRepository = dossiersRepository;
        }

        public List<Domain.Entities.DossierModel.Dossier> Dossiers { get; set; }
        public Domain.Entities.DossierModel.Dossier AboutInfo { get; set; }
        public string RCName { get; set; }

        public async Task OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            Dossiers = await _dossiersRepository.GetListAsync<Domain.Entities.DossierModel.Dossier>(i => i.IsAboutInfo == false);
            AboutInfo = await _dossiersRepository.GetAsync<Domain.Entities.DossierModel.Dossier>(i => i.IsAboutInfo == true);
        }
    }
}
