using EFDemo.Domain.Specifications;

namespace EFDemo.Domain.Repositories
{
    /// <summary>
    /// Generic repository interface demonstrating the Repository pattern.
    /// Abstracts data access operations from specific technologies.
    /// </summary>
    /// <typeparam name="T">The entity type this repository manages</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities. Consider using specifications for filtering in production.
        /// </summary>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Gets entities matching the specified business rules.
        /// Core method demonstrating Repository + Specification pattern integration.
        /// </summary>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        /// <summary>
        /// Gets the first entity matching a specification.
        /// </summary>
        Task<T?> GetEntityWithSpec(ISpecification<T> spec);

        /// <summary>
        /// Adds an entity to the context. Call SaveChangesAsync() to persist.
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Removes an entity from the database.
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Persists all pending changes to the database.
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}