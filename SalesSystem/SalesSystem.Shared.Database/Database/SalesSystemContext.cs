using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Entities;

namespace SalesSystem.Database
{
    public class SalesSystemContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<SalesLog> SalesLog { get; set; }

        public SalesSystemContext(DbContextOptions<SalesSystemContext> options)
            : base(options)
        {
        }

        public SalesSystemContext() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products);
            base.OnModelCreating(modelBuilder);
        }

    }
}

