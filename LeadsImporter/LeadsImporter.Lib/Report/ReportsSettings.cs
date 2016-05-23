using System;
using System.Collections.Generic;
using System.IO;
using LeadsImporter.Lib.Log;
using static System.Int32;

namespace LeadsImporter.Lib.Report
{
    public class ReportsSettings
    {
        private readonly List<ReportSettings> _all;
        private readonly Settings.Settings _settings;
        private readonly ILogger _logger;

        public ReportsSettings(Settings.Settings settings, ILogger logger)
        {
            _all = new List<ReportSettings>();
            _settings = settings;
            _logger = logger;
        }

        public ReportsSettings ReadAll()
        {
            try
            {
                _logger.AddInfo("ReportsSettings >>> ReadAll: Reading reports settings file...");
                var lines = File.ReadAllLines(_settings.ReportsSettingsFilePath);
                for (var i = 0; i < lines.Length; i++)
                {
                    _logger.AddInfo($"ReportsSettings >>> ReadAll: Reading {i} line...");
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue; //skip empty lines
                    if (i == 0) continue; //skip header
                    var reportSettings = MapReportSettings(line, i);
                    _all.Add(reportSettings);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> ReadAll: {ex.Message}");
            }

            return this;
        }

        private ReportSettings MapReportSettings(string line, int i)
        {
            try
            {
                _logger.AddInfo($"ReportsSettings >>> MapReportSettings: Mapping {i} line...");

                var parts = line.Split(',');

                var type = parts[0];
                var aquariumQueryId = parts[1];
                var executionSequnece = parts[2];
                var leadIdIndex = Parse(parts[3]);
                var clientIdIndex = Parse(parts[4]);
                var lenderIdIndex = Parse(parts[5]);
                var dateOfCreditIndex = Parse(parts[6]);
                var loanAmountIndex = Parse(parts[7]);
                var dateTimeLeadCreateIndex = Parse(parts[8]);
                var proclaimDropPath = parts[9];

                return new ReportSettings()
                {
                    Type = type,
                    AquariumQueryId = aquariumQueryId,
                    ExecutionSequnece = executionSequnece,
                    LeadIdIndex = leadIdIndex,
                    ClientIdIndex = clientIdIndex,
                    LenderIdIndex = lenderIdIndex,
                    DateOfCreditIndex = dateOfCreditIndex,
                    LoanAmountIndex = loanAmountIndex,
                    DateTimeLeadCreateIndex = dateTimeLeadCreateIndex,
                    ProclaimDropPath = proclaimDropPath
                };
            }
            catch (Exception ex)
            {
                _logger.AddError($"ReportsSettings >>> MapReportSettings [Line {i}]: {ex.Message}");
            }

            return null;
        }
    }
}
