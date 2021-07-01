using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sh.Domain.Entities.UserModel;

namespace Sh.Infrastructure.Data
{
    public class SeedData
    {
        public static async void Initialize(IServiceProvider service)
        {
            await CreateUserRolesAsync(service);
            await CreateUserAccountAsync(service);
            await AddRoleToUserAccountAsync(service);
        }

        private static async Task CreateUserRolesAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<UserRole>>();

            var superAdminRole = await roleManager.RoleExistsAsync(Role.SuperAdmin.ToString());
            var adminRole = await roleManager.RoleExistsAsync(Role.Admin.ToString());

            if (!superAdminRole)
            {
                try
                {
                    await roleManager.CreateAsync(new UserRole(Role.SuperAdmin));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }

            }
            if (!adminRole)
            {
                await roleManager.CreateAsync(new UserRole(Role.Admin));
            }
        }

        private static async Task CreateUserAccountAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            var config = service.GetRequiredService<IConfiguration>();

            var SuperAccount = await userManager.FindByNameAsync("Futuricon");
            var AdminAccount = await userManager.FindByNameAsync("Shakhlo");
            if (SuperAccount == null)
            {
                try
                {
                    await userManager.CreateAsync(new AppUser()
                    {
                        UserName = "Futuricon",
                        Email = "kudratovsuhrob@gmail.com",
                        EmailConfirmed = true
                    },
                    config.GetSection("SuperPassword").Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }

            }
            if (AdminAccount == null)
            {
                try
                {
                    await userManager.CreateAsync(new AppUser()
                    {
                        UserName = "Shakhlo",
                        Email = "keepline@inbox.ru",
                        EmailConfirmed = true
                    },
                    config.GetSection("AdminPassword").Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
        }

        private static async Task AddRoleToUserAccountAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            var siteSuper = await userManager.FindByNameAsync("Futuricon");
            var siteAdmin = await userManager.FindByNameAsync("Shakhlo");

            if (siteSuper == null)
                return;
            var hasSuperRole = await userManager.IsInRoleAsync(siteSuper, Role.SuperAdmin.ToString());
            if (!hasSuperRole)
            {
                await userManager.AddToRoleAsync(siteSuper, Role.SuperAdmin.ToString());
            }

            if (siteAdmin == null)
                return;
            var hasAdminRole = await userManager.IsInRoleAsync(siteAdmin, Role.Admin.ToString());
            if (!hasAdminRole)
            {
                await userManager.AddToRoleAsync(siteAdmin, Role.Admin.ToString());
            }

        }
    }
}
