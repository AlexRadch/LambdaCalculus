namespace LambdaCalculus.Tests;

using static LambdaCalculus.Church;

public class ChurchPairsTests
{
    #region Operations

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void CreatePairTest(object first, object second)
    {
        var pair = Pair(first)(second);
        Assert.Equal(first, pair(TrueF<dynamic, dynamic>));
        Assert.Equal(second, pair(FalseF<dynamic, dynamic>));
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void CreateUIntPairTest(uint first, uint second)
    {
        var pair = Pair(first)(second);
        Assert.Equal(first, pair(TrueF<dynamic, dynamic>));
        Assert.Equal(second, pair(FalseF<dynamic, dynamic>));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void FirstTest(object first, object second)
    {
        var pair = Pair(first)(second);
        Assert.Equal(first, First(pair));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void SecondTest(object first, object second)
    {
        var pair = Pair(first)(second);
        Assert.Equal(second, Second(pair));
    }

    #endregion

    #region Extensions

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void UnChurchTest(object first, object second)
    {
        var pair = Pair(first)(second);
        Assert.Equal((first, second), pair.UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void UnChurchUIntTest(uint first, uint second)
    {
        var pair = Pair(first)(second);
        Assert.Equal((first, second), pair.UnChurch());
    }


    #endregion

    #region GetData

    public static IEnumerable<object[]> GetDynamicsData2() => ChurchBooleansTests.GetDynamicsData2();

    public static IEnumerable<object[]> GetUIntsData2() => ChurchNumeralsTests.GetUIntsData2();

    #endregion
}
