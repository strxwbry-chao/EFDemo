using Microsoft.EntityFrameworkCore;
using EFDemo.Domain.Repositories;
using EFDemo.Domain.Specifications;
using EFDemo.Infrastructure.Data;

namespace EFDemo.Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository implementation using Entity Framework.
    /// 
    /// LEARNING NOTE: This class implements the generic repository interface
    /// using Entity Framework as the data access technology. The beauty of the
    /// repository pattern is that we could swap this implementation for a
    /// different one (like Dapper, ADO.NET, or even a mock for testing)
    /// without changing any business logic.
    /// 
    /// The generic nature means this single class can handle any entity type.
    /// </summary>
    /// <typeparam name="T">The entity type this repository manages</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The Entity Framework context for database operations.
        /// LEARNING NOTE: We use 'protected readonly' so that derived classes
        /// can access the context but can't reassign it.
        /// </summary>
        protected readonly CustomerContext _context;

        /// <summary>
        /// The DbSet for the entity type T.
        /// LEARNING NOTE: DbSet<T> represents the table for entity T in the database.
        /// We get this from the context and use it for all operations.
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor that takes the DbContext.
        /// LEARNING NOTE: This is dependency injection in action. The repository
        /// depends on the DbContext abstraction, not a specific implementation.
        /// </summary>
        /// <param name="context">The Entity Framework context</param>
        public Repository(CustomerContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            // LEARNING NOTE: FindAsync is optimized for primary key lookups
            // and will check the context's change tracker first before
            // hitting the database.
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            // LEARNING NOTE: ToListAsync() executes the query and returns
            // a materialized list. AsReadOnly() prevents callers from
            // modifying the collection, following the principle of
            // defensive programming.
            return (await _dbSet.ToListAsync()).AsReadOnly();
        }

        /// <summary>
        /// Gets entities that match a specification.
        /// LEARNING NOTE: This is where the magic happens! The specification
        /// pattern provides the business rules, and this method applies them
        /// to build the appropriate Entity Framework query.
        /// </summary>
        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return (await ApplySpecification(spec).ToListAsync()).AsReadOnly();
        }

        /// <summary>
        /// Gets a single entity that matches a specification.
        /// </summary>
        public virtual async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Counts entities that match a specification.
        /// </summary>
        public virtual async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        public virtual async Task<T> AddAsync(T entity)
        {
            // LEARNING NOTE: AddAsync adds the entity to the context's change tracker
            // but doesn't save to database until SaveChangesAsync is called.
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        public virtual async Task<T> UpdateAsync(T entity)
        {
            // LEARNING NOTE: Update tells EF to mark all properties as modified.
            // For more granular control, you might want to attach the entity
            // and only mark specific properties as modified.
            _dbSet.Update(entity);
            return await Task.FromResult(entity);
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        public virtual async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Applies a specification to a queryable to build the appropriate query.
        /// LEARNING NOTE: This is the heart of the specification pattern implementation.
        /// It takes the business rules defined in the specification and applies them
        /// to build an Entity Framework query.
        /// 
        /// This method is private because it's an implementation detail that
        /// callers don't need to know about.
        /// </summary>
        /// <param name="spec">The specification to apply</param>
        /// <returns>A queryable with the specification applied</returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            // Start with the basic DbSet
            var query = _dbSet.AsQueryable();

            // Apply the criteria (WHERE clause)
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply includes (JOIN operations for eager loading)
            // LEARNING NOTE: Includes tell EF to load related data in the same query
            // instead of making separate round trips (avoiding N+1 query problems)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            // Apply ordering
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // Apply paging
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }
    }
}