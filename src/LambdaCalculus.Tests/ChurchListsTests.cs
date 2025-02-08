namespace LambdaCalculus.Tests;

using static LambdaCalculus.Church;

public class ChurchListsTests
{
    #region Operations

    [Fact]
    public void NilTest()
    {
        {
            ListNode nil = NilF;

            Boolean result = nil(False)(True);
            Assert.True(result.UnChurch());
        }
        {

            var nil = Nil;

            Boolean result = nil(False)(True);
            Assert.True(result.UnChurch());
        }
    }

    [Fact]
    public void ConsTest()
    {
        var list0 = Nil;
        Assert.Equal(Nil, list0);

        var list1 = Cons(1)(list0);
        Assert.Equal(1, (int)list1(True));
        Assert.Equal(list0, (ListNode)list1(False));

        var list2 = Cons(2)(list1);
        Assert.Equal(2, (int)list2(True));
        Assert.Equal(list1, (ListNode)list2(False));

        var list3 = Cons(3)(list2);
        Assert.Equal(3, (int)list3(True));
        Assert.Equal(list2, (ListNode)list3(False));
    }

    [Fact]
    public void HeadTest()
    {
        var list0 = Nil;

        var list1 = Cons(1)(list0);
        Assert.Equal(1, (int)Head(list1));

        var list2 = Cons(2)(list1);
        Assert.Equal(2, (int)Head(list2));

        var list3 = Cons(3)(list2);
        Assert.Equal(3, (int)Head(list3));
    }

    [Fact]
    public void TailTest()
    {
        var list0 = Nil;

        var list1 = Cons(1)(list0);
        Assert.Equal(list0, Tail(list1));

        var list2 = Cons(2)(list1);
        Assert.Equal(list1, Tail(list2));

        var list3 = Cons(3)(list2);
        Assert.Equal(list2, Tail(list3));
    }

    #endregion

    #region Logical

    [Fact]
    public void IsNilTest()
    {
        var list0 = Nil;
        Assert.True(IsNil(list0).UnChurch());

        var list1 = Cons(1)(list0);
        Assert.False(IsNil(list1).UnChurch());

        var list2 = Cons(2)(list1);
        Assert.False(IsNil(list2).UnChurch());

        var list3 = Cons(3)(list2);
        Assert.False(IsNil(list3).UnChurch());
    }

    #endregion

    #region Extensions

    [Fact]
    public void ToChurchListTest()
    {
        var array = new[] { 1, 2, 3 };
        var churchList = array.ToChurchList();

        Assert.Equal(1, (int)Head(churchList));
        Assert.Equal(2, (int)Head(Tail(churchList)));
        Assert.Equal(3, (int)Head(Tail(Tail(churchList))));
        Assert.True(IsNil(Tail(Tail(Tail(churchList)))).UnChurch());
    }

    [Fact]
    public void UnChurchTest()
    {
        var list = Cons(1)(Cons(2)(Cons(3)(Nil)));
        var array = list.UnChurch().ToArray();

        Assert.Equal(3, array.Length);
        Assert.Equal(1, array[0]);
        Assert.Equal(2, array[1]);
        Assert.Equal(3, array[2]);
    }

    #endregion

    #region GetData

    public static IEnumerable<object[]> GetDynamicsData1() => ChurchBooleansTests.GetDynamicsData1();

    #endregion
}
