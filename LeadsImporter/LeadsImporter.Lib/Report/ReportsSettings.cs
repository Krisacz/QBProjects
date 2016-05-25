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
        private readonly Settings.Settings _settings;
        private readonly List<ReportSettings> _all;

        public ReportsSettings(ILogger logger, Settings.Settings settings)
        {
            _all = new List<ReportSettings>();
            _logger = logger;
            _settings = settings;
        }

        public ReportsSettings ReadAll()
        {
            try
            {
                _logger.AddInfo("ReportsSettings >>> ReadAll: Reading reports settings file...");
                CreateIfNotExist();
                var lines = File.ReadAllLines(_settings.ReportsSettingsFilePath);
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

        private void CreateIfNotExist()
        {
            try
            {
                if (File.Exists(_settings.ReportsSettingsFilePath)) return;
                _logger.AddInfo($"ReportsSettings >>> CreateIfNotExist: {_settings.ReportsSettingsFilePath} does't exist - creating new file...");
                File.WriteAllLines(_settings.ReportsSettingsFilePath,
                    new[] { "Type,ReportId,ExecutionSequence,LeadIdColumnName,ClientIdColumnName,LenderIdColumnName,DateOfCreditColumnName,DateTimeLeadCreatedColumnName,ProclaimDropPath" });

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
                    AquariumQueryId = aquariumQueryId,
                    ExecutionSequnece = Parse(executionSequnece),
                    LeadIdColumnName = leadIdColumnName,
                    ClientIdColumnName = clientIdColumnName,
                    LenderIdColumnName = lenderIdColumnName,
                    DateOfCreditColumnName = dateOfCreditColumnName,
                    DateTimeLeadCreatedColumnName = dateTimeLeadCreatedColumnName,
                    ProclaimDropPath = proclaimDropPath
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> MapReportSettings [Line {i}]: {ex.Message}");
            }

            return null;
        }

        public List<string> GetReportsId()
        {
            return _all.Select(x => x.AquariumQueryId).ToList();
        }
    }
}
