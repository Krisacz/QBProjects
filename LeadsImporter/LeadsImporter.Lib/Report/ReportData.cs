using System.Collections.Generic;

namespace LeadsImporter.Lib.Report
{
    public class ReportData
    {
        public List<string> Headers;
        public List<ReportDataRow> Rows;
        public ReportSettings Settings;
    }

    public class ReportDataRow
    {
        public List<string> Data;
    }
}
