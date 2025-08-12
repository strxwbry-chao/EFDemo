using Microsoft.EntityFrameworkCore;
using EFDemo.Domain;
using EFDemo.Domain.Specifications;
using EFDemo.Infrastructure.Data;
using EFDemo.Infrastructure.Repositories;
using Xunit;

namespace EFDemo.Tests.Integration
{
    public class CustomerRepositoryIntegrationTests : IDisposable
    {
        private readonly CustomerContext _context;
        private readonly CustomerRepository _repository;

        public CustomerRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<CustomerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CustomerContext(options);
            _repository = new CustomerRepository(_context);

            // Seed test data
            SeedTestData();
        }

        [Fact]
        public async Task ListAsync_WithActiveCustomersSpecification_ShouldIntegrateRepositoryAndSpecificationPatterns()
        {
            // Arrange - Create specification directly
            var specification = new ActiveCustomersSpecification();

            // Act - Use generic repository method with specification
            var result = await _repository.ListAsync(specification);

            // Assert - Verify the Repository + Specification integration
            Assert.Equal(2, result.Count);
            Assert.All(result, customer => Assert.True(customer.IsActive));
            
            // Verify this is the same result as the specific repository method
            var specificMethodResult = await _repository.GetActiveCustomersAsync();
            Assert.Equal(result.Count, specificMethodResult.Count);
        }

        private void SeedTestData()
        {
            var customers = new[]
            {
                new Customer { Id = 1, FirstName = "John", LastName = "Smith", IsActive = true },
                new Customer { Id = 2, FirstName = "Jane", LastName = "Brown", IsActive = true },
                new Customer { Id = 3, FirstName = "Bob", LastName = "Wilson", IsActive = false }
            };

            _context.Customers.AddRange(customers);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}