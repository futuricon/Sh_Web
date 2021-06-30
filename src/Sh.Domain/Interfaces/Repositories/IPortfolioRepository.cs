using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Entities.PortfolioModel;
using System.Threading.Tasks;

namespace Sh.Domain.Interfaces.Repositories
{
    public interface IPortfolioRepository : IRepository
    {
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Project project);
        Task UpdateTagsAsync(Project project, params Tag[] tags);
    }
}
