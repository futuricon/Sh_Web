using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Sh.Web.Areas.Identity.IdentityHostingStartup))]
namespace Sh.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}