using System;
using System.Collections.Generic;
using System.Linq;

namespace FingerprintCombos.Helpers
{
    internal class ConsoleHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
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

                int Index = OptionIndex - 1;

                var Element = options.Values.ElementAtOrDefault(Index);

                // https://stackoverflow.com/a/3949120
                if (Element == null)
                    continue;

                return Element;
            }
        }
    }
}