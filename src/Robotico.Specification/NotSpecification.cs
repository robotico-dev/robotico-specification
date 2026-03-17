using System.Linq.Expressions;

namespace Robotico.Specification;

internal sealed class NotSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _spec;

    internal NotSpecification(ISpecification<T> spec) => _spec = spec;

    public bool IsSatisfiedBy(T candidate) => !_spec.IsSatisfiedBy(candidate);

    public System.Linq.Expressions.Expression<Func<T, bool>>? ToExpression()
    {
        Expression<Func<T, bool>>? expr = _spec.ToExpression();
        if (expr is null)
        {
            return null;
        }

        ParameterExpression param = Expression.Parameter(typeof(T));
        UnaryExpression body = Expression.Not(Expression.Invoke(expr, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
