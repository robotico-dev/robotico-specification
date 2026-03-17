namespace Robotico.Specification;

/// <summary>
/// Extensions for optional criteria: convert <see cref="Robotico.Option.Option{T}"/> of specification to a single <see cref="ISpecification{T}"/>.
/// None is treated as "no filter" (all candidates satisfy); Some(spec) uses the given specification.
/// </summary>
/// <remarks>
/// <para><b>When to use</b>: Use when a filter is optional (e.g. from query string or UI). <c>optionalFilter.ToSpecification()</c> yields <see cref="Specification.All{T}"/> when None, or the inner spec when Some.</para>
/// </remarks>
public static class OptionSpecificationExtensions
{
    /// <summary>
    /// Converts an optional specification to a concrete specification: None becomes All (satisfied by every candidate); Some(spec) returns spec.
    /// </summary>
    /// <param name="option">The optional specification (e.g. from optional search criteria).</param>
    /// <typeparam name="T">The type the specification applies to.</typeparam>
    /// <returns>All specification if None or if the inner value is null; otherwise the inner specification. Note: <see cref="Robotico.Option.Option{T}.Some"/> does not accept null; the null case applies only if the option was constructed by other means.</returns>
    public static ISpecification<T> ToSpecification<T>(this Robotico.Option.Option<ISpecification<T>> option)
    {
        return option.TryGetValue(out ISpecification<T>? spec) && spec is not null ? spec : Specification.All<T>();
    }
}
