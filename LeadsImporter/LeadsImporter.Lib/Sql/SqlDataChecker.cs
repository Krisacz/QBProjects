using System.Collections.Generic;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Sql
{
    public class SqlDataChecker
    {
        public void RemoveExceptions(ReportData reportData, List<SqlDataExceptionObject> exceptions)
        {
            foreach (var exception in exceptions)
            {
                
            }
        }

        public List<ReportDataRow> GetNewDuplicates(ReportData reportData, List<SqlDataObject> allData)
        {
            throw new System.NotImplementedException();
        }
    }
}