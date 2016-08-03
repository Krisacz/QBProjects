namespace LeadsImporterExceptionRemover
{
    public class Settings
    {
        public string SqlConnectionString { get; private set; }
        
        public Settings(string sqlConnectionString)
        {
            SqlConnectionString = sqlConnectionString;
        }
    }
}
