using System;
using System.IO;

namespace LeadsImporter.Lib.Log
{
    public class FileLogger : ILogger
    {
        public bool EnableDetailedLog = false;
        private const string LogFile = @"log.txt";

        public FileLogger()
        {
            if (!File.Exists(LogFile)) File.Create(LogFile);
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

        public bool IsDetailedLogEnabled()
        {
            return EnableDetailedLog;
        }

        public void AddDetailedLog(string detail)
        {
            if (!IsDetailedLogEnabled()) return;
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var txt = $"{dt} [INFO] \t{detail}{Environment.NewLine}";
            File.AppendAllText(LogFile, txt);
        }
    }
}
