namespace ChurchApp;

using System.Diagnostics.CodeAnalysis;
using static LambdaCalculus.Church;

public static class ChurchMath
{
    public static uint GcdEuclideanMinusR1(uint a, uint b)
    {
        return EuclideanMinusR1(a.ToChurch(), b.ToChurch()).UnChurch();
    }

    static Numeral EuclideanMinusR1(Numeral a, Numeral b)
    {
        return
        IsZero(b)
            (new Numeral(x => a(x)))
        (new Numeral(x => GEQ(a)(b)
            (new Numeral(y => EuclideanMinusR1(b, Minus(a)(b))(x)))
            (new Numeral(y => EuclideanMinusR1(Minus(b)(a), a)(x)))
        (x)));
    }

    public static uint GcdEuclideanMinusR2(uint a, uint b)
    {
        return EuclideanMinusR2(a.ToChurch(), b.ToChurch()).UnChurch();
    }

    static Numeral EuclideanMinusR2(Numeral a, Numeral b)
    {
        return
        LazyIf(IsZero(b))
            (() => a)
        (() => LazyIf(GEQ(a)(b))
            (() => EuclideanMinusR2(b, Minus(a)(b)))
            (() => EuclideanMinusR2(Minus(b)(a), a))
        );
    }

}
