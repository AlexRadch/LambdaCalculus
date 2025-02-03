
using BenchmarkDotNet.Attributes;
using LambdaCalculus;

namespace ChurchApp;

public class Bool_If_Bench
{
    [Params(10, 100)]
    public int Count { get; set; }

    [Params(true, false)]
    public bool Pridicate { get; set; }

    [Benchmark(Baseline = true)]
    public void If()
    {
        var P = Pridicate.AsChurch();

        for (var i = 0; i < Count; i++)
            Church.If(P)(Church.True)(Church.False);
    }

    [Benchmark]
    public void If_P()
    {
        var P = Pridicate.AsChurch();

        for (var i = 0; i < Count; i++)
            P(Church.True)(Church.False);
    }
}
