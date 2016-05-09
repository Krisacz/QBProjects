using System;
using System.Collections.Generic;
using System.IO;
using IMRR.Lib;

namespace IMRR.ConsoleTester
{
    public class ExceptionsReader
    {
        public static Exceptions Read(FileLogger logger)
        {
            try
            {
                logger.AddInfo("Reading exceptions file...");

                if(!File.Exists("exceptions.txt"))
                File.ReadAllLines("exceptions.txt");

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

    public class Exceptions
    {
        private List<string> _exceptions;

        public Exceptions(List<string> exceptions)
        {
            _exceptions = exceptions;
        }

        public bool IsException(string str)
        {

        }
    }
}