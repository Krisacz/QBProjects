using System;
using System.Configuration;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Setting
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

                var poolingTimeInSec = ConfigurationManager.AppSettings["PoolingTimeInSec"];
                logger.AddInfo($"SettingsReader >>> Read: PoolingTimeInSec: {poolingTimeInSec}");
                
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

                logger.AddInfo("SettingsReader >>> Read: Config file valid.");
                return new Settings(sqlConnectionString, poolingTimeInSec, aquariumUsername, aquariumPassword, 
                    aquariumLogonUrl, aquariumLogonAction, aquariumRunReportUrl, aquariumRunReportAction);
            }
            catch (Exception ex)
            {
                logger.AddError($"SettingsReader >>> Read:", ex);
            }

            return null;
        }
    }
}
