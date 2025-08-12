using EFDemo.Domain;

namespace EFDemo.Domain.Repositories
{
    /// <summary>
    /// Customer-specific repository interface.
    /// Demonstrates how domain-specific methods extend the generic repository pattern.
    /// </summary>
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Gets all active customers - demonstrates domain-specific query methods.
        /// </summary>
        Task<IReadOnlyList<Customer>> GetActiveCustomersAsync();

        /// <summary>
        /// Searches customers by name - shows parameterized repository methods.
        /// </summary>
        Task<IReadOnlyList<Customer>> SearchByNameAsync(string searchTerm);
    }
}