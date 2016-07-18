namespace LeadsImporter.Lib.Setting
{
    public class Settings
    {
        public string SqlConnectionString { get; private set; }
        public string PoolingTimeInSec { get; private set; }
        public string AquariumUsername { get; private set; }
        public string AquariumPassword { get; private set; }
        public string AquariumLogonUrl { get; private set; }
        public string AquariumLogonAction { get; private set; }
        public string AquariumRunReportUrl { get; private set; }
        public string AquariumRunReportAction { get; private set; }
        public bool SupressSqlUpdates { get; private set; }
        public bool DetailedLog { get; private set; }

        public Settings(string sqlConnectionString, string poolingTimeInSec, string aquariumUsername, string aquariumPassword, string aquariumLogonUrl, 
            string aquariumLogonAction, string aquariumRunReportUrl, string aquariumRunReportAction, bool supressSqlUpdates, bool detailedLog)
        {
            SqlConnectionString = sqlConnectionString;
            PoolingTimeInSec = poolingTimeInSec;
            AquariumUsername = aquariumUsername;
            AquariumPassword = aquariumPassword;
            AquariumLogonUrl = aquariumLogonUrl;
            AquariumLogonAction = aquariumLogonAction;
            AquariumRunReportUrl = aquariumRunReportUrl;
            AquariumRunReportAction = aquariumRunReportAction;
            SupressSqlUpdates = supressSqlUpdates;
            DetailedLog = detailedLog;
        }
    }
}
