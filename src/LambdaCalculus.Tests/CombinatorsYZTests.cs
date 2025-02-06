using static LambdaCalculus.Church;

namespace LambdaCalculus.Tests;

using static LambdaCalculus.Combinators;

public class CombinatorsYZTests
{
    // SumSA :⁡= λf.λx IsZero⁡ x x (x + (f (x - 1)))
    Func<Numeral, Numeral> SumSA(Func<Numeral, Numeral> f) => x =>
        LazyIf(IsZero(x))
            (() => x)
            (() => Plus(x)(f(Pred(x))))
        ;

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void ItSelfTest(uint x)
    {
        var cx = x.ToChurch();
        var expected = (uint)Enumerable.Range(0, (int)x + 1).Sum();

        Func<Numeral, Numeral> Sum2 = SumSA(SumSA(SumSA(null!)));

        if (x < 3)
            Assert.Equal(expected, Sum2(cx).UnChurch());
        else
            Assert.Throws<NullReferenceException>(() => Sum2(cx).UnChurch());

        var Sum = Z<Numeral, Numeral>(SumSA);
        Assert.Equal(expected, Sum(cx).UnChurch());
    }

    #region GetData

    public static IEnumerable<uint> GetUInts() =>
        Enumerable.Range(0, 5).Select(x => (uint)x);

    public static IEnumerable<object[]> GetUIntsData1() =>
        GetUInts().Select(x => new object[] { x });

    #endregion
}
