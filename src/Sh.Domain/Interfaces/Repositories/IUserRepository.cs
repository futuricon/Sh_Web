using System.Collections.Generic;
using System.Threading.Tasks;
using Sh.Domain.Entities.UserModel;

namespace Sh.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task UpdateUserRolesAsync(AppUser user, IEnumerable<string> roles);
        Task DeleteDeeplyAsync(AppUser user);
    }
}
