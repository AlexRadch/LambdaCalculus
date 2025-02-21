using static LambdaCalculus.Church;

namespace LambdaCalculus.Tests;

public class ChurchNumeralsTests
{
    #region Constants

    [Theory]
    [MemberData(nameof(GetNextIntsData))]
    public void ZeroTest(Func<int, int> succ, int zero)
    {
        NextValue next = x => succ(x);
        Assert.Equal(zero, ZeroF(next)(zero));
        Assert.Equal(zero, Zero(next)(zero));
        Assert.Equal(zero, LazyZero()(next)(zero));

        Assert.Equal(zero, ZeroF_False(next)(zero));
        Assert.Equal(zero, Zero_False(next)(zero));

        next = x => throw new NotImplementedException();
        Assert.Equal(zero, ZeroF(next)(zero));
        Assert.Equal(zero, Zero(next)(zero));
        Assert.Equal(zero, LazyZero()(next)(zero));

        Assert.Equal(zero, ZeroF_False(next)(zero));
        Assert.Equal(zero, Zero_False(next)(zero));
    }

    [Theory]
    [MemberData(nameof(GetNextIntsData))]
    public void OneTest(Func<int, int> succ, int zero)
    {
        NextValue next = x => succ(x);
        var one = succ(zero);
        Assert.Equal(one, OneF(next)(zero));
        Assert.Equal(one, One(next)(zero));
        Assert.Equal(one, LazyOne()(next)(zero));

        Assert.Equal(one, OneF(OneF(next))(zero));
        Assert.Equal(one, One(One(next))(zero));
        Assert.Equal(one, LazyOne()(LazyOne()(next))(zero));

        var two = succ(one);
        Assert.Equal(two, OneF(next)(one));
        Assert.Equal(two, One(next)(one));
        Assert.Equal(two, LazyOne()(LazyOne()(next))(one));

        next = x => succ(succ(x));
        Assert.Equal(two, OneF(next)(zero));
        Assert.Equal(two, One(next)(zero));
        Assert.Equal(two, LazyOne()(next)(zero));
    }

    #endregion

    #region Convertions

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void ToChurchTest(uint value)
    {
        var cv = value.ToChurch();

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
            var cv = value.ToChurch();
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
            Assert.Equal((uint)Math.Pow(expected, 2), Exp(result)(2u.ToChurch()).UnChurch());

        result = Exp_Mult(cm)(cn);
        Assert.Equal(expected, result.UnChurch());

        if (expected <= 10)
            Assert.Equal((uint)Math.Pow(expected, 2), Exp_Mult(result)(2u.ToChurch()).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void PredTest(uint value)
    {
        var cv = Pred(value.ToChurch());

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
        var result = Minus(m.ToChurch())(n.ToChurch());
        Assert.Equal(expected, result.UnChurch());
        Assert.Equal(expected + 1 - expected, Minus(Succ(result))(result).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void DiffTest(uint m, uint n)
    {
        var expected = m > n ? m - n : n - m;
        var result = Diff(m.ToChurch())(n.ToChurch());
        Assert.Equal(expected, result.UnChurch());
        Assert.Equal(expected + 1 - expected, Diff(Succ(result))(result).UnChurch());
        Assert.Equal(expected + 1 - expected, Diff(result)(Succ(result)).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void MinTest(uint m, uint n)
    {
        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(Math.Min(m, n), Min(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void MaxTest(uint m, uint n)
    {
        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(Math.Max(m, n), Max(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void DivideTest(uint m, uint n)
    {
        if (n == 0)
            return;

        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(m / n, DivideR_mn(cm)(cn).UnChurch());

        Assert.Equal((m > 0 ? (m - 1) : 0) / n, Divide1R(cm)(cn).UnChurch());
        Assert.Equal(m / n, DivideR(cm)(cn).UnChurch());

        {
            Func<Numeral, Func<Numeral, Numeral>> div1 = Div1(null!);
            if (Minus(cm)(cn).UnChurch() == 0)
                Assert.Equal((m > 0 ? (m - 1) : 0) / n, div1(cm)(cn).UnChurch());
            else
                Assert.Throws<NullReferenceException>(() => div1(cm)(cn));
        }

        Assert.Equal((m > 0 ? (m - 1) : 0) / n, Divide1(cm)(cn).UnChurch());
        Assert.Equal(m / n, Divide(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void ModuloTest(uint m, uint n)
    {
        if (n == 0)
            return;

        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(m % n, ModuloR_mn(cm)(cn).UnChurch());

        {
            Func<Numeral, Func<Numeral, Numeral>> mod1 = Mod1(null!);
            if (Minus(cm)(cn).UnChurch() == 0)
                Assert.Equal((m > 0 ? (m - 1) : 0) % n, mod1(cm)(cn).UnChurch());
            else
                Assert.Throws<NullReferenceException>(() => mod1(cm)(cn));
        }

        //Assert.Equal((m > 0 ? (m - 1) : 0) % n, Modulo1(cm)(cn).UnChurch());
        Assert.Equal(m % n, Modulo(cm)(cn).UnChurch());
    }

    #endregion

    #region Functions

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void FactorialTest(uint x)
    {
        if (x > 5)
            return;

        var cx = x.ToChurch();
        var expected = 1u;
        for (uint i = 1; i <= x; i++)
            expected *= i;

        Assert.Equal(expected, FactorialR(cx).UnChurch());
        Assert.Equal(expected, Factorial(cx).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void FibonacciTest(uint x)
    {
        var cx = x.ToChurch();
        var expected = fib(x);

        Assert.Equal(expected, FibonacciR(cx).UnChurch());
        Assert.Equal(expected, Fibonacci(cx).UnChurch());

        static uint fib(uint x) => (x <= 1) ? x : fib(x - 1) + fib(x - 2);
    }

    #endregion

    #region Logical

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void IsZeroTest(uint value)
    {
        var cv = value.ToChurch();
        Assert.Equal(value == 0, IsZero(cv).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void GEQTest(uint m, uint n)
    {
        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(m >= n, GEQ(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void LEQTest(uint m, uint n)
    {
        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(m <= n, LEQ(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void GTTest(uint m, uint n)
    {
        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(m > n, GT(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void LTTest(uint m, uint n)
    {
        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(m < n, LT(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void EQTest(uint m, uint n)
    {
        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(m == n, EQ(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void NEQTest(uint m, uint n)
    {
        var cm = m.ToChurch();
        var cn = n.ToChurch();
        Assert.Equal(m != n, NEQ(cm)(cn).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void IsEvenTest(uint value)
    {
        var cv = value.ToChurch();
        Assert.Equal(value % 2 == 0, IsEven(cv).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void IsOddTest(uint value)
    {
        var cv = value.ToChurch();
        Assert.Equal(value % 2 != 0, IsOdd(cv).UnChurch());
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

    public static IEnumerable<int> GetInts() => ChurchSignedNumbersTests.GetInts();

    public static IEnumerable<uint> GetUInts()
    {
        yield return 0;
        yield return 1;
        yield return 4;
        yield return 5;
    }

    public static IEnumerable<object[]> GetUIntsData1() => GetUInts().Select(x => new object[] { x });

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
