using System.Linq.Expressions;

namespace EFDemo.Domain.Specifications
{
    /// <summary>
    /// Helper class for dynamically building predicate expressions.
    /// 
    /// LEARNING NOTE: This utility class helps when you need to build complex
    /// WHERE clauses dynamically. It's especially useful for search forms
    /// where users can select different combinations of filters.
    /// 
    /// The techniques used here are more advanced but are very powerful
    /// for building flexible query systems.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that always returns true.
        /// LEARNING NOTE: This is the starting point for building AND expressions.
        /// true AND something = something, so this acts as a neutral starting point.
        /// </summary>
        public static Expression<Func<T, bool>> True<T>() => x => true;

        /// <summary>
        /// Creates a predicate that always returns false.
        /// LEARNING NOTE: This is the starting point for building OR expressions.
        /// false OR something = something, so this acts as a neutral starting point.
        /// </summary>
        public static Expression<Func<T, bool>> False<T>() => x => false;

        /// <summary>
        /// Combines two expressions with AND logic.
        /// LEARNING NOTE: This method uses Expression.Invoke and Expression.AndAlso
        /// to combine lambda expressions. This is advanced reflection programming.
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left!, right!), parameter);
        }

        /// <summary>
        /// Combines two expressions with OR logic.
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left!, right!), parameter);
        }

        /// <summary>
        /// Helper class for replacing parameter expressions in lambda expressions.
        /// LEARNING NOTE: This is quite advanced - it's using the Visitor pattern
        /// to traverse and modify expression trees. Don't worry if this seems complex;
        /// it's specialized functionality for dynamic query building.
        /// </summary>
        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression? Visit(Expression? node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }
    }
}