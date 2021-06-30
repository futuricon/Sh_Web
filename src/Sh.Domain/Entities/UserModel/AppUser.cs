using Microsoft.AspNetCore.Identity;
using Sh.Domain.Entities.Base;
using Sh.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Sh.Domain.Entities.UserModel
{
    public class AppUser : IdentityUser<string>, IEntity<string>
    {
        [StringLength(32)]
        public override string Id { get; set; } = GeneratorId.GenerateComplex();

        [Display(Name = "Is Blocked")]
        public bool IsBlocked { get; set; }
    }
}
