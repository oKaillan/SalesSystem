using Microsoft.EntityFrameworkCore;
using SalesSystem.Entities;
using System.Configuration;

namespace SalesSystem.Database
{
    public class SalesSystemContext : DbContext
    {

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<SalesLog> SalesLog { get; set; }

        public SalesSystemContext(DbContextOptions<SalesSystemContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products);
        }

    }
}

