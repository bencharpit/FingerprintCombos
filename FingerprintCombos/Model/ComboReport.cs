using System.Collections.Generic;

namespace FingerprintCombos.Model
{
    /// <summary>
    /// Fingerprinting report on the combo
    /// </summary>
    internal class ComboReport
    {
        /// <summary>
        /// Number of traces found in combo
        /// </summary>
        internal List<string> EqualLinesFound { get; } = new List<string>();
    }
}
