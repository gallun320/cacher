using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheApp.Utils
{
    public static class Logger
    {
        public static void Log(string logText)
        {
            Console.WriteLine($"{DateTime.Now.ToString()} {logText}");
            LogToFile($"{DateTime.Now.ToString()} {logText}").ConfigureAwait(false);
        }

        public static async Task LogToFile(string logText)
        {
            var path = $"{ConfigPaths.LogPath}{DateTime.UtcNow.ToShortDateString()}_log.txt";
            if (!File.Exists(path))
            {
                using (var sw = File.CreateText(path))
                {
                    await sw.WriteLineAsync(logText);
                }
            }
            else
            {
                using (var sw = File.AppendText(path))
                {
                    await sw.WriteLineAsync(logText);
                }
            }
        }
    }
}
