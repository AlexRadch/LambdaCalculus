namespace LambdaCalculus.Tests;

using static LambdaCalculus.Church;

public class ChurchListsTests
{
    #region Constants

    [Fact]
    public void NilTest()
    {
        {
            ListNode nil = NilF;

            Boolean result = nil(h => t => False)(True);
            Assert.True(result.UnChurch());
        }
        {

            var nil = Nil;

            Boolean result = nil(h => t => False)(True);
            Assert.True(result.UnChurch());
        }
    }

    #endregion

    #region Constructors

    [Fact]
    public void ConsTest()
    {
        var list0 = Nil;
        Assert.Equal(Nil, list0);

        var list1 = Cons(1)(list0);
        Assert.Equal(1, (int)list1(h => t => h));
        Assert.Equal(list0, list1(h => t => t));

        var list2 = Cons(2)(list1);
        Assert.Equal(2, (int)list2(h => t => h));
        Assert.Equal(list1, (ListNode)list2(h => t => t));

        var list3 = Cons(3)(list2);
        Assert.Equal(3, (int)list3(h => t => h));
        Assert.Equal(list2, (ListNode)list3(h => t => t));
    }

    #endregion

    #region Properties

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

    [Fact]
    public void LengthTest()
    {
        var list0 = Nil;
        var result = Length(list0);
        Assert.Equal(0u, result.UnChurch());

        var list1 = Cons(1)(list0);
        result = Length(list1);
        Assert.Equal(1u, result.UnChurch());

        var list2 = Cons(2)(list1);
        result = Length(list2);
        Assert.Equal(2u, result.UnChurch());

        var list3 = Cons(3)(list2);
        result = Length(list3);
        Assert.Equal(3u, result.UnChurch());
    }

    #endregion

    #region Operations

    [Fact]
    public void MapTest()
    {
        var list0 = Nil;
        var mappedList = Map(x => x * 2)(list0);
        Assert.True(IsNil(mappedList).UnChurch());

        var list1 = Cons(1)(list0);
        mappedList = Map(x => x * 2)(list1);
        Assert.Equal(2, (int)Head(mappedList));
        Assert.True(IsNil(Tail(mappedList)).UnChurch());

        var list2 = Cons(2)(list1);
        mappedList = Map(x => x * 2)(list2);
        Assert.Equal(4, (int)Head(mappedList));
        Assert.Equal(2, (int)Head(Tail(mappedList)));
        Assert.True(IsNil(Tail(Tail(mappedList))).UnChurch());

        var list3 = Cons(3)(list2);
        mappedList = Map(x => x * 2)(list3);
        Assert.Equal(6, (int)Head(mappedList));
        Assert.Equal(4, (int)Head(Tail(mappedList)));
        Assert.Equal(2, (int)Head(Tail(Tail(mappedList))));
        Assert.True(IsNil(Tail(Tail(Tail(mappedList)))).UnChurch());
    }

    [Fact]
    public void FoldTest()
    {
        static Func<dynamic, dynamic> Add(dynamic a) => x => -a + x;

        var list0 = Nil;
        var result = Fold(Add)(100)(list0);
        Assert.Equal(100, result);

        var list1 = Cons(1)(list0);
        result = Fold(Add)(100)(list1);
        Assert.Equal(-100 + 1, result);

        var list2 = Cons(2)(list1);
        result = Fold(Add)(100)(list2);
        Assert.Equal(-(-100 + 2) + 1, result);

        var list3 = Cons(3)(list2);
        result = Fold(Add)(100)(list3);
        Assert.Equal(-(-(-100 + 3) + 2) + 1, result);
    }

    [Fact]
    public void RFoldTest()
    {
        static Func<dynamic, dynamic> Add(dynamic a) => x => -a + x;

        var list0 = Nil;
        var result = RFold(Add)(100)(list0);
        Assert.Equal(100, result);

        var list1 = Cons(1)(list0);
        result = RFold(Add)(100)(list1);
        Assert.Equal(-100 + 1, result);

        var list2 = Cons(2)(list1);
        result = RFold(Add)(100)(list2);
        Assert.Equal(-(-100 + 1) + 2, result);

        var list3 = Cons(3)(list2);
        result = RFold(Add)(100)(list3);
        Assert.Equal(-(-(-100 + 1) + 2) + 3, result);
    }

