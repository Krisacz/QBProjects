using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Sql;


//TODO LEAD CREATED NEEDS TO BE CONVERTED TO STANDARD DATE/TIME FORMAT
namespace LeadsImporter.Lib.Validation
{
    public class Validator
    {
        private readonly ILogger _logger;
        private Validations _validations;
        private readonly ReportDataManager _reportDataManager;
        private readonly string _validationFilesPath = @"Validations\\";
        private readonly SqlDataChecker _sqlDataChecker;
        private readonly CharactersValidator _charactersValidator;
        private readonly ICache _cache;

        public Validator(ILogger logger, ReportDataManager reportDataManager, SqlDataChecker sqlDataChecker, CharactersValidator charactersValidator, ICache cache)
        {
            _logger = logger;
            _reportDataManager = reportDataManager;
            _sqlDataChecker = sqlDataChecker;
            _charactersValidator = charactersValidator;
            _cache = cache;
        }

        #region READ ALL VALIDATION FILES
        public Validator Read()
        {
            try
            {
                _logger.AddInfo("Validator >>> Read: Reading all validation files...");
                CreateIfNotExist();
                var allFiles = Directory.GetFiles(_validationFilesPath);
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
                        _logger.AddError($"Validator >>> ReadAll: Invalid validation file: {validationFile}", null);
                    }

                }
                _logger.AddInfo($"Validator >>> ReadAll: Found {_validations.Count()} validation file(s).");
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> ReadAll:", ex);
            }

