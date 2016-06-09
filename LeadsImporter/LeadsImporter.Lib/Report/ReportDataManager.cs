using System;
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
                        AddHeaderIfNotExist(reportData, joinReportDataHeader);
                        reportData.Rows[(int)existingReportDataRowIndex].Data.Add(joinReportDataValue);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> Join: {ex.Message}");
            }
        }
        #endregion

        #region GET VALUE FOR COLUMN
        public string GetValueForColumn(ReportData reportData, ReportDataRow row, string columnName)
        {
            try
            {
                var columnIndex = GetColumnIndex(reportData, columnName);
                return row.Data[columnIndex];
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> GetValueForColumn: {ex.Message}");
            }

            return null;
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
                _logger.AddError($"ReportDataManager >>> GetExistingReportDataRowIndex: {ex.Message}");
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
                _logger.AddError($"ReportDataManager >>> GetColumnIndex: {ex.Message}");
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

        private void AddHeaderIfNotExist(ReportData reportData, string newHeader)
        {
            try
            {
                if (!reportData.Headers.Contains(newHeader)) reportData.Headers.Add(newHeader);
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportDataManager >>> AddHeaderIfNotExist: {ex.Message}");
            }
        }
        #endregion
    }
}