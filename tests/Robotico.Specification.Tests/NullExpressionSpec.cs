using System.Linq.Expressions;

namespace Robotico.Specification.Tests;

internal sealed class NullExpressionSpec : ISpecification<int>
{
    public bool IsSatisfiedBy(int candidate) => false;
    public Expression<Func<int, bool>>? ToExpression() => null;
}
