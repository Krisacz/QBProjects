using System;
using System.Collections.Generic;
using System.Configuration;

namespace LeadsImporterCrossChecker
{
    public static class SettingsReader
    {
        public static Settings Read(ILogger logger)
        {
            try
            {
                logger.AddInfo("SettingsReader >>> Read: Reading config file...");

                var sqlConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
                logger.AddInfo($"SettingsReader >>> Read: SqlConnectionString: {sqlConnectionString}");
                
                var aquariumUsername = ConfigurationManager.AppSettings["AquariumUsername"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumUsername: {aquariumUsername}");

                var aquariumPassword = ConfigurationManager.AppSettings["AquariumPassword"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumPassword: {aquariumPassword}");

                var aquariumLogonUrl = ConfigurationManager.AppSettings["AquariumLogonUrl"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumLogonUrl: {aquariumLogonUrl}");

                var aquariumLogonAction = ConfigurationManager.AppSettings["AquariumLogonAction"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumLogonAction: {aquariumLogonAction}");

                var aquariumRunReportUrl = ConfigurationManager.AppSettings["AquariumRunReportUrl"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumRunReportUrl: {aquariumRunReportUrl}");

                var aquariumRunReportAction = ConfigurationManager.AppSettings["AquariumRunReportAction"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumRunReportAction: {aquariumRunReportAction}");

                var reportIds = new List<ReportSetting>();
                var exit = false;
                var counter = 1;
                while (!exit)
                {
                    var settingName = $"ReportId{counter}";
                    var reportSetting = ConfigurationManager.AppSettings[settingName];
                    if (reportSetting != null)
                    {
                        reportIds.Add(new ReportSetting(reportSetting));
                        counter++;
                    }
                    else
                    {
                        exit = true;
                    }
                }
                logger.AddInfo($"SettingsReader >>> Read: ReportIds: Found {reportIds.Count} report ids");

                logger.AddInfo("SettingsReader >>> Read: Config file valid.");
                return new Settings(sqlConnectionString, aquariumUsername, aquariumPassword,
                    aquariumLogonUrl, aquariumLogonAction, aquariumRunReportUrl, aquariumRunReportAction, reportIds);
            }
            catch (Exception ex)
            {
                logger.AddError($"SettingsReader >>> Read:", ex);
            }

            return null;
        }
    }
}
