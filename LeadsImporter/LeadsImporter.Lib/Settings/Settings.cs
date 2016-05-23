namespace LeadsImporter.Lib.Settings
{
    public class Settings
    {
        public string SqlConnectionString { get; private set; }
        public string TempCachePath { get; private set; }
        public string PoolingTimeInSec { get; private set; }
        public string LogFilePath { get; private set; }
        public string ReportsSettingsFilePath { get; private set; }
        public string AquariumUsername { get; private set; }
        public string AquariumPassword { get; private set; }

        public Settings(string sqlConnectionString, string tempCachePath, string poolingTimeInSec, string logFilePath, string reportsSettingsFilePath, string aquariumUsername, string aquariumPassword)
        {
            SqlConnectionString = sqlConnectionString;
            TempCachePath = tempCachePath;
            PoolingTimeInSec = poolingTimeInSec;
            LogFilePath = logFilePath;
            ReportsSettingsFilePath = reportsSettingsFilePath;
            AquariumUsername = aquariumUsername;
            AquariumPassword = aquariumPassword;
        }
    }
}
