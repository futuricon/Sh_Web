using System.Threading.Tasks;
using Sh.Domain.Entities.GalleryModel;

namespace Sh.Domain.Interfaces.Repositories
{
    public interface IGalleryRepository : IRepository
    {
        Task AddMediaAsync(Media media);
        Task UpdateMediaAsync(Media media);
        Task DeleteMediaAsync(Media media);
        Task UpdateCategoriesAsync(Media media, params Category[] categories);
    }
}
