using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeadsImporter.Lib.Log;
using static System.Int32;

namespace LeadsImporter.Lib.Report
{
    public class ReportsSettings
    {
        private readonly ILogger _logger;
        private readonly List<ReportSettings> _all;
        private readonly string _reportsSettingsPath = "ReportsSettings.txt";

        public ReportsSettings(ILogger logger)
        {
            _all = new List<ReportSettings>();
            _logger = logger;
        }

        #region READ ALL
        public ReportsSettings ReadAll()
        {
            try
            {
                _logger.AddInfo("ReportsSettings >>> Read: Reading reports settings file...");
                CreateIfNotExist();
                var lines = File.ReadAllLines(_reportsSettingsPath);
                for (var i = 0; i < lines.Length; i++)
                {
                    _logger.AddInfo($"ReportsSettings >>> ReadAll: Reading line {i}...");
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue; //skip empty lines
                    if (i == 0) continue; //skip header
                    var reportSettings = MapReportSettings(line, i);
                    _all.Add(reportSettings);
                }
                _logger.AddInfo($"ReportsSettings >>> ReadAll: Found {_all.Count} report settings line(s).");
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> ReadAll: {ex.Message}");
            }

            return this;
        }
        #endregion

        #region GET TYPES
        public IEnumerable<string> GetReportTypes()
        {
            try
            {
                _logger.AddInfo("ReportsSettings >>> GetReportTypes: Getting all report types...");
                return _all.GroupBy(p => p.Type).Select(g => g.First().Type).ToList();
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> GetTypes: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region GET SEQUENCES COUNT FOR TYPE
        public int? GetSequencesCountForType(string type)
        {
            try
            {
                _logger.AddInfo("ReportsSettings >>> GetSequencesCountForType: Getting sequences count per type...");
                return _all.Count(reportSettings => reportSettings.Type == type);
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> GetSequencesPerType: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region GET REPORTS SETTINGS
        public ReportSettings GetReportSettings(string type, int sequence)
        {
            try
            {
                _logger.AddInfo("ReportsSettings >>> GetReportSettings: Getting report settings for type/sequence...");
                return _all.First(reportSettings => reportSettings.Type == type && reportSettings.ExecutionSequnece == sequence);
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> GetReportSettings: {ex.Message}");
            }

            return null;
        }
        
        public ReportSettings GetReportSettings(int queryId)
        {
            try
            {
                _logger.AddInfo("ReportsSettings >>> GetReportSettings: Getting report settings for query id...");
                return _all.First(reportSettings => reportSettings.QueryId == queryId);
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> GetReportSettings: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region GET TYPE FROM QUERY ID
        public string GetReportType(int reportId)
        {
            try
            {
                return _all.First(x => x.QueryId == reportId).Type;
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> GetTypeFromQueryId: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region HELP METHODS
        private void CreateIfNotExist()
        {
            try
            {
                if (File.Exists(_reportsSettingsPath)) return;
                _logger.AddInfo($"ReportsSettings >>> CreateIfNotExist: {_reportsSettingsPath} does't exist - creating new file...");
                File.WriteAllLines(_reportsSettingsPath,
                    new[] { "Type,ReportId,ExecutionSequence,LeadIdColumnName,CustomerIdColumnName,LenderIdColumnName,LoanDateColumnName,LeadCreatedColumnName,OutputPath" });
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> CreateIfNotExist: {ex.Message}");
            }
        }

        private ReportSettings MapReportSettings(string line, int i)
        {
            try
            {
                _logger.AddInfo($"ReportsSettings >>> MapReportSettings: Mapping line {i}...");

                var parts = line.Split(',');

                var type = parts[0];
                var aquariumQueryId = parts[1];
                var executionSequnece = parts[2];
                var leadIdColumnName = parts[3];
                var clientIdColumnName = parts[4];
                var lenderIdColumnName = parts[5];
                var dateOfCreditColumnName = parts[6];
                var dateTimeLeadCreatedColumnName =parts[7];
                var proclaimDropPath = parts[8];

                return new ReportSettings()
                {
                    Type = type,
                    QueryId = Parse(aquariumQueryId),
                    ExecutionSequnece = Parse(executionSequnece),
                    LeadIdColumnName = leadIdColumnName,
                    CustomerIdColumnName = clientIdColumnName,
                    LenderIdColumnName = lenderIdColumnName,
                    LoanDateColumnName = dateOfCreditColumnName,
                    LeadCreatedColumnName = dateTimeLeadCreatedColumnName,
                    OutputPath = proclaimDropPath
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> MapReportSettings [Line {i}]: {ex.Message}");
            }

            return null;
        }
        #endregion
    }
}
