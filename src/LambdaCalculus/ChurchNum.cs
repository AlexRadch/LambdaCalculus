using System;

namespace LambdaCalculus
{
    public static partial class ChurchNum
    {
        public static Func<Z, Z> Zero<Z>(Func<Z, Z> succ) => (Z zero) => zero;

        //public static void Succ<Z>(Func<Func<Z, Z>, Z> n, Func<Z, Z> succ, Z zero) => succ(n(succ)(zero));
    }
}
