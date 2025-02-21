using static LambdaCalculus.Church;

namespace LambdaCalculus.Tests;

using static LambdaCalculus.Combinators;

public class CombinatorsRecursionTests
{
    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void SelfApplicableTest(uint x)
    {
        // SaSum :⁡= λf.λx IsZero⁡ x x (x + (f f (x - 1)))
        Func<Numeral, Numeral> SaSum(SelfApplicable<Func<Numeral, Numeral>> f) => x =>
            LazyIf(IsZero(x))(() => x)(() => Plus(x)(f(f)(Pred(x))));

        Func<Numeral, Numeral> Sum = SaSum(SaSum);

        var cx = x.ToChurch();
        var expected = (uint)Enumerable.Range(1, (int)x).Sum();
        Assert.Equal(expected, Sum(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void UTest(uint x)
    {
        // SaSum :⁡= λf.λx IsZero⁡ x x (x + (f f (x - 1)))
        Func<Numeral, Numeral> SaSum(SelfApplicable<Func<Numeral, Numeral>> f) => x =>
            LazyIf(IsZero(x))(() => x)(() => Plus(x)(f(f)(Pred(x))));

        Func<Numeral, Numeral> uSum = U<Func<Numeral, Numeral>>(SaSum);

        var cx = x.ToChurch();
        var expected = (uint)Enumerable.Range(1, (int)x).Sum();
        Assert.Equal(expected, uSum(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void ItSelfTest(uint x)
    {
        // RSum :⁡= λr.λx IsZero⁡ x x (x + (r (x - 1)))
        Func<Numeral, Numeral> RSum(Func<Numeral, Numeral> r) => x =>
            LazyIf(IsZero(x))(() => x)(() => Plus(x)(r(Pred(x))));

        var cx = x.ToChurch();
        var expected = (uint)Enumerable.Range(0, (int)x + 1).Sum();

        Func<Numeral, Numeral> Sum = RSum(RSum(RSum(null!)));

        if (x < 3)
            Assert.Equal(expected, Sum(cx).UnChurch());
        else
            Assert.Throws<NullReferenceException>(() => Sum(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void ZTest(uint x)
    {
        // RSum :⁡= λr.λx IsZero⁡ x x (x + (r (x - 1)))
        Func<Numeral, Numeral> RSum(Func<Numeral, Numeral> r) => x =>
            LazyIf(IsZero(x))(() => x)(() => Plus(x)(r(Pred(x))));

        var cx = x.ToChurch();
        var expected = (uint)Enumerable.Range(0, (int)x + 1).Sum();

        var Sum = Z<Numeral, Numeral>(RSum);
        Assert.Equal(expected, Sum(cx).UnChurch());
    }

    #region GetData

    public static IEnumerable<uint> GetUInts() =>
        Enumerable.Range(0, 5).Select(x => (uint)x);

    public static IEnumerable<object[]> GetUIntsData1() =>
        GetUInts().Select(x => new object[] { x });

    #endregion
}
