using System;
using System.Collections.Generic;

namespace LambdaCalculus
{
    public class ChurchBoolTest
    {
        #region True

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void TrueTest(dynamic @true, dynamic @false)
        {
            Assert.Equal(@true, ChurchBool.True(@true)(@false));
            Assert.Equal(@true, ChurchBool.TrueV(@true)(@false));

            Assert.Equal(@true.ToString(), ChurchBool.True(@true)(@false).ToString());
            Assert.Equal(@true.ToString(), ChurchBool.TrueV(@true)(@false).ToString());

            Action exception = () => throw new NotImplementedException();

            Assert.Equal(@true, ChurchBool.True(@true)(exception));
            Assert.Equal(@true, ChurchBool.TrueV(@true)(exception));

            Assert.Equal(@true.ToString(), ChurchBool.True(@true)(exception).ToString());
            Assert.Equal(@true.ToString(), ChurchBool.TrueV(@true)(exception).ToString());
        }

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void TrueLazyTest(dynamic @true, dynamic @false)
        {
            var concat = (string acc, dynamic b) => { acc += b.ToString(); return acc; };
            {
                var acc = "";
                var r = ChurchBool.True(concat(acc, @true))(concat(acc, @false));
                Assert.Equal(@true.ToString(), r);
            }
            {
                var acc = "";
                var r = ChurchBool.TrueV(concat(acc, @true))(concat(acc, @false));
                Assert.Equal(@true.ToString(), r);
            }
        }

        #endregion

        #region False

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void FalseTest(dynamic @true, dynamic @false)
        {
            Assert.Equal(@false, ChurchBool.False(@true)(@false));
            Assert.Equal(@false, ChurchBool.FalseV(@true)(@false));

            Assert.Equal(@false.ToString(), ChurchBool.False(@true)(@false).ToString());
            Assert.Equal(@false.ToString(), ChurchBool.FalseV(@true)(@false).ToString());

            Action exception = () => throw new NotImplementedException();

            Assert.Equal(@false, ChurchBool.False(exception)(@false));
            Assert.Equal(@false, ChurchBool.FalseV(exception)(@false));

            Assert.Equal(@false.ToString(), ChurchBool.False(exception)(@false).ToString());
            Assert.Equal(@false.ToString(), ChurchBool.FalseV(exception)(@false).ToString());
        }

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void FalseLazyTest(dynamic @true, dynamic @false)
        {
            var concat = (string acc, dynamic b) => { acc += b.ToString(); return acc; };
            {
                var acc = "";
                var r = ChurchBool.False(concat(acc, @true))(concat(acc, @false));
                Assert.Equal(@false.ToString(), r);
            }
            {
                var acc = "";
                var r = ChurchBool.FalseV(concat(acc, @true))(concat(acc, @false));
                Assert.Equal(@false.ToString(), r);
            }
        }

        #endregion

        #region ChurchTest

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void ChurchTest(dynamic @true, dynamic @false)
        {
            foreach (var value in GetBools())
            {

                Assert.Equal(value ? @true : @false, ChurchBool.Church(value)(@true)(@false));
                Assert.Equal(value ? @true : @false, value.Church()(@true)(@false));

                Assert.Equal((value ? @true : @false).ToString(), ChurchBool.Church(value)(@true)(@false).ToString());
                Assert.Equal((value ? @true : @false).ToString(), value.Church()(@true)(@false).ToString());
            }
        }

        #endregion

        #region UnchurchTest

        [Theory]
        [MemberData(nameof(GetBoolsData))]
        public void UnchurchTest(bool a)
        {
            var ca = a.Church();
            Assert.Equal(a, ChurchBool.Unchurch(ca));
            Assert.Equal(a, ca.Unchurch());

            // predicate should work them self also
            Assert.Equal(a, ca(true)(false));

            // If(predicate) should work also
            Assert.Equal(a, ChurchBool.If(ca)(true)(false));
            Assert.Equal(a, ca.If()(true)(false));
            Assert.Equal(a, ChurchBool.If(ca, true, false));
            Assert.Equal(a, ca.If(true, false));
        }

        #endregion

        [Theory]
        [MemberData(nameof(GetDynamicData2))]
        public void IfTest(dynamic @then, dynamic @else)
        {
            foreach (var value in GetBools())
            {
                var cv = value.Church();
                Assert.Equal(value ? @then : @else, ChurchBool.If(cv)(@then)(@else));
                Assert.Equal(value ? @then : @else, cv.If()(@then)(@else));
                Assert.Equal(value ? @then : @else, ChurchBool.If(cv, @then, @else));
                Assert.Equal(value ? @then : @else, cv.If((object)@then, (object)@else));
            }
        }

        [Theory]
        [MemberData(nameof(GetBoolsData))]
        public void NotTest(bool a)
        {
            var ca = a.Church();
            Assert.Equal(!a, ChurchBool.Not(ca).Unchurch());
            Assert.Equal(!!a, ChurchBool.Not(ChurchBool.Not(ca)).Unchurch());
            Assert.Equal(!a, ca.Not().Unchurch());
            Assert.Equal(!!a, ca.Not().Not().Unchurch());
        }

        [Theory]
        [MemberData(nameof(GetBoolsData2))]
        public void OrTest(bool a, bool b)
        {
            var ca = a.Church();
            var cb = b.Church();
            Assert.Equal(a || b, ChurchBool.Or(ca)(cb).Unchurch());
            Assert.Equal(a || b, ca.Or()(cb).Unchurch());
            Assert.Equal(a || b, ChurchBool.Or(ca, cb).Unchurch());
            Assert.Equal(a || b, ca.Or(cb).Unchurch());
        }

