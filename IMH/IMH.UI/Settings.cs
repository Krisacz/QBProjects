namespace IMH.UI
{
    public class Settings
    {
        public string PdfFilesFolderPath { get; private set; }
        public string SubmittedFilesFolederPath { get; private set; }
        public string UnknownCaseRefFilesPath { get; private set; }
        public string ExceptionFilesPath { get; private set; }
        public string CorrespondentsDataFilesPath { get; private set; }

        public Settings(string pdfFilesFolderPath, string submittedFilesFolederPath, string unknownCaseRefFilesPath, string exceptionFilesPath, string correspondentsDataFilesPath)
        {
            PdfFilesFolderPath = pdfFilesFolderPath;
            SubmittedFilesFolederPath = submittedFilesFolederPath;
            UnknownCaseRefFilesPath = unknownCaseRefFilesPath;
            ExceptionFilesPath = exceptionFilesPath;
            CorrespondentsDataFilesPath = correspondentsDataFilesPath;
        }
    }
}
