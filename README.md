# Entity Framework Demo with Repository and Specification Patterns

This project demonstrates a complete .NET 9 Web API implementation using Entity Framework Core with the Repository and Specification patterns. It's designed as both a working application and an educational resource for junior developers.

## ??? Architecture Overview

The solution follows a layered architecture approach with clear separation of concerns:

### **Domain Layer** (`EFDemo.Domain`)
- **Entities**: Contains the `Customer` entity with business logic
- **Specifications**: Implements the Specification pattern for reusable business rules
- **Repository Interfaces**: Defines contracts for data access

### **Infrastructure Layer** (`EFDemo.Infrastructure`)
- **Data**: Entity Framework DbContext and database configuration
- **Repository Implementations**: Concrete implementations of repository interfaces
- **Services**: Business logic layer that orchestrates operations

### **API Layer** (`EFDemo.Api`)
- **Controllers**: HTTP endpoints for customer operations
- **DTOs**: Data transfer objects for API communication
- **Configuration**: Dependency injection and middleware setup

## ?? Design Patterns Implemented

### 1. **Repository Pattern**
- **Purpose**: Abstracts data access logic from business logic
- **Benefits**: Makes unit testing easier, centralizes data access, enables technology switching
- **Files**: `IRepository.cs`, `Repository.cs`, `ICustomerRepository.cs`, `CustomerRepository.cs`

### 2. **Specification Pattern**
- **Purpose**: Encapsulates business rules in reusable, composable objects
- **Benefits**: Keeps query logic out of repositories, enables business rule testing, supports complex queries
- **Files**: `ISpecification.cs`, `BaseSpecification.cs`, `CustomerSpecifications.cs`

### 3. **Dependency Injection**
- **Purpose**: Inverts dependencies to promote loose coupling
- **Benefits**: Improves testability, flexibility, and maintainability
- **Implementation**: Throughout the application, configured in `Program.cs`

### 4. **Service Layer Pattern**
- **Purpose**: Encapsulates business logic and coordinates between layers
- **Benefits**: Keeps controllers thin, centralizes business rules, manages transactions
- **Files**: `ICustomerService.cs`, `CustomerService.cs`

## ?? Getting Started

### Prerequisites
- .NET 9 SDK
- Any IDE (Visual Studio, VS Code, Rider)

### Running the Application

1. **Clone and Navigate**
   ```bash
   cd EFDemo.Api
   ```

2. **Run the Application**
   ```bash
   dotnet run
   ```

3. **Access the API**
   - Swagger UI: `https://localhost:7xxx/swagger` (port varies)
   - API Base URL: `https://localhost:7xxx/api`

### Sample Data
The application automatically creates a SQLite database (`customers.db`) and seeds it with sample customer data including both active and inactive customers.

## ?? API Endpoints

### **GET** `/api/customers`
- Returns all active customers
- **Learning Focus**: Basic repository usage, specification pattern

### **GET** `/api/customers/{id}`
- Returns a specific customer by ID
- **Learning Focus**: Route parameters, error handling

### **GET** `/api/customers/search`
- Searches customers with pagination
- **Query Parameters**: `searchTerm`, `isActive`, `pageNumber`, `pageSize`
- **Learning Focus**: Complex specifications, pagination

### **POST** `/api/customers`
- Creates a new customer
- **Learning Focus**: DTO mapping, business rule validation

### **PUT** `/api/customers/{id}`
- Updates an existing customer
- **Learning Focus**: Entity updates, business methods

### **POST** `/api/customers/{id}/activate`
- Activates a customer
- **Learning Focus**: Domain methods, state changes

### **POST** `/api/customers/{id}/deactivate`
- Deactivates a customer (soft delete)
- **Learning Focus**: Soft deletes vs hard deletes

### **DELETE** `/api/customers/{id}`
- Permanently deletes a customer
- **Learning Focus**: Hard deletes, data integrity

## ?? Learning Objectives

