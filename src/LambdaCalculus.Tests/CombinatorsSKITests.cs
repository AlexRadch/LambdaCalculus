namespace LambdaCalculus.Tests;

using static LambdaCalculus.SKI;

public class CombinatorsSKITests
{
    #region S K I

    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void ITest(object x)
    {
        Assert.Equal(x, I(x));
        Assert.Equal(x, I(I(x)));

        Assert.Equal(x, Id(x));
        Assert.Equal(x, Id(Id(x)));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void KTest(object x, object y)
    {
        Assert.Equal(x, K(x)(y));
        Assert.Equal(x, K(K(x)(y))(y));
    }

    [Fact]
    public void STest()
    {
        foreach (var x in GetBoolXs())
        {
            foreach (var y in GetBoolYs())
            {
                foreach (var z in ChurchBooleansTests.GetBools())
                {
                    var expected = x(z)(y(z));
                    Assert.Equal(expected, S(x)(y)(z));
                }
            }
        }
    }

    #endregion

    #region Combinators

    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void SkITest(object x)
    {
        Assert.Equal(x, SkI(x));
        Assert.Equal(x, SkI(SkI(x)));

        Assert.Equal(x, SkId(x));
        Assert.Equal(x, SkId(SkId(x)));
    }

    #region Iota

    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void SkιITest(object x)
    {
        Assert.Equal(x, Skι(Skι)(x));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void SkιKTest(object x, object y)
    {
        Assert.Equal(x, Skι(Skι(Skι(Skι)))(x)(y));
    }

    [Fact]
    public void SkιSTest()
    {
        foreach (var x in GetBoolXs())
        {
            foreach (var y in GetBoolYs())
            {
                foreach (var z in ChurchBooleansTests.GetBools())
                {
                    var expected = x(z)(y(z));
                    Assert.Equal(expected, Skι(Skι(Skι(Skι(Skι))))(x)(y)(z));
                }
            }
        }
    }

    #endregion

    #region BCKW

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void BTest(int z)
    {
        {
            // Test case 1: Simple numeric composition
            // multiplyBy2 = n => n * 2
            // add1 = n => n + 1
            // Expected: f1(f2(z)) = (z + 1) * 2
            // Expected: f2(f1(z)) = (z * 2) + 1
            // Expected: f1(f1(z)) = (z * 2) * 2
            // Expected: f2(f2(z)) = (z + 1) + 1

            Func<dynamic, dynamic> multiplyBy2 = n => n * 2;
            Func<dynamic, dynamic> add1 = n => n + 1;

            Assert.Equal((z + 1) * 2, B(multiplyBy2)(add1)(z));
            Assert.Equal((z * 2) + 1, B(add1)(multiplyBy2)(z));
            Assert.Equal(z * 2 * 2, B(multiplyBy2)(multiplyBy2)(z));
            Assert.Equal(z + 1 + 1, B(add1)(add1)(z));
        }

        {
            // Test case 2: More complex composition
            // square = n => n * n
            // add2 = n => n + 2
            // Expected: f1(f2(z)) = (z + 2)^2
            // Expected: f2(f1(z)) = (z^2) + 2
            // Expected: f1(f1(z)) = (z^2)^2
            // Expected: f2(f2(z)) = (z + 2) + 2

            Func<int, int> square = n => n * n;
            Func<int, int> add2 = n => n + 2;

            Assert.Equal((z + 2) * (z + 2), B(square)(add2)(z));
            Assert.Equal((z * z) + 2, B(add2)(square)(z));
            Assert.Equal((z * z) * (z * z), B(square)(square)(z));
            Assert.Equal(z + 2 + 2, B(add2)(add2)(z));
        }
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void CTest(int y)
    {
        {
            // Test case 1: Simple numeric composition
            // x = f => n => f(n) * 2
            // z = n => n + 1
            // Expected: x(z(y)) = (y + 1) * 2

#pragma warning disable IDE0039 // Use local function
            Func<dynamic, dynamic> x = f => new Func<dynamic, dynamic>(n => f(n) * 2);
            Func<dynamic, dynamic> z = n => n + 1;
#pragma warning restore IDE0039 // Use local function

            Assert.Equal((y + 1) * 2, C(x)(y)(z));
        }

        {
            // Test case 2: More complex composition
            // x = f => n => f(n)^2
            // z = n => n + 2
            // Expected: x(y(z)) = (y + 2)^2

#pragma warning disable IDE0039 // Use local function
            Func<Func<int, int>, Func<int, int>> x = f => n => { var t = f(n); return t * t; };
            Func<int, int> z = n => n + 2;
#pragma warning restore IDE0039 // Use local function

            Assert.Equal((y + 2) * (y + 2), C(x)(y)(z));
        }
    }

    [Theory]
    [MemberData(nameof(GetIntsData1))]
    public void WTest(int y)
    {
        {
            // Test case 1: Addition
            // x = a => b => a + b
            // Expected: x(y)(y) = y + y

            Func<dynamic, Func<dynamic, dynamic>> x = a => b => a + b;

            Assert.Equal(y + y, W(x)(y));
        }

        {
            // Test case 2: Multiplication
            // x = a => b => a * b
            // Expected: x(y)(y) = y * y

            Func<int, Func<int, int>> x = a => b => a * b;

            Assert.Equal(y * y, W(x)(y));
        }

        {
            // Test case 3: More complex operation
            // x = a => b => a * b - b / 2 + a
            // Expected: x(y)(y) = y * y - y / 2 + y

            Func<int, Func<int, int>> x = a => b => a * b - b / 2 + a;

            Assert.Equal(y * y - y / 2 + y, W(x)(y));
        }
    }

    #endregion

    #endregion

    #region Self-application and recursion

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void UTest(uint x)
    {
        Func<dynamic, dynamic> SaSum(dynamic r) => x =>
            x == 0 ? 0 : x + r(r)(x - 1);

        var uSum = U(new Func<dynamic, dynamic>(SaSum));

        var expected = (uint)Enumerable.Range(1, (int)x).Sum();
        Assert.Equal(expected, (uint)uSum(x));
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void UfTest(uint x)
    {
        Func<uint, uint> SaSum(SelfApplicable<Func<uint, uint>> r) => x =>
            x == 0 ? 0 : x + r(r)(x - 1);

        var uSum = Uf<Func<uint, uint>>(SaSum);

        var expected = (uint)Enumerable.Range(1, (int)x).Sum();
        Assert.Equal(expected, uSum(x));
    }

    [Theory]
    [MemberData(nameof(GetUIntsData1))]
    public void ZSumTest(uint x)
    {
        {
            Func<uint, uint> RSum(Func<uint, uint> r) => x => x == 0 ? x : x + r(x - 1);

            var Sum = Zf<uint, uint>(RSum);
            var expected = (uint)Enumerable.Range(0, (int)x + 1).Sum();
            Assert.Equal(expected, Sum(x));
        }

        {
            var RSum = (dynamic r) => (dynamic x) => x == 0 ? x : x + r(x - 1);
            var expected = (uint)Enumerable.Range(0, (int)x + 1).Sum();
            Assert.Equal(expected, Z(RSum)(x));
        }
    }

    #endregion

    #region SKI

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void TTest(object @true, object @false)
    {
        Assert.Equal(@true, T(@true)(@false));
        Assert.Equal(@true, TB(@true)(@false));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void FTest(object @true, object @false)
    {
        Assert.Equal(@false, F(K(@true))(K(@false))(K));
        Assert.Equal(@false, FB(K(@true))(K(@false))(K));
    }

    [Theory]
    [MemberData(nameof(GetBoolsData1))]
    public void NotTest(bool x)
    {
        var sx = x.ToSki();
        Assert.Equal(!x, UnSkiBoolean(Not(sx)));
        Assert.Equal(x, UnSkiBoolean(Not(Not(sx))));

        Assert.Equal(!x, NotB(sx).UnSki());
        Assert.Equal(x, NotB(NotB(sx)).UnSki());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void OrTest(bool x, bool y)
    {
        var sx = x.ToSki();
        var sy = y.ToSki();

        var expected = x || y;
        Assert.Equal(expected, UnSkiBoolean(Or(sx)(sy)));
        Assert.Equal(expected, OrB(sx)(sy).UnSki());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void AndTest(bool x, bool y)
    {
        var sx = x.ToSki();
        var sy = y.ToSki();

        var expected = x && y;
        Assert.Equal(expected, UnSkiBoolean(And(sx)(sy)));
        Assert.Equal(expected, AndB(sx)(sy).UnSki());
    }

    #endregion

    #region Convertions

    [Theory]
    [MemberData(nameof(GetBoolsData1))]
    public void BoolToSkiTest(bool b)
    {
        foreach (var @true in GetDynamics())
            foreach (var @false in GetDynamics())
            {
                var expected = b ? @true : @false;
                Assert.Equal(expected, b.ToSki()(K(@true))(K(@false))(K));
            }
    }

    [Theory]
    [MemberData(nameof(GetBoolsData1))]
    public void UnSkiTest(bool a)
    {
        var ca = a.ToSki();
        Assert.Equal(a, ca.UnSki());
        Assert.Equal(a, UnSkiBoolean(ca));
    }

    #endregion

    #region GetData

    public static IEnumerable<object[]> GetBoolsData1 => ChurchBooleansTests.GetBoolsData1();

    public static IEnumerable<object[]> GetBoolsData2 => ChurchBooleansTests.GetBoolsData2();

    public static IEnumerable<dynamic> GetDynamics()
    {
        foreach (var a in ChurchBooleansTests.GetBools())
        {
            yield return a;
            yield return Convert.ToInt32(a);
            yield return Convert.ToString(a);
        }
        yield return new NotImplementedException();
        yield return (Action)(static () => throw new NotImplementedException());
    }

    public static IEnumerable<object[]> GetDynamicsData1() =>
        GetDynamics().Select(a => new object[] { a });

    public static IEnumerable<object[]> GetDynamicsData2() =>
        from a in GetDynamics()
        from b in GetDynamics()
        select new object[] { a, b };

    public static IEnumerable<Func<bool, bool>> GetBoolYs()
    {
        yield return new Func<bool, bool>(x => false);
        yield return new Func<bool, bool>(x => !x);
        yield return new Func<bool, bool>(x => x);
        yield return new Func<bool, bool>(x => true);
    }

    public static IEnumerable<Func<bool, Func<bool, bool>>> GetBoolXs()
    {
        yield return new Func<bool, Func<bool, bool>>(x => y => false);
        yield return new Func<bool, Func<bool, bool>>(x => y => !(x || y));
        yield return new Func<bool, Func<bool, bool>>(x => y => !x && y);
        yield return new Func<bool, Func<bool, bool>>(x => y => !x);

        yield return new Func<bool, Func<bool, bool>>(x => y => x && !y);
        yield return new Func<bool, Func<bool, bool>>(x => y => !y);
        yield return new Func<bool, Func<bool, bool>>(x => y => x ^ y);
        yield return new Func<bool, Func<bool, bool>>(x => y => !(x && y));

        yield return new Func<bool, Func<bool, bool>>(x => y => x && y);
        yield return new Func<bool, Func<bool, bool>>(x => y => !(x ^ y));
        yield return new Func<bool, Func<bool, bool>>(x => y => y);
        yield return new Func<bool, Func<bool, bool>>(x => y => !x || y);

        yield return new Func<bool, Func<bool, bool>>(x => y => x);
        yield return new Func<bool, Func<bool, bool>>(x => y => x || !y);
        yield return new Func<bool, Func<bool, bool>>(x => y => x || y);
        yield return new Func<bool, Func<bool, bool>>(x => y => true);
    }

    public static IEnumerable<object[]> GetIntsData1 => ChurchSignedNumbersTests.GetIntsData1();

    public static IEnumerable<object[]> GetUIntsData1 => ChurchNumeralsTests.GetUIntsData1();

    #endregion
}
