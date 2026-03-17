namespace Robotico.Specification.Tests;

/// <summary>
/// Tests that specification composition obeys expected algebraic laws: All as identity for And, None as identity for Or, and double negation.
/// </summary>
public sealed class SpecificationLawsTests
{
    [Fact]
    public void All_And_identity_same_as_other()
    {
        ISpecification<int> all = Specification.All<int>();
        ISpecification<int> other = Specification.FromExpression<int>(x => x > 0);
        ISpecification<int> combined = all.And(other);
        Assert.True(combined.IsSatisfiedBy(1));
        Assert.False(combined.IsSatisfiedBy(0));
    }

    [Fact]
    public void None_Or_identity_same_as_other()
    {
        ISpecification<int> none = Specification.None<int>();
        ISpecification<int> other = Specification.FromExpression<int>(x => x == 5);
        ISpecification<int> combined = none.Or(other);
        Assert.False(combined.IsSatisfiedBy(0));
        Assert.True(combined.IsSatisfiedBy(5));
    }

    [Fact]
    public void Not_Not_identity()
    {
        ISpecification<int> spec = Specification.FromExpression<int>(x => x % 2 == 0);
        ISpecification<int> notNot = spec.Not().Not();
        Assert.True(spec.IsSatisfiedBy(4));
        Assert.True(notNot.IsSatisfiedBy(4));
        Assert.False(spec.IsSatisfiedBy(3));
        Assert.False(notNot.IsSatisfiedBy(3));
    }
}
