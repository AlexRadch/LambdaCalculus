namespace LambdaCalculus.Tests;

using static LambdaCalculus.Church;

public class ChurchSignedNumbersTests
{
    #region Convertions

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void CreateSignedTest(uint p, uint n)
    {
        var cv = CreateSigned(p.ToChurch())(n.ToChurch());
        Numeral cp = cv(cp => cn => cp);
        Numeral cn = cv(cp => cn => cn);

        uint ep = (uint)Math.Max(0, (int)p - (int)n);
        uint en = (uint)Math.Max(0, (int)n - (int)p);
        Assert.Equal(ep, cp.UnChurch());
        Assert.Equal(en, cn.UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void NumeralToSignedTest(uint x)
    {
        var cx = x.ToChurch();
        var cv = NumeralToSigned(cx);
        Numeral cp = cv(cp => cn => cp);
        Numeral cn = cv(cp => cn => cn);

        Assert.Equal(x, cp.UnChurch());
        Assert.Equal(0u, cn.UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void NumeralToNegSignedTest(uint x)
    {
        var cx = x.ToChurch();
        var cv = NumeralToNegSigned(cx);
        Numeral cp = cv(cp => cn => cp);
        Numeral cn = cv(cp => cn => cn);

        Assert.Equal(0u, cp.UnChurch());
        Assert.Equal(x, cn.UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void SignedToAbsNumeralTest(int x)
    {
        var cx = x.ToChurch();
        var cv = SignedToAbsNumeral(cx);

        uint expected = (uint)Math.Abs(x);
        Assert.Equal(expected, cv.UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void SignedToTruncNumeralTest(int x)
    {
        var cx = x.ToChurch();
        var cv = SignedToTruncNumeral(cx);

        uint expected = (uint)Math.Max(0, x);
        Assert.Equal(expected, cv.UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void OneZeroTest(uint p, uint n)
    {
        Signed cv = b => Pair<Numeral, Numeral>(p.ToChurch())(n.ToChurch())(b);
        Numeral cp = cv(cp => cn => cp);
        Numeral cn = cv(cp => cn => cn);
        Assert.Equal(p, cp.UnChurch());
        Assert.Equal(n, cn.UnChurch());

        cv = OneZero(cv);
        cp = cv(cp => cn => cp);
        cn = cv(cp => cn => cn);

        uint ep = (uint)Math.Max(0, (int)p - (int)n);
        uint en = (uint)Math.Max(0, (int)n - (int)p);
        Assert.Equal(ep, cp.UnChurch());
        Assert.Equal(en, cn.UnChurch());
    }

    #endregion

    #region Constants

    [Fact]
    public void ZeroSTest()
    {
        Assert.Equal(0, ZeroS.UnChurch());
    }

    [Fact]
    public void OneSTest()
    {
        Assert.Equal(1, OneS.UnChurch());
    }

    #endregion

    #region Arithmetic

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void NegSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(-x, NegS(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void AbsSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(Math.Abs(x), AbsS(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void PlusSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x + y, PlusS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void SuccSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x + 1, SuccS(cx).UnChurch());
        Assert.Equal(x + 2, SuccS(SuccS(cx)).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void MinusSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x - y, MinusS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void PredSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x - 1, PredS(cx).UnChurch());
        Assert.Equal(x - 2, PredS(PredS(cx)).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void MultSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x * y, MultS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void DivideSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();

        var expected = y == 0 ? 0 : x / y;
        Assert.Equal(expected, DivideS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void ModuloSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();

        var expected = y == 0 ? 0 : x % y;
        Assert.Equal(expected, ModuloS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void ExpSTest(int x, int y)
    {
        if (Math.Abs(y) > 3)
            return;

        var cx = x.ToChurch();
        var cy = y.ToChurch();

        var expected = (int)Math.Pow(x, y);
        if (expected == int.MaxValue)
            expected = 0;

        Assert.Equal(expected, ExpS(cx)(cy).UnChurch());
    }

    #endregion

    #region Logical

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void IsZeroSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x == 0, IsZeroS(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void IsPosSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x > 0, IsPosS(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void IsNegSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x < 0, IsNegS(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void IsZPosSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x >= 0, IsZPosS(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void IsZNegSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x <= 0, IsZNegS(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void GEQSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x >= y, GEQS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void LEQSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x <= y, LEQS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void GTSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x > y, GTS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void LTSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x < y, LTS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void EQSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x == y, EQS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData2))]
    public void NEQSTest(int x, int y)
    {
        var cx = x.ToChurch();
        var cy = y.ToChurch();
        Assert.Equal(x != y, NEQS(cx)(cy).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void IsEvenSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x % 2 == 0, IsEvenS(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void IsOddSTest(int x)
    {
        var cx = x.ToChurch();
        Assert.Equal(x % 2 != 0, IsOddS(cx).UnChurch());
    }

    #endregion

    #region Extensions

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void ToChurchTest(int x)
    {
        var cx = x.ToChurch();
        Numeral cp = cx(cp => cn => cp);
        Numeral cn = cx(cp => cn => cn);

        uint ep = (uint)Math.Max(0, x);
        uint en = (uint)Math.Max(0, -x);
        Assert.Equal(ep, cp.UnChurch());
        Assert.Equal(en, cn.UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void UnChurchTest(uint p, uint n)
    {
        var cv = CreateSigned(p.ToChurch())(n.ToChurch());

        var expected = (int)p - (int)n;
        Assert.Equal(expected, cv.UnChurch());
    }

    #endregion

    #region GetData

    public static IEnumerable<int> GetInts()
        => ChurchNumeralsTests.GetUInts().SelectMany(x => new int[] { (int)x, -(int)x });

    public static IEnumerable<object[]> GetIntsData1() => GetInts().Select(x => new object[] { x });

    public static IEnumerable<object[]> GetIntsData2() =>
        from x in GetInts()
        from y in GetInts()
        select new object[] { x, y };


    public static IEnumerable<object[]> GetUIntsData1() => ChurchNumeralsTests.GetUIntsData1();
    public static IEnumerable<object[]> GetUIntsData2() => ChurchNumeralsTests.GetUIntsData2();

    #endregion
}
