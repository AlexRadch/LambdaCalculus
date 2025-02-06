namespace LambdaCalculus.Tests;

using static LambdaCalculus.Church;

public class ChurchPairsTests
{
    #region Operations

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void CreatePairTest(object first, object second)
    {
        var pair = CreatePair(first)(second);
        Assert.Equal(first, pair(True));
        Assert.Equal(second, pair(False));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void FirstTest(object first, object second)
    {
        var pair = CreatePair(first)(second);
        Assert.Equal(first, First(pair));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void SecondTest(dynamic first, dynamic second)
    {
        var pair = CreatePair(first)(second);
        Assert.Equal(second, Second(pair));
    }

    #endregion

    #region Extensions

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void UnChurchTest(object first, object second)
    {
        var pair = CreatePair(first)(second);
        Assert.Equal((first, second), pair.UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetUIntsData2))]
    public void UnChurchUIntTest(uint first, uint second)
    {
        var pair = CreatePair(first)(second);
        Assert.Equal((first, second), pair.UnChurch<uint, uint>());
    }


    #endregion

    #region GetData

    public static IEnumerable<object[]> GetDynamicsData2() => ChurchBooleansTests.GetDynamicsData2();

    public static IEnumerable<object[]> GetUIntsData2() => ChurchNumeralsTests.GetUIntsData2();

    #endregion
}
