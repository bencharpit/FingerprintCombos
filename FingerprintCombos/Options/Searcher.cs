using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FingerprintCombos.Options
{
    internal class Searcher : OptionBase
    {
        private List<KeyValuePair<string, List<string>>> _Combos = new();

        public Searcher(IEnumerable<string> Combos)
        {
            foreach (var ComboFileName in Combos)
            {
                _Combos.Add(new KeyValuePair<string, List<string>>(ComboFileName, new List<string>()));
            }
        }

        public override async void Start()
        {
            var ImportComboFiles = _Combos.Select(async combo =>
            {
                
            });

            await Task.WhenAll(ImportComboFiles);

            throw new NotImplementedException();
        }

        private void GetCombos(string ComboFileName)
        {

        }
    }
}
