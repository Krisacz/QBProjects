using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace IMRR.Lib
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
        
        public void MoveToProcessedFolder(string filePath)
        {
            //Making sure that EVERY file can get to processed dir
            var fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var uniqueEnd = Guid.NewGuid().ToString().Substring(0, 4);
            var processedFileName = string.Format("{0}#{1}{2}", fileNameOnly, uniqueEnd, extension);
            var processedFilePath = Path.Combine(_settings.ProcessedFolderPath, processedFileName);
            Move(filePath, processedFilePath);
        }

        public void MoveToErrorFolder(string filePath)
        {
            //Making sure that EVERY file can get to error dir
            var fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var uniqueEnd = Guid.NewGuid().ToString().Substring(0, 4);
            var errorFileName = string.Format("{0}#{1}{2}", fileNameOnly, uniqueEnd, extension);
            var errorFilePath = Path.Combine(_settings.ErrorFolderPath, errorFileName);
            Move(filePath, errorFilePath);
        }

        private void Move(string sourceFileName, string destFileName)
        {
            _logger.AddInfo(string.Format("Moving {0} to {1}", sourceFileName, destFileName));
            WaitReady(sourceFileName);
            File.Move(sourceFileName, destFileName);
        }

        public void CreateDirIfNotExist(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }

        public static bool FileAlreadyExistInDir(string filePath, string dirToCheck)
        {
            var files = Directory.EnumerateFiles(dirToCheck).ToList();
            var fileName = Path.GetFileName(filePath);
            if (fileName == null) return false;
            foreach (var file in files)
            {
                var f = Path.GetFileName(file);
                if (f == null) continue;
                if (string.Equals(f, fileName, StringComparison.CurrentCultureIgnoreCase)) return true;
            }
            return false;
        }

        public string SaveImage(string scannedFile, Image image)
        {
            var fileName = Path.GetFileNameWithoutExtension(scannedFile);
            var uniqueEnd = Guid.NewGuid().ToString().Substring(0, 4);
            var tempImageFileName = string.Format("{0}#{1}{2}", fileName, uniqueEnd, ".jpg");
            var imageFilePath = Path.Combine(_settings.TempFolderPath, tempImageFileName);
            _logger.AddInfo(string.Format("Saving image: {0}", imageFilePath));
            image.Save(imageFilePath);
            return imageFilePath;
        }

        public string SaveOcrData(string imageFilePath, string ocrData)
        {
            var fileName = Path.GetFileNameWithoutExtension(imageFilePath);
            var ocrDataFileName = string.Format("{0}.txt", fileName);
            var ocrDataFilePath = Path.Combine(_settings.TempFolderPath, ocrDataFileName);
            _logger.AddInfo(string.Format("Saving OCR data file: {0}", ocrDataFilePath));
            File.WriteAllText(ocrDataFilePath, ocrData);
            return ocrDataFilePath;
        }

        public void CopyAndRenameScannedFile(string scannedFile, ValidateType validateType)
        {
            var extension = Path.GetExtension(scannedFile);
            var uniqueEnd = Guid.NewGuid().ToString().Substring(0, 4);
            var fileName = string.Format("{0}_#{1}{2}", validateType.Name, uniqueEnd, extension);

            string targetPath = null;
            if (validateType.Level == CorrectLevel.Direct)
            {
                targetPath = _settings.ProclaimProcessFolderPath;
            }
            else if (validateType.Level == CorrectLevel.Generic)
            {
                targetPath = _settings.ProclaimProcessFolderPath;
            }
            else if (validateType.Level == CorrectLevel.Partial)
            {
                targetPath = _settings.ToBeCheckedPath;
            }

            if (targetPath == null) throw new Exception("Target path is null!");

            var filePath = Path.Combine(targetPath, fileName);
            _logger.AddInfo(string.Format("Copying file to: {0}", filePath));
            WaitReady(scannedFile);
            File.Copy(scannedFile, filePath);
        }
        
        public void WaitReady(string fileName)
        {
            while (true)
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
                Thread.Sleep(500);
            }
        }
    }
}
