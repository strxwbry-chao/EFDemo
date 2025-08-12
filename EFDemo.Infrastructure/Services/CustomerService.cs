using EFDemo.Domain;
using EFDemo.Domain.Repositories;

namespace EFDemo.Infrastructure.Services
{
    /// <summary>
    /// Customer service implementation.
    /// Demonstrates the Service layer pattern coordinating between API and Repository layers.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<Customer>> GetActiveCustomersAsync()
        {
            return await _customerRepository.GetActiveCustomersAsync();
        }

        public async Task<IReadOnlyList<Customer>> SearchCustomersAsync(string searchTerm)
        {
            return await _customerRepository.SearchByNameAsync(searchTerm);
        }

        public async Task<Customer> CreateCustomerAsync(CreateCustomerDto createDto)
        {
            var customer = new Customer
            {
                FirstName = createDto.FirstName.Trim(),
                LastName = createDto.LastName.Trim(),
                IsActive = createDto.IsActive
            };

            var createdCustomer = await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return createdCustomer;
        }

        public async Task<Customer> UpdateCustomerAsync(UpdateCustomerDto updateDto)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(updateDto.Id);
            if (existingCustomer == null)
                throw new ArgumentException($"Customer with ID {updateDto.Id} not found.");

            existingCustomer.FirstName = updateDto.FirstName.Trim();
            existingCustomer.LastName = updateDto.LastName.Trim();
            existingCustomer.IsActive = updateDto.IsActive;

            var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer);
            await _customerRepository.SaveChangesAsync();
            return updatedCustomer;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return false;

            await _customerRepository.DeleteAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return true;
        }
    }
}