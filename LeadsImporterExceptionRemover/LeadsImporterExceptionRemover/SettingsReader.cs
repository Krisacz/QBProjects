using System;
using System.Configuration;

namespace LeadsImporterExceptionRemover
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

                return new Settings(sqlConnectionString);
            }
            catch (Exception ex)
            {
                logger.AddError($"SettingsReader >>> Read:", ex);
            }

            return null;
        }
    }
}
