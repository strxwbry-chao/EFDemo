using EFDemo.Domain;

namespace EFDemo.Infrastructure.Services
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating a new customer.
    /// 
    /// LEARNING NOTE: DTOs are used to transfer data between layers or systems.
    /// They help to:
    /// 1. Decouple the API from the domain model
    /// 2. Control exactly what data is exposed
    /// 3. Provide validation attributes for input
    /// 4. Version your API independently from your domain
    /// </summary>
    public class CreateCustomerDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO for updating an existing customer.
    /// </summary>
    public class UpdateCustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for customer search/filter criteria.
    /// LEARNING NOTE: This DTO encapsulates all the possible search criteria
    /// in one object, making the API cleaner and easier to extend.
    /// </summary>
    public class CustomerSearchDto
    {
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// DTO for paginated results.
    /// LEARNING NOTE: This DTO provides all the information needed for
    /// pagination in the UI, including total counts and page information.
    /// </summary>
    /// <typeparam name="T">The type of items in the page</typeparam>
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}