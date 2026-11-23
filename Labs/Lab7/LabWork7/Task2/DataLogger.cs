using System;
using System.Collections.Generic;
using System.Text;

namespace Task2
{
    public class DataLogger
    {
        public static async Task LogOperationsAsync(string logFilePath, string methodName, long ms) =>
            File.AppendAllText(logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation={methodName}," +
            $"Elapsed={ms}\n");
    }
}
