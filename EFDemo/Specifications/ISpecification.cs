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
        Expression<Func<T, bool>>? Criteria { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }
    }
}