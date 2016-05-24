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
                        var validation = ParseValidationFile(lines);
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
            if (!Directory.Exists(_settings.ValidationFilesPath))
            {
                Directory.CreateDirectory(_settings.ValidationFilesPath);
                var readMeStr = new List<string>();

                #region READ ME TEXT
                readMeStr.Add(@"1. File extension needs to be "".val"" (name doesn't matter)");
                readMeStr.Add(@"2. Mendatory Lines:");
                readMeStr.Add(@"		1-> Aquarium Report Id");
                readMeStr.Add(@"		2-> Name of the column/header");
                readMeStr.Add(@"		3-> TRUE or FALSE - Can this fields be empty? (not case sensitive)");
                readMeStr.Add(@"		4-> Type of field: STRING, FIXED, DATE, VALUE (not case sensitive)");
                readMeStr.Add(@"3. Optional Lines if field type is:");
                readMeStr.Add(@"		STRING:");
                readMeStr.Add(@"			5-> Minimum Length (can be empty)");
                readMeStr.Add(@"			6-> Maximum Length (can be empty)");
                readMeStr.Add(@"			");
                readMeStr.Add(@"		FIXED:");
                readMeStr.Add(@"			5-> String as allowed value (not case sensitive)");
                readMeStr.Add(@"			6+  Any additional lines will add allowed values");
                readMeStr.Add(@"		");
                readMeStr.Add(@"		DATE:");
                readMeStr.Add(@"			5-> Minimum date value in format dd/MM/yyyy (can be empty)");
                readMeStr.Add(@"			6-> Maximum date value in format dd/MM/yyyy (can be empty)");
                readMeStr.Add(@"			7-> Minimum number of years from today to value(can be empty)");
                readMeStr.Add("");
                readMeStr.Add(@"		VALUE:");
                readMeStr.Add(@"			5-> Minimum value (numbers only, no commas or currency symbols, can be empty)");
                readMeStr.Add(@"			6-> Maximum value (numbers only, no commas or currency symbols, can be empty)");
                readMeStr.Add(@"-----------------------------------------------------------------------------------------");
                readMeStr.Add(@"Example 1:");
                readMeStr.Add(@"----------");
                readMeStr.Add(@"321321");
                readMeStr.Add(@"DOB");
                readMeStr.Add(@"FALSE");
                readMeStr.Add(@"DATE");
                readMeStr.Add("");
                readMeStr.Add("");
                readMeStr.Add(@"18");
                readMeStr.Add(@"----------");
                readMeStr.Add(@"Above validation will make sure that field in column named ""DOB"" will not be empty");
                readMeStr.Add(@"and number of years from today is no less than 18.");
                readMeStr.Add(@"-----------------------------------------------------------------------------------------");
                readMeStr.Add(@"Example 2:");
                readMeStr.Add(@"----------");
                readMeStr.Add(@"321321");
                readMeStr.Add(@"URSC-Q1");
                readMeStr.Add(@"FALSE");
                readMeStr.Add(@"FIXED");
                readMeStr.Add(@"Yes");
                readMeStr.Add(@"No");
                readMeStr.Add(@"----------");
                readMeStr.Add(@"Above validation will make sure that field in column named ""URSC - Q1"" will not be empty and ");
                readMeStr.Add(@"will contain value ""Yes"" or ""No"".");    
                #endregion

                File.WriteAllLines(Path.Combine(_settings.ValidationFilesPath, "READ_ME.txt"), readMeStr);
            }
        }

        #endregion

        #region HELP METHODS
        private static Validation ParseValidationFile(IReadOnlyList<string> lines)
        {
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

        private bool IsFileValid(IReadOnlyList<string> lines, string validationFile)
        {
            try
            {
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
