using static LambdaCalculus.Church;

namespace LambdaCalculus.Tests;

public class ChurchBooleansTests
{
    #region Constants

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void TrueTest(object @true, object @false)
    {
        Assert.Equal(@true, TrueL<object, object>(@true)(@false));
        Assert.Equal(@true, TrueL<dynamic, dynamic>(@true)(@false));
        Assert.Equal(@true, True(@true)(@false));
    }

    [Theory]
    [MemberData(nameof(GetDynamicsData2))]
    public void FalseTest(object @true, object @false)
    {
        Assert.Equal(@false, FalseL<object, object>(@true)(@false));
        Assert.Equal(@false, FalseL<dynamic, dynamic>(@true)(@false));
        Assert.Equal(@false, False(@true)(@false));
    }

    #endregion

    #region Conversations

    [Theory]
    [MemberData(nameof(GetBoolsData))]
    public void AsChurchTest(bool b)
    {
        foreach(var @true in GetDynamics())
            foreach (var @false in GetDynamics())
            {
                var expected = b ? @true : @false;
                Assert.Equal(expected, b.AsChurch()(@true)(@false));
            }
    }

    [Theory]
    [MemberData(nameof(GetBoolsData))]
    public void UnChurchTest(bool a)
    {
        var ca = a.AsChurch();
        Assert.Equal(a, ca.UnChurch());
    }

    #endregion

    #region Operations

    [Theory]
    [MemberData(nameof(GetBoolsData))]
    public void NotTest(bool a)
    {
        var ca = a.AsChurch();
        Assert.Equal(!a, Not(ca).UnChurch());
        Assert.Equal(a, Not(Not(ca)).UnChurch());

        Assert.Equal(!a, Not_ab(ca).UnChurch());
        Assert.Equal(a, Not_ab(Not_ab(ca)).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void OrTest(bool a, bool b)
    {
        var expected = a || b;
        var ca = a.AsChurch();
        var cb = b.AsChurch();

        Assert.Equal(expected, Or(ca)(cb).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void AndTest(bool a, bool b)
    {
        var ca = a.AsChurch();
        var cb = b.AsChurch();
        Assert.Equal(a && b, And(ca)(cb).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void XorTest(bool a, bool b)
    {
        var ca = a.AsChurch();
        var cb = b.AsChurch();
        Assert.Equal(a ^ b, Xor(ca)(cb).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void NorTest(bool a, bool b)
    {
        var ca = a.AsChurch();
        var cb = b.AsChurch();
        Assert.Equal(!(a || b), Nor(ca)(cb).UnChurch());
        Assert.Equal(!(a || b), Nor_not(ca)(cb).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void NandTest(bool a, bool b)
    {
        var ca = a.AsChurch();
        var cb = b.AsChurch();
        Assert.Equal(!(a && b), Nand(ca)(cb).UnChurch());
    }

    [Theory]
    [MemberData(nameof(GetBoolsData2))]
    public void XnorTest(bool a, bool b)
    {
        var ca = a.AsChurch();
        var cb = b.AsChurch();
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
                var @bool = b.AsChurch();

                Assert.Equal(expected, If(@bool)(@then)(@else));
                Assert.Equal(expected, @bool(@then)(@else)); // predicate should work them self also
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
        var cItems = new List<Church.Boolean>(len);

        while (bItems.Count < len)
        {
            var bv = random.NextDouble() >= 0.5;
            var cv = bv.AsChurch();
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

    private static Church.Boolean EvaluateOp(List<Church.Boolean> items, BoolOp op, int opIndex) => op switch
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

    public static IEnumerable<object[]> GetDynamicsData2() =>
        from a in GetDynamics()
        from b in GetDynamics()
        select new object[] { a, b };

    #endregion
}
