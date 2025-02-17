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

    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void SkITest(object x)
    {
        Assert.Equal(x, SkI(x));
        Assert.Equal(x, SkI(SkI(x)));

        Assert.Equal(x, SkId(x));
        Assert.Equal(x, SkId(SkId(x)));
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

    #endregion
}
