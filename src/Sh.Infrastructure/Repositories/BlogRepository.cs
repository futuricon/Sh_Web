using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sh.Infrastructure.Repositories
{
    public class BlogRepository : Repository, IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //////////////// Blog ////////////////
        public Task AddBlogAsync(Blog blog)
        {
            blog.Slug = GetVerifiedBlogSlug(blog);
            return AddAsync(blog);
        }

        public Task UpdateBlogAsync(Blog blog, bool verifySlug = true)
        {
            if (verifySlug)
            {
                blog.Slug = GetVerifiedBlogSlug(blog);
            }
            return UpdateAsync(blog);
        }

        public async Task DeleteBlogAsync(Blog blog)
        {
            await DeleteAsync(blog);
        }

        private string GetVerifiedBlogSlug(Blog slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var hasSameSlug = _context.Set<Blog>().Where(x => x.Id != slugifiedEntity.Id).Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<Blog>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }


        //////////////// Tag ////////////////

        public async Task UpdateTagsAsync(Blog blog, params Tag[] tags)
        {
            foreach (var tag in tags)
            {
                var originTag = await GetAsync<Tag>(i => i.Name.ToLower() == tag.Name.ToLower());

                if (originTag == null)
                {
                    originTag = new Tag(tag);
                    await _context.Set<Tag>().AddAsync(originTag);
                }

                if (blog.Tags.Any(i => i.Name.ToLower() == originTag.Name.ToLower()))
                {
                    continue;
                }

                blog.Tags.Add(originTag);
            }

            await UpdateAsync(blog);
        }
    }
}
