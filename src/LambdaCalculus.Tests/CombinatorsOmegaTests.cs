namespace LambdaCalculus.Tests;

using static LambdaCalculus.Church;
using static LambdaCalculus.Combinators;

public class CombinatorsOmegaTests
{
    // SASum :⁡= λf.λx IsZero⁡ x x (x + (f f (x - 1)))
    Func<Numeral, Numeral> SASum(SelfApplicable<Func<Numeral, Numeral>> f) => x =>
        LazyIf(IsZero(x))
            (() => x)
            (() => Plus(x)(f(f)(Pred(x))))
        ;

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void SelfAppliedTest(uint x)
    {
        var Sum1 = SASum(SASum);
        var Sum2 = SelfApplied<Func<Numeral, Numeral>>(SASum);

        var cx = x.ToChurch();
        var expected = (uint)Enumerable.Range(1, (int)x).Sum();
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
