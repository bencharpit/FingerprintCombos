using System;
using System.Collections.Generic;

namespace FingerprintCombos.Helpers
{
    internal static class EnumerableExtensions
    {
        // https://www.techiedelight.com/split-a-list-into-sublists-of-size-n-in-csharp/
        internal static IEnumerable<List<T>> Partition<T>(this List<T> values, int chunkSize)
        {
            for (int i = 0; i < values.Count; i += chunkSize)
            {
                yield return values.GetRange(i, Math.Min(chunkSize, values.Count - i));
            }
        }
    }
}