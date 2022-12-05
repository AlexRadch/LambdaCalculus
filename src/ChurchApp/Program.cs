using System.Numerics;
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
            var _ = BenchmarkRunner.Run(typeof(GcdBenchmarks));

            //BoolEval();
            //GcdEval();
            //NumEval();
        }

        static void BoolEval()
        {
            var t1 = true.Church();
            var f1 = false.Church();
            var g1 = True((object)t1)("123");

            var t2 = false.Church();
            var f2 = False((object)t2)(true.Church());
            var g2 = False((object)"567")(f2);

            Console.WriteLine($"{t1} {f1} {g1} {g1(1)} {g1(1)(0)}");
            Console.WriteLine($"{t2} {f2} {g2} {g2(1)} {g2(1)(0)}");

            Console.WriteLine($"{t1 == f2} {f1 == t2} {g1 == g2}");

            Console.WriteLine($"{ReferenceEquals(t1, f2)} {ReferenceEquals(f1, t2)} {ReferenceEquals(g1, g2)}");
        }

        #region GCD

        static void GcdEval()
        {
            {
                var a = 252;
                var b = 105;
                var c1 = Gcd(a, b);
                var c2 = ChurchGcd2(a, b);
                var c3 = ChurchGcd1(a)(b);

                Console.WriteLine($"GDC({a}, {b}) = {c1} {c2} {c3}");
            }

            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var a = random.Next(1_000);
                var b = random.Next(1_000);
                var c1 = Gcd(a, b);
                var c2 = ChurchGcd2(a, b);
                var c3 = ChurchGcd1(a)(b);
                Console.WriteLine($"GDC({a}, {b}) = {c1} {c2} {c3}");
            }

        }

        internal static T Gcd<T>(T a, T b) where T : INumber<T>
        {
            if (a < b)
                return Gcd(b - a, a);
            if (a > b)
                return Gcd(a - b, b);
            return a;
        }

        internal static T ChurchGcd2<T>(T a, T b) where T : INumber<T> =>
            ChurchBool.If((a < b).Church())
                (ToLazy(() => ChurchGcd2(b - a, a)))
                (ChurchBool.If((a > b).Church())
                    (ToLazy(() => ChurchGcd2(a - b, b)))
                    (ToLazy(() => a))
                )
            ();

        internal static Func<T, T> ChurchGcd1<T>(T a) where T : INumber<T> => b =>
            ChurchBool.If((a < b).Church())
                (ToLazy(() => ChurchGcd1(b - a)(a)))
                (ChurchBool.If((a > b).Church())
                    (ToLazy(() => ChurchGcd1(a - b)(b)))
                    (ToLazy(() => a))
                )
            ();

        #endregion

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
