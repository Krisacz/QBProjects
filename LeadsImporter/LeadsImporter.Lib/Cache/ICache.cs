using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Cache
{
    public interface ICache
    {
        void Clear();
        void Store(string type, ReportData data);
        void StoreExceptions(string type, ReportDataExceptions exceptions);
        ReportData Get(string type);
        ReportDataExceptions GetExceptions(string type);
    }
}
