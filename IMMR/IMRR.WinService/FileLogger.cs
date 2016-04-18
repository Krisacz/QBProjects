using System;
using System.IO;

namespace IMRR.Lib
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
            var txt = string.Format("{0} [ERROR] \t{1}{2}", dt, error, Environment.NewLine);
            File.AppendAllText(LogFile, txt);
        }

        public void AddInfo(string info)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var txt = string.Format("{0} [INFO] \t{1}{2}", dt, info, Environment.NewLine);
            File.AppendAllText(LogFile, txt);
        }
    }
}
