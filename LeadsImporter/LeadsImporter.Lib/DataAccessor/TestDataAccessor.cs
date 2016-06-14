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
                    case 38380: return GetUrscReport2();

                    case 38375: return GetRppiReport();
                    case 38376: return GetRppiReport2();

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
                    Headers = new List<string>() { "LeadID", "CustomerID", "Lender ID", "Date of Loan", "WhenCreate", "SomeData1", "SomeData2", "SomeData3" },
                    Rows = new List<ReportDataRow>()
                    {
                        new ReportDataRow() { Data = new List<string>() { "111", "1111", "11111", "2016-01-25", "25/01/2016 11:11:11", "25-04-2014£", "Bbb,,,,££££1", "Yes" } },
                        new ReportDataRow() { Data = new List<string>() { "222", "2222", "22222", "2016-02-25", "25/02/2016 11:12:12", "15-05-2016", "Bbb2", "no" } },
                        new ReportDataRow() { Data = new List<string>() { "333", "3333", "33333", "2016-03-25", "25/03/2016 11:13:13", "22-07-2015", "B,bb3", "don't know" } },
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"TestDataAccessor >>> GetUrscReport: {ex.Message}");
            }

            return null;
        }

        private ReportData GetUrscReport2()
        {
            try
            {
                _logger.AddInfo($"TestDataAccessor >>> GetUrscReport2: Getting test data for URSC...");
                return new ReportData()
                {
                    QueryId = 38380,
                    Headers = new List<string>() { "LeadID", "CustomerID", "Lender ID", "Date of Loan", "WhenCreate", "AdditionalData1", "AdditionalData2", "AdditionalData3" },
                    Rows = new List<ReportDataRow>()
                    {
                        new ReportDataRow() { Data = new List<string>() { "111", "1111", "11111", "2016-01-25", "25/01/2016 11:11:11", "Additional1-1", "Additional1-2", "Additional1-3" } },
                        new ReportDataRow() { Data = new List<string>() { "222", "2222", "22222", "2016-02-25", "25/02/2016 11:12:12", "Additional2-1", "Additional2-2", "Additional2-3" } },
                        new ReportDataRow() { Data = new List<string>() { "333", "3333", "33333", "2016-03-25", "25/03/2016 11:13:13", "Additional3-1", "Additional3-2", "Additional3-3" } },
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"TestDataAccessor >>> GetUrscReport2: {ex.Message}");
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
                        new ReportDataRow() { Data = new List<string>() { "666", "6666", "66666", "2016-06-25", "25/01/2016 11:16:11", "Aa,a6", "Bbb6", "Ccc6" } },
                        new ReportDataRow() { Data = new List<string>() { "777", "7777", "77777", "2016-07-25", "25/02/2016 11:17:12", "Aaa7", "B£bb7", "Ccc7" } },
                        new ReportDataRow() { Data = new List<string>() { "888", "8888", "88888", "2016-08-25", "25/03/2016 11:18:13", "Aaa8", "Bb,b8", "Ccc8" } },
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"TestDataAccessor >>> GetRppiReport: {ex.Message}");
            }

            return null;
        }

        private ReportData GetRppiReport2()
        {
            try
            {
                _logger.AddInfo($"TestDataAccessor >>> GetUrscReport2: Getting test data for RPPI...");
                return new ReportData()
                {
                    QueryId = 38376,
                    Headers = new List<string>() { "LeadID", "CustomerID", "Lender ID", "Date of Credit", "Date Executed", "AdditionalDataA", "AdditionalDataB", "AdditionalDataC" },
                    Rows = new List<ReportDataRow>()
                    {
                        new ReportDataRow() { Data = new List<string>() { "666", "6666", "66666", "2016-06-25", "25/01/2016 11:16:11", "AdditionalA-1", "AdditionalA-2", "AdditionalA-3" } },
                        new ReportDataRow() { Data = new List<string>() { "777", "7777", "77777", "2016-07-25", "25/02/2016 11:17:12", "AdditionalB-1", "AdditionalB-2", "AdditionalB-3" } },
                        new ReportDataRow() { Data = new List<string>() { "888", "8888", "88888", "2016-08-25", "25/03/2016 11:18:13", "AdditionalC-1", "AdditionalC-2", "AdditionalC-3" } },
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"TestDataAccessor >>> GetRppiReport2: {ex.Message}");
            }

            return null;
        }
        #endregion
    }
}