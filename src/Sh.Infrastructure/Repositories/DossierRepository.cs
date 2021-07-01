using System.Threading.Tasks;
using Sh.Domain.Entities.DossierModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Infrastructure.Data;

namespace Sh.Infrastructure.Repositories
{
    public class DossierRepository : Repository, IDossierRepository
    {
        private readonly ApplicationDbContext _context;

        public DossierRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task AddDossierAsync(Dossier testimonial)
        {
            return AddAsync(testimonial);
        }

        public async Task DeleteDossierAsync(Dossier testimonial)
        {
            await DeleteAsync(testimonial); 
        }

        public Task UpdateDossierAsync(Dossier testimonial)
        {
            return UpdateAsync(testimonial);
        }
    }
}
