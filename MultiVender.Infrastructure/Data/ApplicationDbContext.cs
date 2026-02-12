using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiVender.Domain.Entities;

namespace MultiVender.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                           .HasOne(p => p.Shop)
                           .WithMany()
                           .HasForeignKey(p => p.ShopId)
                           .OnDelete(DeleteBehavior.Restrict); // This breaks the cycle

            modelBuilder.Entity<Role>().HasData(
                    new Role { Id = 1, RoleName = "Admin" },
                    new Role { Id = 2, RoleName = "Vendor" },
                    new Role { Id = 3, RoleName = "User" }

            );

        }

    }
}
