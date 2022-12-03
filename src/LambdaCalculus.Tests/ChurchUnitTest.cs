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
            Assert.Equal(x, ChurchUnit.Id (x));

            Assert.Equal(x, ChurchUnit.Id(ChurchUnit.Id(x)) );
            Assert.Equal(x, ChurchUnit.Id ((object)ChurchUnit.Id) (x) );
        }

        #region GetData

        public static IEnumerable<object[]> GetDynamicData1()
        {
            foreach (var a in ChurchBoolTest.GetBools())
            {
                yield return new object[] { a };
                yield return new object[] { Convert.ToString(a) };
                yield return new object[] { Convert.ToInt32(a) };
            }
            yield return new object[] { NotImplementedV };
        }

        internal static void NotImplemented() => throw new NotImplementedException();

        internal static readonly Action NotImplementedV = NotImplemented;

        #endregion
    }
}
