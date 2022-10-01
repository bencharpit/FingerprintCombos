using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FingerprintCombos.Options;
using static FingerprintCombos.Program;

namespace FingerprintCombos
{
    internal class Builder
    {
        private Option _option;

        private IEnumerable<string> _directories;

        public Builder(Option option)
        {
            _option = option;
        }

        public Builder WithCombosFolder(string FolderName)
        {
            if (!Directory.Exists(FolderName))
                throw new DirectoryNotFoundException(FolderName);

            IEnumerable<string> Directories = Directory.GetFiles(FolderName, "*.txt");

            if (!Directories.Any())
                throw new ArgumentNullException(FolderName);

            _directories = Directories;

            return this;
        }

        public OptionBase Build()
        {
            return _option switch
            {
                Option.ADD => new Fingerprint(),
                Option.SEARCHER => new Searcher(_directories),
                _ => throw new Exception()
            };
        }
    }
}