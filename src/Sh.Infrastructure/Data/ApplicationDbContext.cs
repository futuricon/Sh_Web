using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Entities.GalleryModel;
using Sh.Domain.Entities.DossierModel;
using Sh.Domain.Entities.UserModel;
using System;

namespace Sh.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, UserRole, string>
    {

        public ApplicationDbContext(){}

        public ApplicationDbContext(DbContextOptions options) : base(options){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost;port=3306;user=root;password=root;database=portfolio_db;")
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<UserRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
            builder.Entity<Dossier>();
            //builder.Entity<Dossier>().HasData(
            //    new Dossier
            //    {
            //        FirstName = "Shakhlo",
            //        FirstNameRu = "Шахло",
            //        FirstNameUz = "Shahlo",
            //        LastName = "Ergasheva",
            //        LastNameRu = "Эргашева",
            //        LastNameUz = "Ergasheva",
            //        Position = "Pupil",
            //        PositionRu = "Ученица",
            //        PositionUz = "O'quvchi",
            //        Description = "test",
            //        DescriptionRu = "тест",
            //        DescriptionUz = "test",
            //        CoverPhotoPath = "",
            //        Address = "some address",
            //        PhoneNumber = "+998908007060",
            //        Email = "test@mail.com",
            //        CVFilePath = "",
            //        IsAboutInfo = true
            //    }
            //);
            builder.Entity<Tag>(entity =>
            {
                entity.HasMany(m => m.Blogs)
                    .WithMany(m => m.Tags);

                entity.HasMany(m => m.Projects)
                    .WithMany(m => m.Tags);
            });
            builder.Entity<Category>(entity =>
            {
                entity.HasMany(m => m.Medias)
                    .WithMany(m => m.Categories);
            });
        }
    }
}
