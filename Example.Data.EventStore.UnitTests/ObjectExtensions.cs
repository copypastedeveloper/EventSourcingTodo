using System.Collections.Generic;
using System.Linq;
using Given.Common;

namespace Example.Data.EventStore.UnitTests
{
    public static class EnumerableExtensions
    {
        public static void ShouldBeInTheSameOrderAs<T>(this IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable)
        {
            if (!firstEnumerable.SequenceEqual(secondEnumerable))
            {
                throw new SpecificationException("Expected enumerated items to be in the same order, but they were not.");
            }
        }
    }
}