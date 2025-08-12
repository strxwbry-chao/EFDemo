# Test Examples for Repository & Specification Patterns

This project demonstrates testing strategies for the Repository and Specification patterns.

## ?? Unit Test: ActiveCustomersSpecificationTests

**Location:** `EFDemo.Tests\Unit\ActiveCustomersSpecificationTests.cs`

**Purpose:** Tests the Specification pattern in isolation

### What it demonstrates:
- ? **Pure Unit Test**: No database, no external dependencies
- ? **Fast Execution**: Tests business logic using in-memory objects
- ? **Narrow Focus**: Tests only the specification's criteria and ordering logic
- ? **Expression Testing**: Shows how to test lambda expressions by compiling them

### Key Learning Points:
```csharp
// Testing the business rule logic directly
var result = specification.Criteria!.Compile()(activeCustomer);
Assert.True(result);
```

The unit test verifies that:
1. Active customers return `true` for the criteria
2. Inactive customers return `false` for the criteria  
3. Ordering is correctly configured

---

## ?? Integration Test: CustomerRepositoryIntegrationTests

**Location:** `EFDemo.Tests\Integration\CustomerRepositoryIntegrationTests.cs`

**Purpose:** Tests Repository + Specification + Entity Framework working together

### What it demonstrates:
- ? **Multi-Component Integration**: Repository, Specification, EF Core, In-Memory Database
- ? **End-to-End Flow**: From repository method through specification to SQL query
- ? **Real Database Simulation**: Uses EF Core In-Memory provider
- ? **Pattern Verification**: Confirms Repository and Specification patterns work together

### Key Learning Points:
```csharp
// Testing the full integration flow
var activeCustomers = await _repository.GetActiveCustomersAsync();
Assert.Equal(2, activeCustomers.Count);
Assert.All(activeCustomers, customer => Assert.True(customer.IsActive));
```

The integration test verifies that:
1. Repository correctly applies specifications to EF queries
2. Database operations work as expected
3. Ordering and filtering work end-to-end
4. Generic and specific repository methods produce consistent results

---

## ?? Key Differences

| **Unit Test** | **Integration Test** |
|---------------|---------------------|
| Tests business logic only | Tests components working together |
| No database dependencies | Uses in-memory database |
| Fast execution | Slower but more comprehensive |
| Narrow, focused scope | Broader, integration scope |
| Easy to debug | Tests real-world scenarios |

---

## ?? Running the Tests

```bash
# Run all tests
dotnet test

# Run only unit tests
dotnet test --filter "FullyQualifiedName~Unit"

# Run only integration tests  
dotnet test --filter "FullyQualifiedName~Integration"
```

---

## ?? Learning Takeaways

1. **Unit Tests** should focus on testing business logic in isolation
2. **Integration Tests** verify that patterns work correctly with real infrastructure
3. **Specification Pattern** is easily unit testable by compiling expressions
4. **Repository Pattern** requires integration testing to verify EF Core integration
5. **In-Memory Database** provides fast integration testing without external dependencies

These tests demonstrate how proper architecture (Repository + Specification patterns) makes code both unit testable and integration testable.