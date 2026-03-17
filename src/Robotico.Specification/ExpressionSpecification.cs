using System.Linq.Expressions;

namespace Robotico.Specification;

/// <summary>
/// A specification that wraps a predicate expression. Use for LINQ/EF Core: <see cref="ToExpression"/> returns the same expression for provider translation.
/// Works with <see cref="Robotico.Domain.IEntity{TId}"/> and other domain types.
/// </summary>
/// <typeparam name="T">The type the specification applies to (e.g. an entity or value object).</typeparam>
/// <remarks>
/// <para><b>When to use</b>: Use <see cref="ExpressionSpecification{T}"/> when you have an <see cref="Expression{TDelegate}"/> (e.g. from a repository or EF Core query) and want to compose it with other specifications via <see cref="SpecificationExtensions.And{T}"/>, <see cref="SpecificationExtensions.Or{T}"/>, <see cref="SpecificationExtensions.Not{T}"/>.</para>
/// <para><b>LINQ</b>: Composing with other expression-based specs preserves <see cref="ToExpression"/> for use in <c>IQueryable.Where(spec.ToExpression())</c>.</para>
/// </remarks>
public sealed class ExpressionSpecification<T> : ISpecification<T>
{
    private readonly Expression<Func<T, bool>> _expression;
    private readonly Lazy<Func<T, bool>> _compiled;

    /// <summary>
    /// Creates a specification from a predicate expression.
    /// </summary>
    /// <param name="expression">The predicate expression (e.g. <c>x => x.Name == "A"</c>).</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression"/> is null.</exception>
    public ExpressionSpecification(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _expression = expression;
        _compiled = new Lazy<Func<T, bool>>(() => _expression.Compile());
    }

    /// <inheritdoc />
    public bool IsSatisfiedBy(T candidate) => _compiled.Value(candidate);

    /// <inheritdoc />
    public Expression<Func<T, bool>>? ToExpression() => _expression;
}
