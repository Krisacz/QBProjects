namespace IMRR.Lib
{
    public class Settings
    {
        public string IncomingFolderPath { get; private set; }
        public string ProcessedFolderPath { get; private set; }
        public string ToBeCheckedPath { get; private set; }
        public string ErrorFolderPath { get; private set; }
        public string ProclaimProcessFolderPath { get; private set; }
        public string TempFolderPath { get; private set; }
        public string InboundFilesFilter { get; private set; }
        

        public Settings(string incomingFolderPath, string processedFolderPath, string toBeCheckedPath, string errorFolderPath, string proclaimProcessFolderPath, string tempFolderPath, string inboundFilesFilter)
        {
            IncomingFolderPath = incomingFolderPath;
            ProcessedFolderPath = processedFolderPath;
            ToBeCheckedPath = toBeCheckedPath;
            ErrorFolderPath = errorFolderPath;
            ProclaimProcessFolderPath = proclaimProcessFolderPath;
            TempFolderPath = tempFolderPath;
            InboundFilesFilter = inboundFilesFilter;
        }
    }
}
