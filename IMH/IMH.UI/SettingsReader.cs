using System;
using System.Configuration;

namespace IMH.UI
{
    public static class SettingsReader
    {
        public static Settings Read(ILogger logger)
        {
            try
            {
                logger.AddInfo("Reading config file...");

                var pdfFilesFolderPath = ConfigurationManager.AppSettings["PdfFilesFolderPath"];
                logger.AddInfo(string.Format("PdfFilesFolderPath: {0}", pdfFilesFolderPath));

                var submittedFilesFolederPath = ConfigurationManager.AppSettings["SubmittedFilesFolederPath"];
                logger.AddInfo(string.Format("SubmittedFilesFolederPath: {0}", submittedFilesFolederPath));

                var unknownCaseRefFilesPath = ConfigurationManager.AppSettings["UnknownCaseRefFilesPath"];
                logger.AddInfo(string.Format("UnknownCaseRefFilesPath: {0}", unknownCaseRefFilesPath));

                var exceptionFilesPath = ConfigurationManager.AppSettings["ExceptionFilesPath"];
                logger.AddInfo(string.Format("ExceptionFilesPath: {0}", exceptionFilesPath));

                var correspondentsDataFilesPath = ConfigurationManager.AppSettings["CorrespondentsDataFilesPath"];
                logger.AddInfo(string.Format("CorrespondentsDataFilesPath: {0}", correspondentsDataFilesPath));

                logger.AddInfo("Config file valid.");
                return new Settings(pdfFilesFolderPath, submittedFilesFolederPath, unknownCaseRefFilesPath, exceptionFilesPath, correspondentsDataFilesPath);
            }
            catch (Exception ex)
            {
                logger.AddError(ex.Message);
            }

            return null;
        }
    }
}
