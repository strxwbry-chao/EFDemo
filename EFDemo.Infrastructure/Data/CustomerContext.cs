using Microsoft.EntityFrameworkCore;
using EFDemo.Domain;

namespace EFDemo.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework DbContext for the Customer demo.
    /// Demonstrates basic EF configuration with the Repository pattern.
    /// </summary>
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Essential configuration only
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.HasIndex(e => e.IsActive); // Important for your queries
            });
        }
    }
}