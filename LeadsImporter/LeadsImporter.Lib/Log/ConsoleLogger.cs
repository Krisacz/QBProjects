using System;

namespace LeadsImporter.Lib.Log
{
    public class ConsoleLogger : ILogger
    {
        public bool EnableDetailedLog = true;

        public void AddError(string error, Exception exception)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var txt = $"{dt} [ERROR]\t{error} ";
            if (exception == null)
            {
                txt += Environment.NewLine;
            }
            else
            {
                txt += $"Error Message: {exception.Message}";
                if (exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.ToString()))
                {
                    txt += $"\t\tInnerException: {exception.InnerException}";
                }
            }
            Console.WriteLine(txt);
        }

        public void AddInfo(string info)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("{0} [INFO] \t{1}", dt, info);
        }

        public void AddDetailedLog(string detail)
        {
            if(!EnableDetailedLog) return;
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("{0} [INFO] \t{1}", dt, detail);
        }
    }
}
