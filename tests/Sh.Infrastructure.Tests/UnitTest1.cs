using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sh.Infrastructure.Data;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using Sh.Domain.Entities.BlogModel;

namespace Sh.Infrastructure.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestPopularTags()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>();
            dbOptions.UseSqlServer(GetAppConfiguration().GetConnectionString("WUCSA_DB"))
                .UseLazyLoadingProxies();

            var context = new ApplicationDbContext(dbOptions.Options);

            var articles = context;
            var tags = new List<string>();

            foreach (var article in articles.)
            {
                tags.AddRange(article.GetTags());
            }

            var popularTags = tags.GroupBy(str => str)
                .Select(i => new { Name = i.Key, Count = i.Count() })
                .OrderByDescending(k => k.Count);
        }

        private IConfigurationRoot GetAppConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;
        }

    }
}
