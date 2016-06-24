using System.Collections.Generic;

namespace LeadsImporter.Lib.Report
{
    public class ReportDataExceptions
    {
        public int QueryId;
        public List<string> Headers;
        public List<ReportDataRowExceptions> Rows;
    }
}