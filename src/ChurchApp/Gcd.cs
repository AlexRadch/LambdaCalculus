using System.Numerics;

namespace ChurchApp
{
    public static class Math
    {
        #region EuclideanMinus

        public static T GcdEuclideanMinus<T>(T a, T b) where T : INumber<T>
        {
            return EuclideanMinus(T.Abs(a), T.Abs(b));
        }

        static T EuclideanMinus<T>(T a, T b) where T : INumber<T>
        {
            if (T.IsZero(b))
                return a;
            if (a >= b)
                return EuclideanMinus(b, a - b);
            return EuclideanMinus(b - a, a);
        }

        #endregion

        #region EuclideanDiv

        public static T GcdEuclideanDiv<T>(T a, T b) where T : INumber<T>
        {
            a = T.Abs(a);
            b = T.Abs(b);
            return EuclideanDiv(T.Max(a, b), T.Min(a, b));
        }
        static T EuclideanDiv<T>(T a, T b) where T : INumber<T>
        {
            if (T.IsZero(b))
                return a;
            return EuclideanDiv(b, a % b);
        }

        #endregion
    }
}
