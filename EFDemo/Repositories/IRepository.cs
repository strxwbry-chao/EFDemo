using EFDemo.Domain.Specifications;

namespace EFDemo.Domain.Repositories
{
    /// <summary>
    /// Generic repository interface that defines common data access operations.
    /// 
    /// LEARNING NOTE: The Repository pattern provides several benefits:
    /// 1. Abstracts data access logic from business logic
    /// 2. Makes unit testing easier (you can mock the repository)
    /// 3. Centralizes data access logic
    /// 4. Makes it easier to switch data storage technologies
    /// 5. Follows the Dependency Inversion Principle (depend on abstractions, not concretions)
    /// 
    /// The generic <T> means this interface can work with any entity type.
    /// </summary>
    /// <typeparam name="T">The entity type this repository manages</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its ID.
        /// LEARNING NOTE: Async methods are important for scalability. They allow
        /// the thread to be released while waiting for database operations,
        /// enabling the server to handle more concurrent requests.
        /// </summary>
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities of type T.
        /// LEARNING NOTE: Be careful with this method in production - it can return
        /// a lot of data! Consider using paging or specifications to limit results.
        /// </summary>
        /// <returns>All entities</returns>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Gets entities that match a specification.
        /// LEARNING NOTE: This is where the Repository and Specification patterns
        /// work together beautifully. The repository handles data access,
        /// while specifications handle business rules.
        /// </summary>
        /// <param name="spec">The specification to apply</param>
        /// <returns>Entities matching the specification</returns>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        /// <summary>
        /// Gets a single entity that matches a specification.
        /// </summary>
        /// <param name="spec">The specification to apply</param>
        /// <returns>The first entity matching the specification, or null</returns>
        Task<T?> GetEntityWithSpec(ISpecification<T> spec);

        /// <summary>
        /// Counts entities that match a specification.
        /// LEARNING NOTE: This is useful for pagination - you can get the total
        /// count for calculating page numbers without loading all the data.
        /// </summary>
        /// <param name="spec">The specification to apply</param>
        /// <returns>The count of matching entities</returns>
        Task<int> CountAsync(ISpecification<T> spec);

        /// <summary>
        /// Adds a new entity.
        /// LEARNING NOTE: This method doesn't call SaveChanges - that's typically
        /// handled by a Unit of Work pattern or explicitly by the caller.
        /// This gives more control over when database changes are committed.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The updated entity</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity.
        /// LEARNING NOTE: This is a "hard delete" - the record is actually removed
        /// from the database. For many business applications, you might prefer
        /// "soft deletes" where you just mark records as inactive.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        Task DeleteByIdAsync(int id);

        /// <summary>
        /// Saves all pending changes to the database.
        /// LEARNING NOTE: This is where Entity Framework actually executes
        /// the SQL commands. Until this is called, changes are just tracked
        /// in memory by the DbContext.
        /// </summary>
        /// <returns>The number of affected records</returns>
        Task<int> SaveChangesAsync();
    }
}