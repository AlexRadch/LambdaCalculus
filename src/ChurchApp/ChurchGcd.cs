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
        IsZero(b)
            (AsLazy(() => a))
            (AsLazy(() => 
        GEQ(a)(b)
            (AsLazy(() => EuclideanMinus(b, Minus(a)(b))))
            (AsLazy(() => EuclideanMinus(Minus(b)(a), a)))
            ()
        ))
        ();
    }
}
