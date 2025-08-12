using EFDemo.Domain;

namespace EFDemo.Infrastructure.Services
{
    /// <summary>
    /// Customer service interface demonstrating the Service layer pattern.
    /// Orchestrates business operations between the API and Repository layers.
    /// </summary>
    public interface ICustomerService
    {
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<IReadOnlyList<Customer>> GetActiveCustomersAsync();
        Task<IReadOnlyList<Customer>> SearchCustomersAsync(string searchTerm);
        Task<Customer> CreateCustomerAsync(CreateCustomerDto createDto);
        Task<Customer> UpdateCustomerAsync(UpdateCustomerDto updateDto);
        Task<bool> DeleteCustomerAsync(int id);
    }
}