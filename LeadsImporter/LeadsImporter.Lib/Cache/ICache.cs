using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Cache
{
    public interface ICache
    {
        void Clear();
        void Store(string type, ReportData data);
        ReportData Get(string type);
    }
}
