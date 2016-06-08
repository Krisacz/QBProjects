using System.Collections.Generic;

namespace LeadsImporter.Lib.Report
{
    public class ReportData
    {
        public int QueryId;
        public List<string> Headers;
        public List<ReportDataRow> Rows;
    }
}
