using EFDemo.Domain;

namespace EFDemo.Domain.Specifications
{
    public class ActiveCustomersSpecification : BaseSpecification<Customer>
    {
        public ActiveCustomersSpecification() : base(x => x.IsActive)
        {
            ApplyOrderBy(x => x.LastName);
        }
    }

    public class InactiveCustomersSpecification : BaseSpecification<Customer>
    {
        public InactiveCustomersSpecification() : base(x => !x.IsActive)
        {
            ApplyOrderBy(x => x.LastName);
        }
    }

    public class CustomersByNameSpecification : BaseSpecification<Customer>
    {
        public CustomersByNameSpecification(string searchTerm) : base(x => x.FirstName.ToLower().Contains(searchTerm.ToLower()) ||  x.LastName.ToLower().Contains(searchTerm.ToLower()))
        {
            ApplyOrderBy(x => x.LastName);
        }
    }
}