using System.Linq;
using System.Threading.Tasks;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Entities.GalleryModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Infrastructure.Data;

namespace Sh.Infrastructure.Repositories
{
    public class GalleryRepository : Repository, IGalleryRepository
    {
        private readonly ApplicationDbContext _context;

        public GalleryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task AddMediaAsync(Media media)
        {
            return AddAsync(media);
        }

        public async Task DeleteMediaAsync(Media media)
        {
            await DeleteAsync(media);
        }

        public Task UpdateMediaAsync(Media media)
        {
            return UpdateAsync(media);
        }

        //////////////// Category ////////////////

        public async Task UpdateCategoriesAsync(Media media, params Category[] categories)
        {
            var toRemove = media.Categories.Where(l2 =>!categories.Any(l1 => l1.Name.ToLower() == l2.Name.ToLower())).ToList();
            
            foreach (var category in categories)
            {
                var originCategory = await GetAsync<Category>(i => i.Name.ToLower() == category.Name.ToLower());

                if (originCategory == null)
                {
                    originCategory = new Category(category);
                    await _context.Set<Category>().AddAsync(originCategory);
                }

                if (media.Categories.Any(i => i.Name.ToLower() == originCategory.Name.ToLower()))
                {
                    continue;
                }

                media.Categories.Add(originCategory);
            }
            foreach (var category in toRemove)
            {
                media.Categories.Remove(category);
                if (category.Medias.Count == 0)
                {
                    _context.Remove<Category>(category);
                }
            }

            await UpdateAsync(media);
        }
    }
}
