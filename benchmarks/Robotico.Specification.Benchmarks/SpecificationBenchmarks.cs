using BenchmarkDotNet.Attributes;
using Robotico.Specification;

namespace Robotico.Specification.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class SpecificationBenchmarks
{
    private static readonly ISpecification<int> SpecAll = Specification.All<int>();
    private static readonly ISpecification<int> SpecPositive = new ExpressionSpecification<int>(x => x > 0);
    private static readonly ISpecification<int> SpecLessThan100 = new ExpressionSpecification<int>(x => x < 100);
    private static readonly ISpecification<int> Composed = SpecPositive.And(SpecLessThan100);

    [Benchmark(Baseline = true)]
    public bool All_IsSatisfiedBy_True()
    {
        return SpecAll.IsSatisfiedBy(42);
    }

    [Benchmark]
    public bool Expression_IsSatisfiedBy_Pass()
    {
        return SpecPositive.IsSatisfiedBy(42);
    }

    [Benchmark]
    public bool Expression_IsSatisfiedBy_Fail()
    {
        return SpecPositive.IsSatisfiedBy(-1);
    }

    [Benchmark]
    public bool Composed_And_IsSatisfiedBy()
    {
        return Composed.IsSatisfiedBy(50);
    }

    [Benchmark]
    public bool Composed_Not_IsSatisfiedBy()
    {
        return SpecPositive.Not().IsSatisfiedBy(-1);
    }
}
