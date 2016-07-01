using System;

namespace LeadsImporterCrossChecker
{
    public interface ILogger
    {
        void AddError(string error, Exception exception);
        void AddInfo(string info);
        void AddDetailedLog(string detail);
    }
}
