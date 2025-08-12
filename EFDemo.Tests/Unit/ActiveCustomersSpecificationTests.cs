using EFDemo.Domain;
using EFDemo.Domain.Specifications;
using Xunit;

namespace EFDemo.Tests.Unit
{
    public class ActiveCustomersSpecificationTests
    {
        [Fact]
        public void ActiveCustomersSpecification_ShouldReturnTrueForActiveCustomer()
        {
            var specification = new ActiveCustomersSpecification();
            var activeCustomer = new Customer 
            { 
                Id = 1, 
                FirstName = "John", 
                LastName = "Doe", 
                IsActive = true 
            };

            var result = specification.Criteria!.Compile()(activeCustomer);

            Assert.True(result);
        }
    }
}