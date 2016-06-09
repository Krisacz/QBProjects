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

                var incomingFolderPath = ConfigurationManager.AppSettings["SqlConnectionString"];
                logger.AddInfo($"SettingsReader >>> Read: SqlConnectionString: {incomingFolderPath}");

                var toBeCheckedPath = ConfigurationManager.AppSettings["PoolingTimeInSec"];
                logger.AddInfo($"SettingsReader >>> Read: PoolingTimeInSec: {toBeCheckedPath}");
                
                var aquariumUsername = ConfigurationManager.AppSettings["AquariumUsername"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumUsername: {aquariumUsername}");

                var aquariumPassword = ConfigurationManager.AppSettings["AquariumPassword"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumPassword: {aquariumPassword}");

                logger.AddInfo("SettingsReader >>> Read: Config file valid.");
                return new Settings(incomingFolderPath, toBeCheckedPath, aquariumUsername, aquariumPassword);
            }
            catch (Exception ex)
            {
                logger.AddError($"SettingsReader >>> Read: {ex.Message}");
            }

            return null;
        }
    }
}
