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
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Gets entities matching the specified business rules.
        /// Core method demonstrating Repository + Specification pattern integration.
        /// </summary>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<T?> GetEntityWithSpec(ISpecification<T> spec);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<int> SaveChangesAsync();
    }
}