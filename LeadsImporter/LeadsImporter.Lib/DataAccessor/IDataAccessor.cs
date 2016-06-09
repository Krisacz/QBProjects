using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.DataAccessor
{
    public interface IDataAccessor
    {
        ReportData GetReportData(int reportId);
    }
}
