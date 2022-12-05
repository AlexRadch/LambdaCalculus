using BenchmarkDotNet.Running;

using LambdaCalculus;
using static LambdaCalculus.ChurchBool;
using static LambdaCalculus.ChurchNum;

namespace ChurchApp
{
    class Program
    {
        static void Main()
        {
            //BoolEval();

            GcdEval();
            var _ = BenchmarkRunner.Run(typeof(GcdBenchmarks));

            //NumEval();
        }

        static void BoolEval()
        {
            var t1 = true.Church();
            var f1 = false.Church();
            var g1 = True(t1, "123");

            var t2 = false.Church();
            var f2 = False(t2, true.Church());
            var g2 = False("567", f2);

            Console.WriteLine($"{t1} {f1} {g1} {g1(1)} {g1(1)(0)}");
            Console.WriteLine($"{t2} {f2} {g2} {g2(1)} {g2(1)(0)}");

            Console.WriteLine($"{t1 == f2} {f1 == t2} {g1 == g2}");

            Console.WriteLine($"{ReferenceEquals(t1, f2)} {ReferenceEquals(f1, t2)} {ReferenceEquals(g1, g2)}");
        }

        static void GcdEval()
        {
            {
                var a = -252;
                var b = -105;
                var em = Math.GcdEuclideanMinus(a, b);
                var ed = Math.GcdEuclideanMinus(a, b);
                var cem = ChurchMath.GcdEuclideanMinus(a, b);

                Console.WriteLine($"GDC({a}, {b}) = {em} {ed} {cem}");
            }

            var random = new Random();
            for (var i = 0; i < 15; i++)
            {
                var a = random.Next(2_000) - 999;
                var b = random.Next(2_000) - 999;
                var em = Math.GcdEuclideanMinus(a, b);
                var ed = Math.GcdEuclideanMinus(a, b);
                var cem = ChurchMath.GcdEuclideanMinus(a, b);

                Console.WriteLine($"GDC({a}, {b}) = {em} {ed} {cem}");
            }

        }

        static void NumEval()
        {
            var inc = (dynamic x) => x + 1;

            var zero = ZeroV;
            Console.WriteLine(zero(inc)(0));

            //var one = OneV;
            //Console.WriteLine(one(inc)(0));

            //var two = TwoV;
            //Console.WriteLine(two(inc)(0));

            var one = zero.Succ();
            Console.WriteLine(one(inc)(0));

            var two = one.Succ();
            Console.WriteLine(two(inc)(0));

            two = zero.Succ().Succ();
            Console.WriteLine(two(inc)(0));

            var three = two.Add(one);
            Console.WriteLine(three(inc)(0));

            three = one.Add(two);
            Console.WriteLine(three(inc)(0));

            three = zero.Add(one).Add(one).Add(one);
            Console.WriteLine(three(inc)(0));

            var four = two.Add(two);
            Console.WriteLine(four(inc)(0));
        }
    }
}
