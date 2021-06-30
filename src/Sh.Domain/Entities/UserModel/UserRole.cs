using Microsoft.AspNetCore.Identity;
using Sh.Domain.Entities.Base;
using Sh.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Sh.Domain.Entities.UserModel
{
    public enum Role
    {
        SuperAdmin,
        Admin
    }

    public class UserRole : IdentityRole, IEntity<string>
    {
        public UserRole(Role role) : base(role.ToString())
        {
            Id = GeneratorId.GenerateLong();
            Role = role;
        }

        [StringLength(32)]
        public override string Id { get; set; }

        public Role Role { get; set; }
    }
}
