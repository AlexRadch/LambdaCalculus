using BenchmarkDotNet.Attributes;

namespace ChurchApp
{
    public class GcdBenchmarks
    {
        [Params(100, 1_000, 100_00)]
        public int Count { get; set; }

        private Random random = new();
        int Next() => random.Next(-999, 1_000);

        [IterationSetup]
        public void IterationSetup()
        {
            random = new Random(Count);
        }

        [Benchmark]
        public void Gcd_EuclideanDiv_Bench()
        {
            for (var i = 0; i < Count; i++)
                Math.GcdEuclideanDiv(Next(), Next());
        }

        [Benchmark]
        public void Gcd_EuclideanMinus_Bench()
        {
            for (var i = 0; i < Count; i++) 
                Math.GcdEuclideanMinus(Next(), Next());
        }

        [Benchmark]
        public void ChurchGcd_EuclideanMinus_Bench()
        {
            for (var i = 0; i < Count; i++)
                ChurchMath.GcdEuclideanMinus(Next(), Next());
        }
    }
}
