namespace ChurchApp;

using static LambdaCalculus.Church;

public static class ChurchMath
{
    public static uint GcdEuclideanMinus(uint a, uint b)
    {
        return EuclideanMinus(a.AsChurch(), b.AsChurch()).UnChurch();
    }

    static Numeral EuclideanMinus(Numeral a, Numeral b)
    {
        return
        LazyIf(IsZero(b))
            (() => a)
            (() =>
        LazyIf(GEQ(a)(b))
            (() => EuclideanMinus(b, Minus(a)(b)))
            (() => EuclideanMinus(Minus(b)(a), a))
        );
    }
}
