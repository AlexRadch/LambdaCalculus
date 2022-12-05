using System.Numerics;

using LambdaCalculus;
using static LambdaCalculus.ChurchBool;

namespace ChurchApp
{
    public static class ChurchMath
    {
        public static T GcdEuclideanMinus<T>(T a, T b) where T : INumber<T>
        {
            a = T.Abs(a);
            b = T.Abs(b);
            var r = EuclideanMinus(T.Max(a, b), T.Min(a, b));
            return r;
        }

        static T EuclideanMinus<T>(T a, T b) where T : INumber<T>
        {
            return
            ChurchBool.If(T.IsZero(b).Church(),
                ToLazy<Func<T>>(() => () => a),
            ToLazy(() => ChurchBool.If((a > b).Church(),
                ToLazy(() => EuclideanMinus(b, a - b)),
            ToLazy(() => EuclideanMinus(a, b - a))
            )))
            ()()
            ;
        }

        public static Func<T, T> GcdEuclideanMinus<T>(T a) where T : INumber<T> => b =>
        {
            a = T.Abs(a);
            b = T.Abs(b);
            var r = EuclideanMinus(T.Max(a, b))(T.Min(a, b));
            return r;
        };

        static Func<T, T> EuclideanMinus<T>(T a) where T : INumber<T> => b =>
        {
            return
            ChurchBool.If(T.IsZero(b).Church(),
                ToLazy<Func<T>>(() => () => a),
            ToLazy(() => ChurchBool.If((a > b).Church(),
                ToLazy(() => EuclideanMinus(b)(a - b)),
            ToLazy(() => EuclideanMinus(a)(b - a))
            )))
            ()()
            ;
        };

    }
}
