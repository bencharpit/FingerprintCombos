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

            var protectedLines = ProtectCombo(File.ReadAllLines(Combo), FileName);

            logService.WriteLog($"'{FileName}' has been protected =)");

            File.AppendAllLines(Path.Combine("ProtectedCombos", $"{FileName}{DefaultDate}", $"{FileName}-Protected.txt"), protectedLines);
        }

        /// <summary>
        /// Protect the combo by adding unique fingerprints between lines and then randomizing the combo
        /// </summary>
        /// <param name="ComboLines">Unsafe combo</param>
        /// <param name="FileName">File name</param>
        /// <returns>Combo with fingerprints</returns>
        private IEnumerable<string> ProtectCombo(IEnumerable<string> ComboLines, string FileName)
        {
            List<string> ProtectedLines = new();

            /* The number of traces will be assigned depending on the size of the combo, 
             * the bigger the combo, the more traces there will be.
             * 
             * This is to have a greater certainty of finding your lines in a fully edited combo
             */
            Dictionary<int, int> FingerprintsDependingOnComboLines = new()
            {
                [100000] = 183,
                [300000] = 549,
                [500000] = 1098,
                [1000000] = 2196
            };

            int FingerprintsToAdd = default;

            try
            {
                var MajorFingerprints = FingerprintsDependingOnComboLines
                    .Where(x => x.Key >= ComboLines.Count());

                // If the combo is greater than the max amount (1M)
                if (!MajorFingerprints.Any())
                    throw new ArgumentNullException();

                FingerprintsToAdd = MajorFingerprints.OrderByDescending(x => x.Key)
                    .Last()
                    .Value;
            }
            catch (ArgumentNullException)
            {
                // If the amount is greater than 1M,
                // will be assigned 3K of unique traces
                // spread over the total combo.
                FingerprintsToAdd = 3000;
            }
            catch (Exception exceptionMessage)
            {
                throw exceptionMessage;
            }

            /* In a list with 100,000 lines; 
             * 183 fingerprint's will be added (1 every 546 lines)
             * (then it will be randomized to avoid that they can be removed). */
            int BetweenLinesLenght = ComboLines.Count() / FingerprintsToAdd;

            var ChunkedLines = ComboLines.ToList().Partition(BetweenLinesLenght);

            foreach (var chunkLines in ChunkedLines)
            {
                ProtectedLines.AddRange(chunkLines);

                string fingerPrint = GetCustomFingerPrint(chunkLines);

                ProtectedLines.Add(fingerPrint);
                
                //lock (Locker)
                //{
                    File.AppendAllText(Path.Combine("ProtectedCombos", $"{FileName}{DefaultDate}", "fingerprints.txt"), fingerPrint + Environment.NewLine);
                //}
            }

            return ProtectedLines.OrderBy(_ => random.Next());
        }

        /// <summary>
        /// There is probably a more effective way to make a fingerprint for the combos (with a longer duration). 
        /// In my opinion, this is the fastest one.
        /// </summary>
        /// <returns>A real line based on the lines already integrated into the combo</returns>
        protected string GetCustomFingerPrint(IEnumerable<string> chunkCombo)
        {
            List<string> TwoRandomLines = chunkCombo.OrderBy(_ => random.Next()).Take(2).ToList();

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
                + StringHelper.RandomString(random.Next(3))
                + SecondData[(SecondData.Length/3)..];

            return FingerPrint;
        }
    }
}
