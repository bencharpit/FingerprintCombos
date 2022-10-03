using FingerprintCombos.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintCombos.Options
{
    internal class Searcher : OptionBase
    {
        private readonly List<string> _Combos = new();

        /// <summary>
        /// KeyValuePair Name = Folder Name
        /// KeyValuePair Value = Fingerprints Lines
        /// </summary>
        private List<KeyValuePair<string, List<string>>> _fingerPrints = new();

        /// <summary>
        /// Create instance
        /// </summary>
        /// <param name="Combos">All file names in the CheckCombos directory</param>
        /// <param name="FingerPrintsFolder">All fingerprint directory names</param>
        public Searcher(IEnumerable<string> Combos, IEnumerable<string> FingerPrintsFolder)
        {
            _Combos = Combos.ToList();

            foreach (var FingerPrintFolderName in FingerPrintsFolder)
            {
                List<string> Fingerprints = File
                    .ReadAllLines(Path.Combine(FingerPrintFolderName, "fingerprints.txt"))
                    .ToList();

                _fingerPrints.Add(new KeyValuePair<string, List<string>>(FingerPrintFolderName, Fingerprints));
            }
        }

        /// <summary>
        /// Start checking all combos from /CheckCombos by looking 
        /// for any fingerprints in the previously protected combos
        /// <see cref="Fingerprint"/>
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        /// <seealso cref="ComboReport"/>
        public override async Task Start()
        {
            var ResearchComboFile = _Combos.Select(async comboName =>
            {
                logService.WriteLog($"Getting combo lines {comboName}");

                var comboLines = await GetCombos(comboName);

                logService.WriteLog($"{comboLines.Count():N0} Imported lines from {comboName}");

                logService.WriteLog($"{comboName} creating the report ...");

                var comboReport = await FingerprintSearchInTextFile(comboLines);

                HandleComboReportAsync(comboReport, Path.GetFileName(comboName));

                logService.WriteLog($"{comboName} report it's done @ {comboReport.EqualLinesFound.Count:N0} traces found");
            });

            await Task.WhenAll(ResearchComboFile);
        }

        /// <summary>
        /// Save the combo report
        /// </summary>
        /// <param name="comboReport"><see cref="ComboReport"/></param>
        /// <param name="comboName">Combo name</param>
        /// <returns><see cref="Task"/></returns>
        protected void HandleComboReportAsync(ComboReport comboReport, string comboName)
        {
            if (!Directory.Exists("Reports"))
                Directory.CreateDirectory("Reports");

            string ComboReportPath = Path.Combine("Reports", $"report{DefaultDate}.txt");

            StringBuilder reportText = new StringBuilder($"{Environment.NewLine}{Environment.NewLine}")
                .AppendLine($"{DateTime.Now} [REPORT] {comboName}{Environment.NewLine}")
                .AppendLine($"{comboReport.EqualLinesFound.Count:N0} Fingerprints found")
                .AppendLine()
                .AppendLine($"With at least 1 fingerprint found, it means it comes from your combo")
                .AppendLine($"The less fingerprints found means that the combination is more edited (only if there are 1 or more)")
                .AppendLine($"(but still, traces found >= 1 : it is a combo with lines belonging to yours)")
                .AppendLine($"{Environment.NewLine}")
                .AppendLine($"---------------------------------------------------------");

            lock (Locker)
            {
                File.AppendAllText(ComboReportPath, reportText.ToString());
            }
        }

        /// <summary>
        /// Fingerprint search in uploaded combos
        /// </summary>
        /// <param name="LinesToCheck">combo lines</param>
        /// <returns><see cref="ComboReport"/></returns>
        protected async Task<ComboReport> FingerprintSearchInTextFile(IEnumerable<string> LinesToCheck)
        {
            ComboReport comboReport = new();

            var DetectingFingerprint = LinesToCheck.Select(async line =>
            {
                _fingerPrints.ForEach(fingerPrint =>
                {
                    if (fingerPrint.Value.Contains(line))
                    {
                        lock (Locker)
                        {
                            comboReport.EqualLinesFound.Add(line);
                        }
                    }
                });

                await Task.CompletedTask;
            });

            await Task.WhenAll(DetectingFingerprint);

            return comboReport;
        }

        /// <summary>
        /// Get all lines within a combo text file
        /// </summary>
        /// <param name="ComboFileName">Text file name</param>
        /// <returns><see cref="IEnumerable{T}"/> with text file data</returns>
        protected async Task<IEnumerable<string>> GetCombos(string ComboFileName)
        {
            if (!File.Exists(ComboFileName))
            {
                _Combos.Remove(_Combos.Where(c => c == ComboFileName).Single());
            }

            var comboIndex = _Combos.FindIndex(p => p == ComboFileName);

            IEnumerable<string> Lines = await File.ReadAllLinesAsync(ComboFileName);

            return Lines;
        }
    }
}
