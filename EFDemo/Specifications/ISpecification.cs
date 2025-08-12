using System.Linq.Expressions;

namespace EFDemo.Domain.Specifications
{
    /// <summary>
    /// Core Specification pattern interface for encapsulating business query rules.
    /// Focuses on essential filtering and ordering capabilities.
    /// </summary>
    /// <typeparam name="T">The entity type this specification applies to</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// The criteria expression that defines the business rule (WHERE clause).
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// OrderBy expression for sorting results.
        /// </summary>
        Expression<Func<T, object>>? OrderBy { get; }

        /// <summary>
        /// OrderByDescending expression for sorting results in descending order.
        /// </summary>
        Expression<Func<T, object>>? OrderByDescending { get; }
    }
}