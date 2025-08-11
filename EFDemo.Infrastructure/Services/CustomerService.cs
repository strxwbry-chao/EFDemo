using EFDemo.Domain;
using EFDemo.Domain.Repositories;
using EFDemo.Domain.Specifications;

namespace EFDemo.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Customer business operations.
    /// 
    /// LEARNING NOTE: This class implements the business logic for customer operations.
    /// It demonstrates several important patterns:
    /// 1. Dependency Injection - depends on abstractions (interfaces), not concretions
    /// 2. Single Responsibility - focused only on customer business logic
    /// 3. Separation of Concerns - coordinates between repositories without doing data access itself
    /// 4. Domain-Driven Design - uses domain entities and enforces business rules
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// Constructor that accepts dependencies.
        /// LEARNING NOTE: This is constructor injection, one of the most common
        /// dependency injection patterns. The service depends on the repository
        /// abstraction, making it testable and loosely coupled.
        /// </summary>
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            // LEARNING NOTE: Simple pass-through to the repository.
            // In more complex scenarios, you might add logging,
            // caching, or business rule validation here.
            return await _customerRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets all active customers.
        /// </summary>
        public async Task<IReadOnlyList<Customer>> GetActiveCustomersAsync()
        {
            return await _customerRepository.GetActiveCustomersAsync();
        }

        /// <summary>
        /// Searches for customers with pagination.
        /// </summary>
        public async Task<PagedResult<Customer>> SearchCustomersAsync(CustomerSearchDto searchCriteria)
        {
            // LEARNING NOTE: This method shows how the service layer orchestrates
            // multiple repository calls to provide a complete business operation.
            
            // Use specifications to build the search criteria
            ISpecification<Customer> spec;

            if (!string.IsNullOrWhiteSpace(searchCriteria.SearchTerm))
            {
                spec = new CustomersByNameSpecification(searchCriteria.SearchTerm);
            }
            else if (searchCriteria.IsActive.HasValue)
            {
                spec = searchCriteria.IsActive.Value 
                    ? new ActiveCustomersSpecification()
                    : new InactiveCustomersSpecification();
            }
            else
            {
                // Get paginated customers
                spec = new CustomersWithPagingSpecification(
                    searchCriteria.PageNumber, 
                    searchCriteria.PageSize, 
                    searchCriteria.IsActive ?? true);
            }

            // Get the results and total count
            var customers = await _customerRepository.ListAsync(spec);
            var totalCount = await _customerRepository.GetCustomerCountAsync(searchCriteria.IsActive ?? true);

            // Return the paginated result
            return new PagedResult<Customer>
            {
                Items = customers,
                PageNumber = searchCriteria.PageNumber,
                PageSize = searchCriteria.PageSize,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        public async Task<Customer> CreateCustomerAsync(CreateCustomerDto createDto)
        {
            // LEARNING NOTE: This method demonstrates business rule validation
            // and the conversion from DTO to domain entity.

            // Validate business rules
            await ValidateCustomerNameUniqueness(createDto.FirstName, createDto.LastName);

            // Create the domain entity
            var customer = new Customer
            {
                FirstName = createDto.FirstName.Trim(),
                LastName = createDto.LastName.Trim(),
                IsActive = createDto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            // Save to repository
            var createdCustomer = await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return createdCustomer;
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        public async Task<Customer> UpdateCustomerAsync(UpdateCustomerDto updateDto)
        {
            // Get the existing customer
            var existingCustomer = await _customerRepository.GetByIdAsync(updateDto.Id);
            if (existingCustomer == null)
            {
                throw new ArgumentException($"Customer with ID {updateDto.Id} not found.", nameof(updateDto.Id));
            }

            // Validate business rules (excluding the current customer)
            await ValidateCustomerNameUniqueness(updateDto.FirstName, updateDto.LastName, updateDto.Id);

            // Update the domain entity using its business methods
            // LEARNING NOTE: We use the domain entity's business methods
            // rather than setting properties directly. This ensures
            // business rules and side effects are properly handled.
            existingCustomer.UpdateName(updateDto.FirstName.Trim(), updateDto.LastName.Trim());
            
            if (updateDto.IsActive != existingCustomer.IsActive)
            {
                if (updateDto.IsActive)
                {
                    existingCustomer.Activate();
                }
                else
                {
                    existingCustomer.Deactivate();
                }
            }

            // Save changes
            var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer);
            await _customerRepository.SaveChangesAsync();

            return updatedCustomer;
        }

        /// <summary>
        /// Deactivates a customer (soft delete).
        /// </summary>
        public async Task<bool> DeactivateCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return false;
            }

            // Use the domain entity's business method
            customer.Deactivate();

            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Activates a customer.
        /// </summary>
        public async Task<bool> ActivateCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return false;
            }

            customer.Activate();

            await _customerRepository.UpdateAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Permanently deletes a customer (hard delete).
        /// </summary>
        public async Task<bool> DeleteCustomerAsync(int id)
        {
            // LEARNING NOTE: Hard deletes should be used carefully.
            // Consider if you need to check for related data,
            // audit the deletion, or require special permissions.
            
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return false;
            }

            await _customerRepository.DeleteAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Validates that a customer name is unique.
        /// LEARNING NOTE: This is a private helper method that encapsulates
        /// a business rule. By making it private, we ensure it's only used
        /// within this service and can't be bypassed.
        /// </summary>
        private async Task ValidateCustomerNameUniqueness(string firstName, string lastName, int? excludeId = null)
        {
            var exists = await _customerRepository.CustomerExistsAsync(firstName, lastName, excludeId);
            if (exists)
            {
                throw new InvalidOperationException($"A customer with the name '{firstName} {lastName}' already exists.");
            }
        }
    }
}