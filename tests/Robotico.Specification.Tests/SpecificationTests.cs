using Robotico.Specification;
using Xunit;

namespace Robotico.Specification.Tests;

public sealed class DummySpec : ISpecification<int>
{
    private readonly bool _value;

    public DummySpec(bool value) => _value = value;

    public bool IsSatisfiedBy(int candidate) => _value;
}

public sealed class SpecificationTests
{
    [Fact]
    public void And_combines_specifications()
    {
        var t = new DummySpec(true);
        var f = new DummySpec(false);
        Assert.True(t.And(t).IsSatisfiedBy(0));
        Assert.False(t.And(f).IsSatisfiedBy(0));
        Assert.False(f.And(t).IsSatisfiedBy(0));
        Assert.False(f.And(f).IsSatisfiedBy(0));
    }

    [Fact]
    public void Or_combines_specifications()
    {
        var t = new DummySpec(true);
        var f = new DummySpec(false);
        Assert.True(t.Or(t).IsSatisfiedBy(0));
        Assert.True(t.Or(f).IsSatisfiedBy(0));
        Assert.True(f.Or(t).IsSatisfiedBy(0));
        Assert.False(f.Or(f).IsSatisfiedBy(0));
    }

    [Fact]
    public void Not_negates_specification()
    {
        var t = new DummySpec(true);
        var f = new DummySpec(false);
        Assert.False(t.Not().IsSatisfiedBy(0));
        Assert.True(f.Not().IsSatisfiedBy(0));
    }
}
