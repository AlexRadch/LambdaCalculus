namespace LambdaCalculus.Tests;

using static LambdaCalculus.Combinators;

public class CombinatorsSKITests
{
    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void ITest(object x)
    {
        Assert.Equal(x, I(x));
        Assert.Equal(x, I(I(x)));

        dynamic dx = x;
        Assert.Equal(dx, I(dx));
        Assert.Equal(dx, I(I(dx)));
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

    #region SK

    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void SkITest(object x)
    {
        Assert.Equal(x, SkI(x));
    }

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

    #region GetData

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
