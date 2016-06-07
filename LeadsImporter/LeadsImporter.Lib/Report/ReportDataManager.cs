using System;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Report
{
    public class ReportDataManager
    {
        private readonly ILogger _logger;

        public ReportDataManager(ILogger logger)
        {
            _logger = logger;
        }

        private int? GetExistingReportDataRowIndex(ReportData existing, ReportData join, int joinRowIndex)
        {
            try
            {
                var existingLeadIdColumnIndex = GetColumnIndex(existing, existing.Settings.LeadIdColumnName);
                var existingCustomerIdColumnIndex = GetColumnIndex(existing, existing.Settings.CustomerIdColumnName);
                var existingLenderIdColumnIndex = GetColumnIndex(existing, existing.Settings.LenderIdColumnName);
                var existingLoanDateColumnIndex = GetColumnIndex(existing, existing.Settings.LoanDateColumnName);
                var existingLeadCreatedColumnIndex = GetColumnIndex(existing, existing.Settings.LeadCreatedColumnName);

                var joinLeadIdColumnIndex = GetColumnIndex(join, join.Settings.LeadIdColumnName);
                var joinCustomerIdColumnIndex = GetColumnIndex(join, join.Settings.CustomerIdColumnName);
                var joinLenderIdColumnIndex = GetColumnIndex(join, join.Settings.LenderIdColumnName);
                var joinLoanDateColumnIndex = GetColumnIndex(join, join.Settings.LoanDateColumnName);
                var joinLeadCreatedColumnIndex = GetColumnIndex(join, join.Settings.LeadCreatedColumnName);

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
                        if (ExcludeColumn(joinReportData.Settings, joinReportDataHeader)) continue;
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

        private static void AddHeaderIfNotExist(ReportData reportData, string newHeader)
        {
            if(!reportData.Headers.Contains(newHeader)) reportData.Headers.Add(newHeader);
        }
    }
}