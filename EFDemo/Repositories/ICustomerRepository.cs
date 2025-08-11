using EFDemo.Domain;

namespace EFDemo.Domain.Repositories
{
    /// <summary>
    /// Specific repository interface for Customer entities.
    /// 
    /// LEARNING NOTE: While the generic IRepository<T> provides common operations,
    /// specific repository interfaces allow you to add domain-specific methods
    /// that don't fit the generic pattern.
    /// 
    /// This follows the Interface Segregation Principle - clients should not
    /// be forced to depend on methods they don't use.
    /// </summary>
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Gets all active customers.
        /// LEARNING NOTE: This is a domain-specific method that encapsulates
        /// a common business query. It's more expressive than using a generic
        /// specification every time.
        /// </summary>
        /// <returns>All active customers</returns>
        Task<IReadOnlyList<Customer>> GetActiveCustomersAsync();

        /// <summary>
        /// Searches for customers by name.
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>Customers whose first or last name contains the search term</returns>
        Task<IReadOnlyList<Customer>> SearchByNameAsync(string searchTerm);

        /// <summary>
        /// Gets customers with pagination.
        /// LEARNING NOTE: Pagination is so common that it's worth having a
        /// dedicated method for it, even though it could be done with specifications.
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="activeOnly">Whether to include only active customers</param>
        /// <returns>Paginated customer results</returns>
        Task<IReadOnlyList<Customer>> GetCustomersPagedAsync(int pageNumber, int pageSize, bool activeOnly = true);

        /// <summary>
        /// Gets the total count of customers (for pagination calculations).
        /// </summary>
        /// <param name="activeOnly">Whether to count only active customers</param>
        /// <returns>Total customer count</returns>
        Task<int> GetCustomerCountAsync(bool activeOnly = true);

        /// <summary>
        /// Checks if a customer with the given name already exists.
        /// LEARNING NOTE: This kind of business rule checking is perfect for
        /// repository methods. It keeps the business logic close to the data access.
        /// </summary>
        /// <param name="firstName">First name to check</param>
        /// <param name="lastName">Last name to check</param>
        /// <param name="excludeId">ID to exclude from the check (for updates)</param>
        /// <returns>True if a customer with this name exists</returns>
        Task<bool> CustomerExistsAsync(string firstName, string lastName, int? excludeId = null);
    }
}