using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Sql
{
    public class SqlDataChecker
    {
        private readonly ReportDataManager _reportDataManager;
        private readonly ILogger _logger;

        public SqlDataChecker(ReportDataManager reportDataManager, ILogger logger)
        {
            _reportDataManager = reportDataManager;
            _logger = logger;
        }

        #region REMOVE EXCEPTIONS
        public ReportData RemoveExceptions(ReportData reportData, IEnumerable<SqlDataExceptionObject> exceptions)
        {
            try
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
                        var leadId = _reportDataManager.GetValueForLeadId(reportData, reportDataRow);
                        var customerId = _reportDataManager.GetValueForCustomerId(reportData, reportDataRow);
                        var lenderId = _reportDataManager.GetValueForLenderId(reportData, reportDataRow);
                        var loanDate = _reportDataManager.GetValueForLoanDate(reportData, reportDataRow);

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
            catch (Exception ex)
            {
                _logger.AddError($"SqlDataChecker >>> RemoveExceptions: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region GET NEW DUPLICATES
        public IEnumerable<SqlDataExceptionObject> GetNewDuplicates(ReportData reportData, IEnumerable<SqlDataObject> allData)
        {
            try
            {
                var duplicates = new List<SqlDataExceptionObject>();

                foreach (var data in allData)
                {
                    foreach (var reportDataRow in reportData.Rows)
                    {
                        var leadId = _reportDataManager.GetValueForLeadId(reportData, reportDataRow);
                        var customerId = _reportDataManager.GetValueForCustomerId(reportData, reportDataRow);
                        var lenderId = _reportDataManager.GetValueForLenderId(reportData, reportDataRow);
                        var loanDate = _reportDataManager.GetValueForLoanDate(reportData, reportDataRow);

                        //Is duplicate?
                        if (data.LeadId != leadId || data.CustomerId != customerId || data.CustomerId != customerId ||
                            data.LenderId != lenderId || data.LoanDate != loanDate) continue;

                        var type = _reportDataManager.GetReportType(reportData);
                        var leadCreated = _reportDataManager.GetValueForLeadCreated(reportData, reportDataRow);
                        var exceptionDesc = $"{data.Id}";
                        duplicates.Add(new SqlDataExceptionObject(type, leadId, customerId, lenderId, loanDate, leadCreated, "DUPLICATE", exceptionDesc));
                    }
                }

                return duplicates;
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlDataChecker >>> GetNewDuplicates: {ex.Message}");
            }

            return null;
        }
        #endregion
    }
}