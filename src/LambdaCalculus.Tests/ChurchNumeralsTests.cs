using static LambdaCalculus.Church;

namespace LambdaCalculus.Tests;

public class ChurchNumeralsTests
{
    #region Constants

    [Theory]
    [MemberData(nameof(GetNextIntsData))]
    public void ZeroTest(Func<int, int> succ, int zero)
    {
        Assert.Equal(zero, ZeroL(x => succ(x))(zero));
        Assert.Equal(zero, Zero(x => succ(x))(zero));

        Assert.Equal(zero, ZeroL_False(x => succ(x))(zero));
        Assert.Equal(zero, Zero_False(x => succ(x))(zero));

        var one = succ(zero);
        Assert.Equal(one, ZeroL(x => succ(x))(one));
        Assert.Equal(one, Zero(x => succ(x))(one));

        Assert.Equal(one, ZeroL_False(x => succ(x))(one));
        Assert.Equal(one, Zero_False(x => succ(x))(one));
    }

    [Theory]
    [MemberData(nameof(GetNextIntsData))]
    public void OneTest(Func<int, int> succ, int zero)
    {
        var one = succ(zero);
        Assert.Equal(one, OneL(x => succ(x))(zero));
        Assert.Equal(one, One(x => succ(x))(zero));

        Assert.Equal(one, OneL(OneL(x => succ(x)))(zero));
        Assert.Equal(one, One(One(x => succ(x)))(zero));

        var two = succ(one);
        Assert.Equal(two, OneL(x => succ(succ(x)))(zero));
        Assert.Equal(two, One(x => succ(succ(x)))(zero));

        Assert.Equal(two, OneL(x => succ(x))(one));
        Assert.Equal(two, One(x => succ(x))(one));
    }

    #endregion

