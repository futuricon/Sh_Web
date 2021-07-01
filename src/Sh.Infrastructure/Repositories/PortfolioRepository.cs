using System.Linq;
using System.Threading.Tasks;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Entities.PortfolioModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Infrastructure.Data;

namespace Sh.Infrastructure.Repositories
{
    public class PortfolioRepository : Repository, IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;

        public PortfolioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task AddProjectAsync(Project project)
        {
            project.Slug = GetVerifiedProjectSlug(project);
            return AddAsync(project);
        }

        public async Task DeleteProjectAsync(Project project)
        {
            await DeleteAsync(project);
        }

        public Task UpdateProjectAsync(Project project, bool verifySlug = true)
        {
            if (verifySlug)
            {
                project.Slug = GetVerifiedProjectSlug(project);
            }
            return UpdateAsync(project);
        }

        private string GetVerifiedProjectSlug(Project slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var hasSameSlug = _context.Set<Project>().Where(x => x.Id != slugifiedEntity.Id).Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<Project>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }

        //////////////// Tag ////////////////

        public async Task UpdateTagsAsync(Project project, params Tag[] tags)
        {
            foreach (var tag in tags)
            {
                var originTag = await GetAsync<Tag>(i => i.Name.ToLower() == tag.Name.ToLower());

                if (originTag == null)
                {
                    originTag = new Tag(tag);
                    await _context.Set<Tag>().AddAsync(originTag);
                }

                if (project.Tags.Any(i => i.Name.ToLower() == originTag.Name.ToLower()))
                {
                    continue;
                }

                project.Tags.Add(originTag);
            }

            await UpdateAsync(project);
        }
    }
}
