using System;
using System.Collections.Generic;
using Xunit;

namespace LambdaCalculus
{
    public class ChurchBoolTest
    {
        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void FalseTest(dynamic @true, dynamic @false)
        {
            Assert.Equal(@false, ChurchBool.False(@true)(@false));
            Assert.Equal(@false, ChurchBool.FalseV(@true)(@false));
        }

        //[Fact]
        //public void FalseTypedTest()
        //{
        //    var trueStr = Convert.ToString(true);
        //    var falseStr = Convert.ToString(false);

        //    var trueInt = Convert.ToInt32(true);
        //    var falseInt = Convert.ToInt32(false);

        //    Assert.False(ChurchEncoding.False(true)(false));
        //    Assert.True(ChurchEncoding.False(false)(true));

        //    Assert.Equal(falseStr, ChurchEncoding.FalseT(trueStr)(falseStr));
        //    Assert.Equal(trueStr, ChurchEncoding.FalseT(falseStr)(trueStr));

        //    Assert.Equal(falseInt, ChurchEncoding.FalseT(trueInt)(falseInt));
        //    Assert.Equal(trueInt, ChurchEncoding.FalseT(falseInt)(trueInt));

        //    Assert.Equal(falseStr, ChurchEncoding.FalseT<int, string>(trueInt)(falseStr));
        //    Assert.Equal(trueStr, ChurchEncoding.FalseT<int, string>(falseInt)(trueStr));

        //    Assert.Equal(falseInt, ChurchEncoding.FalseT<string, int>(trueStr)(falseInt));
        //    Assert.Equal(trueInt, ChurchEncoding.FalseT<string, int>(falseStr)(trueInt));
        //}

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void TrueTest(dynamic @true, dynamic @false)
        {
            Assert.Equal(@true, ChurchBool.True(@true)(@false));
            Assert.Equal(@true, ChurchBool.TrueV(@true)(@false));
        }

        //[Fact]
        //public void TrueTypedTest()
        //{
        //    var trueStr = Convert.ToString(true);
        //    var falseStr = Convert.ToString(false);

        //    var trueInt = Convert.ToInt32(true);
        //    var falseInt = Convert.ToInt32(false);

        //    Assert.True(ChurchEncoding.True(true)(false));
        //    Assert.False(ChurchEncoding.True(false)(true));

        //    Assert.Equal(trueStr, ChurchEncoding.TrueT(trueStr)(falseStr));
        //    Assert.Equal(falseStr, ChurchEncoding.TrueT(falseStr)(trueStr));

        //    Assert.Equal(trueInt, ChurchEncoding.TrueT(trueInt)(falseInt));
        //    Assert.Equal(falseInt, ChurchEncoding.TrueT(falseInt)(trueInt));

        //    Assert.Equal(trueInt, ChurchEncoding.TrueT<int, string>(trueInt)(falseStr));
        //    Assert.Equal(falseInt, ChurchEncoding.TrueT<int, string>(falseInt)(trueStr));

        //    Assert.Equal(trueStr, ChurchEncoding.TrueT<string, int>(trueStr)(falseInt));
        //    Assert.Equal(falseStr, ChurchEncoding.TrueT<string, int>(falseStr)(trueInt));
        //}

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void ChurchTest(dynamic @true, dynamic @false)
        {
            foreach (var value in GetBools())
                Assert.Equal(value ? @true : @false, ChurchBool.Church(value)(@true)(@false));
        }

        [Fact]
        public void UnchurchTest()
        {
            foreach (var value in GetBools())
                Assert.Equal(value, ChurchBool.Church(value).Unchurch());
        }

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void IfTest(dynamic @then, dynamic @else)
        {
            foreach (var value in GetBools())
            {
                Assert.Equal(value ? @then : @else, ChurchBool.If(ChurchBool.Church(value))(@then)(@else));

                Assert.Equal(value ? @then : @else, ChurchBool.Church(value).If()(@then)(@else));

                Assert.Equal(value ? @then : @else, ChurchBool.Church(value).If((object)@then, (object)@else));

                Assert.Equal(value ? @then : @else, ChurchBool.If(ChurchBool.Church(value), @then, @else));
            }
        }

