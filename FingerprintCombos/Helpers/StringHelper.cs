using System;
using System.Linq;

namespace FingerprintCombos.Helpers
{
    /// <summary>
    /// https://stackoverflow.com/a/1344242
    /// </summary>
    internal class StringHelper
    {
        internal static Random random = new();

        internal static string RandomString(int length)
        {
            const string chars = "abcdef0123456789";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
