namespace Robotico.Specification;

/// <summary>
/// Defines a specification (query criteria) for type <typeparamref name="T"/>.
/// Composable with And, Or, Not.
/// </summary>
/// <typeparam name="T">The type the specification applies to.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Returns true if <paramref name="candidate"/> satisfies the specification.
    /// </summary>
    bool IsSatisfiedBy(T candidate);

    /// <summary>
    /// Optional expression for use with EF Core or other LINQ providers.
    /// Return null to use in-memory evaluation only.
    /// </summary>
    System.Linq.Expressions.Expression<Func<T, bool>>? ToExpression() => null;
}
