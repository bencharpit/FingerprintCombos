using FingerprintCombos.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return _option == Option.ADD
                ? new Fingerprint()
                : new Searcher(_directories);
        }
    }
}
