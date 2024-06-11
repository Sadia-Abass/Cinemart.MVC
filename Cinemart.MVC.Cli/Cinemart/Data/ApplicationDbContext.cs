using Cinemart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinemart.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<FilmShowing> FilmShowing { get; set; }
        public DbSet<TicketSale> TicketSale { get; set; }

    }
}
