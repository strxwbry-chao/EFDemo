using System.Linq.Expressions;

namespace EFDemo.Domain.Specifications
{
    /// <summary>
    /// Base implementation of the Specification pattern providing common functionality.
    /// Simplified to focus on core filtering and ordering capabilities.
    /// </summary>
    /// <typeparam name="T">The entity type this specification applies to</typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Constructor that accepts the criteria expression.
        /// </summary>
        protected BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }

        protected BaseSpecification()
        {
            Criteria = null;
        }

        public Expression<Func<T, bool>>? Criteria { get; }

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
    }
}