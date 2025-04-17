using Microsoft.EntityFrameworkCore;
using SalesSystem.Entities;

namespace SalesSystem.Database
{
    internal class SalesSystemContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<SalesLog> SalesLog { get; set; }

        private string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SalesSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connection);
        }

    }
}

