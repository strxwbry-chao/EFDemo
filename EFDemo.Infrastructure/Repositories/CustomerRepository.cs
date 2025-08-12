using Microsoft.EntityFrameworkCore;
using EFDemo.Domain;
using EFDemo.Domain.Repositories;
using EFDemo.Domain.Specifications;
using EFDemo.Infrastructure.Data;

namespace EFDemo.Infrastructure.Repositories
{
    /// <summary>
    /// Customer repository implementation.
    /// Demonstrates how specific repositories extend generic repository functionality
    /// while leveraging the Specification pattern for business queries.
    /// </summary>
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext context) : base(context) { }

        public async Task<IReadOnlyList<Customer>> GetActiveCustomersAsync()
        {
            var spec = new ActiveCustomersSpecification();
            return await ListAsync(spec);
        }

        public async Task<IReadOnlyList<Customer>> SearchByNameAsync(string searchTerm)
        {
            var spec = new CustomersByNameSpecification(searchTerm);
            return await ListAsync(spec);
        }
    }
}