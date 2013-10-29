using System.Collections.Generic;
using System.Linq;
using Given.Common;

namespace MedArchon.Common.Testing
{
    public static class EnumerableExtensions
    {
        public static void CountShouldBe<T>(this IEnumerable<T> enumerable, int expectedCount)
        {
            var count = enumerable.Count();

            if (count != expectedCount)
                throw new SpecificationException(string.Format("Should contain {0} items but contained {1} items", expectedCount, count));
        }
    }
}
