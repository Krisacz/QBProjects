using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Sql
{
    //TODO Add logging
    public class SqlDataChecker
    {
        private readonly ReportsSettings _reportsSettings;
        private readonly ReportDataManager _reportDataManager;

        public SqlDataChecker(ReportsSettings reportsSettings, ReportDataManager reportDataManager)
        {
            _reportsSettings = reportsSettings;
            _reportDataManager = reportDataManager;
        }

        public ReportData RemoveExceptions(ReportData reportData, IEnumerable<SqlDataExceptionObject> exceptions)
        {
            var reportDataWithoutExceptions = new ReportData()
            {
                QueryId = reportData.QueryId,
                Headers = reportData.Headers,
                Rows = new List<ReportDataRow>()
            };

            foreach (var exception in exceptions)
            {
                foreach (var reportDataRow in reportData.Rows)
                {
                    var leadId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LeadIdColumnName);
                    var customerId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).CustomerIdColumnName);
                    var lenderId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LenderIdColumnName);
                    var loanDate = DateTime.Parse(_reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LoanDateColumnName));

                    //Is on the exceptions list?
                    if (exception.LeadId == leadId && exception.CustomerId == customerId && exception.CustomerId == customerId
                        && exception.LenderId == lenderId && exception.LoanDate == loanDate)
                    {
                        //Lead is in exceptions - do not add it to output
                    }
                    else
                    {
                        reportDataWithoutExceptions.Rows.Add(reportDataRow);
                    }
                }
            }

            return reportDataWithoutExceptions;
        }

        public List<SqlDataExceptionObject> GetNewDuplicates(ReportData reportData, IEnumerable<SqlDataObject> allData)
        {
            var duplicates = new List<SqlDataExceptionObject>();

            foreach (var data in allData)
            {
                foreach (var reportDataRow in reportData.Rows)
                {
                    var leadId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LeadIdColumnName);
                    var customerId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).CustomerIdColumnName);
                    var lenderId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LenderIdColumnName);
                    var loanDate = DateTime.Parse(_reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LoanDateColumnName));

                    //Is duplicate?
                    if (data.LeadId != leadId || data.CustomerId != customerId || data.CustomerId != customerId ||
                        data.LenderId != lenderId || data.LoanDate != loanDate) continue;

                    var type = _reportsSettings.GetTypeFromQueryId(reportData.QueryId);
                    var leadCreated = DateTime.Parse(_reportDataManager.GetValueForColumn(reportData, reportDataRow, 
                        _reportsSettings.GetReportSettings(reportData.QueryId).LeadCreatedColumnName));
                    var exceptionDesc = $"{data.Id}";
                    duplicates.Add(new SqlDataExceptionObject(type, leadId, customerId, lenderId, loanDate, leadCreated, "DUPLICATE", exceptionDesc));
                }
            }

            return duplicates;
        }
    }
}