using System;
using System.IO;
using System.Text.RegularExpressions;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Validation
{
    public class CharactersValidator
    {
        private readonly ILogger _logger;
        private readonly string _illegalCharactersFilePath = "IllegalCharacters.txt";
        private string _characters;
        private string _defaultIllegalCharacters = ",£";

        public CharactersValidator(ILogger logger)
        {
            _logger = logger;
        }

        #region READ ALL VALIDATION FILES
        public CharactersValidator Read()
        {
            try
            {
                _logger.AddInfo("CharactersValidator >>> Read: Reading illegal characters file...");
                CreateIfNotExist();
                _characters = File.ReadAllText(_illegalCharactersFilePath);
                _logger.AddInfo($"CharactersValidator >>> Read: Found {_characters.Length} illegal characters: {_characters}");
            }
            catch (Exception ex)
            {
                _logger.AddError($"CharactersValidator >>> Read: {ex.Message}");
            }

            return this;
        }

        private void CreateIfNotExist()
        {
            try
            {
                if (File.Exists(_illegalCharactersFilePath)) return;
                _logger.AddInfo($"CharactersValidator >>> CreateIfNotExist: {_illegalCharactersFilePath} does't exist - creating new file...");
                File.WriteAllText(_illegalCharactersFilePath, _defaultIllegalCharacters);
            }
            catch (Exception ex)
            {
                _logger.AddError($"CharactersValidator >>> CreateIfNotExist: {ex.Message}");
            }
        }
        #endregion

        #region REMOVE
        public void Remove(ReportData reportData)
        {
            try
            {
                var illegalCharacters = new Regex($"[{_characters}]");
                foreach (var reportDataRow in reportData.Rows)
                {
                    for (var index = 0; index < reportDataRow.Data.Count; index++)
                    {
                        var str = reportDataRow.Data[index];
                        reportDataRow.Data[index] = illegalCharacters.Replace(str, "");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"CharactersValidator >>> Remove: {ex.Message}");
            }
        }
        #endregion
    }
}
