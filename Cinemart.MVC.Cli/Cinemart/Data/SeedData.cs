using Cinemart.Models;
using Microsoft.AspNetCore.Identity;
namespace Cinemart.Data
{
    public class SeedData
    {
        static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Create roles if they don't exist
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    //await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }

            // Create admin user if doesn't exist
            var adminEmail = configuration["AdminUser:Email"] ?? "admin@example.com";
            var adminPassword = configuration["AdminUser:Password"] ?? "Admin@123456";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Firstname = "System",
                    Lastname = "Administrator",
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
