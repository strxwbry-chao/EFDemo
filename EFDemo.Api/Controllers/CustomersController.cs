using Microsoft.AspNetCore.Mvc;
using EFDemo.Domain;
using EFDemo.Infrastructure.Services;

namespace EFDemo.Api.Controllers
{
    /// <summary>
    /// API Controller for Customer operations.
    /// 
    /// LEARNING NOTE: This controller demonstrates the API layer in a layered architecture.
    /// Controllers should be thin and focused on HTTP concerns:
    /// 1. Route handling and parameter binding
    /// 2. HTTP status codes and response formatting
    /// 3. Input validation (basic)
    /// 4. Exception handling and error responses
    /// 
    /// Business logic should be delegated to service classes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        /// <summary>
        /// Constructor with dependency injection.
        /// LEARNING NOTE: The controller depends on the service interface,
        /// not the concrete implementation. This makes it testable and
        /// follows the Dependency Inversion Principle.
        /// </summary>
        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all active customers.
        /// GET /api/customers
        /// 
        /// LEARNING NOTE: This endpoint demonstrates a simple GET operation.
        /// It returns 200 OK with the data, or 500 if something goes wrong.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetActiveCustomers()
        {
            try
            {
                _logger.LogInformation("Getting all active customers");
                var customers = await _customerService.GetActiveCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active customers");
                return StatusCode(500, "An error occurred while retrieving customers");
            }
        }

        /// <summary>
        /// Gets a customer by ID.
        /// GET /api/customers/{id}
        /// 
        /// LEARNING NOTE: This shows how to handle route parameters and
        /// return different HTTP status codes based on the result.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Customer ID must be greater than 0");
                }

                _logger.LogInformation("Getting customer with ID: {CustomerId}", id);
                var customer = await _customerService.GetCustomerByIdAsync(id);
                
                if (customer == null)
                {
                    _logger.LogWarning("Customer with ID {CustomerId} not found", id);
                    return NotFound($"Customer with ID {id} not found");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer with ID: {CustomerId}", id);
                return StatusCode(500, "An error occurred while retrieving the customer");
            }
        }

        /// <summary>
        /// Searches customers with pagination.
        /// GET /api/customers/search?searchTerm=john&pageNumber=1&pageSize=10&isActive=true
        /// 
        /// LEARNING NOTE: This demonstrates query parameter binding and
        /// returning structured data with pagination information.
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<Customer>>> SearchCustomers(
            [FromQuery] string? searchTerm = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Basic input validation
                if (pageNumber < 1)
                {
                    return BadRequest("Page number must be greater than 0");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100");
                }

                var searchCriteria = new CustomerSearchDto
                {
                    SearchTerm = searchTerm,
                    IsActive = isActive,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                _logger.LogInformation("Searching customers with criteria: {SearchCriteria}", searchCriteria);
                var result = await _customerService.SearchCustomersAsync(searchCriteria);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching customers");
                return StatusCode(500, "An error occurred while searching customers");
            }
        }

        /// <summary>
        /// Creates a new customer.
        /// POST /api/customers
        /// 
        /// LEARNING NOTE: This demonstrates POST operations, request body binding,
        /// and returning 201 Created with the location of the new resource.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CreateCustomerDto createDto)
        {
            try
            {
                // Basic input validation
                if (string.IsNullOrWhiteSpace(createDto.FirstName))
                {
                    return BadRequest("First name is required");
                }

                if (string.IsNullOrWhiteSpace(createDto.LastName))
                {
                    return BadRequest("Last name is required");
                }

                _logger.LogInformation("Creating customer: {FirstName} {LastName}", createDto.FirstName, createDto.LastName);
                var customer = await _customerService.CreateCustomerAsync(createDto);
                
                // Return 201 Created with the location of the new resource
                return CreatedAtAction(
                    nameof(GetCustomer), 
                    new { id = customer.Id }, 
                    customer);
            }
            catch (InvalidOperationException ex)
            {
                // Business rule violation (e.g., duplicate name)
                _logger.LogWarning(ex, "Business rule violation while creating customer");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                return StatusCode(500, "An error occurred while creating the customer");
            }
        }

        /// <summary>
        /// Updates an existing customer.
        /// PUT /api/customers/{id}
        /// 
        /// LEARNING NOTE: PUT is used for full updates. The ID in the route
        /// should match the ID in the request body for consistency.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, [FromBody] UpdateCustomerDto updateDto)
        {
            try
            {
                if (id != updateDto.Id)
                {
                    return BadRequest("Route ID does not match request body ID");
                }

                if (string.IsNullOrWhiteSpace(updateDto.FirstName))
                {
                    return BadRequest("First name is required");
                }

                if (string.IsNullOrWhiteSpace(updateDto.LastName))
                {
                    return BadRequest("Last name is required");
                }

                _logger.LogInformation("Updating customer with ID: {CustomerId}", id);
                var customer = await _customerService.UpdateCustomerAsync(updateDto);
                
                return Ok(customer);
            }
            catch (ArgumentException ex)
            {
                // Customer not found
                _logger.LogWarning(ex, "Customer not found for update: {CustomerId}", id);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Business rule violation
                _logger.LogWarning(ex, "Business rule violation while updating customer");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer with ID: {CustomerId}", id);
                return StatusCode(500, "An error occurred while updating the customer");
            }
        }

        /// <summary>
        /// Deactivates a customer (soft delete).
        /// POST /api/customers/{id}/deactivate
        /// 
        /// LEARNING NOTE: This uses POST for an action that changes state.
        /// We use a specific endpoint rather than DELETE to make the
        /// business intent clear (deactivate vs. permanent delete).
        /// </summary>
        [HttpPost("{id:int}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeactivateCustomer(int id)
        {
            try
            {
                _logger.LogInformation("Deactivating customer with ID: {CustomerId}", id);
                var success = await _customerService.DeactivateCustomerAsync(id);
                
                if (!success)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                return Ok($"Customer {id} has been deactivated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating customer with ID: {CustomerId}", id);
                return StatusCode(500, "An error occurred while deactivating the customer");
            }
        }

        /// <summary>
        /// Activates a customer.
        /// POST /api/customers/{id}/activate
        /// </summary>
        [HttpPost("{id:int}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ActivateCustomer(int id)
        {
            try
            {
                _logger.LogInformation("Activating customer with ID: {CustomerId}", id);
                var success = await _customerService.ActivateCustomerAsync(id);
                
                if (!success)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                return Ok($"Customer {id} has been activated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating customer with ID: {CustomerId}", id);
                return StatusCode(500, "An error occurred while activating the customer");
            }
        }

        /// <summary>
        /// Permanently deletes a customer (hard delete).
        /// DELETE /api/customers/{id}
        /// 
        /// LEARNING NOTE: DELETE is used for permanent removal.
        /// In many business applications, you might want to restrict
        /// this operation or require special permissions.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                _logger.LogInformation("Permanently deleting customer with ID: {CustomerId}", id);
                var success = await _customerService.DeleteCustomerAsync(id);
                
                if (!success)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                // Return 204 No Content for successful deletion
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer with ID: {CustomerId}", id);
                return StatusCode(500, "An error occurred while deleting the customer");
            }
        }
    }
}