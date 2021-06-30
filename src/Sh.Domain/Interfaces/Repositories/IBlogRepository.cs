using Sh.Domain.Entities.BlogModel;
using System.Threading.Tasks;

namespace Sh.Domain.Interfaces.Repositories
{
    public interface IBlogRepository : IRepository
    {
        Task AddBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog, bool verifySlug = true);
        Task DeleteBlogAsync(Blog blog);
        Task UpdateTagsAsync(Blog blog, params Tag[] tags);
    }
}
