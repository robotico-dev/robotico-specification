namespace Robotico.Specification.Tests;

/// <summary>
/// Property-style and parameterized tests for specification composition laws using [Theory] and [InlineData].
/// Aligns with robotico-results-csharp ResultLawsTheoryTests quality.
/// </summary>
public sealed class SpecificationLawsTheoryTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(10, true)]
    [InlineData(11, true)]
    public void FromExpression_evaluates_correctly_for_candidates(int candidate, bool expected)
    {
        ISpecification<int> spec = Specification.FromExpression<int>(x => x > 0);
        Assert.Equal(expected, spec.IsSatisfiedBy(candidate));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100)]
    public void All_is_satisfied_by_any_candidate(int candidate)
    {
        ISpecification<int> all = Specification.All<int>();
        Assert.True(all.IsSatisfiedBy(candidate));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-5)]
    public void None_is_satisfied_by_no_candidate(int candidate)
    {
        ISpecification<int> none = Specification.None<int>();
        Assert.False(none.IsSatisfiedBy(candidate));
    }

    [Theory]
    [InlineData(2, true)]
    [InlineData(4, true)]
    [InlineData(3, false)]
    [InlineData(5, false)]
    public void And_combines_two_specs(int candidate, bool expected)
    {
        ISpecification<int> even = Specification.FromExpression<int>(x => x % 2 == 0);
        ISpecification<int> positive = Specification.FromExpression<int>(x => x > 0);
        ISpecification<int> combined = even.And(positive);
        Assert.Equal(expected, combined.IsSatisfiedBy(candidate));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(5, true)]
    [InlineData(42, true)]
    public void Or_combines_two_specs(int candidate, bool expected)
    {
        ISpecification<int> eq5 = Specification.FromExpression<int>(x => x == 5);
        ISpecification<int> eq42 = Specification.FromExpression<int>(x => x == 42);
        ISpecification<int> combined = eq5.Or(eq42);
        Assert.Equal(expected, combined.IsSatisfiedBy(candidate));
    }

    [Theory]
    [InlineData(4, false)]
    [InlineData(3, true)]
    public void Not_negates_spec(int candidate, bool expected)
    {
        ISpecification<int> even = Specification.FromExpression<int>(x => x % 2 == 0);
        ISpecification<int> odd = even.Not();
        Assert.Equal(expected, odd.IsSatisfiedBy(candidate));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(7)]
    public void Not_Not_identity_preserves_result(int candidate)
    {
        ISpecification<int> spec = Specification.FromExpression<int>(x => x % 2 == 0);
        ISpecification<int> notNot = spec.Not().Not();
        Assert.Equal(spec.IsSatisfiedBy(candidate), notNot.IsSatisfiedBy(candidate));
    }
}
