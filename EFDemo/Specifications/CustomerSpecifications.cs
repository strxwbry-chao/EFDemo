using EFDemo.Domain;

namespace EFDemo.Domain.Specifications
{
    /// <summary>
    /// Specification for active customers.
    /// Demonstrates basic business rule encapsulation.
    /// </summary>
    public class ActiveCustomersSpecification : BaseSpecification<Customer>
    {
        public ActiveCustomersSpecification() : base(x => x.IsActive)
        {
            ApplyOrderBy(x => x.LastName);
        }
    }

    /// <summary>
    /// Specification for inactive customers.
    /// </summary>
    public class InactiveCustomersSpecification : BaseSpecification<Customer>
    {
        public InactiveCustomersSpecification() : base(x => !x.IsActive)
        {
            ApplyOrderBy(x => x.LastName);
        }
    }

    /// <summary>
    /// Specification for name-based customer search.
    /// Demonstrates parameterized specifications for flexible querying.
    /// </summary>
    public class CustomersByNameSpecification : BaseSpecification<Customer>
    {
        public CustomersByNameSpecification(string searchTerm) 
            : base(x => x.FirstName.ToLower().Contains(searchTerm.ToLower()) || 
                       x.LastName.ToLower().Contains(searchTerm.ToLower()))
        {
            ApplyOrderBy(x => x.LastName);
        }
    }
}