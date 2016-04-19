using System;
using System.IO;

namespace IMH.UI
{
    public class AuditLogger : ILogger
    {
        private const string LogFile = @"audit.csv";

        public AuditLogger()
        {
            if (!File.Exists(LogFile)) File.Create(LogFile);
        }

        public void AddEmptyLine()
        {
            //Audit doesn't have emptyLine
        }

        public void AddError(string error)
        {
            //Audit doesn't have erorr
        }

        public void AddInfo(string action)
        {
            var dt = DateTime.Now;
            var date = dt.ToString("yyyy-MM-dd");
            var time = dt.ToString("HH:mm:ss");
            var user = "UnknownUser";
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (windowsIdentity != null) user = windowsIdentity.Name;
            var txt = string.Format("{0},{1},{2},{3}{4}", date, time, user, action, Environment.NewLine);
            File.AppendAllText(LogFile, txt);
        }
    }
}