        [Theory]
        [MemberData(nameof(GetBoolsData2))]
        public void AndTest(bool a, bool b)
        {
            var ca = a.Church();
            var cb = b.Church();
            Assert.Equal(a && b, ChurchBool.And(ca)(cb).Unchurch());
            Assert.Equal(a && b, ca.And()(cb).Unchurch());
            Assert.Equal(a && b, ChurchBool.And(ca, cb).Unchurch());
            Assert.Equal(a && b, ca.And(cb).Unchurch());
        }

        [Theory]
        [MemberData(nameof(GetBoolsData2))]
        public void XorTest(bool a, bool b)
        {
            var ca = a.Church();
            var cb = b.Church();
            Assert.Equal(a ^ b, ChurchBool.Xor(ca)(cb).Unchurch());
            Assert.Equal(a ^ b, ca.Xor()(cb).Unchurch());
            Assert.Equal(a ^ b, ChurchBool.Xor(ca, cb).Unchurch());
            Assert.Equal(a ^ b, ca.Xor(cb).Unchurch());
        }

        [Theory]
        [MemberData(nameof(GetBoolsData2))]
        public void NorTest(bool a, bool b)
        {
            var ca = a.Church();
            var cb = b.Church();
            Assert.Equal(!(a || b), ChurchBool.Nor(ca)(cb).Unchurch());
            Assert.Equal(!(a || b), ca.Nor()(cb).Unchurch());
            Assert.Equal(!(a || b), ChurchBool.Nor(ca, cb).Unchurch());
            Assert.Equal(!(a || b), ca.Nor(cb).Unchurch());
        }

        [Theory]
        [MemberData(nameof(GetBoolsData2))]
        public void NandTest(bool a, bool b)
        {
            var ca = a.Church();
            var cb = b.Church();
            Assert.Equal(!(a && b), ChurchBool.Nand(ca)(cb).Unchurch());
            Assert.Equal(!(a && b), ca.Nand()(cb).Unchurch());
            Assert.Equal(!(a && b), ChurchBool.Nand(ca, cb).Unchurch());
            Assert.Equal(!(a && b), ca.Nand(cb).Unchurch());
        }

        [Theory]
        [MemberData(nameof(GetBoolsData2))]
        public void EqTest(bool a, bool b)
        {
            var ca = a.Church();
            var cb = b.Church();
            Assert.Equal(a == b, ChurchBool.Eq(ca)(cb).Unchurch());
            Assert.Equal(a == b, ca.Eq()(cb).Unchurch());
            Assert.Equal(a == b, ChurchBool.Eq(ca, cb).Unchurch());
            Assert.Equal(a == b, ca.Eq(cb).Unchurch());
        }

        #region FormulaTest

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
                var bv = random.NextDouble() >= 0.5;
                var cv = bv.Church();
                Assert.Equal(bv, cv.Unchurch());

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
                Assert.Equal(bv, cv.Unchurch());

                bItems.RemoveRange(opIndex + 1, opLen - 1);
                cItems.RemoveRange(opIndex + 1, opLen - 1);

                bItems[opIndex] = bv;
                cItems[opIndex] = cv;
            }

            Assert.Equal(bItems[0], cItems[0].Unchurch());
        }

        private static bool EvaluateOp(List<bool> items, BoolOp op, int opIndex)
        {
            return op switch
            {
                BoolOp.Not => !items[opIndex],
                BoolOp.Or => items[opIndex] || items[opIndex + 1],
                BoolOp.And => items[opIndex] && items[opIndex + 1],
                BoolOp.Xor => items[opIndex] ^ items[opIndex + 1],
                BoolOp.Nor => !(items[opIndex] || items[opIndex + 1]),
                BoolOp.Nand => !(items[opIndex] && items[opIndex + 1]),
                BoolOp.Eq => items[opIndex] == items[opIndex + 1],
                BoolOp.If => items[opIndex] ? items[opIndex + 1] : items[opIndex + 2],
                _ => throw new NotImplementedException(),
            };
        }

        private static Bool EvaluateOp(List<Bool> items, BoolOp op, int opIndex)
        {
            return op switch
            {
                BoolOp.Not => items[opIndex].Not(),
                BoolOp.Or => items[opIndex].Or(items[opIndex + 1]),
                BoolOp.And => items[opIndex].And(items[opIndex + 1]),
                BoolOp.Xor => items[opIndex].Xor(items[opIndex + 1]),
                BoolOp.Nor => items[opIndex].Nor(items[opIndex + 1]),
                BoolOp.Nand => items[opIndex].Nand(items[opIndex + 1]),
                BoolOp.Eq => items[opIndex].Eq(items[opIndex + 1]),
                BoolOp.If => items[opIndex].If(items[opIndex + 1], items[opIndex + 2]),
                _ => throw new NotImplementedException(),
            };
        }

        private static int GetBoolOpLen(BoolOp op)
        {
            return op switch
            {
                BoolOp.Not => 1,
                BoolOp.Or or BoolOp.And or BoolOp.Xor or BoolOp.Nor or BoolOp.Nand or BoolOp.Eq => 2,
                BoolOp.If => 3,
                _ => throw new NotImplementedException(),
            };
        }

        private enum BoolOp { Not, Or, And, Xor, Nor, Nand, Eq, If }

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

        public static IEnumerable<object[]> GetBoolsData()
        {
            foreach (var b in GetBools())
                yield return new object[] { b };
        }

        public static IEnumerable<object[]> GetBoolsData2()
        {
            foreach (var a in GetBools())
                foreach (var b in GetBools())
                    yield return new object[] { a, b };
        }

        public static IEnumerable<object[]> GetDynamicData2()
        {
            foreach (var a in ChurchUnitTest.GetDynamicData1())
                foreach (var b in ChurchUnitTest.GetDynamicData1())
                    yield return new object[] { a[0], b[0] };
        }

        #endregion

    }
}