using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using FingerprintCombos.Helpers;

namespace FingerprintCombos
{
    internal class Program
    {
        /// <summary>
        /// FingerprintCombo Mode
        /// </summary>
        internal enum Option
        {
            ADD, SEARCHER
        }

        static async Task Main()
        {
            Console.Title = "FingerprintCombos @ https://cracked.to/char";

            Console.WriteLine("1. Adding fingerprints to your combo");
            Console.WriteLine("2. Check your fingerprints on combos uploaded to /check-combos");

            var OptionChoosed = ConsoleHelper.GetIndexedOption(new Dictionary<string, Option?>
            {
                ["Adding fingerprints to your combo"] = Option.ADD,
                ["Check your fingerprints on combos uploaded to /CheckCombos"] = Option.SEARCHER
            });;

            if (!OptionChoosed.HasValue)
                Environment.Exit(0);

            await Start(OptionChoosed.Value).ConfigureAwait(false);

            await Task.Delay(-1);
        }

        protected static async Task Start(Option Option)
        {
            var option = new Builder(Option)
                .WithFingerprints(Path.Combine("ProtectedCombos"))
                .WithCombosFolder(Path.Combine("CheckCombos"))
                .Build();

            await option.Start().ConfigureAwait(false);

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine();

            option.logService.WriteLog("Finished reports");
        }
    }
}
