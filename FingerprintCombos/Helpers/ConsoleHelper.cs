using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FingerprintCombos.Helpers
{
    internal class ConsoleHelper
    {
        /// <summary>
        /// Display dictionary keys in the console and return value
        /// </summary>
        /// <typeparam name="T">Enum options</typeparam>
        /// <param name="options">Dictionary with keys & values</param>
        /// <returns>Enum option</returns>
        internal static T GetIndexedOption<T>(Dictionary<string, T> options)
        {
            while (true)
            {
                Console.Clear();

                foreach (var Option in options.Keys.Select((Value, Index) => new { Value, Index }))
                    Console.WriteLine($"[{Option.Index + 1}]. {Option.Value}");

                Console.Write(">> ");

                if (!int.TryParse(Console.ReadLine(), out var OptionIndex))
                    continue;

                Console.WriteLine();

                int Index = OptionIndex - 1;

                var Element = options.Values.ElementAtOrDefault(Index);

                // https://stackoverflow.com/a/3949120
                if (Element == null)
                    continue;

                return Element;
            }
        }

        /// <summary>
        /// Read a file dragged to the console
        /// </summary>
        /// <returns>File full path</returns>
        internal static string GetTextFileName()
        {
            Console.WriteLine(
                "Drag the text file (combo) that will be protected with your fingerprints");

            Console.Write(">> ");

            string Combo = Console.ReadLine()
                .Replace("\"", string.Empty);

            if (!File.Exists(Combo))
            {
                throw new FileNotFoundException(Combo);
            }

            return Combo;
        }
    }
}