            return this;
        }

        private void CreateIfNotExist()
        {
            try
            {
                if (Directory.Exists(_validationFilesPath)) return;
                _logger.AddInfo($"Validator >>> CreateIfNotExist: {_validationFilesPath} does't exist - creating new folder...");
                Directory.CreateDirectory(_validationFilesPath);
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
                File.WriteAllLines(Path.Combine(_validationFilesPath, "READ_ME.txt"), readMeStr);
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> ReadAll:", ex);
            }
        }
        #endregion
        
        #region VALIDATE REPORT
        public ReportData ValidateReport(ReportData reportData, List<SqlDataExceptionObject> existingSqlExceptions, List<SqlDataExceptionObject> newSqlExceptions)
        {
            try
            {
                _charactersValidator.Remove(reportData);
                var validations = _validations.GetAll();
                var reportExceptions = new ReportDataExceptions() { QueryId = reportData.QueryId, Headers = reportData.Headers, Rows = new List<ReportDataRowExceptions>() };
                var correctedReportData = new ReportData() { QueryId = reportData.QueryId, Headers = reportData.Headers, Rows = new List<ReportDataRow>() };
                foreach (var reportDataRow in reportData.Rows)
                {
                    if(!_sqlDataChecker.InExceptionsList(reportData, reportDataRow, existingSqlExceptions))
                    {
                        ValidateRow(reportData, reportDataRow, validations, correctedReportData, newSqlExceptions, reportExceptions);
                    }
                }

                _cache.StoreExceptions(_reportDataManager.GetReportType(reportData), reportExceptions);
                return correctedReportData;
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> ValidateReport:", ex);
            }

            return null;
        }

        private void ValidateRow(ReportData reportData, ReportDataRow reportDataRow, IReadOnlyCollection<Validation> validations, 
            ReportData correctedReportData, ICollection<SqlDataExceptionObject> newSqlExceptions, ReportDataExceptions reportExceptions)
        {
            try
            {
                var anyException = false;

                var type = _reportDataManager.GetReportType(reportData);
                var leadCreated = _reportDataManager.GetValueForLeadCreated(reportData, reportDataRow);

                var leadId = _reportDataManager.GetValueForLeadId(reportData, reportDataRow);
                if (string.IsNullOrWhiteSpace(leadId)) reportExceptions.Rows.Add(new ReportDataRowExceptions() { Data = reportDataRow.Data, Exception = "Invalid LeadId" });

                var customerId = _reportDataManager.GetValueForCustomerId(reportData, reportDataRow);
                if (string.IsNullOrWhiteSpace(customerId)) reportExceptions.Rows.Add(new ReportDataRowExceptions() { Data = reportDataRow.Data, Exception = "Invalid CustomerId" });

                var lenderId = _reportDataManager.GetValueForLenderId(reportData, reportDataRow);
                if (string.IsNullOrWhiteSpace(lenderId)) reportExceptions.Rows.Add(new ReportDataRowExceptions() { Data = reportDataRow.Data, Exception = "Invalid LenderId" });

                var loanDate = _reportDataManager.GetValueForLoanDate(reportData, reportDataRow);
                if (loanDate == DateTime.MinValue) reportExceptions.Rows.Add(new ReportDataRowExceptions() { Data = reportDataRow.Data, Exception = "Invalid LoanDate" });

                if (leadId == null || customerId == null || lenderId == null || loanDate == DateTime.MinValue)
                {
                    newSqlExceptions.Add(new SqlDataExceptionObject(type, leadId, customerId, lenderId, loanDate, leadCreated, "VALIDATION", "Invalid LeadId/CustomerId/LenderId/LoanDate"));
                    anyException = true;
                }
                
                for (var index = 0; index < reportData.Headers.Count; index++)
                {
                    foreach (var validation in validations)
                    {
                        if (validation.QueryId != reportData.QueryId) continue;
                        var header = reportData.Headers[index];
                        if (header != validation.ColumnName) continue;
                        var exception = Validate(reportDataRow.Data[index], validation);
                        if (exception == null) continue;
                        var exceptionDesc = $"[{header}] " + exception;
                        newSqlExceptions.Add(new SqlDataExceptionObject(type, leadId, customerId, lenderId, loanDate, leadCreated, "VALIDATION", exceptionDesc));
                        reportExceptions.Rows.Add(new ReportDataRowExceptions() { Data = reportDataRow.Data, Exception = $"VALIDATION: {exceptionDesc}" });
                        anyException = true;
                    }
                }

                if (!anyException) correctedReportData.Rows.Add(reportDataRow);
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> ValidateRow:", ex);
            }
        }

        private string Validate(string value, Validation validation)
        {
            try
            {
                switch (validation.FieldType)
                {
                    case FieldType.STRING:  return ValidateString(value, validation.CanBeEmpty, validation.Parameters);
                    case FieldType.FIXED:   return ValidateFixed(value, validation.CanBeEmpty, validation.Parameters);
                    case FieldType.DATE:    return ValidateDate(value, validation.CanBeEmpty, validation.Parameters);
                    case FieldType.VALUE:   return ValidateValue(value, validation.CanBeEmpty, validation.Parameters);

                    default: throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> Validate:", ex);
            }

            return null;
        }

        private static string ValidateString(string value, bool canBeEmpty, IReadOnlyList<string> parameters)
        {
            //Check if string can be empty
            if (string.IsNullOrWhiteSpace(value) && !canBeEmpty) return "STRING can not be empty.";
            if (string.IsNullOrWhiteSpace(value) && canBeEmpty) return null;

            //Check if we have minimum len
            if (parameters.Count > 0 && !string.IsNullOrWhiteSpace(parameters[0]))
            {
                var minimumLen = int.Parse(parameters[0]);
                if (string.IsNullOrEmpty(value) &&  minimumLen > 0) return $"STRING must be at least {minimumLen} length.";
                if (!string.IsNullOrEmpty(value) && value.Length < minimumLen) return $"STRING must be at least {minimumLen} length.";
            }

            //Check if we have maximum len
            if (parameters.Count > 1 && !string.IsNullOrWhiteSpace(parameters[1]))
            {
                var maximumLen = int.Parse(parameters[1]);
                if (!string.IsNullOrEmpty(value) && value.Length > maximumLen) return $"STRING must be at not longer than {maximumLen}.";
            }

            return null;
        }

        private static string ValidateFixed(string value, bool canBeEmpty, IReadOnlyList<string> parameters)
        {
            //Check if string can be empty
            if (string.IsNullOrWhiteSpace(value) && !canBeEmpty) return "FIXED can not be empty.";
            if (string.IsNullOrWhiteSpace(value) && canBeEmpty) return null;

            //Check if content is one of the fixed values
            var correct = false;
            foreach (var parameter in parameters)
            {
                if (string.Equals(parameter, value, StringComparison.CurrentCultureIgnoreCase))
                {
                    correct = true;
                }
            }
            return !correct ? $"FIXED must be one of the provided values: {string.Join(",", parameters)}" : null;
        }

        private static string ValidateDate(string value, bool canBeEmpty, IReadOnlyList<string> parameters)
        {
            //Check if string can be empty
            if (string.IsNullOrWhiteSpace(value) && !canBeEmpty) return "DATE can not be empty.";
            if (string.IsNullOrWhiteSpace(value) && canBeEmpty) return null;

            //Try to parse date
            DateTime date;
            if (!DateTime.TryParse(value, out date)) return "DATE could not be parsed.";

            //Check if we have minimum date
            if (parameters.Count > 0 && !string.IsNullOrWhiteSpace(parameters[0]))
            {
                var minimumDate = DateTime.Parse(parameters[0]);
                if (date < minimumDate) return $"DATE can not be earlier than {minimumDate}.";
            }

            //Check if we have minimum date
            if (parameters.Count > 1 && !string.IsNullOrWhiteSpace(parameters[1]))
            {
                var maximumDate = DateTime.Parse(parameters[1]);
                if (date > maximumDate) return $"DATE can not be later than {maximumDate}.";
            }

            //Check if date is at least X years from today
            if (parameters.Count > 2 && !string.IsNullOrWhiteSpace(parameters[2]))
            {
                var years = int.Parse(parameters[2]);
                var dateCheck = date.AddYears(years);
                var today = DateTime.Now;
                if (dateCheck < today) return $"DATE must be at least {years} till today {today.ToString("dd-MM-yyyy")}.";
            }

            return null;
        }

        private static string ValidateValue(string value, bool canBeEmpty, List<string> parameters)
        {
            //Check if string can be empty
            if (string.IsNullOrWhiteSpace(value) && !canBeEmpty) return "VALUE can not be empty.";
            if (string.IsNullOrWhiteSpace(value) && canBeEmpty) return null;

            //Try to parse date
            int v;
            if (!int.TryParse(value, out v)) return "VALUE could not be parsed.";

            //Check if we have minimum value
            if (parameters.Count > 0 && !string.IsNullOrWhiteSpace(parameters[0]))
            {
                var minimumValue = int.Parse(parameters[0]);
                if (v < minimumValue) return $"VALUE can not be less than {minimumValue}.";
            }

            //Check if we have maximum value
            if (parameters.Count > 1 && !string.IsNullOrWhiteSpace(parameters[1]))
            {
                var maximumValue = int.Parse(parameters[1]);
                if (v > maximumValue) return $"VALUE can not be more than {maximumValue}.";
            }

            return null;
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
                var fieldType = (FieldType) Enum.Parse(typeof (FieldType), lines[3].ToUpper().Trim(), true);
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
                _logger.AddError($"Validator >>> ReadAll[{validationFile}]:", ex);
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
                if (string.IsNullOrWhiteSpace(lines[0])) return false;
                int n;
                var isNumeric = int.TryParse(lines[0], out n);
                if (!isNumeric)
                {
                    _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: Line {0} invalid - Value should be numeric", null);
                    return false;
                }

                //2nd line - can not be empty
                if (string.IsNullOrWhiteSpace(lines[1]))
                {
                    _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: Line {1} invalid - Can not be empty", null);
                    return false;
                }

                //3rd line - can be only true or false
                if (string.IsNullOrWhiteSpace(lines[2])) return false;
                if (lines[2].ToLower() != "true" && lines[2].ToLower() != "false")
                {
                    _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: Line {2} invalid - Value should be TRUE or FALSE", null);
                    return false;
                }

                //4th line can be only: STRING, FIXED, DATE or VALUE
                if (string.IsNullOrWhiteSpace(lines[3])) return false;
                if (lines[3].ToLower() != "string" && lines[3].ToLower() != "fixed" && lines[3].ToLower() != "date" && lines[3].ToLower() != "value")
                {
                    _logger.AddError($"Validator >>> IsFileValid[{validationFile}]: Line {2} invalid - Value should be STRING, FIXED, DATE or VALUE", null);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.AddError($"Validator >>> IsFileValid[{validationFile}]:", ex);
            }

            return false;
        }

        #endregion
    }
}
