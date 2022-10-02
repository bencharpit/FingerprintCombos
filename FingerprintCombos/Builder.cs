using System;
using System.Collections.Generic;
using System.IO;
using FingerprintCombos.Helpers;
using FingerprintCombos.Options;
using static System.Net.WebRequestMethods;
using static FingerprintCombos.Program;

namespace FingerprintCombos
{
    internal class Builder
    {
        private Option _option;

        private IEnumerable<string> _directories;

        /// <summary>
        /// Directories with fingerprint
        /// </summary>
        private IEnumerable<string> _fingerPrints;

        public Builder(Option option)
        {
            _option = option;
        }

        public Builder WithCombosFolder(string FolderName)
        {
            if (!Directory.Exists(FolderName))
                Directory.CreateDirectory(FolderName);

            try
            {
                _directories = Directory.GetFiles(FolderName, "*.txt");
            }
            // Temporary solution to the exception in option 1
            catch
            {
                _directories = default;
            }

            return this;
        }

        public Builder WithFingerprints(string FolderName)
        {
            if (!Directory.Exists(FolderName))
                Directory.CreateDirectory(FolderName);

            _fingerPrints = Directory.GetDirectories(FolderName, "*", SearchOption.TopDirectoryOnly);

            return this;
        }

        public OptionBase Build()
        {
            return _option switch
            {
                Option.ADD => new Fingerprint(),
                Option.SEARCHER => new Searcher(_directories, _fingerPrints),
                _ => throw new Exception()
            };
        }
    }
}