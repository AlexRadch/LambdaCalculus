namespace LambdaCalculus.Tests;

using static LambdaCalculus.Church;
using static LambdaCalculus.Combinators;

public class CombinatorsOmegaTests
{
    // SumSA :⁡= λf.λx IsZero⁡ x x (x + (f f (x - 1)))
    Func<Numeral, Numeral> SumSA(SelfApplicable<Func<Numeral, Numeral>> f) => x =>
        LazyIf(IsZero(x))
            (() => x)
            (() => Plus(x)(f(f)(Pred(x))))
        ;

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void ItSelfTest(uint x)
    {
        var Sum1 = SumSA(SumSA);
        var Sum2 = ItSelf((SelfApplicable<Func<Numeral, Numeral>>)SumSA);

        var cx = x.ToChurch();
        var expected = (uint)Enumerable.Range(0, (int)x + 1).Sum();
        Assert.Equal(expected, Sum1(cx).UnChurch());
        Assert.Equal(expected, Sum2(cx).UnChurch());
    }

    #region GetData

    public static IEnumerable<uint> GetUInts() =>
        Enumerable.Range(0, 5).Select(x => (uint)x);

    public static IEnumerable<object[]> GetUIntsData1() =>
        GetUInts().Select(x => new object[] { x });

    #endregion
}
