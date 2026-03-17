using System.Linq.Expressions;

namespace Robotico.Specification.Tests;

/// <summary>
/// Tests for ToExpression() on all specification types to achieve full line coverage (All, None, And, Or, Not, ExpressionSpecification).
/// </summary>
public sealed class SpecificationToExpressionTests
{
    [Fact]
    public void All_ToExpression_returns_non_null_expression_that_evaluates_true()
    {
        ISpecification<int> all = Specification.All<int>();
        Expression<Func<int, bool>>? expr = all.ToExpression();
        Assert.NotNull(expr);
        Func<int, bool> compiled = expr.Compile();
        Assert.True(compiled(0));
        Assert.True(compiled(-1));
        Assert.True(compiled(42));
    }

    [Fact]
    public void None_ToExpression_returns_non_null_expression_that_evaluates_false()
    {
        ISpecification<int> none = Specification.None<int>();
        Expression<Func<int, bool>>? expr = none.ToExpression();
        Assert.NotNull(expr);
        Func<int, bool> compiled = expr.Compile();
        Assert.False(compiled(0));
        Assert.False(compiled(42));
    }

    [Fact]
    public void And_ToExpression_when_both_have_expression_returns_combined_expression()
    {
        ISpecification<int> even = Specification.FromExpression<int>(x => x % 2 == 0);
        ISpecification<int> positive = Specification.FromExpression<int>(x => x > 0);
        ISpecification<int> combined = even.And(positive);
        Expression<Func<int, bool>>? expr = combined.ToExpression();
        Assert.NotNull(expr);
        Func<int, bool> compiled = expr.Compile();
        Assert.False(compiled(0));
        Assert.False(compiled(3));
        Assert.True(compiled(4));
    }

    [Fact]
    public void Or_ToExpression_when_both_have_expression_returns_combined_expression()
    {
        ISpecification<int> eq5 = Specification.FromExpression<int>(x => x == 5);
        ISpecification<int> eq42 = Specification.FromExpression<int>(x => x == 42);
        ISpecification<int> combined = eq5.Or(eq42);
        Expression<Func<int, bool>>? expr = combined.ToExpression();
        Assert.NotNull(expr);
        Func<int, bool> compiled = expr.Compile();
        Assert.False(compiled(0));
        Assert.True(compiled(5));
        Assert.True(compiled(42));
    }

    [Fact]
    public void Not_ToExpression_when_has_expression_returns_negated_expression()
    {
        ISpecification<int> even = Specification.FromExpression<int>(x => x % 2 == 0);
        ISpecification<int> odd = even.Not();
        Expression<Func<int, bool>>? expr = odd.ToExpression();
        Assert.NotNull(expr);
        Func<int, bool> compiled = expr.Compile();
        Assert.False(compiled(4));
        Assert.True(compiled(3));
    }

    [Fact]
    public void And_ToExpression_when_left_returns_null_returns_null()
    {
        ISpecification<int> left = new NullExpressionSpec();
        ISpecification<int> right = Specification.FromExpression<int>(x => x > 0);
        ISpecification<int> combined = left.And(right);
        Assert.Null(combined.ToExpression());
    }

    [Fact]
    public void And_ToExpression_when_right_returns_null_returns_null()
    {
        ISpecification<int> left = Specification.FromExpression<int>(x => x > 0);
        ISpecification<int> right = new NullExpressionSpec();
        ISpecification<int> combined = left.And(right);
        Assert.Null(combined.ToExpression());
    }

    [Fact]
    public void Or_ToExpression_when_left_returns_null_returns_null()
    {
        ISpecification<int> left = new NullExpressionSpec();
        ISpecification<int> right = Specification.FromExpression<int>(x => x == 5);
        ISpecification<int> combined = left.Or(right);
        Assert.Null(combined.ToExpression());
    }

    [Fact]
    public void Or_ToExpression_when_right_returns_null_returns_null()
    {
        ISpecification<int> left = Specification.FromExpression<int>(x => x == 5);
        ISpecification<int> right = new NullExpressionSpec();
        ISpecification<int> combined = left.Or(right);
        Assert.Null(combined.ToExpression());
    }

    [Fact]
    public void Not_ToExpression_when_inner_returns_null_returns_null()
    {
        ISpecification<int> spec = new NullExpressionSpec();
        ISpecification<int> not = spec.Not();
        Assert.Null(not.ToExpression());
    }

    [Fact]
    public void FromExpression_throws_on_null_expression()
    {
        Assert.Throws<ArgumentNullException>(() => Specification.FromExpression<int>(null!));
    }

    [Fact]
    public void ExpressionSpecification_constructor_throws_on_null_expression()
    {
        Assert.Throws<ArgumentNullException>(() => new ExpressionSpecification<int>(null!));
    }
}
