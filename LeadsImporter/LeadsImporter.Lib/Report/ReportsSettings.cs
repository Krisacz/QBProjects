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
                CreateIfNotExist();
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

        private void CreateIfNotExist()
        {
            File.WriteAllLines(_settings.ReportsSettingsFilePath, new []{ "Type,ReportId,ExecutionSequence,LeadIdColumnName,ClientIdColumnName,LenderIdColumnName,DateOfCreditColumnName,DateTimeLeadCreatedColumnName,ProclaimDropPath" });
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
    }
}
