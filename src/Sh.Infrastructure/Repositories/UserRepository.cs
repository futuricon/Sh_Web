using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Entities.UserModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Infrastructure.Data;

namespace Sh.Infrastructure.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager,
            ApplicationDbContext context) : base(context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task UpdateUserRolesAsync(AppUser user, IEnumerable<string> roles)
        {
            var previousRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, previousRoles);
            if (roles != null)
            {
                var actualRoles = roles.ToList();

                foreach (var role in actualRoles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public async Task DeleteDeeplyAsync(AppUser user)
        {
            if (user == null)
            {
                return;
            }

            var deletedUserAccount = await _userManager.FindByNameAsync("DELETED_USER");
            var articles = _context.Set<Blog>().Where(i => i.Author.Id == user.Id);

            foreach (var article in articles)
            {
                article.Author = deletedUserAccount;
            }

            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(user);
        }
    }
}
