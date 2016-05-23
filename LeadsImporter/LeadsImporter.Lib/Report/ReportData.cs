using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsImporter.Lib.Report
{
    public class ReportData
    {
        public List<string> Headers;
        public List<ReportDataRow> Rows;
        public ReportData(List<string> headers, List<ReportDataRow> rows)
        {
            Headers = headers;
            Rows = rows;
        }
    }

    public class ReportDataRow
    {
        public List<string> Data;
        public ReportDataRow(List<string> data)
        {
            Data = data;
        }
    }
}
