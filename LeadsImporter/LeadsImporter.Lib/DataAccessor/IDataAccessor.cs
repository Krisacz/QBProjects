using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.DataAccessor
{
    public interface IDataAccessor
    {
        ReportData GetReportData(int reportId);
    }

    public class TestDataAccessor : IDataAccessor
    {
        public ReportData GetReportData(int reportId)
        {
            switch (reportId)
            {
                case 38379:     return GetUrscReport(); 
                case 38375:     return GetRppiReport(); 
                default: throw new ArgumentException(); 
            }
        }

        #region GET URSC REPORT
        private static ReportData GetUrscReport()
        {
            return new ReportData()
            {
                QueryId = 38379,
                Headers = new List<string>() { "LeadID", "CustomerID", "Lender ID", "Date of Loan", "WhenCreate", "SomeData1", "SomeData2", "SomeData3" },
                Rows = new List<ReportDataRow>()
                {
                    new ReportDataRow() { Data = new List<string>() { "111", "1111", "11111", "2016-01-25", "25/01/2016 11:11:11", "Aaa1", "Bbb1", "Ccc1" } },
                    new ReportDataRow() { Data = new List<string>() { "222", "2222", "22222", "2016-02-25", "25/02/2016 11:12:12", "Aaa2", "Bbb2", "Ccc2" } },
                    new ReportDataRow() { Data = new List<string>() { "333", "3333", "33333", "2016-03-25", "25/03/2016 11:13:13", "Aaa3", "Bbb3", "Ccc3" } },
                    new ReportDataRow() { Data = new List<string>() { "444", "4444", "44444", "2016-04-25", "25/04/2016 11:14:14", "Aaa4", "Bbb4", "Ccc4" } },
                    new ReportDataRow() { Data = new List<string>() { "555", "5555", "55555", "2016-05-25", "25/05/2016 11:15:15", "Aaa5", "Bbb5", "Ccc5" } },
                }
            };
        }
        #endregion

        #region GET RPPI REPORT
        private static ReportData GetRppiReport()
        {
            return new ReportData()
            {
                QueryId = 38375,
                Headers = new List<string>() { "LeadID", "CustomerID", "Lender ID", "Date of Credit", "Date Executed", "SomeDataA", "SomeDataB", "SomeDataC" },
                Rows = new List<ReportDataRow>()
                {
                    new ReportDataRow() { Data = new List<string>() { "666", "6666", "66666", "2016-06-25", "25/01/2016 11:16:11", "Aaa6", "Bbb6", "Ccc6" } },
                    new ReportDataRow() { Data = new List<string>() { "777", "7777", "77777", "2016-07-25", "25/02/2016 11:17:12", "Aaa7", "Bbb7", "Ccc7" } },
                    new ReportDataRow() { Data = new List<string>() { "888", "8888", "88888", "2016-08-25", "25/03/2016 11:18:13", "Aaa8", "Bbb8", "Ccc8" } },
                    new ReportDataRow() { Data = new List<string>() { "999", "9999", "99999", "2016-09-25", "25/04/2016 11:19:14", "Aaa9", "Bbb9", "Ccc9" } },
                    new ReportDataRow() { Data = new List<string>() { "000", "0000", "00000", "2016-10-25", "25/05/2016 11:20:15", "Aaa0", "Bbb0", "Ccc0" } },
                }
            };
        }
        #endregion
    }
}
