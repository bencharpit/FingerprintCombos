using FingerprintCombos.Services;
using System;
using System.Threading.Tasks;

namespace FingerprintCombos.Options
{
    internal abstract class OptionBase
    {
        /// <summary>
        /// Log Service
        /// </summary>
        public LogService logService = new();

        /// <summary>
        /// DefaultDate to result folder
        /// </summary>
        public readonly string DefaultDate = DateTime.Now.ToString("-dd-MM-yyyy-(hh-mm-ss)");

        /// <summary>
        /// To avoid multi-thread exceptions
        /// </summary>
        public static object Locker = new();

        public abstract Task Start();
    }
}