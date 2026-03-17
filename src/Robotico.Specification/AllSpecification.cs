namespace Robotico.Specification;

internal sealed class AllSpecification<T> : ISpecification<T>
{
    internal static readonly AllSpecification<T> Instance = new();

    private AllSpecification() { }

    public bool IsSatisfiedBy(T candidate) => true;

    public System.Linq.Expressions.Expression<Func<T, bool>>? ToExpression() =>
        System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
            System.Linq.Expressions.Expression.Constant(true),
            System.Linq.Expressions.Expression.Parameter(typeof(T)));
}
