using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sh.Domain.Entities.Base;
using Sh.Domain.Interfaces.Repositories;

namespace Sh.Web.Pages.Blog
{
    public class ListModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;

        public ListModel(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public PaginatedList<Domain.Entities.BlogModel.Blog> Blogs { get; set; }
        public Domain.Entities.BlogModel.Blog[] PopularArticles { get; set; }
        public string[] PopularTags { get; set; }
        public string RCName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1, string tag = null)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var blogs = _blogRepository.GetAll<Domain.Entities.BlogModel.Blog>();

            if (!string.IsNullOrEmpty(SearchString))
            {
                blogs = blogs.Where(s => s.Title.Contains(SearchString) ||
                s.TitleRu.Contains(SearchString) || s.TitleUz.Contains(SearchString) ||
                s.Tags.Where(x => x.Name.Contains(SearchString)).Any());
                ViewData["CurrentFilter"] = SearchString;
                Blogs = PaginatedList<Domain.Entities.BlogModel.Blog>.Create(blogs.OrderByDescending(i => i.PostedDate), pageIndex, 6);
            }
            else if (tag != null)
            {
                var taggedBlogs = blogs.Where(s => s.Tags.Where(x => x.Name.ToLower().Contains(tag.ToLower())).Any());
                Blogs = PaginatedList<Domain.Entities.BlogModel.Blog>.Create(taggedBlogs.OrderByDescending(i => i.PostedDate), pageIndex, 6);
            }
            else
            {
                Blogs = PaginatedList<Domain.Entities.BlogModel.Blog>.Create(blogs.OrderByDescending(i => i.PostedDate), pageIndex, 6);
            }

            PopularArticles = blogs.OrderByDescending(i => i.ViewCount).Take(4).ToArray();
            PopularTags = await GetPopularTagsAsync(blogs);
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