        [Fact]
        public void NotTest()
        {
            foreach (var value in GetBools())
            {
                Assert.Equal(!value, ChurchBool.Not(value.Church()).Unchurch());
                Assert.Equal(!!value, ChurchBool.Not(ChurchBool.Not(value.Church())).Unchurch());

                Assert.Equal(!value, value.Church().Not().Unchurch());
                Assert.Equal(!!value, value.Church().Not().Not().Unchurch());
            }
        }

        [Fact]
        public void OrTest()
        {
            foreach (var a in GetBools())
                foreach (var b in GetBools())
                {
                    Assert.Equal(a || b, ChurchBool.Or(ChurchBool.Church(a))(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a || b, ChurchBool.Church(a).Or()(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a || b, ChurchBool.Church(a).Or(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a || b, ChurchBool.Or(ChurchBool.Church(a), ChurchBool.Church(b)).Unchurch());
                }
        }

        [Fact]
        public void AndTest()
        {
            foreach (var a in GetBools())
                foreach (var b in GetBools())
                {
                    Assert.Equal(a && b, ChurchBool.And(ChurchBool.Church(a))(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a && b, ChurchBool.Church(a).And()(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a && b, ChurchBool.Church(a).And(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a && b, ChurchBool.And(ChurchBool.Church(a), ChurchBool.Church(b)).Unchurch());
                }
        }

        [Fact]
        public void XorTest()
        {
            foreach (var a in GetBools())
                foreach (var b in GetBools())
                {
                    Assert.Equal(a ^ b, ChurchBool.Xor(ChurchBool.Church(a))(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a ^ b, ChurchBool.Church(a).Xor()(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a ^ b, ChurchBool.Church(a).Xor(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a ^ b, ChurchBool.Xor(ChurchBool.Church(a), ChurchBool.Church(b)).Unchurch());
                }
        }

        [Fact]
        public void NorTest()
        {
            foreach (var a in GetBools())
                foreach (var b in GetBools())
                {
                    Assert.Equal(!(a || b), ChurchBool.Nor(ChurchBool.Church(a))(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(!(a || b), ChurchBool.Church(a).Nor()(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(!(a || b), ChurchBool.Church(a).Nor(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(!(a || b), ChurchBool.Nor(ChurchBool.Church(a), ChurchBool.Church(b)).Unchurch());
                }
        }

        [Fact]
        public void NandTest()
        {
            foreach (var a in GetBools())
                foreach (var b in GetBools())
                {
                    Assert.Equal(!(a && b), ChurchBool.Nand(ChurchBool.Church(a))(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(!(a && b), ChurchBool.Church(a).Nand()(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(!(a && b), ChurchBool.Church(a).Nand(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(!(a && b), ChurchBool.Nand(ChurchBool.Church(a), ChurchBool.Church(b)).Unchurch());
                }
        }

        [Fact]
        public void EqTest()
        {
            foreach (var a in GetBools())
                foreach (var b in GetBools())
                {
                    Assert.Equal(a == b, ChurchBool.Eq(ChurchBool.Church(a))(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a == b, ChurchBool.Church(a).Eq()(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a == b, ChurchBool.Church(a).Eq(ChurchBool.Church(b)).Unchurch());
                    Assert.Equal(a == b, ChurchBool.Eq(ChurchBool.Church(a), ChurchBool.Church(b)).Unchurch());
                }
        }

        [Theory]
        [InlineData(438, 5)]
        [InlineData(704, 10)]
        [InlineData(163, 20)]
        public void FormulaTest(int seed, int len)
        {
            var random = new Random(seed);

            var bItems = new List<bool>(len);
            var cItems = new List<Bool>(len);

            while (bItems.Count < len)
            {
                var bv = random.NextDouble() < 0.5 ? false : true;
                var cv = ChurchBool.Church(bv);
                Assert.Equal(bv, cv.Unchurch());

                bItems.Add(bv);
                cItems.Add(cv);
            }

            while (bItems.Count > 1)
            {
                int opIndex = random.Next(bItems.Count);
                var op = GetRandomEnumValue<BoolOp>(random);
                var opLen = GetBoolOpLen(op);

                // Not enought values
                if (opIndex + opLen > bItems.Count)
                    continue;

                var bv = EvaluateOp(bItems, op, opIndex);
                var cv = EvaluateOp(cItems, op, opIndex);
                Assert.Equal(bv, cv.Unchurch());

                bItems.RemoveRange(opIndex + 1, opLen - 1);
                cItems.RemoveRange(opIndex + 1, opLen - 1);

                bItems[opIndex] = bv;
                cItems[opIndex] = cv;
            }

            Assert.Equal(bItems[0], cItems[0].Unchurch());
        }

        public static bool EvaluateOp(List<bool> items, BoolOp op, int opIndex)
        {
            switch (op)
            {
                case BoolOp.Not: return !items[opIndex];
                case BoolOp.Or: return items[opIndex] || items[opIndex + 1];
                case BoolOp.And: return items[opIndex] && items[opIndex + 1];
                case BoolOp.Xor: return items[opIndex] ^ items[opIndex + 1];
                case BoolOp.Nor: return !(items[opIndex] || items[opIndex + 1]);
                case BoolOp.Nand: return !(items[opIndex] && items[opIndex + 1]);
                case BoolOp.Eq: return items[opIndex] == items[opIndex + 1];
                case BoolOp.If: return items[opIndex] ? items[opIndex + 1] : items[opIndex + 2];
            }
            throw new NotImplementedException();
        }

        public static Bool EvaluateOp(List<Bool> items, BoolOp op, int opIndex)
        {
            switch (op)
            {
                case BoolOp.Not: return ChurchBool.Not(items[opIndex]);
                case BoolOp.Or: return ChurchBool.Or(items[opIndex])(items[opIndex + 1]);
                case BoolOp.And: return ChurchBool.And(items[opIndex])(items[opIndex + 1]);
                case BoolOp.Xor: return ChurchBool.Xor(items[opIndex])(items[opIndex + 1]);
                case BoolOp.Nor: return ChurchBool.Nor(items[opIndex])(items[opIndex + 1]);
                case BoolOp.Nand: return ChurchBool.Nand(items[opIndex])(items[opIndex + 1]);
                case BoolOp.Eq: return ChurchBool.Eq(items[opIndex])(items[opIndex + 1]);
                case BoolOp.If: return ChurchBool.If(items[opIndex])(items[opIndex + 1])(items[opIndex + 2]);
            }
            throw new InvalidOperationException();
        }

        public int GetBoolOpLen(BoolOp op)
        {
            switch (op)
            {
                case BoolOp.Not: return 1;
                case BoolOp.Or:
                case BoolOp.And:
                case BoolOp.Xor:
                case BoolOp.Nor:
                case BoolOp.Nand:
                case BoolOp.Eq: return 2;
                case BoolOp.If: return 3;
            }
            throw new NotImplementedException();
        }

        public enum BoolOp { Not, Or, And, Xor, Nor, Nand, Eq, If }

        public static T GetRandomEnumValue<T>(Random random)
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(random.Next(values.Length));
        }

        public static IEnumerable<bool> GetBools()
        {
            yield return false;
            yield return true;
        }

        public static IEnumerable<object[]> GetDynamicData2()
        {
            foreach (var a in ChurchUnitTest.GetDynamicData1())
                foreach (var b in ChurchUnitTest.GetDynamicData1())
                    yield return new object[] { a[0], b[0] };
        }
    }
}
