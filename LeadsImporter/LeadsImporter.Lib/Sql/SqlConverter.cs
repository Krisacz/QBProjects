using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Sql
{
    public static class SqlConverter
    {
        public static IEnumerable<SqlDataObject> GetReportDataAsSqlDataObject(ReportData reportData, ReportDataManager reportDataManager, ILogger logger)
        {
            try
            {
                var list = new List<SqlDataObject>();
                foreach (var reportDataRow in reportData.Rows)
                {
                    var type = reportDataManager.GetReportType(reportData);
                    var leadId = reportDataManager.GetValueForLeadId(reportData, reportDataRow);
                    var customerId = reportDataManager.GetValueForCustomerId(reportData, reportDataRow);
                    var lenderId = reportDataManager.GetValueForLenderId(reportData, reportDataRow);
                    var loanDate = reportDataManager.GetValueForLoanDate(reportData, reportDataRow);
                    var leadCreated = reportDataManager.GetValueForLeadCreated(reportData, reportDataRow);

                    list.Add(new SqlDataObject(type, leadId, customerId, lenderId, loanDate, leadCreated));
                }
                return list;
            }
            catch (Exception ex)
            {
                logger.AddError($"SqlConverter >>> GetReportDataAsSqlDataObject: {ex.Message}");
            }

            return null;
        }
    }
}
