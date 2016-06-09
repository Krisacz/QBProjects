using System;

namespace LeadsImporter.Lib.Log
{
    public class ConsoleLogger : ILogger
    {
        public bool EnableDetailedLog = true;

        public void AddError(string error)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("{0} [ERROR] \t{1}", dt, error);
        }

        public void AddInfo(string info)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("{0} [INFO] \t{1}", dt, info);
        }

        public bool IsDetailedLogEnabled()
        {
            return EnableDetailedLog;
        }

        public void AddDetailedLog(string detail)
        {
            if(!IsDetailedLogEnabled()) return;
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("{0} [INFO] \t{1}", dt, detail);
        }
    }
}
