namespace LeadsImporter.Lib.Log
{
    public interface ILogger
    {
        void AddError(string error);
        void AddInfo(string info);
        bool IsDetailedLogEnabled();
        void AddDetailedLog(string detail);
    }
}
