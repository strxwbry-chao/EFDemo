using Microsoft.AspNetCore.Mvc;
using EFDemo.Domain;
using EFDemo.Infrastructure.Services;

namespace EFDemo.Api.Controllers
{
    /// <summary>
    /// Customer API Controller demonstrating Repository and Specification patterns.
    /// Shows how patterns flow from API layer through Service layer to Repository layer.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// GET /api/customers - Returns all active customers
        /// Demonstrates: Repository pattern with Specification pattern (ActiveCustomersSpecification)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetActiveCustomers()
        {
            var customers = await _customerService.GetActiveCustomersAsync();
            return Ok(customers);
        }

        /// <summary>
        /// GET /api/customers/{id} - Returns specific customer
        /// Demonstrates: Basic repository usage
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        /// <summary>
        /// GET /api/customers/search?term={searchTerm} - Search customers by name
        /// Demonstrates: Parameterized Specification pattern (CustomersByNameSpecification)
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term is required");

            var customers = await _customerService.SearchCustomersAsync(term);
            return Ok(customers);
        }

        /// <summary>
        /// POST /api/customers - Create new customer
        /// Demonstrates: Service layer coordination with Repository pattern
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CreateCustomerDto createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.FirstName) || string.IsNullOrWhiteSpace(createDto.LastName))
                return BadRequest("First name and last name are required");

            var customer = await _customerService.CreateCustomerAsync(createDto);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// PUT /api/customers/{id} - Update existing customer
        /// Demonstrates: Repository pattern with entity updates
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, [FromBody] UpdateCustomerDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest("Route ID does not match request body ID");

            try
            {
                var customer = await _customerService.UpdateCustomerAsync(updateDto);
                return Ok(customer);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// DELETE /api/customers/{id} - Delete customer
        /// Demonstrates: Repository pattern with deletion
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var success = await _customerService.DeleteCustomerAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}