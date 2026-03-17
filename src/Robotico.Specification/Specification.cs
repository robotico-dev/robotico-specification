using System.Diagnostics.CodeAnalysis;

namespace Robotico.Specification;

/// <summary>
/// Factory for common specifications: All (satisfied by every candidate), None (satisfied by no candidate), and from expression.
/// </summary>
[SuppressMessage("Naming", "CA1724:Type names should not match namespaces", Justification = "Specification is the standard domain term; namespace is package identity.")]
public static class Specification
{
    /// <summary>
    /// A specification satisfied by every candidate. Use as identity for And; as default when optional criteria is None.
    /// </summary>
    /// <typeparam name="T">The type the specification applies to.</typeparam>
    public static ISpecification<T> All<T>() => AllSpecification<T>.Instance;

    /// <summary>
    /// A specification satisfied by no candidate. Use as identity for Or.
    /// </summary>
    /// <typeparam name="T">The type the specification applies to.</typeparam>
    public static ISpecification<T> None<T>() => NoneSpecification<T>.Instance;

    /// <summary>
    /// Creates a specification from a predicate expression (for LINQ/EF Core).
    /// </summary>
    /// <param name="expression">The predicate expression.</param>
    /// <typeparam name="T">The type the specification applies to.</typeparam>
    /// <returns>An <see cref="ExpressionSpecification{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression"/> is null.</exception>
    public static ISpecification<T> FromExpression<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) =>
        new ExpressionSpecification<T>(expression);
}
