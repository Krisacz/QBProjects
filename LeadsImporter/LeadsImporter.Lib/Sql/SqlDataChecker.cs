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
        public ReportData RemoveExceptions(ReportData reportData, List<SqlDataExceptionObject> exceptions)
        {
            try
            {
                //If there are no exceptions - all rows as valid
                if (exceptions.Count == 0)
                {
                    return reportData;
                }
                else
                {
                    var reportDataWithoutExceptions = new ReportData()
                    {
                        QueryId = reportData.QueryId,
                        Headers = reportData.Headers,
                        Rows = new List<ReportDataRow>()
                    };

                    foreach (var reportDataRow in reportData.Rows)
                    {
                        var leadId = _reportDataManager.GetValueForLeadId(reportData, reportDataRow);
                        var customerId = _reportDataManager.GetValueForCustomerId(reportData, reportDataRow);
                        var lenderId = _reportDataManager.GetValueForLenderId(reportData, reportDataRow);
                        var loanDate = _reportDataManager.GetValueForLoanDate(reportData, reportDataRow);
                        var exceptionLead = false;

                        foreach (var exception in exceptions)
                        {
                            //Is on the exceptions list?
                            if (exception.LeadId == leadId && exception.CustomerId == customerId && exception.LenderId == lenderId && exception.LoanDate == loanDate)
                            {
                                //Lead is in exceptions
                                exceptionLead = true;
                                break;
                            }
                        }

                        //Lead not on the exception list
                        if (!exceptionLead) reportDataWithoutExceptions.Rows.Add(reportDataRow);
                    }

                    return reportDataWithoutExceptions;
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlDataChecker >>> RemoveExceptions: {ex.Message}");
            }

            return null;
        }
        #endregion
        
        #region REMOVE EXISTING DATA
        public ReportData RemoveExistingData(ReportData reportData, List<SqlDataObject> allData, List<SqlDataExceptionObject> duplicates)
        {
            try
            {
                //If there are no exceptions - assume all rows as valid
                if (allData.Count == 0)
                {
                    return reportData;
                }
                else
                {
                    var returnReportData = new ReportData()
                    {
                        QueryId = reportData.QueryId,
                        Headers = reportData.Headers,
                        Rows = new List<ReportDataRow>()
                    };

                    foreach (var reportDataRow in reportData.Rows)
                    {
                        var leadId = _reportDataManager.GetValueForLeadId(reportData, reportDataRow);
                        var customerId = _reportDataManager.GetValueForCustomerId(reportData, reportDataRow);
                        var lenderId = _reportDataManager.GetValueForLenderId(reportData, reportDataRow);
                        var loanDate = _reportDataManager.GetValueForLoanDate(reportData, reportDataRow);
                        var processed = false;

                        foreach (var data in allData)
                        {
                            //Already exist?
                            if (data.LeadId == leadId && data.CustomerId == customerId && data.LenderId == lenderId && data.LoanDate == loanDate)
                            {
                                //Lead already exist - ignore it
                                processed = true;
                                break;
                            }

                            //Is Duplicate?
                            if (data.LeadId != leadId && data.CustomerId == customerId && data.LenderId == lenderId && data.LoanDate == loanDate)
                            {
                                var type = _reportDataManager.GetReportType(reportData);
                                var leadCreated = _reportDataManager.GetValueForLeadCreated(reportData, reportDataRow);
                                var exceptionDesc = $"Id[{data.Id}]";
                                duplicates.Add(new SqlDataExceptionObject(type, leadId, customerId, lenderId, loanDate, leadCreated, "DUPLICATE", exceptionDesc));
                                processed = true;
                                break;
                            }
                        }

                        //New row of data
                        if (!processed) returnReportData.Rows.Add(reportDataRow);

                    }
                    return returnReportData;
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlDataChecker >>> RemoveExisitngData: {ex.Message}");
            }
            
            return null;
        }
        #endregion

        #region REMOVE DUPLICATES
        public ReportData RemoveDuplicates(ReportData reportData, List<SqlDataExceptionObject> duplicates)
        {
            try
            {
                var uniqueList = new List<ReportDataRow>();

                foreach (var dataRow in reportData.Rows)
                {
                    var rowDuplicated = false;

                    var customerId = _reportDataManager.GetValueForCustomerId(reportData, dataRow);
                    var lenderId = _reportDataManager.GetValueForLenderId(reportData, dataRow);
                    var loanDate = _reportDataManager.GetValueForLoanDate(reportData, dataRow);

                    foreach (var uniqueRow in uniqueList)
                    {
                        var uniqueCustomerId = _reportDataManager.GetValueForCustomerId(reportData, uniqueRow);
                        var uniqueLenderId = _reportDataManager.GetValueForLenderId(reportData, uniqueRow);
                        var uniqueLoanDate = _reportDataManager.GetValueForLoanDate(reportData, uniqueRow);

                        rowDuplicated = customerId == uniqueCustomerId && lenderId == uniqueLenderId && loanDate == uniqueLoanDate;
                        if (rowDuplicated) break;
                    }

                    if (rowDuplicated)
                    {
                        var type = _reportDataManager.GetReportType(reportData);
                        var leadId = _reportDataManager.GetValueForLeadId(reportData, dataRow);
                        var leadCreated = _reportDataManager.GetValueForLeadCreated(reportData, dataRow);
                        var exceptionDesc = $"LeadId[{leadId}]";
                        duplicates.Add(new SqlDataExceptionObject(type, leadId, customerId, lenderId, loanDate, leadCreated, "DUPLICATE", exceptionDesc));
                    }
                    else
                    {
                        uniqueList.Add(dataRow);
                    }
                }

                return new ReportData()
                {
                    QueryId = reportData.QueryId,
                    Headers = reportData.Headers,
                    Rows = uniqueList
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlDataChecker >>> GetDuplicatesInNewDataSet: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region IN EXCEPTION LIST
        public bool InExceptionsList(ReportData reportData, ReportDataRow reportDataRow, List<SqlDataExceptionObject> exceptions)
        {
            try
            {
                foreach (var exceptionObject in exceptions)
                {
                    var leadId = _reportDataManager.GetValueForLeadId(reportData, reportDataRow);
                    var customerId = _reportDataManager.GetValueForCustomerId(reportData, reportDataRow);
                    var lenderId = _reportDataManager.GetValueForLenderId(reportData, reportDataRow);
                    var loanDate = _reportDataManager.GetValueForLoanDate(reportData, reportDataRow);

                    if (leadId == exceptionObject.LeadId && customerId == exceptionObject.CustomerId
                        && lenderId == exceptionObject.LenderId && loanDate == exceptionObject.LoanDate)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlDataChecker >>> InExceptionsList: {ex.Message}");
            }

            return false;
        }
        #endregion
    }
}