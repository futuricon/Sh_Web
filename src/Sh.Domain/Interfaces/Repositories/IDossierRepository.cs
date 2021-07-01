using Sh.Domain.Entities.DossierModel;
using System.Threading.Tasks;

namespace Sh.Domain.Interfaces.Repositories
{
    public interface IDossierRepository : IRepository
    {
        Task AddDossierAsync(Dossier dossier);
        Task UpdateDossierAsync(Dossier dossier);
        Task DeleteDossierAsync(Dossier dossier);
    }
}
