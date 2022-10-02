using FingerprintCombos.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FingerprintCombos.Options
{
    internal class Fingerprint : OptionBase
    {
        private static readonly Random random = new();

        /// <summary>
        /// Start digitizing the combo with unique fingerprints
        /// </summary>
        /// <returns></returns>
        public override async Task Start()
        {
            string Combo = ConsoleHelper.GetTextFileName();

            string FileName = Path.GetFileName(Combo);

            Console.WriteLine();

            logService.WriteLog($"Protecting '{FileName}' ...");

            Directory.CreateDirectory(Path.Combine("ProtectedCombos", $"{FileName}{DefaultDate}"));

            var protectedLines = await ProtectCombo(File.ReadAllLines(Combo), FileName).ConfigureAwait(false);

            logService.WriteLog($"'{FileName}' has been protected =)");

            File.AppendAllLines(Path.Combine("ProtectedCombos", $"{FileName}{DefaultDate}", $"{FileName}-Protected.txt"), protectedLines);
        }

        /// <summary>
        /// Protect the combo by adding unique fingerprints between lines and then randomizing the combo
        /// </summary>
        /// <param name="ComboLines">Unsafe combo</param>
        /// <param name="FileName">File name</param>
        /// <returns>Combo with fingerprints</returns>
        private async Task<IEnumerable<string>> ProtectCombo(IEnumerable<string> ComboLines, string FileName)
        {
            List<string> ProtectedLines = new();

            int TotalFingerPrints = 45;

            /* Only 45 fingerprints will be added by default, 
             * that means that; in a list with 100,000 lines, 
             * 1 fingerprint will be added every 2,2K lines 
             * (then it will be randomized to avoid that they can be removed). */
            int BetweenLinesLenght = ComboLines.Count() / TotalFingerPrints;

            var ChunkedLines = ComboLines.ToList().Partition(BetweenLinesLenght);

            foreach (var chunkLines in ChunkedLines)
            {
                ProtectedLines.AddRange(chunkLines);

                string fingerPrint = GetCustomFingerPrint(chunkLines);

                ProtectedLines.Add(fingerPrint);

                lock (Locker)
                {
                    File.AppendAllText(Path.Combine("ProtectedCombos", $"{FileName}{DefaultDate}", "fingerprints.txt"), fingerPrint + Environment.NewLine);
                }
            }

            await Task.CompletedTask;

            return ProtectedLines.OrderBy(_ => random.Next());
        }

        /// <summary>
        /// There is probably a more effective way to make a fingerprint for the combos (with a longer duration). 
        /// In my opinion, this is the fastest one.
        /// </summary>
        /// <returns>A real line based on the lines already integrated into the combo</returns>
        protected string GetCustomFingerPrint(IEnumerable<string> chunkCombo)
        {
            List<string> TwoRandomLines;

            lock (Locker)
            {
                TwoRandomLines = chunkCombo.OrderBy(_ => random.Next()).Take(2).ToList();
            }

            string FirstData = TwoRandomLines[0].Split(':')[0];
            string SecondData = TwoRandomLines[1].Split(':')[0];

            if (FirstData.Contains("@"))
                FirstData = FirstData.Split("@")[0];

            if (SecondData.Contains("@"))
                SecondData = SecondData.Split("@")[0];

            string FingerPrint = string.Empty;

            FingerPrint += SecondData[..(SecondData.Length / 2)] + FirstData[(FirstData.Length / 3)..];

            // If is email:pass
            if (TwoRandomLines.Any(x => x.Contains("@")))
            {
                // add the 1st of the two random lines domain
                FingerPrint += "@" + TwoRandomLines.First()
                    .Split('@')[1]
                    .Split(':')[0];
            }

            FingerPrint += ":" 
                + StringHelper.RandomString(random.Next(12)) 
                + FirstData[..(FirstData.Length / 2)] 
                + StringHelper.RandomString(random.Next(3));

            return FingerPrint;
        }
    }
}