This project is designed to teach:

### **For Junior Developers**
1. **Clean Architecture**: How to structure a .NET application with proper separation of concerns
2. **Entity Framework**: Database operations, change tracking, migrations
3. **Design Patterns**: Repository, Specification, Dependency Injection, Service Layer
4. **API Design**: RESTful endpoints, HTTP status codes, error handling
5. **Best Practices**: Nullable reference types, async/await, logging

### **For Intermediate Developers**
1. **Advanced EF Core**: Fluent API configuration, performance optimization
2. **Pattern Implementation**: Proper specification pattern with expression trees
3. **Domain-Driven Design**: Rich domain models with business logic
4. **Testing Strategies**: How the architecture supports unit testing

## ??? Key Technical Concepts Demonstrated

### **Nullable Reference Types**
```csharp
public string FirstName { get; set; } = string.Empty; // Non-nullable with default
public DateTime? UpdatedAt { get; set; }              // Nullable datetime
```

### **Expression Trees for Specifications**
```csharp
public class ActiveCustomersSpecification : BaseSpecification<Customer>
{
    public ActiveCustomersSpecification() : base(x => x.IsActive)
    {
        ApplyOrderBy(x => x.LastName);
    }
}
```

### **Dependency Injection Configuration**
```csharp
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
```

### **Business Logic in Domain Entities**
```csharp
public void Deactivate()
{
    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
}
```

## ?? Educational Features

Every file contains extensive comments explaining:
- **What** the code does
- **Why** specific decisions were made
- **How** patterns are implemented
- **When** to use different approaches
- **Learning Notes** for junior developers

## ?? Testing the Application

### Using Swagger UI
1. Start the application
2. Navigate to the Swagger UI
3. Try the different endpoints with the pre-seeded data

### Sample Test Scenarios
1. **Search for customers**: Try searching for "John" or "Smith"
2. **Pagination**: Use different page numbers and page sizes
3. **Create a customer**: Add a new customer with the POST endpoint
4. **Update a customer**: Modify an existing customer's information
5. **Deactivate/Activate**: Change customer status using the action endpoints

## ?? Project Structure

```
EFDemo/
??? EFDemo.Domain/
?   ??? Customer.cs (Domain Entity)
?   ??? Specifications/
?   ?   ??? ISpecification.cs
?   ?   ??? BaseSpecification.cs
?   ?   ??? CustomerSpecifications.cs
?   ??? Repositories/
?       ??? IRepository.cs
?       ??? ICustomerRepository.cs
??? EFDemo.Infrastructure/
?   ??? Data/
?   ?   ??? CustomerContext.cs (EF DbContext)
?   ?   ??? DatabaseSeeder.cs
?   ??? Repositories/
?   ?   ??? Repository.cs
?   ?   ??? CustomerRepository.cs
?   ??? Services/
?       ??? CustomerDtos.cs
?       ??? ICustomerService.cs
?       ??? CustomerService.cs
??? EFDemo.Api/
    ??? Controllers/
    ?   ??? CustomersController.cs
    ??? Program.cs (Configuration)
```

## ?? Next Steps for Learning

1. **Add Unit Tests**: Create tests for specifications, services, and repositories
2. **Add Authentication**: Implement JWT authentication and authorization
3. **Add Validation**: Use FluentValidation for input validation
4. **Add Caching**: Implement Redis or in-memory caching
5. **Add Logging**: Use Serilog for structured logging
6. **Add Health Checks**: Monitor application health
7. **Add More Entities**: Extend with Orders, Products, etc.
8. **Add Advanced Specifications**: Implement specification composition

## ?? Contributing

This is an educational project. Feel free to:
- Add more examples
- Improve documentation
- Fix issues or typos
- Suggest better patterns or practices

---

**Remember**: This project prioritizes education over brevity. In production code, you might choose different approaches based on specific requirements, but the patterns demonstrated here provide a solid foundation for scalable, maintainable applications.