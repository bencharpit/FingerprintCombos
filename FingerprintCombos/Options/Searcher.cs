using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FingerprintCombos.Options
{
    internal class Searcher : OptionBase
    {
        private List<string> _Combos = new();

        /// <summary>
        /// KeyValuePair Name = Folder Name
        /// KeyValuePair Value = Fingerprints Lines
        /// </summary>
        private List<KeyValuePair<string, List<string>>> _fingerPrints = new();


        public Searcher(IEnumerable<string> Combos, IEnumerable<string> FingerPrintsFolder)
        {
            //foreach (var ComboFileName in Combos)
            //{
            //    _Combos.Add(new KeyValuePair<string, List<string>>(ComboFileName, new List<string>()));
            //}

            foreach (var FingerPrintFolderName in FingerPrintsFolder)
            {
                List<string> Fingerprints = File
                    .ReadAllLines(Path.Combine(FingerPrintFolderName, "fingerprints.txt"))
                    .ToList();

                _fingerPrints.Add(new KeyValuePair<string, List<string>>(FingerPrintFolderName, Fingerprints));
            }
        }

        public override async Task Start()
        {
            var ImportComboFiles = _Combos.Select(async comboName =>
            {
                var comboLines = await GetCombos(comboName);

                await FingerprintSearchInTextFile(comboLines);
            });

            await Task.WhenAll(ImportComboFiles);

            throw new NotImplementedException();
        }

        protected async Task FingerprintSearchInTextFile(IEnumerable<string> Lines)
        {

        }

        private async Task<IEnumerable<string>> GetCombos(string ComboFileName)
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
