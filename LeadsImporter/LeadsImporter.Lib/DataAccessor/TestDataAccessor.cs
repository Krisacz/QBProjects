using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.DataAccessor
{
    public class TestDataAccessor : IDataAccessor
    {
        private readonly ILogger _logger;

        public TestDataAccessor(ILogger logger)
        {
            _logger = logger;
        }

        public ReportData GetReportData(int reportId)
        {
            try
            {
                switch (reportId)
                {
                    case 38379: return GetUrscReport();
                    case 38375: return GetRppiReport();
                    default: throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"TestDataAccessor >>> GetReportData[{reportId}]: {ex.Message}");
            }

            return null;
        }

        #region GET URSC REPORT
        private ReportData GetUrscReport()
        {
            try
            {
                _logger.AddInfo($"TestDataAccessor >>> GetUrscReport: Getting test data for URSC...");
                return new ReportData()
                {
                    QueryId = 38379,
                    Headers = new List<string>() { "LeadID", "CustomerID", "Lender ID", "Date of Loan", "WhenCreate", "SomeData1", "SomeData2", "SomeData3", "SomeData4" },
                    Rows = new List<ReportDataRow>()
                    {
                        new ReportDataRow() { Data = new List<string>() { "111", "1111", "11111", "2016-01-25", "25/01/2016 11:11:11", "25-04-2014£", "Bbb1", "Yes", "10000" } },
                        new ReportDataRow() { Data = new List<string>() { "222", "2222", "22222", "2016-02-25", "25/02/2016 11:12:12", "15-05-2016", "Bbb2", "no", "25000" } },
                        new ReportDataRow() { Data = new List<string>() { "333", "3333", "33333", "2016-03-25", "25/03/2016 11:13:13", "22-07-2015", "B,bb3", "don't know", "" } },
                        //new ReportDataRow() { Data = new List<string>() { "444", "4444", "44444", "2016-04-25", "25/04/2016 11:14:14", "Aa£a4", "Bbb4", "Ccc4" } },
                        //new ReportDataRow() { Data = new List<string>() { "555", "5555", "55555", "2016-05-25", "25/05/2016 11:15:15", "25-07-2016", "Bbb5", "C,cc5" } },
                        //new ReportDataRow() { Data = new List<string>() { "777", "3333", "33333", "2016-03-25", "25/03/2016 11:13:13", "Aaa3", "B,bb3", "Ccc3" } },
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"TestDataAccessor >>> GetUrscReport: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region GET RPPI REPORT
        private ReportData GetRppiReport()
        {
            try
            {
                _logger.AddInfo($"TestDataAccessor >>> GetUrscReport: Getting test data for RPPI...");
                return new ReportData()
                {
                    QueryId = 38375,
                    Headers = new List<string>() { "LeadID", "CustomerID", "Lender ID", "Date of Credit", "Date Executed", "SomeDataA", "SomeDataB", "SomeDataC" },
                    Rows = new List<ReportDataRow>()
                    {
                        //new ReportDataRow() { Data = new List<string>() { "666", "6666", "66666", "2016-06-25", "25/01/2016 11:16:11", "Aa,a6", "Bbb6", "Ccc6" } },
                        //new ReportDataRow() { Data = new List<string>() { "777", "7777", "77777", "2016-07-25", "25/02/2016 11:17:12", "Aaa7", "B£bb7", "Ccc7" } },
                        //new ReportDataRow() { Data = new List<string>() { "888", "8888", "88888", "2016-08-25", "25/03/2016 11:18:13", "Aaa8", "Bb,b8", "Ccc8" } },
                        //new ReportDataRow() { Data = new List<string>() { "999", "9999", "99999", "2016-09-25", "25/04/2016 11:19:14", "Aa£a9", "Bbb9", "Ccc9" } },
                        //new ReportDataRow() { Data = new List<string>() { "000", "0000", "00000", "2016-10-25", "25/05/2016 11:20:15", "Aaa0", "Bbb0", "Ccc,0" } },
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"TestDataAccessor >>> GetRppiReport: {ex.Message}");
            }

            return null;
        }
        #endregion
    }
}