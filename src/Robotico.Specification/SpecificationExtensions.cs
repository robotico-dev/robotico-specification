using System.Linq.Expressions;

namespace Robotico.Specification;

/// <summary>
/// Extension methods for composing specifications.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// And-combines two specifications.
    /// </summary>
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) =>
        new AndSpecification<T>(left, right);

    /// <summary>
    /// Or-combines two specifications.
    /// </summary>
    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right) =>
        new OrSpecification<T>(left, right);

    /// <summary>
    /// Negates a specification.
    /// </summary>
    public static ISpecification<T> Not<T>(this ISpecification<T> spec) =>
        new NotSpecification<T>(spec);
}

internal sealed class AndSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    internal AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) && _right.IsSatisfiedBy(candidate);

    public System.Linq.Expressions.Expression<Func<T, bool>>? ToExpression()
    {
        Expression<Func<T, bool>>? leftExpr = _left.ToExpression();
        Expression<Func<T, bool>>? rightExpr = _right.ToExpression();
        if (leftExpr is null || rightExpr is null)
            return null;
        ParameterExpression param = Expression.Parameter(typeof(T));
        InvocationExpression leftBody = Expression.Invoke(leftExpr, param);
        InvocationExpression rightBody = Expression.Invoke(rightExpr, param);
        BinaryExpression and = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(and, param);
    }
}

internal sealed class OrSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    internal OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);

    public System.Linq.Expressions.Expression<Func<T, bool>>? ToExpression()
    {
        Expression<Func<T, bool>>? leftExpr = _left.ToExpression();
        Expression<Func<T, bool>>? rightExpr = _right.ToExpression();
        if (leftExpr is null || rightExpr is null)
            return null;
        ParameterExpression param = Expression.Parameter(typeof(T));
        InvocationExpression leftBody = Expression.Invoke(leftExpr, param);
        InvocationExpression rightBody = Expression.Invoke(rightExpr, param);
        BinaryExpression or = Expression.OrElse(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(or, param);
    }
}

internal sealed class NotSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _spec;

    internal NotSpecification(ISpecification<T> spec) => _spec = spec;

    public bool IsSatisfiedBy(T candidate) => !_spec.IsSatisfiedBy(candidate);

    public System.Linq.Expressions.Expression<Func<T, bool>>? ToExpression()
    {
        Expression<Func<T, bool>>? expr = _spec.ToExpression();
        if (expr is null)
            return null;
        ParameterExpression param = Expression.Parameter(typeof(T));
        UnaryExpression body = Expression.Not(Expression.Invoke(expr, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
