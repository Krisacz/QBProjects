using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace IMH.UI
{
    internal class FileDirOperations
    {
        private readonly ILogger _logger;
        private readonly Settings _settings;

        public FileDirOperations(ILogger logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }
        
        public void MoveToSubmittedFolder(string filePath, string caseRef)
        {
            //Making sure that EVERY file can get to processed dir
            var extension = Path.GetExtension(filePath);
            var uniqueEnd = Guid.NewGuid().ToString().Substring(0, 4);
            var submittedFileName = string.Format("{0}{1}_#{2}{3}", "CL", caseRef, uniqueEnd, extension);
            var subbmitedFilePath = Path.Combine(_settings.SubmittedFilesFolederPath, submittedFileName);
            Move(filePath, subbmitedFilePath);
        }

        public void MoveToUnknownCaseRefFolder(string filePath)
        {
            //Making sure that EVERY file can get to error dir
            var fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var uniqueEnd = Guid.NewGuid().ToString().Substring(0, 4);
            var unknownFileName = string.Format("{0}#{1}{2}", fileNameOnly, uniqueEnd, extension);
            var unknownFilePath = Path.Combine(_settings.UnknownCaseRefFilesPath, unknownFileName);
            Move(filePath, unknownFilePath);
        }

        public void CopyException(string filePath)
        {
            //Making sure that EVERY file can get to error dir
            var fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var uniqueEnd = Guid.NewGuid().ToString().Substring(0, 4);
            var exceptionFileName = string.Format("{0}#{1}{2}", fileNameOnly, uniqueEnd, extension);
            var exceptionFilePath = Path.Combine(_settings.ExceptionFilesPath, exceptionFileName);
            _logger.AddInfo(string.Format("Copying exception file {0} to {1}", filePath, exceptionFilePath));
            File.Copy(filePath, exceptionFilePath);
        }

        private void Move(string sourceFileName, string destFileName)
        {
            _logger.AddInfo(string.Format("Moving {0} to {1}", sourceFileName, destFileName));
            WaitReady(sourceFileName);
            File.Move(sourceFileName, destFileName);
        }

        public void CreateDirsIfNotExist()
        {
            CreateDirIfNotExist(_settings.SubmittedFilesFolederPath);
            CreateDirIfNotExist(_settings.UnknownCaseRefFilesPath);
            CreateDirIfNotExist(_settings.ExceptionFilesPath);
        }

        private static void CreateDirIfNotExist(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }

        public void WaitReady(string fileName)
        {
            var tries = 0;
            var maxTries = 10;

            while (true && tries < maxTries)
            {
                try
                {
                    using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        if (stream != null)
                        {
                            _logger.AddInfo(string.Format("File {0} ready for processing.", fileName));
                            break;
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("File {0} not yet ready ({1})", fileName, ex.Message));
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("File {0} not yet ready ({1})", fileName, ex.Message));
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("File {0} not yet ready ({1})", fileName, ex.Message));
                }
                tries++;
                Thread.Sleep(500);
            }
        }
    }
}
