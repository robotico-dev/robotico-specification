using System.Linq.Expressions;

namespace Robotico.Specification;

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
        {
            return null;
        }

        ParameterExpression param = Expression.Parameter(typeof(T));
        InvocationExpression leftBody = Expression.Invoke(leftExpr, param);
        InvocationExpression rightBody = Expression.Invoke(rightExpr, param);
        BinaryExpression and = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(and, param);
    }
}
