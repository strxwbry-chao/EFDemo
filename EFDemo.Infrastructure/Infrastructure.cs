using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFDemo.Domain;

namespace EFDemo.Infrastructure.Data
{
    /// <summary>
    /// The Entity Framework DbContext that manages database connections and operations.
    /// 
    /// LEARNING NOTE: DbContext is the primary class responsible for interacting
    /// with the database in Entity Framework. It:
    /// 1. Manages database connections
    /// 2. Tracks changes to entities
    /// 3. Translates LINQ queries to SQL
    /// 4. Handles object-relational mapping (ORM)
    /// 
    /// Think of it as a bridge between your C# objects and the database tables.
    /// </summary>
    public class CustomerContext : DbContext
    {
        /// <summary>
        /// Constructor that accepts DbContextOptions.
        /// LEARNING NOTE: This constructor allows dependency injection to configure
        /// the database connection. The options typically include the connection string
        /// and database provider (SQL Server, SQLite, PostgreSQL, etc.).
        /// </summary>
        /// <param name="options">Configuration options for the context</param>
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet representing the Customers table in the database.
        /// LEARNING NOTE: DbSet<T> represents a table in the database. You query
        /// against DbSet<T> and Entity Framework translates your LINQ queries
        /// into SQL. Each DbSet property typically corresponds to a table.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Configures the model using Fluent API.
        /// LEARNING NOTE: OnModelCreating is where you can configure your entity
        /// mappings using the Fluent API. This is more powerful than data annotations
        /// and keeps configuration separate from your entity classes.
        /// 
        /// The Fluent API allows you to:
        /// 1. Configure table names, column names, and data types
        /// 2. Set up relationships between entities
        /// 3. Define indexes and constraints
        /// 4. Configure inheritance mappings
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure entities</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                // LEARNING NOTE: Explicitly configure the table name
                // This gives you control over database naming conventions
                entity.ToTable("Customers");

                // Configure the primary key
                // LEARNING NOTE: While EF can infer this from the "Id" property name,
                // being explicit makes the code more readable and maintainable
                entity.HasKey(e => e.Id);

                // Configure the Id column
                entity.Property(e => e.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedOnAdd(); // This tells EF that the database generates the value

                // Configure FirstName column
                entity.Property(e => e.FirstName)
                    .HasColumnName("FirstName")
                    .HasMaxLength(100) // Limits the column size
                    .IsRequired(); // Makes it NOT NULL in the database

                // Configure LastName column
                entity.Property(e => e.LastName)
                    .HasColumnName("LastName")
                    .HasMaxLength(100)
                    .IsRequired();

                // Configure IsActive column
                entity.Property(e => e.IsActive)
                    .HasColumnName("IsActive")
                    .HasDefaultValue(true); // Sets a default value in the database

                // Configure CreatedAt column
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasColumnType("datetime2") // Specific SQL Server data type
                    .HasDefaultValueSql("GETUTCDATE()"); // Database generates the default value

                // Configure UpdatedAt column
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("UpdatedAt")
                    .HasColumnType("datetime2")
                    .IsRequired(false); // This column can be NULL

                // Create indexes for better query performance
                // LEARNING NOTE: Indexes speed up queries but slow down inserts/updates.
                // Create them on columns you frequently search by.
                entity.HasIndex(e => e.LastName)
                    .HasDatabaseName("IX_Customers_LastName");

                entity.HasIndex(e => e.IsActive)
                    .HasDatabaseName("IX_Customers_IsActive");

                // Composite index for name searches
                entity.HasIndex(e => new { e.FirstName, e.LastName })
                    .HasDatabaseName("IX_Customers_FullName");
            });
        }

        /// <summary>
        /// Override SaveChanges to add audit information automatically.
        /// LEARNING NOTE: This is a powerful pattern where you can intercept
        /// all save operations and add cross-cutting concerns like auditing,
        /// validation, or business rules.
        /// </summary>
        /// <returns>The number of affected records</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // LEARNING NOTE: ChangeTracker.Entries() gives you access to all
            // entities that EF is tracking for changes. This is where you can
            // implement automatic auditing.
            var entries = ChangeTracker.Entries<Customer>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                // Automatically set UpdatedAt when a customer is modified
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
