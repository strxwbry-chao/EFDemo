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

            // Basic Customer entity configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();
                
                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsRequired();
                
                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Index for common queries
                entity.HasIndex(e => e.IsActive);
            });
        }
    }
}