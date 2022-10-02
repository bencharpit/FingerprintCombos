using System;
using System.Collections.Generic;
using System.IO;
using FingerprintCombos.Helpers;
using FingerprintCombos.Options;
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
                throw new DirectoryNotFoundException(FolderName);

            _directories = FilesHelper.GetAllFilesOnDirectory(FolderName, "*.txt");

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