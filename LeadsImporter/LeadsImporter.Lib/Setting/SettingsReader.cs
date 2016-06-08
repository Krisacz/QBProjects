using System;
using System.Configuration;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Setting
{
    public class SettingsReader
    {
        public static Settings Read(ILogger logger)
        {
            try
            {
                logger.AddInfo("SettingsReader >>> Read: Reading config file...");

                var incomingFolderPath = ConfigurationManager.AppSettings["SqlConnectionString"];
                logger.AddInfo($"SettingsReader >>> Read: SqlConnectionString: {incomingFolderPath}");

                var processedFolderPath = ConfigurationManager.AppSettings["TempCachePath"];
                logger.AddInfo($"SettingsReader >>> Read: TempCachePath: {processedFolderPath}");

                var toBeCheckedPath = ConfigurationManager.AppSettings["PoolingTimeInSec"];
                logger.AddInfo($"SettingsReader >>> Read: PoolingTimeInSec: {toBeCheckedPath}");
                
                var proclaimProcessFolderPath = ConfigurationManager.AppSettings["ReportsSettingsFilePath"];
                logger.AddInfo($"SettingsReader >>> Read: ReportsSettingsFilePath: {proclaimProcessFolderPath}");

                var aquariumUsername = ConfigurationManager.AppSettings["AquariumUsername"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumUsername: {aquariumUsername}");

                var aquariumPassword = ConfigurationManager.AppSettings["AquariumPassword"];
                logger.AddInfo($"SettingsReader >>> Read: AquariumPassword: {aquariumPassword}");

                var validationFilesPath = ConfigurationManager.AppSettings["ValidationFilesPath"];
                logger.AddInfo($"SettingsReader >>> Read: ValidationFilesPath: {validationFilesPath}");

                logger.AddInfo("SettingsReader >>> Read: Config file valid.");
                return new Settings(incomingFolderPath, processedFolderPath, toBeCheckedPath, proclaimProcessFolderPath, aquariumUsername, aquariumPassword, validationFilesPath);
            }
            catch (Exception ex)
            {
                logger.AddError($"SettingsReader >>> Read: {ex.Message}");
            }

            return null;
        }
    }
}
