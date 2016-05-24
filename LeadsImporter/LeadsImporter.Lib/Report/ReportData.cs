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

    }

    public class ReportDataRow
    {
        public List<string> Data;
    }
}
