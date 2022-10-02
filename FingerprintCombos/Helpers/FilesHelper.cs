using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FingerprintCombos.Helpers
{
    internal class FilesHelper
    {
        public static IEnumerable<string> GetAllFilesOnDirectory(string directoryName, string searchPattern, 
            EnumerationOptions enumerationOptions = default)
        {
            IEnumerable<string> Files = Directory.GetFiles(directoryName, searchPattern);

            if (!Files.Any())
                throw new ArgumentNullException(directoryName);

            return Files;
        }
    }
}
