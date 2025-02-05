namespace LambdaCalculus.Tests;

using static LambdaCalculus.Extensions;

public class ExtensionsTests
{
    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void AsLazyTest(object value)
    {
        var func = AsLazy(() => value);
        Assert.Same(func, AsLazy(func));
        Assert.Same(value, func());
    }

    #region GetData

    public static IEnumerable<object[]> GetDynamicsData1() =>
         ChurchBooleansTests.GetDynamicsData1();


    #endregion
}
