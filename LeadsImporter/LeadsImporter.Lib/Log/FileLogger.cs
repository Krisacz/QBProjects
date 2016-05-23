using System;
using System.IO;

namespace LeadsImporter.Lib.Log
{
    public class FileLogger : ILogger
    {
        private const string LogFile = @"log.txt";

        public FileLogger()
        {
            if (!File.Exists(LogFile)) File.Create(LogFile);
        }

        public void AddEmptyLine()
        {
            var newLine = Environment.NewLine;
            File.AppendAllText(LogFile, newLine);
        }

        public void AddError(string error)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var txt = $"{dt} [ERROR] \t{error}{Environment.NewLine}";
            File.AppendAllText(LogFile, txt);
        }

        public void AddInfo(string info)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var txt = $"{dt} [INFO] \t{info}{Environment.NewLine}";
            File.AppendAllText(LogFile, txt);
        }
    }
}
