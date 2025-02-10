using BenchmarkDotNet.Attributes;

namespace ChurchApp
{
    public class GcdBenchmarks
    {
        [Params(1, 10)]
        public int Count { get; set; }

        private Random random = new(391);
        private uint Next() => (uint)random.Next(0, 100);

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

        [Benchmark(Baseline = true)]
        public void Gcd_EuclideanMinus_Bench()
        {
            for (var i = 0; i < Count; i++) 
                Math.GcdEuclideanMinus(Next(), Next());
        }

        [Benchmark]
        public void ChurchGcd_EuclideanMinusR1_Bench()
        {
            for (var i = 0; i < Count; i++)
                ChurchMath.GcdEuclideanMinusR1(Next(), Next());
        }

        [Benchmark]
        public void ChurchGcd_EuclideanMinusR2_Bench()
        {
            for (var i = 0; i < Count; i++)
                ChurchMath.GcdEuclideanMinusR2(Next(), Next());
        }
    }
}
