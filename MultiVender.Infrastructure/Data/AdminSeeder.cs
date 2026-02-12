using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultiVender.Domain.Entities;

namespace MultiVender.Infrastructure.Data
{
    public class AdminSeeder
    {
        public static async Task SeedAdminAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

           
            // 2️ Check if Admin already exists
            if (await context.Users.AnyAsync(u => u.FullName == "Admin Admin"))
                return;

            var adminRole = await context.Roles
                .FirstAsync(r => r.RoleName == "Admin");

            // 3️⃣ Create Admin User
            var admin = new User
            {
                FullName = "Admin Admin",
                Email = "admin@gmail.com",
                RoleId = adminRole.Id,
                IsVendor = false
            };

            admin.PasswordHash = new PasswordHasher<User>()
                .HashPassword(admin, "Admin@123");

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }

    
}
