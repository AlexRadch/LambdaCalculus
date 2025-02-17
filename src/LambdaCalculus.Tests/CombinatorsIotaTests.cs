namespace LambdaCalculus.Tests;

using static LambdaCalculus.Iota;

public class CombinatorsIotaTests
{
    //[Fact]
    //public void ιTest()
    //{
    //}

    #region SKI

    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void IotaITest(object x)
    {
        Assert.Equal(x, IotaI(x));
        Assert.Equal(x, IotaI(IotaI(x)));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void IotaKTest(object x, object y)
    {
        Assert.Equal(x, IotaK(x)(y));
        Assert.Equal(x, IotaK(IotaK(x)(y))(y));
    }

    [Fact]
    public void IotaSTest()
    {
        foreach (var x in GetBoolXs())
        {
            foreach (var y in GetBoolYs())
            {
                foreach (var z in ChurchBooleansTests.GetBools())
                {
                    var expected = x(z)(y(z));
                    Assert.Equal(expected, IotaS(x)(y)(z));
                }
            }
        }
    }

    #endregion

    #region GetData

    public static IEnumerable<object[]> GetDynamicsData1() => CombinatorsSKITests.GetDynamicsData1();

    public static IEnumerable<object[]> GetDynamicsData2() => CombinatorsSKITests.GetDynamicsData2();

    public static IEnumerable<Func<bool, bool>> GetBoolYs() => CombinatorsSKITests.GetBoolYs();

    public static IEnumerable<Func<bool, Func<bool, bool>>> GetBoolXs() => CombinatorsSKITests.GetBoolXs();

    #endregion
}
