namespace Robotico.Specification;

internal sealed class NoneSpecification<T> : ISpecification<T>
{
    internal static readonly NoneSpecification<T> Instance = new();

    private NoneSpecification() { }

    public bool IsSatisfiedBy(T candidate) => false;

    public System.Linq.Expressions.Expression<Func<T, bool>>? ToExpression() =>
        System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
            System.Linq.Expressions.Expression.Constant(false),
            System.Linq.Expressions.Expression.Parameter(typeof(T)));
}
