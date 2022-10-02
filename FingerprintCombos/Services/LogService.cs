using System;

namespace FingerprintCombos.Services
{
    internal class LogService
    {
        public void WriteLog(string messageText)
        {
            Console.WriteLine($"[{DateTime.Now}] {messageText}");
        }
    }
}