    [Fact]
    public void FilterTest()
    {
        static Boolean Test(dynamic x) => Church.ToChurch(x % 2 == 0);

        var list0 = Nil;
        var filteredList = Filter(Test)(list0);
        Assert.True(IsNil(filteredList).UnChurch());

        var list1 = Cons(1)(list0);
        filteredList = Filter(Test)(list1);
        Assert.True(IsNil(filteredList).UnChurch());

        var list2 = Cons(2)(list1);
        filteredList = Filter(Test)(list2);
        Assert.Equal(2, (int)Head(filteredList));
        Assert.True(IsNil(Tail(filteredList)).UnChurch());

        var list3 = Cons(3)(list2);
        filteredList = Filter(Test)(list3);
        Assert.Equal(2, (int)Head(filteredList));
        Assert.True(IsNil(Tail(filteredList)).UnChurch());

        var list4 = Cons(4)(list3);
        filteredList = Filter(Test)(list4);
        Assert.Equal(4, (int)Head(filteredList));
        Assert.Equal(2, (int)Head(Tail(filteredList)));
        Assert.True(IsNil(Tail(Tail(filteredList))).UnChurch());
    }

    [Fact]
    public void ReverseTest()
    {
        var list0 = Nil;
        var reversedList = Reverse(list0);
        Assert.True(IsNil(reversedList).UnChurch());

        var list1 = Cons(1)(list0);
        reversedList = Reverse(list1);
        Assert.Equal(1, (int)Head(reversedList));
        Assert.True(IsNil(Tail(reversedList)).UnChurch());

        var list2 = Cons(2)(list1);
        reversedList = Reverse(list2);
        Assert.Equal(1, (int)Head(reversedList));
        Assert.Equal(2, (int)Head(Tail(reversedList)));
        Assert.True(IsNil(Tail(Tail(reversedList))).UnChurch());

        var list3 = Cons(3)(list2);
        reversedList = Reverse(list3);
        Assert.Equal(1, (int)Head(reversedList));
        Assert.Equal(2, (int)Head(Tail(reversedList)));
        Assert.Equal(3, (int)Head(Tail(Tail(reversedList))));
        Assert.True(IsNil(Tail(Tail(Tail(reversedList)))).UnChurch());
    }

    [Fact]
    public void ConcatTest()
    {
        var list0 = Nil;
        var list1 = Cons(1)(list0);
        var list2 = Cons(2)(list1);
        var list3 = Cons(3)(list2);

        var list4 = Nil;
        var list5 = Cons(4)(list4);
        var list6 = Cons(5)(list5);
        var list7 = Cons(6)(list6);
        var concatenatedList = Concat(list3)(list7);

        Assert.Equal(3, (int)Head(concatenatedList));
        Assert.Equal(2, (int)Head(Tail(concatenatedList)));
        Assert.Equal(1, (int)Head(Tail(Tail(concatenatedList))));
        Assert.Equal(6, (int)Head(Tail(Tail(Tail(concatenatedList)))));
        Assert.Equal(5, (int)Head(Tail(Tail(Tail(Tail(concatenatedList))))));
        Assert.Equal(4, (int)Head(Tail(Tail(Tail(Tail(Tail(concatenatedList)))))));
        Assert.True(IsNil(Tail(Tail(Tail(Tail(Tail(Tail(concatenatedList))))))).UnChurch());
    }

    [Fact]
    public void AppendTest()
    {
        var list0 = Nil;
        var list1 = Cons(1)(list0);
        var list2 = Cons(2)(list1);
        var list3 = Cons(3)(list2);


        var appendedList = Append(list3)(4);
        appendedList = Append(appendedList)(5);
        appendedList = Append(appendedList)(6);

        Assert.Equal(3, (int)Head(appendedList));
        Assert.Equal(2, (int)Head(Tail(appendedList)));
        Assert.Equal(1, (int)Head(Tail(Tail(appendedList))));
        Assert.Equal(4, (int)Head(Tail(Tail(Tail(appendedList)))));
        Assert.Equal(5, (int)Head(Tail(Tail(Tail(Tail(appendedList))))));
        Assert.Equal(6, (int)Head(Tail(Tail(Tail(Tail(Tail(appendedList)))))));
        Assert.True(IsNil(Tail(Tail(Tail(Tail(Tail(Tail(appendedList))))))).UnChurch());
    }

