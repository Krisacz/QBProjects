using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Validation
{
    public class Validator
    {
        private readonly ILogger _logger;
        private readonly Settings.Settings _settings;
        private Validations _validations;

        public Validator(ILogger logger, Settings.Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        #region READ ALL VALIDATION FILES
        public Validator ReadAll()
        {
            try
            {
                _logger.AddInfo("Validator >>> ReadAll: Reading all validation files...");
                CreateIfNotExist();
                var allFiles = Directory.GetFiles(_settings.ValidationFilesPath);
                var validationFiles = allFiles.Where(x => x.EndsWith("val"));
                _validations = new Validations();

                foreach (var validationFile in validationFiles)
                {
                    var lines = File.ReadAllLines(validationFile);
                    if (IsFileValid(lines, validationFile))
                    {
                        var validation = ParseValidationFile(lines, validationFile);
                        _validations.Add(validation);
                    }
                    else
                    {
                        _logger.AddError($"Validator >>> ReadAll: Invalid validation file: {validationFile}");
                    }

                }
                _logger.AddInfo($"Validator >>> ReadAll: Found {_validations.Count()} validation file(s).");
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> ReadAll: {ex.Message}");
            }

            return this;
        }

        private void CreateIfNotExist()
        {
            try
            {
                if (Directory.Exists(_settings.ValidationFilesPath)) return;
                _logger.AddInfo($"Validator >>> CreateIfNotExist: {_settings.ValidationFilesPath} does't exist - creating new folder...");
                Directory.CreateDirectory(_settings.ValidationFilesPath);
                var readMeStr = new List<string>
                {
                    #region READ ME TEXT
                    @"1. File extension needs to be "".val"" (name doesn't matter)",
                    @"2. Mendatory Lines:",
                    @"		1-> Aquarium Report Id",
                    @"		2-> Name of the column/header",
                    @"		3-> TRUE or FALSE - Can this fields be empty? (not case sensitive)",
                    @"		4-> Type of field: STRING, FIXED, DATE, VALUE (not case sensitive)",
                    @"3. Optional Lines if field type is:",
                    @"		STRING:",
                    @"			5-> Minimum Length (can be empty)",
                    @"			6-> Maximum Length (can be empty)",
                    @"			",
                    @"		FIXED:",
                    @"			5-> String as allowed value (not case sensitive)",
                    @"			6+  Any additional lines will add allowed values",
                    @"		",
                    @"		DATE:",
                    @"			5-> Minimum date value in format dd/MM/yyyy (can be empty)",
                    @"			6-> Maximum date value in format dd/MM/yyyy (can be empty)",
                    @"			7-> Minimum number of years from today to value(can be empty)",
                    "",
                    @"		VALUE:",
                    @"			5-> Minimum value (numbers only, no commas or currency symbols, can be empty)",
                    @"			6-> Maximum value (numbers only, no commas or currency symbols, can be empty)",
                    @"-----------------------------------------------------------------------------------------",
                    @"Example 1:",
                    @"----------",
                    @"321321",
                    @"DOB",
                    @"FALSE",
                    @"DATE",
                    "",
                    "",
                    @"18",
                    @"----------",
                    @"Above validation will make sure that field in column named ""DOB"" will not be empty",
                    @"and number of years from today is no less than 18.",
                    @"-----------------------------------------------------------------------------------------",
                    @"Example 2:",
                    @"----------",
                    @"321321",
                    @"URSC-Q1",
                    @"FALSE",
                    @"FIXED",
                    @"Yes",
                    @"No",
                    @"----------",
                    @"Above validation will make sure that field in column named ""URSC - Q1"" will not be empty and ",
                    @"will contain value ""Yes"" or ""No""."
                    #endregion
                };
                File.WriteAllLines(Path.Combine(_settings.ValidationFilesPath, "READ_ME.txt"), readMeStr);
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> ReadAll: {ex.Message}");
            }
        }
        #endregion

        #region GET ALL
        public List<Validation> GetAll()
        {
            return _validations.GetAll();
        }
        #endregion

        #region HELP METHODS
        private Validation ParseValidationFile(IReadOnlyList<string> lines, string validationFile)
        {
            try
            {
                _logger.AddInfo($"Validator >>> ParseValidationFile: Parsing validation file {validationFile}...");
                var queryId = int.Parse(lines[0].Trim());
                var columnName = lines[1].Trim();
                var canBeEmpty = lines[2].ToLower().Trim() != "false";
                var fieldType = (FieldType)Enum.Parse(typeof(FieldType), lines[3].ToUpper().Trim(), true);
                var parameters = new List<string>();
                //Additional parameters
                if (lines.Count >= 5)
                {
                    for (var i = 4; i < lines.Count; i++)
                    {
                        parameters.Add(lines[i].Trim());
                    }
                }

                return new Validation(queryId, columnName, canBeEmpty, fieldType, parameters);
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> ReadAll[{validationFile}]: {ex.Message}");
            }

            return null;
        }

        private bool IsFileValid(IReadOnlyList<string> lines, string validationFile)
        {
            try
            {
                _logger.AddInfo($"Validator >>> IsFileValid: Checking validation file {validationFile}...");


                //Miniumum 4 lines
                if (lines.Count < 4) return false;

                //1st line - report id - must be numeric
                if(string.IsNullOrWhiteSpace(lines[0])) return false;
                int n;
                var isNumeric = int.TryParse(lines[0], out n);
                if (!isNumeric)
                {
                    _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: Line {0} invalid - Value should be numeric");
                    return false;
                }

                //2nd line - can not be empty
                if (string.IsNullOrWhiteSpace(lines[1]))
                {
                    _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: Line {1} invalid - Can not be empty");
                    return false;
                }

                //3rd line - can be only true or false
                if (string.IsNullOrWhiteSpace(lines[2])) return false;
                if (lines[2].ToLower() != "true" && lines[2].ToLower() != "false")
                {
                    _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: Line {2} invalid - Value should be TRUE or FALSE");
                    return false;
                }

                //4th line can be only: STRING, FIXED, DATE or VALUE
                if (string.IsNullOrWhiteSpace(lines[3])) return false;
                if (lines[3].ToLower() != "string" && lines[3].ToLower() != "fixed" && lines[3].ToLower() != "date" && lines[3].ToLower() != "value")
                {
                    _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: Line {2} invalid - Value should be STRING, FIXED, DATE or VALUE");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: {ex.Message}");
            }

            return false;
        }
        #endregion
    }
}
