using System.Reflection;

namespace Robotico.Specification.Tests;

/// <summary>
/// Tests for <see cref="ISpecification{T}"/>, <see cref="Specification"/>, <see cref="SpecificationExtensions"/>, and <see cref="OptionSpecificationExtensions"/>: And/Or/Not, All/None, FromExpression, ToSpecification, and argument validation.
/// </summary>
public sealed class SpecificationTests
{
    private sealed class DummySpec : ISpecification<int>
    {
        private readonly bool _value;

        public DummySpec(bool value) => _value = value;

        public bool IsSatisfiedBy(int candidate) => _value;
    }

    [Fact]
    public void And_combines_specifications()
    {
        DummySpec t = new(true);
        DummySpec f = new(false);
        Assert.True(t.And(t).IsSatisfiedBy(0));
        Assert.False(t.And(f).IsSatisfiedBy(0));
        Assert.False(f.And(t).IsSatisfiedBy(0));
        Assert.False(f.And(f).IsSatisfiedBy(0));
    }

    [Fact]
    public void Or_combines_specifications()
    {
        DummySpec t = new(true);
        DummySpec f = new(false);
        Assert.True(t.Or(t).IsSatisfiedBy(0));
        Assert.True(t.Or(f).IsSatisfiedBy(0));
        Assert.True(f.Or(t).IsSatisfiedBy(0));
        Assert.False(f.Or(f).IsSatisfiedBy(0));
    }

    [Fact]
    public void Not_negates_specification()
    {
        DummySpec t = new(true);
        DummySpec f = new(false);
        Assert.False(t.Not().IsSatisfiedBy(0));
        Assert.True(f.Not().IsSatisfiedBy(0));
    }

    [Fact]
    public void Specification_All_is_satisfied_by_any_candidate()
    {
        ISpecification<int> all = Specification.All<int>();
        Assert.True(all.IsSatisfiedBy(0));
        Assert.True(all.IsSatisfiedBy(-1));
        Assert.True(all.IsSatisfiedBy(100));
    }

    [Fact]
    public void Specification_None_is_satisfied_by_no_candidate()
    {
        ISpecification<int> none = Specification.None<int>();
        Assert.False(none.IsSatisfiedBy(0));
        Assert.False(none.IsSatisfiedBy(42));
    }

    [Fact]
    public void Specification_FromExpression_evaluates_expression()
    {
        ISpecification<int> spec = Specification.FromExpression<int>(x => x > 10);
        Assert.False(spec.IsSatisfiedBy(5));
        Assert.False(spec.IsSatisfiedBy(10));
        Assert.True(spec.IsSatisfiedBy(11));
    }

    [Fact]
    public void ExpressionSpecification_ToExpression_returns_same_expression()
    {
        System.Linq.Expressions.Expression<Func<int, bool>> expr = x => x % 2 == 0;
        ExpressionSpecification<int> spec = new(expr);
        Assert.NotNull(spec.ToExpression());
        Assert.True(spec.IsSatisfiedBy(4));
        Assert.False(spec.IsSatisfiedBy(3));
    }

    [Fact]
    public void Option_None_ToSpecification_returns_All()
    {
        Option<ISpecification<int>> none = Option<ISpecification<int>>.None;
        ISpecification<int> spec = none.ToSpecification();
        Assert.True(spec.IsSatisfiedBy(0));
        Assert.True(spec.IsSatisfiedBy(99));
    }

    [Fact]
    public void Option_Some_ToSpecification_returns_inner_spec()
    {
        ISpecification<int> inner = Specification.FromExpression<int>(x => x == 42);
        Option<ISpecification<int>> some = Option<ISpecification<int>>.Some(inner);
        ISpecification<int> spec = some.ToSpecification();
        Assert.False(spec.IsSatisfiedBy(0));
        Assert.True(spec.IsSatisfiedBy(42));
    }

    [Fact]
    public void Option_Some_with_null_inner_ToSpecification_returns_All()
    {
        // Option.Some(null!) throws; this path is only reachable via deserialization or other means. Use reflection to construct Option with null inner.
        Type optionType = typeof(Option<ISpecification<int>>);
        ConstructorInfo? ctor = optionType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, [typeof(ISpecification<int>), typeof(bool)], null);
        Assert.NotNull(ctor);
        object option = ctor.Invoke([null, true]);
        ISpecification<int> spec = ((Option<ISpecification<int>>)option).ToSpecification();
        Assert.True(spec.IsSatisfiedBy(0));
        Assert.True(spec.IsSatisfiedBy(99));
    }

    [Fact]
    public void And_throws_on_null_left()
    {
        ISpecification<int> right = Specification.All<int>();
        Assert.Throws<ArgumentNullException>(() => ((ISpecification<int>?)null)!.And(right));
    }

    [Fact]
    public void And_throws_on_null_right()
    {
        ISpecification<int> left = Specification.All<int>();
        Assert.Throws<ArgumentNullException>(() => left.And(null!));
    }

    [Fact]
    public void Or_throws_on_null_left()
    {
        ISpecification<int> right = Specification.All<int>();
        Assert.Throws<ArgumentNullException>(() => ((ISpecification<int>?)null)!.Or(right));
    }

    [Fact]
    public void Or_throws_on_null_right()
    {
        ISpecification<int> left = Specification.All<int>();
        Assert.Throws<ArgumentNullException>(() => left.Or(null!));
    }

    [Fact]
    public void Not_throws_on_null_spec()
    {
        Assert.Throws<ArgumentNullException>(() => ((ISpecification<int>?)null)!.Not());
    }
}
