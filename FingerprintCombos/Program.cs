using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using FingerprintCombos.Helpers;

namespace FingerprintCombos
{
    internal class Program
    {
        internal enum Option
        {
            ADD, SEARCHER
        }

        static async Task Main()
        {
            Console.WriteLine("1. Adding fingerprints to your combo");
            Console.WriteLine("2. Check your fingerprints on combos uploaded to /check-combos");

            var OptionChoosed = ConsoleHelper.GetIndexedOption(new Dictionary<string, Option?>
            {
                ["Adding fingerprints to your combo"] = Option.ADD,
                ["Check your fingerprints on combos uploaded to /check-combos"] = Option.SEARCHER
            });;

            if (!OptionChoosed.HasValue)
                Environment.Exit(0);

            Start(OptionChoosed.Value);

            await Task.Delay(-1);
        }

        protected static void Start(Option Option)
        {
            var option = new Builder(Option)
                .WithCombosFolder(Path.Combine("check-combos"))
                .Build();

            option.Start();
        }
    }
}
