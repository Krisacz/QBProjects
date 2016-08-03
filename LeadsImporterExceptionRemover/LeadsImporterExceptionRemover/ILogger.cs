using System;

namespace LeadsImporterExceptionRemover
{
    public interface ILogger
    {
        void AddError(string error, Exception exception);
        void AddInfo(string info);
        void AddDetailedLog(string detail);
    }
}
