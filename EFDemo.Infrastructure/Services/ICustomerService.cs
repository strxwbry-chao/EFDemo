using EFDemo.Domain;
using EFDemo.Infrastructure.Services;

namespace EFDemo.Infrastructure.Services
{
    /// <summary>
    /// Interface for the Customer service that defines business operations.
    /// 
    /// LEARNING NOTE: The service layer sits between the controllers and repositories.
    /// It's responsible for:
    /// 1. Orchestrating business operations
    /// 2. Enforcing business rules
    /// 3. Coordinating between multiple repositories
    /// 4. Managing transactions
    /// 5. Converting between DTOs and domain entities
    /// 
    /// This layer keeps business logic out of the controllers and repositories.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>The customer if found, null otherwise</returns>
        Task<Customer?> GetCustomerByIdAsync(int id);

        /// <summary>
        /// Gets all active customers.
        /// </summary>
        /// <returns>All active customers</returns>
        Task<IReadOnlyList<Customer>> GetActiveCustomersAsync();

        /// <summary>
        /// Searches for customers with pagination.
        /// LEARNING NOTE: This method demonstrates how the service layer
        /// can provide a clean, high-level interface that hides the complexity
        /// of specifications and repository calls.
        /// </summary>
        /// <param name="searchCriteria">The search and pagination criteria</param>
        /// <returns>Paginated search results</returns>
        Task<PagedResult<Customer>> SearchCustomersAsync(CustomerSearchDto searchCriteria);

        /// <summary>
        /// Creates a new customer.
        /// LEARNING NOTE: This method will validate business rules,
        /// convert the DTO to a domain entity, and handle the persistence.
        /// </summary>
        /// <param name="createDto">The customer creation data</param>
        /// <returns>The created customer</returns>
        Task<Customer> CreateCustomerAsync(CreateCustomerDto createDto);

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="updateDto">The customer update data</param>
        /// <returns>The updated customer</returns>
        Task<Customer> UpdateCustomerAsync(UpdateCustomerDto updateDto);

        /// <summary>
        /// Deactivates a customer (soft delete).
        /// LEARNING NOTE: This is a business operation that's more meaningful
        /// than a generic "delete" - it expresses the business intent clearly.
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>True if the customer was deactivated</returns>
        Task<bool> DeactivateCustomerAsync(int id);

        /// <summary>
        /// Activates a customer.
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>True if the customer was activated</returns>
        Task<bool> ActivateCustomerAsync(int id);

        /// <summary>
        /// Permanently deletes a customer (hard delete).
        /// LEARNING NOTE: This is separated from deactivation because
        /// permanent deletion is a serious business operation that
        /// might require special permissions or audit trails.
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>True if the customer was deleted</returns>
        Task<bool> DeleteCustomerAsync(int id);
    }
}