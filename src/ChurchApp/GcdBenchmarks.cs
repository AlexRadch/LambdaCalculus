using BenchmarkDotNet.Attributes;

namespace ChurchApp
{
    public class GcdBenchmarks
    {
        [Params(100, 1_000, 100_00)]
        public int Count { get; set; }

        const int randomMax = 1_000;

        [Benchmark]
        public void Gcd_Bench()
        {
            var random = new Random(Count);
            for (var i = 0; i < Count; i++) 
                Program.Gcd(random.Next(1, randomMax), random.Next(1, randomMax));
        }

        [Benchmark]
        public void ChurchGcd2_Bench()
        {
            var random = new Random(Count);
            for (var i = 0; i < Count; i++)
                Program.ChurchGcd2(random.Next(1, randomMax), random.Next(1, randomMax));
        }

        [Benchmark]
        public void ChurchGcd1_Bench()
        {
            var random = new Random(Count);
            for (var i = 0; i < Count; i++)
                Program.ChurchGcd1(random.Next(1, randomMax))(random.Next(1, randomMax));
        }
    }
}
