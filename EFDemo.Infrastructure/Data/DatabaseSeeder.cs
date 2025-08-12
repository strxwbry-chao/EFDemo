using EFDemo.Domain;
using EFDemo.Infrastructure.Data;

namespace EFDemo.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(CustomerContext context)
        {
            if (context.Customers.Any())
                return;

            var customers = new List<Customer>
            {
                new Customer { FirstName = "John", LastName = "Smith", IsActive = true },
                new Customer { FirstName = "Jane", LastName = "Johnson", IsActive = true },
                new Customer { FirstName = "Bob", LastName = "Williams", IsActive = false },
                new Customer { FirstName = "Alice", LastName = "Brown", IsActive = true },
                new Customer { FirstName = "Charlie", LastName = "Davis", IsActive = false }
            };

            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();
        }
    }
}