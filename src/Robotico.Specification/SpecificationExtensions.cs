namespace Robotico.Specification;

/// <summary>
/// Extension methods for composing specifications.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// And-combines two specifications.
    /// </summary>
    /// <param name="left">The first specification.</param>
    /// <param name="right">The second specification.</param>
    /// <returns>A specification that is satisfied when both are satisfied.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="left"/> or <paramref name="right"/> is null.</exception>
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return new AndSpecification<T>(left, right);
    }

    /// <summary>
    /// Or-combines two specifications.
    /// </summary>
    /// <param name="left">The first specification.</param>
    /// <param name="right">The second specification.</param>
    /// <returns>A specification that is satisfied when either is satisfied.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="left"/> or <paramref name="right"/> is null.</exception>
    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return new OrSpecification<T>(left, right);
    }

    /// <summary>
    /// Negates a specification.
    /// </summary>
    /// <param name="spec">The specification to negate.</param>
    /// <returns>A specification that is satisfied when the given specification is not satisfied.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="spec"/> is null.</exception>
    public static ISpecification<T> Not<T>(this ISpecification<T> spec)
    {
        ArgumentNullException.ThrowIfNull(spec);
        return new NotSpecification<T>(spec);
    }
}
