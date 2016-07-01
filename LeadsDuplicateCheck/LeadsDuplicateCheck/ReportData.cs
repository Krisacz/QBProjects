using System.Collections.Generic;

namespace LeadsDuplicateCheck
{
    public class ReportData
    {
        public int QueryId;
        public List<string> Headers;
        public List<ReportDataRow> Rows;
    }
}
