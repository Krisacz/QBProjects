namespace LeadsImporter.Lib.Setting
{
    public class Settings
    {
        public string SqlConnectionString { get; private set; }
        public string PoolingTimeInSec { get; private set; }
        public string AquariumUsername { get; private set; }
        public string AquariumPassword { get; private set; }

        public Settings(string sqlConnectionString, string poolingTimeInSec, string aquariumUsername, string aquariumPassword)
        {
            SqlConnectionString = sqlConnectionString;
            PoolingTimeInSec = poolingTimeInSec;
            AquariumUsername = aquariumUsername;
            AquariumPassword = aquariumPassword;
        }
    }
}