    [Fact]
    public void SkipTest()
    {
        var list0 = Nil;
        var list1 = Cons(1)(list0);
        var list2 = Cons(2)(list1);
        var list3 = Cons(3)(list2);

        var skippedList = Skip(0u.ToChurch())(list3);
        Assert.Equal(3, (int)Head(skippedList));
        Assert.Equal(2, (int)Head(Tail(skippedList)));
        Assert.Equal(1, (int)Head(Tail(Tail(skippedList))));
        Assert.True(IsNil(Tail(Tail(Tail(skippedList)))).UnChurch());

        skippedList = Skip(1u.ToChurch())(list3);
        Assert.Equal(2, (int)Head(skippedList));
        Assert.Equal(1, (int)Head(Tail(skippedList)));
        Assert.True(IsNil(Tail(Tail(skippedList))).UnChurch());

        skippedList = Skip(2u.ToChurch())(list3);
        Assert.Equal(1, (int)Head(skippedList));
        Assert.True(IsNil(Tail(skippedList)).UnChurch());

        for(uint i = 3; i < 10; i++)
        {
            skippedList = Skip(i.ToChurch())(list3);
            Assert.True(IsNil(skippedList).UnChurch());
        }
    }

    [Fact]
    public void SkipLastTest()
    {
        var list0 = Nil;

        var list1 = Cons(1)(list0);
        var list2 = Cons(2)(list1);
        var list3 = Cons(3)(list2);

        var skippedList = SkipLast(0u.ToChurch())(list3);
        Assert.Equal(3, (int)Head(skippedList));
        Assert.Equal(2, (int)Head(Tail(skippedList)));
        Assert.Equal(1, (int)Head(Tail(Tail(skippedList))));
        Assert.True(IsNil(Tail(Tail(Tail(skippedList)))).UnChurch());

        skippedList = SkipLast(1u.ToChurch())(list3);
        Assert.Equal(3, (int)Head(skippedList));
        Assert.Equal(2, (int)Head(Tail(skippedList)));
        Assert.True(IsNil(Tail(Tail(skippedList))).UnChurch());

        skippedList = SkipLast(2u.ToChurch())(list3);
        Assert.Equal(3, (int)Head(skippedList));
        Assert.True(IsNil(Tail(skippedList)).UnChurch());

        for (uint i = 3; i < 10; i++)
        {
            skippedList = SkipLast(i.ToChurch())(list3);
            Assert.True(IsNil(skippedList).UnChurch());
        }
    }

    [Fact]
    public void TakeTest()
    {
        var list0 = Nil;
        var list1 = Cons(1)(list0);
        var list2 = Cons(2)(list1);
        var list3 = Cons(3)(list2);

        var takenList = Take(0u.ToChurch())(list3);
        Assert.True(IsNil(takenList).UnChurch());

        takenList = Take(1u.ToChurch())(list3);
        Assert.Equal(3, (int)Head(takenList));
        Assert.True(IsNil(Tail(takenList)).UnChurch());

        takenList = Take(2u.ToChurch())(list3);
        Assert.Equal(3, (int)Head(takenList));
        Assert.Equal(2, (int)Head(Tail(takenList)));
        Assert.True(IsNil(Tail(Tail(takenList))).UnChurch());

        for (uint i = 3; i < 10; i++)
        {
            takenList = Take(3u.ToChurch())(list3);
            Assert.Equal(3, (int)Head(takenList));
            Assert.Equal(2, (int)Head(Tail(takenList)));
            Assert.Equal(1, (int)Head(Tail(Tail(takenList))));
            Assert.True(IsNil(Tail(Tail(Tail(takenList)))).UnChurch());
        }
    }

    [Fact]
    public void TakeLastTest()
    {
        var list0 = Nil;
        var list1 = Cons(1)(list0);
        var list2 = Cons(2)(list1);
        var list3 = Cons(3)(list2);

        var takenList = TakeLast(0u.ToChurch())(list3);
        Assert.True(IsNil(takenList).UnChurch());

        takenList = TakeLast(1u.ToChurch())(list3);
        Assert.Equal(1, (int)Head(takenList));
        Assert.True(IsNil(Tail(takenList)).UnChurch());

        takenList = TakeLast(2u.ToChurch())(list3);
        Assert.Equal(2, (int)Head(takenList));
        Assert.Equal(1, (int)Head(Tail(takenList)));
        Assert.True(IsNil(Tail(Tail(takenList))).UnChurch());

        for (uint i = 3; i < 10; i++)
        {
            takenList = TakeLast(3u.ToChurch())(list3);
            Assert.Equal(3, (int)Head(takenList));
            Assert.Equal(2, (int)Head(Tail(takenList)));
            Assert.Equal(1, (int)Head(Tail(Tail(takenList))));
            Assert.True(IsNil(Tail(Tail(Tail(takenList)))).UnChurch());
        }
    }

