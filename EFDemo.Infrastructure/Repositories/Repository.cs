using Microsoft.EntityFrameworkCore;
using EFDemo.Domain.Repositories;
using EFDemo.Domain.Specifications;
using EFDemo.Infrastructure.Data;

namespace EFDemo.Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository implementation using Entity Framework.
    /// Simplified to demonstrate core Repository and Specification pattern integration.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CustomerContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(CustomerContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return (await _dbSet.ToListAsync()).AsReadOnly();
        }

        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return (await ApplySpecification(spec).ToListAsync()).AsReadOnly();
        }

        public virtual async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Core method that applies Specification pattern to build EF queries.
        /// Simplified to focus on essential filtering and ordering.
        /// </summary>
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var query = _dbSet.AsQueryable();

            // Apply filtering criteria
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            // Apply ordering
            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            return query;
        }
    }
}