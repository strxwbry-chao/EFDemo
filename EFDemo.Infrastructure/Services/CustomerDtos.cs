using EFDemo.Domain;

namespace EFDemo.Infrastructure.Services
{
    /// <summary>
    /// Simple DTOs for the Customer demo.
    /// </summary>
    public class CreateCustomerDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateCustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}