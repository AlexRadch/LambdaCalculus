using BenchmarkDotNet.Running;

namespace ChurchApp
{
    class Program
    {
        static void Main()
        {
            //BoolEval();
            var _ = BenchmarkRunner.Run<Bool_If_Bench>();

            //GcdEval();
            //var _ = BenchmarkRunner.Run<GcdBenchmarks>();

            //NumEval();
        }

        //static void BoolEval()
        //{
        //    var t1 = true.Church();
        //    var f1 = false.Church();
        //    var g1 = True(t1, "123");

        //    var t2 = false.Church();
        //    var f2 = False(t2, true.Church());
        //    var g2 = False("567", f2);

        //    Console.WriteLine($"{t1} {f1} {g1} {g1(1)} {g1(1)(0)}");
        //    Console.WriteLine($"{t2} {f2} {g2} {g2(1)} {g2(1)(0)}");

        //    Console.WriteLine($"{t1 == f2} {f1 == t2} {g1 == g2}");

        //    Console.WriteLine($"{ReferenceEquals(t1, f2)} {ReferenceEquals(f1, t2)} {ReferenceEquals(g1, g2)}");
        //}

        static void GcdEval()
        {
            {
                var a = 252u;
                var b = 105u;
                var em = Math.GcdEuclideanMinus(a, b);
                var ed = Math.GcdEuclideanMinus(a, b);
                var cem = ChurchMath.GcdEuclideanMinus(a, b);

                Console.WriteLine($"GDC({a}, {b}) = {em} {ed} {cem}");
            }

            var random = new Random();
            for (var i = 0; i < 15; i++)
            {
                var a = (uint)random.Next(100);
                var b = (uint)random.Next(100);
                var em = Math.GcdEuclideanMinus(a, b);
                var ed = Math.GcdEuclideanMinus(a, b);
                var cem = ChurchMath.GcdEuclideanMinus(a, b);

                Console.WriteLine($"GDC({a}, {b}) = {em} {ed} {cem}");
            }
        }

        //static void NumEval()
        //{
        //    static dynamic inc(dynamic x) => x + 1;

        //    var zero = ZeroV;
        //    Console.WriteLine(zero(inc, 0));

        //    //var v1 = OneV;
        //    //Console.WriteLine(v1(inc)(0));

        //    //var v2 = TwoV;
        //    //Console.WriteLine(v2(inc)(0));

        //    var v1 = Succ(zero);
        //    Console.WriteLine(v1(inc, 0));

        //    var v2 = Succ(v1);
        //    Console.WriteLine(v2(inc, 0));

        //    v2 = Succ(Succ(zero));
        //    Console.WriteLine(v2(inc, 0));

        //    var v3 = Add(v2, v1);
        //    Console.WriteLine(v3(inc, 0));

        //    v3 = Add(v1, v2);
        //    Console.WriteLine(v3(inc, 0));

        //    v3 = Add(Add(Add(zero, v1), v1), v1);
        //    Console.WriteLine(v3(inc, 0));

        //    var v4 = Add(v2, v2);
        //    Console.WriteLine(v4(inc, 0));

        //    Console.WriteLine();

        //    //var v16 = Mult1(v4, v4);
        //    //Console.WriteLine(v16(inc, 0));

        //    //var v12 = Mult2(v3, v4);
        //    //Console.WriteLine(v12(inc, 0));

        //    //var vp = Exp1(v4, v3);
        //    //Console.WriteLine(vp(inc, 0));

        //    //vp = Exp2(v3, v4);
        //    //Console.WriteLine(vp(inc, 0));
        //}
    }
}
