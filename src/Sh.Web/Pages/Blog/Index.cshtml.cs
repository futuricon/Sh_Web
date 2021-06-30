using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;

        public IndexModel(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public Domain.Entities.BlogModel.Blog Blog { get; set; }

        public Domain.Entities.BlogModel.Blog[] PopularArticles { get; set; }
        public string[] PopularTags { get; set; }
        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var blogs = _blogRepository.GetAll<Domain.Entities.BlogModel.Blog>();
            Blog = blogs.Where(i=>i.Slug == slug).FirstOrDefault();

            if (Blog == null)
            {
                return NotFound();
            }
            if (!Request.Headers["User-Agent"].ToString().ToLower().Contains("bot"))
            {
                Blog.ViewCount++;
            }
            await _blogRepository.UpdateBlogAsync(Blog, false);

            PopularArticles = blogs.OrderByDescending(i => i.ViewCount).Take(5).ToArray();
            PopularTags = await GetPopularTagsAsync(blogs);

            switch (RCName.ToLower())
            {
                case "ru":
                    ViewData["BlogTitle"] = Blog.TitleRu;
                    ViewData["BlogDesription"] = Domain.Entities.BlogModel.Blog.GetShortContent(Blog.ContentRu, 250);
                    break;
                case "uz":
                    ViewData["BlogTitle"] = Blog.TitleUz;
                    ViewData["BlogDesription"] = Domain.Entities.BlogModel.Blog.GetShortContent(Blog.ContentUz, 250);
                    break;
                default:
                    ViewData["BlogTitle"] = Blog.Title;
                    ViewData["BlogDesription"] = Domain.Entities.BlogModel.Blog.GetShortContent(Blog.Content, 250);
                    break;
            }

            return Page();
        }

        public Task<string[]> GetPopularTagsAsync(IQueryable<Domain.Entities.BlogModel.Blog> blogs)
        {
            return Task.Run(() =>
            {
                var tags = new List<string>();
                var blogsList = blogs.ToList();

                foreach (var blog in blogsList)
                {
                    tags.AddRange(blog.Tags.Select(i => i.Name));
                }

                var popularTags = tags.GroupBy(str => str)
                    .Select(i => new { Name = i.Key, Count = i.Count() })
                    .OrderByDescending(k => k.Count).Select(i => i.Name).Take(10).ToArray();

                return popularTags;
            });
        }
    }
}
