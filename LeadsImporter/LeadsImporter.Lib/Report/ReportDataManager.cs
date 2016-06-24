using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Report
{
    public class ReportDataManager
    {
        private readonly ILogger _logger;

        private readonly ReportsSettings _reportsSettings;

        public ReportDataManager(ILogger logger, ReportsSettings reportsSettings)
        {
            _logger = logger;
            _reportsSettings = reportsSettings;
        }

        #region JOIN
        public void Join(ReportData reportData, ReportData joinReportData)
        {
            try
            {
                _logger.AddDetailedLog($"ReportDataManager >>> Join: Joining report {joinReportData.QueryId} to {reportData.QueryId}");
                for (var i = 0; i < joinReportData.Rows.Count; i++)
                {
                    var joinReportDataRow = joinReportData.Rows[i];
                    for (var index = 0; index < joinReportDataRow.Data.Count; index++)
                    {
                        var joinReportDataHeader = joinReportData.Headers[index];
                        if (ExcludeColumn(_reportsSettings.GetReportSettings(joinReportData.QueryId), joinReportDataHeader)) continue;
                        var joinReportDataValue = joinReportDataRow.Data[index];
                        var existingReportDataRowIndex = GetExistingReportDataRowIndex(reportData, joinReportData, i);
                        if (existingReportDataRowIndex == null) continue;
                        AddHeader(reportData, joinReportDataHeader);
                        reportData.Rows[(int)existingReportDataRowIndex].Data.Add(joinReportDataValue);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError("ReportDataManager >>> Join:", ex);
            }
        }
        #endregion

        #region GET VALUE
        public string GetValueForLeadId(ReportData reportData, ReportDataRow reportDataRow)
        {
            try
            {
                return GetValueForColumn(reportData, reportDataRow, _reportsSettings.GetReportSettings(reportData.QueryId).LeadIdColumnName);
            }
            catch (Exception ex)
            {
                _logger.AddError("ReportDataManager >>> GetValueForLeadId", ex);
            }

            return null;
        }

        public string GetValueForCustomerId(ReportData reportData, ReportDataRow reportDataRow)
        {
            try
            {
                return GetValueForColumn(reportData, reportDataRow, _reportsSettings.GetReportSettings(reportData.QueryId).CustomerIdColumnName);
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetValueForCustomerId:", ex);
            }

            return null;
        }

        public string GetValueForLenderId(ReportData reportData, ReportDataRow reportDataRow)
        {
            try
            {
                return GetValueForColumn(reportData, reportDataRow, _reportsSettings.GetReportSettings(reportData.QueryId).LenderIdColumnName);
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetValueForLenderId:", ex);
            }

            return null;
        }

        public DateTime GetValueForLoanDate(ReportData reportData, ReportDataRow reportDataRow)
        {
            try
            {
                return DateTime.Parse(GetValueForColumn(reportData, reportDataRow, _reportsSettings.GetReportSettings(reportData.QueryId).LoanDateColumnName));
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetValueForLoanDate:", ex);
            }

            return DateTime.MinValue;
        }

        public DateTime GetValueForLeadCreated(ReportData reportData, ReportDataRow reportDataRow)
        {
            try
            {
                return DateTime.Parse(GetValueForColumn(reportData, reportDataRow, _reportsSettings.GetReportSettings(reportData.QueryId).LeadCreatedColumnName));
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetValueForLeadCreated:", ex);
            }

            return DateTime.MinValue;
        }

        private string GetValueForColumn(ReportData reportData, ReportDataRow row, string columnName)
        {
            try
            {
                var columnIndex = GetColumnIndex(reportData, columnName);
                return row.Data[columnIndex];
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetValueForColumn:", ex);
            }

            return null;
        }
        #endregion

        #region GET REPORT TYPE
        public string GetReportType(ReportData reportData)
        {
            try
            {
                return _reportsSettings.GetReportType(reportData.QueryId);
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetReportType:", ex);
            }

            return null;
        }
        #endregion

        #region GET REPORT TYPES
        public IEnumerable<string> GetReportTypes()
        {
            return _reportsSettings.GetReportTypes();
        }
        #endregion

        #region GET SEQUENCES COUNT FOR TYPE
        public int? GetSequencesCountForType(string type)
        {
            return _reportsSettings.GetSequencesCountForType(type);
        }
        #endregion

        #region GET REPORT QUERY ID
        public int GetReportQueryId(string type, int sequence)
        {
            return _reportsSettings.GetReportSettings(type, sequence).QueryId;
        }
        #endregion

        #region GET OUTPUT PATH
        public string GetOutputPath(ReportData reportData)
        {
            return _reportsSettings.GetReportSettings(reportData.QueryId).OutputPath;
        }
        #endregion

        #region GET OUTPUT PATH
        public string GetExceptionsPath(ReportDataExceptions reportData)
        {
            return _reportsSettings.GetReportSettings(reportData.QueryId).ExceptionsPath;
        }
        #endregion

        #region HELP METHODS
        private int? GetExistingReportDataRowIndex(ReportData existing, ReportData join, int joinRowIndex)
        {
            try
            {
                var existingLeadIdColumnIndex = GetColumnIndex(existing, _reportsSettings.GetReportSettings(existing.QueryId).LeadIdColumnName);
                var existingCustomerIdColumnIndex = GetColumnIndex(existing, _reportsSettings.GetReportSettings(existing.QueryId).CustomerIdColumnName);
                var existingLenderIdColumnIndex = GetColumnIndex(existing, _reportsSettings.GetReportSettings(existing.QueryId).LenderIdColumnName);
                var existingLoanDateColumnIndex = GetColumnIndex(existing, _reportsSettings.GetReportSettings(existing.QueryId).LoanDateColumnName);
                var existingLeadCreatedColumnIndex = GetColumnIndex(existing, _reportsSettings.GetReportSettings(existing.QueryId).LeadCreatedColumnName);

                var joinLeadIdColumnIndex = GetColumnIndex(join, _reportsSettings.GetReportSettings(join.QueryId).LeadIdColumnName);
                var joinCustomerIdColumnIndex = GetColumnIndex(join, _reportsSettings.GetReportSettings(join.QueryId).CustomerIdColumnName);
                var joinLenderIdColumnIndex = GetColumnIndex(join, _reportsSettings.GetReportSettings(join.QueryId).LenderIdColumnName);
                var joinLoanDateColumnIndex = GetColumnIndex(join, _reportsSettings.GetReportSettings(join.QueryId).LoanDateColumnName);
                var joinLeadCreatedColumnIndex = GetColumnIndex(join, _reportsSettings.GetReportSettings(join.QueryId).LeadCreatedColumnName);

                for (var i = 0; i < existing.Rows.Count; i++)
                {
                    var existingDataRow = existing.Rows[i];
                    var leadIdCheck = existingDataRow.Data[existingLeadIdColumnIndex] == join.Rows[joinRowIndex].Data[joinLeadIdColumnIndex];
                    var customerIdCheck = existingDataRow.Data[existingCustomerIdColumnIndex] == join.Rows[joinRowIndex].Data[joinCustomerIdColumnIndex];
                    var lenderIdCheck = existingDataRow.Data[existingLenderIdColumnIndex] == join.Rows[joinRowIndex].Data[joinLenderIdColumnIndex];
                    var loanDateCheck = existingDataRow.Data[existingLoanDateColumnIndex] == join.Rows[joinRowIndex].Data[joinLoanDateColumnIndex];
                    var leadCreatedCheck = existingDataRow.Data[existingLeadCreatedColumnIndex] == join.Rows[joinRowIndex].Data[joinLeadCreatedColumnIndex];

                    if (leadIdCheck && customerIdCheck && lenderIdCheck && loanDateCheck && leadCreatedCheck) return i;
                }

                throw new Exception($"First report in sequence is missing row of data;" +
                                    $"Lead Id: {join.Rows[joinRowIndex].Data[joinLeadIdColumnIndex]}, " +
                                    $"Customer Id: {join.Rows[joinRowIndex].Data[joinCustomerIdColumnIndex]}, " +
                                    $"Lender Id: {join.Rows[joinRowIndex].Data[joinLenderIdColumnIndex]}, " +
                                    $"Loan Date: {join.Rows[joinRowIndex].Data[joinLoanDateColumnIndex]}, " +
                                    $"Lead Created: {join.Rows[joinRowIndex].Data[joinLeadCreatedColumnIndex]}.");
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetExistingReportDataRowIndex:", ex);
            }

            return null;
        }

        private int GetColumnIndex(ReportData reportData, string headerName)
        {
            try
            {
                for (var i = 0; i < reportData.Headers.Count; i++)
                {
                    if (reportData.Headers[i] == headerName) return i;
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetColumnIndex:", ex);
            }
            
            return -1;
        }

        private static bool ExcludeColumn(ReportSettings reportSettings, string header)
        {
            return reportSettings.LeadIdColumnName == header
                   || reportSettings.CustomerIdColumnName == header
                   || reportSettings.LenderIdColumnName == header
                   || reportSettings.LoanDateColumnName == header
                   || reportSettings.LeadCreatedColumnName == header;
        }

        private void AddHeader(ReportData reportData, string newHeader)
        {
            try
            {
                if (!reportData.Headers.Contains(newHeader)) reportData.Headers.Add(newHeader);
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> AddHeaderIfNotExist:", ex);
            }
        }
        #endregion
    }
}