using Cinemart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinemart.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");

            builder.Entity<ApplicationUser>(e =>
            {
                // Each User can have many UserClaims
                e.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                e.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                e.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                e.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                // 
                e.ToTable(name: "Users");
            });

            builder.Entity<ApplicationRole>(e =>
            {
                // Each Role can have many entries in the UserRole join table
                e.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                e.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

                e.ToTable(name: "Roles");
            });

            builder.Entity<ApplicationUserClaim>(e =>
            {
                e.ToTable(name: "UserClaims");
            });

            builder.Entity<ApplicationUserLogin>(e =>
            {
                e.ToTable(name: "UserLogins");
            });

            builder.Entity<ApplicationRoleClaim>(e =>
            {
                e.ToTable(name: "RoleClaims");
            });

            builder.Entity<ApplicationUserToken>(e =>
            {
                e.ToTable(name: "UserTokens");
            });

            builder.Entity<ApplicationUserRole>(e =>
            {
                e.ToTable(name: "UserRoles");
            });

            var roles = new List<ApplicationRole> 
            {
                new ApplicationRole { Id = 1, Name = Enum.GetName<Role>(Role.Admin), Description = "Adminstrator" },
                new ApplicationRole { Id = 2, Name = Enum.GetName<Role>(Role.Member), Description = "Customer" },
                new ApplicationRole { Id = 3, Name = Enum.GetName<Role>(Role.Manager), Description = "Manager" },
                new ApplicationRole { Id = 4, Name = Enum.GetName<Role>(Role.Employee), Description = "Employee" }
            };

            builder.Entity<ApplicationRole>().HasData(roles);
        }

        public DbSet<Film> Film { get; set; }
        public DbSet<FilmShowing> FilmShowing { get; set; }
        public DbSet<TicketSale> TicketSale { get; set; }

    }


}
