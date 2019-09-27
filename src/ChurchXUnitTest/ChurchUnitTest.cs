using System;
using System.Collections.Generic;
using Xunit;

namespace LambdaCalculus
{
    public class ChurchUnitTest
    {
        [Theory]
        [MemberData(nameof(GetDynamicData1))]
        public void IdTest(dynamic x)
        {
            Assert.Equal(x, ChurchUnit.Id(x));
            Assert.Equal(x, ChurchUnit.IdV(x));

            Assert.Equal(x, ChurchUnit.Id(ChurchUnit.Id(x)));
            Assert.Equal(x, ChurchUnit.Id(ChurchUnit.IdV(x)));
            Assert.Equal(x, ChurchUnit.Id(ChurchUnit.IdV)(x));

            Assert.Equal(x, ChurchUnit.IdV(ChurchUnit.Id(x)));
            Assert.Equal(x, ChurchUnit.IdV(ChurchUnit.IdV(x)));
            Assert.Equal(x, ChurchUnit.IdV(ChurchUnit.IdV)(x));
        }

        //[Fact]
        //public void IdTypedTest()
        //{
        //    var idBool = new Unit<bool>(ChurchUnit.Id);
        //    var idStr = new Unit<string>(ChurchUnit.Id);
        //    var idInt = new Unit<int>(ChurchUnit.Id);

        //    Assert.True(ChurchUnit.Id(true));
        //    Assert.False(ChurchUnit.Id(false));
        //    Assert.True(ChurchUnit.Id(ChurchUnit.Id(true)));
        //    Assert.False(ChurchUnit.Id(ChurchUnit.Id(false)));
        //    Assert.True(ChurchUnit.Id(idBool)(true));
        //    Assert.False(ChurchUnit.Id(idBool)(false));

        //    Assert.Equal("str1", ChurchUnit.Id("str1"));
        //    Assert.Equal("str2", ChurchUnit.Id(ChurchUnit.Id("str2")));
        //    Assert.Equal("str3", ChurchUnit.Id(idStr)("str3"));

        //    Assert.Equal(1, ChurchUnit.Id(1));
        //    Assert.Equal(2, ChurchUnit.Id(ChurchUnit.Id(2)));
        //    Assert.Equal(3, ChurchUnit.Id(idInt)(3));

        //    Assert.Equal(idBool, ChurchUnit.Id(idBool));
        //    Assert.Equal(idBool, ChurchUnit.Id(ChurchUnit.Id(idBool)));

        //    Assert.Equal(idStr, ChurchUnit.Id(idStr));
        //    Assert.Equal(idStr, ChurchUnit.Id(ChurchUnit.Id(idStr)));

        //    Assert.Equal(idInt, ChurchUnit.Id(idInt));
        //    Assert.Equal(idInt, ChurchUnit.Id(ChurchUnit.Id(idInt)));
        //}

        public static IEnumerable<bool> GetBools()
        {
            yield return false;
            yield return true;
        }

        public static IEnumerable<object[]> GetDynamicData1()
        {
            foreach (var a in GetBools())
            {
                yield return new object[] { a };
                yield return new object[] { Convert.ToString(a) };
                yield return new object[] { Convert.ToInt32(a) };
            }
            yield return new object[] { NotImplementedV };
        }

        internal static void NotImplemented() => throw new NotImplementedException();

        public static readonly Action NotImplementedV = NotImplemented;
    }
}
