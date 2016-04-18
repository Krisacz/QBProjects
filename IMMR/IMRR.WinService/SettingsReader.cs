using System;
using System.Configuration;

namespace IMRR.Lib
{
    public class SettingsReader
    {
        public static Settings Read(ILogger logger)
        {
            try
            {
                logger.AddInfo("Reading config file...");

                var incomingFolderPath = ConfigurationManager.AppSettings["IncomingFolderPath"];
                logger.AddInfo(string.Format("IncomingFolderPath: {0}", incomingFolderPath));

                var processedFolderPath = ConfigurationManager.AppSettings["ProcessedFolderPath"];
                logger.AddInfo(string.Format("ProcessedFolderPath: {0}", processedFolderPath));

                var toBeCheckedPath = ConfigurationManager.AppSettings["ToBeCheckedPath"];
                logger.AddInfo(string.Format("ToBeCheckedPath: {0}", toBeCheckedPath));

                var errorFolderPath = ConfigurationManager.AppSettings["ErrorFolderPath"];
                logger.AddInfo(string.Format("ErrorFolderPath: {0}", errorFolderPath));

                var proclaimProcessFolderPath = ConfigurationManager.AppSettings["ProclaimProcessFolderPath"];
                logger.AddInfo(string.Format("ProclaimProcessFolderPath: {0}", proclaimProcessFolderPath));

                var tempFolderPath = ConfigurationManager.AppSettings["TempFolderPath"];
                logger.AddInfo(string.Format("TempFolderPath: {0}", tempFolderPath));

                var inboundFilesFilter = ConfigurationManager.AppSettings["InboundFilesFilter"];
                logger.AddInfo(string.Format("InboundFilesFilter: {0}", inboundFilesFilter));
                
                logger.AddInfo("Config file valid.");
                return new Settings(incomingFolderPath, processedFolderPath, toBeCheckedPath, errorFolderPath, proclaimProcessFolderPath, tempFolderPath, inboundFilesFilter);
            }
            catch (Exception ex)
            {
                logger.AddError(ex.Message);
            }

            return null;
        }
    }
}
