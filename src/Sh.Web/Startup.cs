using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sh.Domain.Entities.UserModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Infrastructure.Data;
using Sh.Infrastructure.Repositories;
using Sh.Web.Pages.Utils;
using Sh.Web.Resources;
using Sh.Web.ViewComponents;


namespace Sh.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages(options => {
                options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("uz")
                };
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
            });
            services.AddLocalization(opts => opts.ResourcesPath = "Resources");

            services.AddRazorPages();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("Portfolio_DB"),
                new MySqlServerVersion(new Version(8, 0, 11))
                ).UseLazyLoadingProxies());

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<ITestimonialRepository, TestimonialRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            ConfigureIdentity(services);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });



            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ImageHelper>();
            services.AddRazorPages()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    // Remove routes ending with /Index
                    options.Conventions.AddFolderRouteModelConvention("/", model =>
                    {
                        var selectorCount = model.Selectors.Count;
                        for (var i = selectorCount - 1; i >= 0; i--)
                        {
                            var selectorTemplate = model.Selectors[i].AttributeRouteModel.Template;
                            if (selectorTemplate.EndsWith("Index"))
                            {
                                model.Selectors.RemoveAt(i);
                            }
                        }
                    });
                });
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddHttpContextAccessor();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddDefaultIdentity<AppUser>()
                .AddRoles<UserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789_.-@";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });
        }
    }
}
