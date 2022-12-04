using LambdaCalculus;
using static LambdaCalculus.ChurchBool;

namespace ChurchApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1 = true.Church();
            var f1 = false.Church();
            var g1 = True(t1)("123");

            var t2 = false.Church();
            var f2 = False(false.Church())(false.Church());
            var g2 = False("567")(f2);

            Console.WriteLine($"{t1} {f1} {g1} {g1(1)} {g1(1)(0)}");
            Console.WriteLine($"{t2} {f2} {g2} {g2(1)} {g2(1)(0)}");

            Console.WriteLine($"{t1 == f2} {f1 == t2} {g1 == g2}");

            Console.WriteLine($"{ReferenceEquals(t1, f2)} {ReferenceEquals(f1, t2)} {ReferenceEquals(g1, g2)}");
        }
    }
}