    [Fact]
    public void AllTest()
    {
        static Boolean Test(dynamic x) => Church.ToChurch(x % 2 == 0);

        var list0 = Nil;
        var result = All(Test)(list0);
        Assert.True(result.UnChurch());

        {
            var list1 = Cons(1)(list0);
            result = All(Test)(list1);
            Assert.False(result.UnChurch());

            var list2 = Cons(2)(list1);
            result = All(Test)(list2);
            Assert.False(result.UnChurch());

            var list3 = Cons(3)(list2);
            result = All(Test)(list3);
            Assert.False(result.UnChurch());

        }

        {
            var list1 = Cons(2)(list0);
            result = All(Test)(list1);
            Assert.True(result.UnChurch());

            var list2 = Cons(3)(list1);
            result = All(Test)(list2);
            Assert.False(result.UnChurch());

            var list3 = Cons(4)(list2);
            result = All(Test)(list3);
            Assert.False(result.UnChurch());
        }

        {
            var list1 = Cons(2)(list0);
            result = All(Test)(list1);
            Assert.True(result.UnChurch());

            var list2 = Cons(4)(list1);
            result = All(Test)(list2);
            Assert.True(result.UnChurch());

            var list3 = Cons(5)(list2);
            result = All(Test)(list3);
            Assert.False(result.UnChurch());
        }

        {
            var list1 = Cons(2)(list0);
            result = All(Test)(list1);
            Assert.True(result.UnChurch());

            var list2 = Cons(4)(list1);
            result = All(Test)(list2);
            Assert.True(result.UnChurch());

            var list3 = Cons(6)(list2);
            result = All(Test)(list3);
            Assert.True(result.UnChurch());
        }
    }

    [Fact]
    public void AnyTest()
    {
        static Boolean Test(dynamic x) => Church.ToChurch(x % 2 == 0);

        var list0 = Nil;
        var result = Any(Test)(list0);
        Assert.False(result.UnChurch());

        {
            var list1 = Cons(2)(list0);
            result = Any(Test)(list1);
            Assert.True(result.UnChurch());

            var list2 = Cons(3)(list1);
            result = Any(Test)(list2);
            Assert.True(result.UnChurch());

            var list3 = Cons(5)(list2);
            result = Any(Test)(list3);
            Assert.True(result.UnChurch());
        }

        {
            var list1 = Cons(1)(list0);
            result = Any(Test)(list1);
            Assert.False(result.UnChurch());

            var list2 = Cons(2)(list1);
            result = Any(Test)(list2);
            Assert.True(result.UnChurch());

            var list3 = Cons(3)(list2);
            result = Any(Test)(list3);
            Assert.True(result.UnChurch());
        }

        {
            var list1 = Cons(1)(list0);
            result = Any(Test)(list1);
            Assert.False(result.UnChurch());

            var list2 = Cons(3)(list1);
            result = Any(Test)(list2);
            Assert.False(result.UnChurch());

            var list3 = Cons(4)(list2);
            result = Any(Test)(list3);
            Assert.True(result.UnChurch());
        }

        {
            var list1 = Cons(1)(list0);
            result = Any(Test)(list1);
            Assert.False(result.UnChurch());

            var list2 = Cons(3)(list1);
            result = Any(Test)(list2);
            Assert.False(result.UnChurch());

            var list3 = Cons(5)(list2);
            result = Any(Test)(list3);
            Assert.False(result.UnChurch());
        }
    }

    [Fact]
    public void ElementAtTest()
    {
        var list0 = Nil;
        var list1 = Cons(1)(list0);
        var list2 = Cons(2)(list1);
        var list3 = Cons(3)(list2);

        var result = ElementAt(0u.ToChurch())(list3);
        Assert.Equal(3, (int)result);

        result = ElementAt(1u.ToChurch())(list3);
        Assert.Equal(2, (int)result);

        result = ElementAt(2u.ToChurch())(list3);
        Assert.Equal(1, (int)result);
    }

