namespace Robotico.Specification;

/// <summary>
/// Defines a specification (query criteria) for type <typeparamref name="T"/>.
/// Composable with And, Or, Not. Use with <see cref="Robotico.Domain.IEntity{TId}"/> and value objects in repository/query layers.
/// </summary>
/// <typeparam name="T">The type the specification applies to (e.g. an entity or DTO).</typeparam>
/// <remarks>
/// <para><b>When to use</b>: Use specifications to encapsulate query criteria (e.g. "active orders", "user by email") and compose them with <see cref="SpecificationExtensions.And{T}"/>, <see cref="SpecificationExtensions.Or{T}"/>, <see cref="SpecificationExtensions.Not{T}"/>.</para>
/// <para><b>LINQ / EF Core</b>: When <see cref="ToExpression"/> returns a non-null expression, it can be used with <c>IQueryable.Where(spec.ToExpression())</c> for provider translation. Return null for in-memory-only evaluation.</para>
/// <para><b>Identity</b>: <see cref="Specification.All{T}"/> is identity for And; <see cref="Specification.None{T}"/> is identity for Or.</para>
/// </remarks>
public interface ISpecification<T>
{
    /// <summary>
    /// Returns true if <paramref name="candidate"/> satisfies the specification.
    /// </summary>
    /// <param name="candidate">The candidate to evaluate.</param>
    /// <returns>True when the candidate satisfies the specification; otherwise false.</returns>
    bool IsSatisfiedBy(T candidate);

    /// <summary>
    /// Optional expression for use with EF Core or other LINQ providers.
    /// Return null to use in-memory evaluation only.
    /// </summary>
    /// <returns>An expression that represents this specification for LINQ translation, or null for in-memory only.</returns>
    System.Linq.Expressions.Expression<Func<T, bool>>? ToExpression() => null;
}
