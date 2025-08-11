using EFDemo.Domain;
using EFDemo.Infrastructure.Data;

namespace EFDemo.Infrastructure.Data
{
    /// <summary>
    /// Service for seeding initial data into the database.
    /// 
    /// LEARNING NOTE: Data seeding is important for:
    /// 1. Providing initial data for testing
    /// 2. Setting up reference data (lookup tables, etc.)
    /// 3. Creating demo data for development
    /// 4. Ensuring the application has baseline data to function
    /// 
    /// In production systems, you might have different seeding strategies:
    /// - Migration-based seeding for reference data
    /// - Script-based seeding for large datasets
    /// - Environment-specific seeding (different data for dev/staging/prod)
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Seeds the database with initial customer data.
        /// 
        /// LEARNING NOTE: This method demonstrates several important patterns:
        /// 1. Checking if data already exists before seeding
        /// 2. Using Entity Framework to insert data
        /// 3. Creating realistic test data
        /// 4. Handling both active and inactive records
        /// </summary>
        /// <param name="context">The database context</param>
        public static async Task SeedAsync(CustomerContext context)
        {
            // Check if we already have customers - don't seed if data exists
            // LEARNING NOTE: This prevents duplicate data if the application
            // is restarted multiple times. In production, you might want
            // more sophisticated seeding logic.
            if (context.Customers.Any())
            {
                return; // Database already seeded
            }

            // Create sample customers
            // LEARNING NOTE: We create a variety of customers to demonstrate
            // different scenarios: active, inactive, different names for searching, etc.
            var customers = new List<Customer>
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Smith",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Customer
                {
                    FirstName = "Jane",
                    LastName = "Johnson",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Customer
                {
                    FirstName = "Bob",
                    LastName = "Williams",
                    IsActive = false, // Inactive customer for testing
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Customer
                {
                    FirstName = "Alice",
                    LastName = "Brown",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Customer
                {
                    FirstName = "Charlie",
                    LastName = "Davis",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Customer
                {
                    FirstName = "Diana",
                    LastName = "Miller",
                    IsActive = false, // Another inactive customer
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Customer
                {
                    FirstName = "Edward",
                    LastName = "Wilson",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Customer
                {
                    FirstName = "Fiona",
                    LastName = "Moore",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            // Add customers to the context
            // LEARNING NOTE: AddRange is more efficient than adding one at a time
            // when inserting multiple records
            context.Customers.AddRange(customers);

            // Save changes to the database
            // LEARNING NOTE: SaveChanges executes all pending operations in a single transaction
            await context.SaveChangesAsync();

            // Log the seeding operation
            // LEARNING NOTE: In a real application, you would use a proper logging framework
            Console.WriteLine($"Seeded {customers.Count} customers into the database.");
        }
    }
}