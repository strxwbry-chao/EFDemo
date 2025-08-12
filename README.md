# Repository & Specification Pattern Demo

A focused demonstration of the **Repository Pattern** and **Specification Pattern** in .NET 9 with Entity Framework Core.

## ?? Learning Objectives

This simplified demo showcases:
1. **Repository Pattern** - Abstracting data access logic
2. **Specification Pattern** - Encapsulating business query logic
3. **Clean Architecture** - Proper separation of concerns
4. **Entity Framework Integration** - Core pattern integration

---

## ??? Architecture Overview

```
???????????????????    ???????????????????    ???????????????????
?   API Layer     ??????  Service Layer  ?????? Repository Layer?
?  (Controllers)  ?    ?   (Business)    ?    ?  (Data Access)  ?
???????????????????    ???????????????????    ???????????????????
                                                        ?
                                               ???????????????????
                                               ? Specification   ?
                                               ?    Pattern      ?
                                               ???????????????????
```

### Domain Layer (`EFDemo.Domain`)
- **Customer Entity**: Simple domain object (Id, FirstName, LastName, IsActive)
- **Repository Interfaces**: Contracts for data access
- **Specifications**: Business query logic encapsulation

### Infrastructure Layer (`EFDemo.Infrastructure`)
- **DbContext**: Entity Framework configuration
- **Repository Implementations**: Concrete data access using EF
- **Services**: Business logic coordination

### API Layer (`EFDemo.Api`)
- **Controllers**: HTTP endpoints
- **DTOs**: Data transfer objects

---

## ?? Repository Pattern Demonstration

### Generic Repository Interface
```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);  // Core method
    Task<T> AddAsync(T entity);
    Task<int> SaveChangesAsync();
}
```

### Customer-Specific Repository
```csharp
public interface ICustomerRepository : IRepository<Customer>
{
    Task<IReadOnlyList<Customer>> GetActiveCustomersAsync();
    Task<IReadOnlyList<Customer>> SearchByNameAsync(string searchTerm);
}
```

**Key Benefits:**
- ? Abstracts data access technology
- ? Centralizes data access logic
- ? Enables easy unit testing with mocks
- ? Follows Dependency Inversion Principle

---

## ?? Specification Pattern Demonstration

### Core Specification Interface
```csharp
public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }       // WHERE clause
    Expression<Func<T, object>>? OrderBy { get; }      // ORDER BY
    Expression<Func<T, object>>? OrderByDescending { get; }
}
```

### Example Specifications
```csharp
// Business rule: "Get active customers"
public class ActiveCustomersSpecification : BaseSpecification<Customer>
{
    public ActiveCustomersSpecification() : base(x => x.IsActive)
    {
        ApplyOrderBy(x => x.LastName);
    }
}

// Business rule: "Search customers by name"
public class CustomersByNameSpecification : BaseSpecification<Customer>
{
    public CustomersByNameSpecification(string searchTerm) 
        : base(x => x.FirstName.Contains(searchTerm) || x.LastName.Contains(searchTerm))
    {
        ApplyOrderBy(x => x.LastName);
    }
}
```

**Key Benefits:**
- ? Encapsulates business query logic
- ? Reusable across different parts of application
- ? Testable in isolation
- ? Keeps repositories focused on data access

---

## ?? Running the Demo

1. **Start the application:**
   ```bash
   cd EFDemo.Api
   dotnet run
   ```

2. **Access Swagger UI:**
   ```
   https://localhost:7xxx/swagger
   ```

3. **Database:** SQLite database (`customers.db`) is created automatically with sample data.

---

## ?? API Endpoints for Testing

### Core Pattern Demonstrations:
- `GET /api/customers` - Uses `ActiveCustomersSpecification`
- `GET /api/customers/{id}` - Basic repository lookup
- `GET /api/customers/search?term=john` - Uses `CustomersByNameSpecification`

### CRUD Operations:
- `POST /api/customers` - Create customer
- `PUT /api/customers/{id}` - Update customer  
- `DELETE /api/customers/{id}` - Delete customer

---

## ?? Key Takeaways

1. **Repository Pattern** abstracts data access and promotes testability
2. **Specification Pattern** encapsulates business query logic  
3. **Separation of Concerns** keeps each layer focused on its responsibility
4. **Clean Architecture** makes code maintainable and scalable

---

## ?? Simplified Code Structure

```
EFDemo/
??? Domain/
?   ??? Customer.cs (Entity)
?   ??? Repositories/ (Interfaces)
?   ??? Specifications/ (Core Business Rules)
??? Infrastructure/
?   ??? Data/ (EF DbContext)
?   ??? Repositories/ (Implementations)
?   ??? Services/ (Business Logic)
??? Api/
    ??? Controllers/ (HTTP Endpoints)
    ??? Program.cs (Configuration)
```

**Simplified Focus:** This version removes eager loading and pagination complexity to focus purely on demonstrating the core Repository and Specification patterns.