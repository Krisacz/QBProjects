using System.Collections.Generic;

namespace LeadsImporterCrossChecker
{
    public class ReportData
    {
        public int QueryId;
        public List<string> Headers;
        public List<ReportDataRow> Rows;
    }
}
