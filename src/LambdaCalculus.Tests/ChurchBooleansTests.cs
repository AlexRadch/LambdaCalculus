namespace LambdaCalculus.Tests;

using static LambdaCalculus.Church;

public class ChurchBooleansTests
{
    #region Constants

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void TrueTest(object @true, object @false)
    {
        Assert.Equal(@true, TrueF<object, object>(@true)(@false));
        Assert.Equal(@true, TrueF<dynamic, dynamic>(@true)(@false));
        Assert.Equal(@true, True(@true)(@false));
        Assert.Equal(@true, LazyTrue()(@true)(@false));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void FalseTest(object @true, object @false)
    {
        Assert.Equal(@false, FalseF<object, object>(@true)(@false));
        Assert.Equal(@false, FalseF<dynamic, dynamic>(@true)(@false));
        Assert.Equal(@false, False(@true)(@false));
        Assert.Equal(@false, LazyFalse()(@true)(@false));
    }

    #endregion

    #region Extensions

    [Theory]
    [MemberData(nameof(GetDynamicsData1))]
    public void AsLazyTest(object value)
    {
        var func = AsLazy(() => value);
        Assert.Same(func, AsLazy(func));
        Assert.Same(value, func());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData))]
    public void ToChurchTest(bool b)
    {
        foreach(var @true in GetDynamics())
            foreach (var @false in GetDynamics())
            {
                var expected = b ? @true : @false;
                Assert.Equal(expected, b.ToChurch()(@true)(@false));
            }
    }

    [Theory]
    [MemberData(nameof(GetBoolsData))]
    public void UnChurchTest(bool a)
    {
        var ca = a.ToChurch();
        Assert.Equal(a, ca.UnChurch());

        var la = () => ca;
        Assert.Equal(a, la.UnChurch());

        la = AsLazy(() => ca);
        Assert.Equal(a, la.UnChurch());
    }

    #endregion

    #region Operations

    [Theory]
    [MemberData(nameof(GetBoolsData))]
    public void NotTest(bool a)
    {
        var ca = a.ToChurch();
        Assert.Equal(!a, Not(ca).UnChurch());
        Assert.Equal(a, Not(Not(ca)).UnChurch());

        Assert.Equal(!a, Not_ab(ca).UnChurch());
        Assert.Equal(a, Not_ab(Not_ab(ca)).UnChurch());

        Boolean la() => ca;
        Assert.Equal(!a, Not(la)().UnChurch());
        Assert.Equal(a, Not(Not(la))().UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void OrTest(bool a, bool b)
    {
        var ca = a.ToChurch();
        var cb = b.ToChurch();
        var expected = a || b;
        Assert.Equal(expected, Or(ca)(cb).UnChurch());

        //Boolean la() => ca;
        var lb = () => cb;
        Assert.Equal(expected, LazyOr(ca)(lb).UnChurch());
        //Assert.Equal(expected, Or(la)(lb).UnChurch());

        lb = () => throw new NotImplementedException();
        if (a)
        {
            Assert.Equal(expected, LazyOr(ca)(lb).UnChurch());
            //Assert.Equal(expected, Or(la)(lb).UnChurch());
        }
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void AndTest(bool a, bool b)
    {
        var ca = a.ToChurch();
        var cb = b.ToChurch();
        var expected = a && b;
        Assert.Equal(expected, And(ca)(cb).UnChurch());

        //Boolean la() => ca;
        var lb = () => cb;
        Assert.Equal(expected, LazyAnd(ca)(lb).UnChurch());
        //Assert.Equal(expected, And(la)(lb).UnChurch());

        lb = () => throw new NotImplementedException();
        if (!a)
        {
            Assert.Equal(expected, LazyAnd(ca)(lb).UnChurch());
            //Assert.Equal(expected, And(la)(lb).UnChurch());
        }
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void XorTest(bool a, bool b)
    {
        var ca = a.ToChurch();
        var cb = b.ToChurch();
        Assert.Equal(a ^ b, Xor(ca)(cb).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void NorTest(bool a, bool b)
    {
        var ca = a.ToChurch();
        var cb = b.ToChurch();
        var expected = !(a || b);
        Assert.Equal(expected, Nor(ca)(cb).UnChurch());
        Assert.Equal(expected, Nor_not(ca)(cb).UnChurch());

        //Boolean la() => ca;
        var lb = () => cb;
        Assert.Equal(expected, LazyNor(ca)(lb).UnChurch());
        //Assert.Equal(expected, Nor(la)(lb).UnChurch());

        lb = () => throw new NotImplementedException();
        if (a)
        {
            Assert.Equal(expected, LazyNor(ca)(lb).UnChurch());
            //Assert.Equal(expected, Nor(la)(lb).UnChurch());
        }
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void NandTest(bool a, bool b)
    {
        var ca = a.ToChurch();
        var cb = b.ToChurch();
        Assert.Equal(!(a && b), Nand(ca)(cb).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void XnorTest(bool a, bool b)
    {
        var ca = a.ToChurch();
        var cb = b.ToChurch();
        Assert.Equal(a == b, Xnor(ca)(cb).UnChurch());
    }

    #endregion

    #region Operators

    [Theory]
    [MemberData(nameof(GetBoolsData))]
    public void IfTest(bool b)
    {
        foreach (var @then in GetDynamics())
            foreach (var @else in GetDynamics())
            {
                var expected = b ? @then : @else;
                var @bool = b.ToChurch();

                Assert.Equal(expected, If(@bool)(@then)(@else));
                Assert.Equal(expected, @bool(@then)(@else)); // predicate should work them self also

                Assert.Equal(expected, LazyIf(@bool)(() => @then)(() => @else));
                //Assert.Equal(expected, If(() => @bool)(() => @then)(() => @else));
            }
    }

    #endregion

    #region Formula Tests

    [Theory]
    [InlineData(438, 5)]
    [InlineData(704, 10)]
    [InlineData(163, 20)]
    public void FormulaTest(int seed, int len)
    {
        var random = new Random(seed);

        var bItems = new List<bool>(len);
        var cItems = new List<Boolean>(len);

        while (bItems.Count < len)
        {
            var bv = random.NextDouble() >= 0.5;
            var cv = bv.ToChurch();
            Assert.Equal(bv, cv.UnChurch());

            bItems.Add(bv);
            cItems.Add(cv);
        }

        while (bItems.Count > 1)
        {
            var op = GetRandomEnumValue<BoolOp>(random);
            var opLen = GetBoolOpLen(op);
            if (bItems.Count < opLen) // Not enought values
                continue;

            int opIndex = random.Next(bItems.Count - opLen + 1);

            var bv = EvaluateOp(bItems, op, opIndex);
            var cv = EvaluateOp(cItems, op, opIndex);
            Assert.Equal(bv, cv.UnChurch());

            bItems.RemoveRange(opIndex + 1, opLen - 1);
            cItems.RemoveRange(opIndex + 1, opLen - 1);

            bItems[opIndex] = bv;
            cItems[opIndex] = cv;
        }

        Assert.Equal(bItems[0], cItems[0].UnChurch());
    }

    private static bool EvaluateOp(List<bool> items, BoolOp op, int opIndex) => op switch
    {
        BoolOp.Not => !items[opIndex],
        BoolOp.Or => items[opIndex] || items[opIndex + 1],
        BoolOp.And => items[opIndex] && items[opIndex + 1],
        BoolOp.Xor => items[opIndex] ^ items[opIndex + 1],
        BoolOp.Nor => !(items[opIndex] || items[opIndex + 1]),
        BoolOp.Nand => !(items[opIndex] && items[opIndex + 1]),
        BoolOp.Xnor => items[opIndex] == items[opIndex + 1],
        BoolOp.If => items[opIndex] ? items[opIndex + 1] : items[opIndex + 2],
        _ => throw new NotImplementedException(),
    };

    private static Boolean EvaluateOp(List<Boolean> items, BoolOp op, int opIndex) => op switch
    {
        BoolOp.Not => Not(items[opIndex]),
        BoolOp.Or => Or(items[opIndex])(items[opIndex + 1]),
        BoolOp.And => And(items[opIndex])(items[opIndex + 1]),
        BoolOp.Xor => Xor(items[opIndex])(items[opIndex + 1]),
        BoolOp.Nor => Nor(items[opIndex])(items[opIndex + 1]),
        BoolOp.Nand => Nand(items[opIndex])(items[opIndex + 1]),
        BoolOp.Xnor => Xnor(items[opIndex])(items[opIndex + 1]),
        BoolOp.If => If(items[opIndex])(items[opIndex + 1])(items[opIndex + 2]),
        _ => throw new NotImplementedException(),
    };

    private static int GetBoolOpLen(BoolOp op) => op switch
    {
        BoolOp.Not => 1,
        BoolOp.Or or BoolOp.And or BoolOp.Xor or BoolOp.Nor or BoolOp.Nand or BoolOp.Xnor => 2,
        BoolOp.If => 3,
        _ => throw new NotImplementedException(),
    };

    private enum BoolOp { Not, Or, And, Xor, Nor, Nand, Xnor, If }

    private static T? GetRandomEnumValue<T>(Random random)
    {
        var values = Enum.GetValues(typeof(T));
        return (T?)values.GetValue(random.Next(values.Length));
    }

    #endregion

    #region GetData

    public static IEnumerable<bool> GetBools()
    {
        yield return false;
        yield return true;
    }

    public static IEnumerable<object[]> GetBoolsData() =>
        GetBools().Select(b => new object[] { b });

    public static IEnumerable<object[]> GetBoolsData2() =>
        from a in GetBools()
        from b in GetBools()
        select new object[] { a, b };

    public static IEnumerable<dynamic> GetDynamics()
    {
        foreach (var a in GetBools())
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

    #endregion
}
