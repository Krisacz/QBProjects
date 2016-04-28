using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace IMRR.Lib
{
    public class FileWatcher
    {
        private readonly Settings _settings;
        private readonly ILogger _logger;
        private FileDirOperations _fileDirOperations;
        private static readonly Regex EndsWithThreeLetterExtensionRegex = new Regex(@".+\.[^.*]{3}\Z", RegexOptions.Compiled);

        #region CONSTRUCTOR
        public FileWatcher(ILogger logger, Settings settings)
        {
            _settings = settings;
            _logger = logger;
        }
        #endregion

        #region START
        public void Start()
        {
            _logger.AddInfo("========== Service STARTED ==========");

            //Ensure settings are configured
            if (string.IsNullOrEmpty(_settings.IncomingFolderPath))         throw new ApplicationException("IncomingFolderPath must be set before calling Start");
            if (string.IsNullOrEmpty(_settings.ProcessedFolderPath))        throw new ApplicationException("ProcessedFolderPath must be set before calling Start");
            if (string.IsNullOrEmpty(_settings.ToBeCheckedPath))            throw new ApplicationException("ToBeCheckedPath must be set before calling Start");
            if (string.IsNullOrEmpty(_settings.ErrorFolderPath))            throw new ApplicationException("ErrorFolderPath must be set before calling Start");
            if (string.IsNullOrEmpty(_settings.ProclaimProcessFolderPath))  throw new ApplicationException("ProclaimProcessFolder must be set before calling Start");
            if (string.IsNullOrEmpty(_settings.TempFolderPath))             throw new ApplicationException("TempImagesFolder must be set before calling Start");
            if (string.IsNullOrEmpty(_settings.InboundFilesFilter))         throw new ApplicationException("Filter must be set before calling Start");

            //FileOperations
            _fileDirOperations = new FileDirOperations(_logger, _settings);

            //Create Directory if don't exist
            _fileDirOperations.CreateDirIfNotExist(_settings.IncomingFolderPath);
            _fileDirOperations.CreateDirIfNotExist(_settings.ProcessedFolderPath);
            _fileDirOperations.CreateDirIfNotExist(_settings.ToBeCheckedPath);
            _fileDirOperations.CreateDirIfNotExist(_settings.ErrorFolderPath);
            _fileDirOperations.CreateDirIfNotExist(_settings.ProclaimProcessFolderPath);
            _fileDirOperations.CreateDirIfNotExist(_settings.TempFolderPath);
            
            //Create and configure FileWatcher
            var watcher = new FileSystemWatcher
            {
                IncludeSubdirectories = false,
                Path = _settings.IncomingFolderPath,
                NotifyFilter = NotifyFilters.FileName,
                Filter = _settings.InboundFilesFilter,
                EnableRaisingEvents = true,
                InternalBufferSize = 65536
            };

            watcher.Created += OnCreated; 
            watcher.Error += OnError;

            // Process any existing files
            ProcessExistingFiles(_settings.IncomingFolderPath, _settings.InboundFilesFilter);

            //Start listening
            _logger.AddInfo(string.Format("Started monitoring {0} folder, waiting for new {1} files...", _settings.IncomingFolderPath, _settings.InboundFilesFilter));
        }
        #endregion

        #region STOP
        public void Stop()
        {
            _logger.AddInfo("========== Service STOPPED ==========");
        }
        #endregion

        #region ON CREATED & ON ERROR
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            _logger.AddEmptyLine();
            _logger.AddInfo(string.Format("New file found: {0}", e.FullPath));
            _fileDirOperations.WaitReady(e.FullPath);
            ProcessFile(e.FullPath);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            var exception = e.GetException();
            _logger.AddEmptyLine();
            _logger.AddError(exception == null ? "FileWatcher error" : exception.Message);
        }
        #endregion
        
        #region PROCESS EXISTING FILES
        private void ProcessExistingFiles(string directoryPath, string filter)
        {
            foreach (var filePath in EnumerateFiles(directoryPath, filter))
            {
                _logger.AddEmptyLine();
                _logger.AddInfo(string.Format("Existing file found: {0}", filePath));
                ProcessFile(filePath);
            }
        }
        #endregion

        #region PROCESS FILE
        private void ProcessFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            if (fileName == null) throw new ArgumentException("Path must be a file path", "filePath");
            
            try
            {
                OnProcessFile(filePath);
            }
            catch (Exception ex)
            {
                _logger.AddError(ex.ToString());
                _logger.AddInfo("Moving file to error folder");
                try
                {
                    _fileDirOperations.MoveToErrorFolder(filePath);
                }
                catch (Exception ex2)
                {
                    _logger.AddError("Failed to move to error folder");
                    _logger.AddError(ex2.ToString());
                }
                return;
            }

            try
            {
                _fileDirOperations.MoveToProcessedFolder(filePath);
            }
            catch (Exception ex)
            {
                _logger.AddError(ex.ToString());
            }
        }
        #endregion

        #region ENUMERATE FILES
        private static IEnumerable<string> EnumerateFiles(string directoryPath, string filter)
        {
            //http://msdn.microsoft.com/en-us/library/wz42302f(v=vs.100)
            //When using the asterisk wildcard character in a searchPattern, such as "*.txt",
            //the matching behavior when the extension is exactly three characters long is 
            //different than when the extension is more or less than three characters long.
            //A searchPattern with a file extension of exactly three characters returns files
            //having an extension of three or more characters, where the first three characters
            //match the file extension specified in the searchPattern. A searchPattern with a file
            //extension of one, two, or more than three characters returns only files having extensions
            //of exactly that length that match the file extension specified in the searchPattern.
            //When using the question mark wildcard character, this method returns only files that
            //match the specified file extension. For example, given two files, "file1.txt" and "file1.txtother",
            //in a directory, a search pattern of "file?.txt" returns just the first file,
            //while a search pattern of "file*.txt" returns both files.

            var files = Directory.EnumerateFiles(directoryPath, filter);
            // if the filter is for a 3 letter file extension then we have to do post filtering as
            // the .NET method also include any other files with an extension that starts with those 
            // 3 letters!
            if (EndsWithThreeLetterExtensionRegex.Match(filter).Success)
            {
                return
                    (
                        from file in files
                        let extension = Path.GetExtension(file)
                        where extension != null && extension.Length == 4
                        select file
                    ).ToList();
            }
            return files;
        }
        #endregion
        
        #region ON PROCESS FILE
        private void OnProcessFile(string scannedFile)
        {
            //Check if file already doesn't exist in process folder
            /* NOT checking for it - all files will end up in processed folder due to the fact that they will receive unique name
            if (FileDirOperations.FileAlreadyExistInDir(scannedFile, _settings.ProcessedFolderPath))
            {
                throw new Exception(string.Format("File {0} alredy exist in {1}", scannedFile, _settings.ProcessedFolderPath));
            }
            */
            
            //Extracting images
            var imageExtractor = new ImageExtractor(_logger);
            var scannedFileObject = imageExtractor.ExtractImages(scannedFile);
            var imageFilePath = _fileDirOperations.SaveImage(scannedFile, scannedFileObject.ImageOccurrences.First().Image);

            //OCR image
            var ocrReader = new OcrReader(_logger);
            var ocrData = ocrReader.ProcessImage(imageFilePath);
            var ocrDataFile = _fileDirOperations.SaveOcrData(imageFilePath, ocrData);
            
            //Valida OCR data
            var fileRenamer = new OcrDataValidator(_logger);
            var validateType = fileRenamer.Validate(ocrData);
            if (validateType == null) throw new Exception(string.Format("Invalid OCR data in {0}", ocrDataFile));

            //Copy file to Proclaim folder
            _fileDirOperations.CopyAndRenameScannedFile(scannedFile, validateType);
        }
        #endregion
    }
}
