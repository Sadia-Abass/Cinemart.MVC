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
            //SeedRoles(builder);

            var roles = new List<ApplicationRole> 
            {
                new ApplicationRole { Id = 1, Name = Enum.GetName<Role>(Role.Admin), Description = "Adminstrator" },
                new ApplicationRole { Id = 2, Name = Enum.GetName<Role>(Role.Member), Description = "General User" },
                new ApplicationRole { Id = 3, Name = Enum.GetName<Role>(Role.Manager), Description = "Manager" },
                new ApplicationRole { Id = 4, Name = Enum.GetName<Role>(Role.Employee), Description = "Employee" }
            };

            builder.Entity<ApplicationRole>().HasData(roles);
        }

        //private static void SeedRoles(ModelBuilder builder)
        //{
        //    builder.Entity<ApplicationRole>().HasData(
        //        new ApplicationRole() { Name = Enum.GetName<Role>(Role.Admin), Description = "Adminstrator" },
        //        new ApplicationRole() { Name = Enum.GetName<Role>(Role.Member), Description = "General User" },
        //        new ApplicationRole() { Name = Enum.GetName<Role>(Role.Manager), Description = "Manager" },
        //        new ApplicationRole() { Name = Enum.GetName<Role>(Role.Employee), Description = "Employee" }
        //        );
        //}

        public DbSet<Film> Film { get; set; }
        public DbSet<FilmShowing> FilmShowing { get; set; }
        public DbSet<TicketSale> TicketSale { get; set; }

    }


}