    #region Conversations

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void AsChurchTest(uint value)
    {
        var cv = value.AsChurch();

        foreach (var succ in GetIntNexts())
        {
            foreach (var zero in GetInts())
            {
                var expected = zero;
                for (int i = 0; i < value; i++)
                    expected = succ(expected);


                Assert.Equal(expected, cv(x => succ(x))(zero));
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void UnChurchTest(uint value)
    {
        {
            var cv = Zero;
            for (int i = 0; i < value; i++)
                cv = Succ(cv);
            Assert.Equal(value, cv.UnChurch());
        }

        {
            var cv = value.AsChurch();
            Assert.Equal(value, cv.UnChurch());
        }
    }

    #endregion

    #region Arithmetic

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void SuccTest(uint value)
    {
        var v1 = Zero;
        var v2 = Zero;
        for (int i = 0; i < value; i++)
        {
            v1 = Succ(v1);
            v2 = SuccR(v2);
        }

        foreach (var succ in GetIntNexts())
        {
            foreach (var zero in GetInts())
            {
                var expected = zero;
                for (int i = 0; i < value; i++)
                    expected = succ(expected);

                Assert.Equal(expected, v1(x => succ(x))(zero));
                Assert.Equal(expected, v2(x => succ(x))(zero));
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void PlusTest(uint m, uint n)
    {
        var cm = Zero;
        for (int i = 0; i < m; i++)
            cm = Succ(cm);

        var cn = Zero;
        for (int i = 0; i < n; i++)
            cn = Succ(cn);

        var expected = m + n;
        var result = Plus(cm)(cn);
        Assert.Equal(expected, result.UnChurch());
        Assert.Equal(expected + expected, Plus(result)(result).UnChurch());

        result = Plus_Succ(cm)(cn);
        Assert.Equal(expected, result.UnChurch());
        Assert.Equal(expected + expected, Plus_Succ(result)(result).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void MultTest(uint m, uint n)
    {
        var cm = Zero;
        for (int i = 0; i < m; i++)
            cm = Succ(cm);

        var cn = Zero;
        for (int i = 0; i < n; i++)
            cn = Succ(cn);

        var expected = m * n;
        var result = Mult(cm)(cn);
        Assert.Equal(expected, result.UnChurch());
        Assert.Equal(expected * expected, Mult(result)(result).UnChurch());

        result = Mult_Plus(cm)(cn);
        Assert.Equal(expected, result.UnChurch());
        //Assert.Equal(expected * expected, Mult_Plus(result)(result).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void ExpTest(uint m, uint n)
    {
        if (n > 3)
            return;

        var cm = Zero;
        for (int i = 0; i < m; i++)
            cm = Succ(cm);

        var cn = Zero;
        for (int i = 0; i < n; i++)
            cn = Succ(cn);

        var expected = (uint)Math.Pow(m, n);
        var result = Exp(cm)(cn);
        Assert.Equal(expected, result.UnChurch());

        if (expected <= 10)
            Assert.Equal((uint)Math.Pow(expected, 2), Exp(result)(2u.AsChurch()).UnChurch());

        result = Exp_Mult(cm)(cn);
        Assert.Equal(expected, result.UnChurch());

        if (expected <= 10)
            Assert.Equal((uint)Math.Pow(expected, 2), Exp_Mult(result)(2u.AsChurch()).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void PredTest(uint value)
    {
        var cv = Pred(value.AsChurch());

        foreach (var succ in GetIntNexts())
        {
            foreach (var zero in GetInts())
            {
                var expected = zero;
                for (int i = 1; i < value; i++)
                    expected = succ(expected);

                Assert.Equal(expected, cv(x => succ(x))(zero));
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void MinusTest(uint m, uint n)
    {
        var expected = m > n ? m - n : 0u;
        var result = Minus(m.AsChurch())(n.AsChurch());
        Assert.Equal(expected, result.UnChurch());
        Assert.Equal(expected + 1 - expected, Minus(Succ(result))(result).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void DiffTest(uint m, uint n)
    {
        var expected = m > n ? m - n : n - m;
        var result = Diff(m.AsChurch())(n.AsChurch());
        Assert.Equal(expected, result.UnChurch());
        Assert.Equal(expected + 1 - expected, Diff(Succ(result))(result).UnChurch());
        Assert.Equal(expected + 1 - expected, Diff(result)(Succ(result)).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void MinTest(uint m, uint n)
    {
        var cm = m.AsChurch();
        var cn = n.AsChurch();
        Assert.Equal(Math.Min(m, n), Min(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void MaxTest(uint m, uint n)
    {
        var cm = m.AsChurch();
        var cn = n.AsChurch();
        Assert.Equal(Math.Max(m, n), Max(cm)(cn).UnChurch());
    }

    #endregion

    #region Logical

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void IsZeroTest(uint value)
    {
        var cv = value.AsChurch();
        Assert.Equal(value == 0, IsZero(cv).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void GEQTest(uint m, uint n)
    {
        var cm = m.AsChurch();
        var cn = n.AsChurch();
        Assert.Equal(m >= n, GEQ(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void LEQTest(uint m, uint n)
    {
        var cm = m.AsChurch();
        var cn = n.AsChurch();
        Assert.Equal(m <= n, LEQ(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void GTTest(uint m, uint n)
    {
        var cm = m.AsChurch();
        var cn = n.AsChurch();
        Assert.Equal(m > n, GT(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void LTTest(uint m, uint n)
    {
        var cm = m.AsChurch();
        var cn = n.AsChurch();
        Assert.Equal(m < n, LT(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void EQTest(uint m, uint n)
    {
        var cm = m.AsChurch();
        var cn = n.AsChurch();
        Assert.Equal(m == n, EQ(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void NEQTest(uint m, uint n)
    {
        var cm = m.AsChurch();
        var cn = n.AsChurch();
        Assert.Equal(m != n, NEQ(cm)(cn).UnChurch());
    }

    #endregion

    #region GetData

    public static IEnumerable<Func<int, int>> GetIntNexts()
    {
        yield return x => x;
        yield return x => x + 2;
        yield return x => x - 5;
    }

    public static IEnumerable<object[]> GetIntNextsData() => 
        GetIntNexts().Select(x => new object[] { x });

    public static IEnumerable<int> GetInts()
    {
        yield return 0;
        yield return 5;
        yield return -3;
    }

    public static IEnumerable<object[]> GetIntsData() =>
        GetInts().Select(x => new object[] { x });

    public static IEnumerable<uint> GetUInts()
    {
        yield return 0;
        yield return 1;
        yield return 3;
        yield return 9;
    }

    public static IEnumerable<object[]> GetUIntsData1() =>
        GetUInts().Select(x => new object[] { x });

    public static IEnumerable<object[]> GetUIntsData2() =>
        from m in GetUInts()
        from n in GetUInts()
        select new object[] { m, n };

    public static IEnumerable<object[]> GetNextIntsData() =>
        from next in GetIntNexts()
        from v in GetInts()
        select new object[] { next, v };

    #endregion

}