    [Fact]
    public void IndexOfTest()
    {
        static Boolean Test(dynamic x) => Church.ToChurch(x % 2 == 0);

        var list0 = Nil;
        var result = IndexOf(Test)(list0);
        Assert.True(IsNil(result).UnChurch());

        {
            var list1 = Cons(1)(list0);
            result = IndexOf(Test)(list1);
            Assert.True(IsNil(result).UnChurch());

            var list2 = Cons(3)(list0);
            result = IndexOf(Test)(list2);
            Assert.True(IsNil(result).UnChurch());

            var list3 = Cons(4)(list2);
            result = IndexOf(Test)(list3);
            Assert.False(IsNil(result).UnChurch());
            Assert.Equal(0u, ((Numeral)Head(result)).UnChurch());

            var list4 = Cons(5)(list3);
            result = IndexOf(Test)(list4);
            Assert.False(IsNil(result).UnChurch());
            Assert.Equal(1u, ((Numeral)Head(result)).UnChurch());

            var list5 = Cons(6)(list4);
            result = IndexOf(Test)(list5);
            Assert.False(IsNil(result).UnChurch());
            Assert.Equal(0u, ((Numeral)Head(result)).UnChurch());
        }

        {
            var list1 = Cons(1)(list0);
            result = IndexOf(Test)(list1);
            Assert.True(IsNil(result).UnChurch());

            var list2 = Cons(3)(list1);
            result = IndexOf(Test)(list2);
            Assert.True(IsNil(result).UnChurch());

            var list3 = Cons(5)(list2);
            result = IndexOf(Test)(list3);
            Assert.True(IsNil(result).UnChurch());
        }
    }

    [Fact]
    public void LastIndexOfTest()
    {
        static Boolean Test(dynamic x) => Church.ToChurch(x % 2 == 0);

        var list0 = Nil;
        var result = LastIndexOf(Test)(list0);
        Assert.True(IsNil(result).UnChurch());

        {
            var list1 = Cons(1)(list0);
            result = LastIndexOf(Test)(list1);
            Assert.True(IsNil(result).UnChurch());

            var list2 = Cons(3)(list0);
            result = LastIndexOf(Test)(list2);
            Assert.True(IsNil(result).UnChurch());

            var list3 = Cons(4)(list2);
            result = LastIndexOf(Test)(list3);
            Assert.False(IsNil(result).UnChurch());
            Assert.Equal(0u, ((Numeral)Head(result)).UnChurch());

            var list4 = Cons(5)(list3);
            result = LastIndexOf(Test)(list4);
            Assert.False(IsNil(result).UnChurch());
            Assert.Equal(1u, ((Numeral)Head(result)).UnChurch());

            var list5 = Cons(6)(list4);
            result = LastIndexOf(Test)(list5);
            Assert.False(IsNil(result).UnChurch());
            Assert.Equal(2u, ((Numeral)Head(result)).UnChurch());
        }

        {
            var list1 = Cons(1)(list0);
            result = LastIndexOf(Test)(list1);
            Assert.True(IsNil(result).UnChurch());

            var list2 = Cons(3)(list1);
            result = LastIndexOf(Test)(list2);
            Assert.True(IsNil(result).UnChurch());

            var list3 = Cons(5)(list2);
            result = LastIndexOf(Test)(list3);
            Assert.True(IsNil(result).UnChurch());
        }
    }

    [Fact]
    public void RangeTest()
    {
        var range = Range(x => Succ(x))(One)(Zero);
        Assert.True(IsNil(range).UnChurch());

        range = Range(x => Succ(x))(One)(2u.ToChurch());
        Assert.Equal(1u, ((Numeral)Head(range)).UnChurch());
        Assert.Equal(2u, ((Numeral)Head(Tail(range))).UnChurch());
        Assert.True(IsNil(Tail(Tail(range))).UnChurch());

        range = Range(x => SuccS(x))((-1).ToChurch())(3u.ToChurch());
        Assert.Equal(-1, ((Signed)Head(range)).UnChurch());
        Assert.Equal(0, ((Signed)Head(Tail(range))).UnChurch());
        Assert.Equal(1, ((Signed)Head(Tail(Tail(range)))).UnChurch());
        Assert.True(IsNil(Tail(Tail(Tail(range)))).UnChurch());
    }

    [Fact]
    public void RepeatTest()
    {
        var repeated = Repeat(0)(0U.ToChurch());
        Assert.True(IsNil(repeated).UnChurch());

        repeated = Repeat(2)(1U.ToChurch());
        Assert.Equal(2, (int)Head(repeated));
        Assert.True(IsNil(Tail(repeated)).UnChurch());

        repeated = Repeat(5)(2U.ToChurch());
        Assert.Equal(5, (int)Head(repeated));
        Assert.Equal(5, (int)Head(Tail(repeated)));
        Assert.True(IsNil(Tail(Tail(repeated))).UnChurch());

        repeated = Repeat(6)(3U.ToChurch());
        Assert.Equal(6, (int)Head(repeated));
        Assert.Equal(6, (int)Head(Tail(repeated)));
        Assert.Equal(6, (int)Head(Tail(Tail(repeated))));
        Assert.True(IsNil(Tail(Tail(Tail(repeated)))).UnChurch());
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
