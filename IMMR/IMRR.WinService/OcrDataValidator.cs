using System;
using System.Text.RegularExpressions;

namespace IMRR.Lib
{
    internal class OcrDataValidator
    {
        private readonly ILogger _logger;

        //Matches: # (any spaces) 2 digits (with any spaces in between and after) Number 1 (any spaces) 5 digits (with any spaces in between and after) Dot(.) (with any spaces in between and after) 3 digits (with any spaces in between and after) #
        //E.g. #23100123.111#  or # 5 1 156 5 6 7  .0    04 #
        private readonly Regex _specificOldCaseRef = new Regex(@"[#]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[1]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[\.]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[#]{1}", RegexOptions.Compiled);

        //Matches: # (any spaces) 2 digits (with any spaces in between and after) Number 3 (any spaces) 5 digits (with any spaces in between and after) #
        //E.g. #64321321#  or #5 5 3   8 4 8 4  5    #
        private readonly Regex _specificNewCaseRef = new Regex(@"[#]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[3]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[#]{1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        //Matches: Number 1 (any spaces) 5 digits (with any spaces in between and after) Dot(.) (with any spaces in between and after) 3 digits (with any spaces in between and after)
        //E.g. "100123.111" or "1 54 6   54  .0 0   4"
        private readonly Regex _genericOldCaseRef = new Regex(@"[\s]+[1]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[\.]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s\\\/\(]", RegexOptions.Compiled);

        //Matches: Number 3 (any spaces) 5 digits (with any spaces in between and after)
        //E.g. "300123" or "3 54 6   54"
        private readonly Regex _genericNewCaseRef = new Regex(@"[\s]+[3]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s\\\/\(]", RegexOptions.Compiled);

        //Matches: Number 1 (any spaces) 5 digits (with any spaces in between and after)
        //E.g. "123123" or " 1 2 3   1  2 3 "
        private readonly Regex _genericOldCaseRefWithoutPostfix = new Regex(@"[1]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}[\s]*[0-9]{1}", RegexOptions.Compiled);
        

        public OcrDataValidator(ILogger logger)
        {
            _logger = logger;
        }

        public ValidateType Validate(string ocrData)
        {
            try
            {
                _logger.AddInfo("Searching for correct data...");

                if (_specificOldCaseRef.IsMatch(ocrData))
                {
                    var foundData = _specificOldCaseRef.Match(ocrData);
                    var cleanedString = CleanString(foundData.Value);
                    _logger.AddInfo(string.Format("Found correct data [Regex01]: {0}", cleanedString));
                    var result = cleanedString;
                    return new ValidateType(result, CorrectLevel.Direct);
                }

                if (_specificNewCaseRef.IsMatch(ocrData))
                {
                    var foundData = _specificNewCaseRef.Match(ocrData);
                    var cleanedString = CleanString(foundData.Value);
                    _logger.AddInfo(string.Format("Found correct data [Regex02]: {0}", cleanedString));
                    var result = cleanedString;
                    return new ValidateType(result, CorrectLevel.Direct);
                }

                if (_genericOldCaseRef.IsMatch(ocrData))
                {
                    var foundData = _genericOldCaseRef.Match(ocrData);
                    var cleanedString = CleanString(foundData.Value);
                    _logger.AddInfo(string.Format("Found correct data [Regex03]: {0}", cleanedString));
                    var result = string.Format("CL{0}", cleanedString);
                    return new ValidateType(result, CorrectLevel.Generic);
                }

                if (_genericNewCaseRef.IsMatch(ocrData))
                {
                    var foundData = _genericNewCaseRef.Match(ocrData);
                    var cleanedString = CleanString(foundData.Value);
                    _logger.AddInfo(string.Format("Found correct data [Regex04]: {0}", cleanedString));
                    var result = string.Format("CL{0}", cleanedString);
                    return new ValidateType(result, CorrectLevel.Generic);
                }

                if (_genericOldCaseRefWithoutPostfix.IsMatch(ocrData))
                {
                    var foundData = _genericOldCaseRefWithoutPostfix.Match(ocrData);
                    var cleanedString = CleanString(foundData.Value);
                    _logger.AddInfo(string.Format("Found correct data [Regex05]: {0}", cleanedString));
                    var result = string.Format("CL{0}", cleanedString);
                    return new ValidateType(result, CorrectLevel.Partial);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError(ex.Message);
            }

            return null;
        }

        private string CleanString(string str)
        {
            var result = Regex.Replace(str, @"\s+", "");
            result = Regex.Replace(result, @"\\", "");
            result = Regex.Replace(result, @"/", "");
            result = Regex.Replace(result, @"\(", "");
            result = Regex.Replace(result, @"#", "");
            return result;
        }
    }

    public class ValidateType
    {
        public string Name { get; private set; }
        public CorrectLevel Level { get; private set; }

        public ValidateType(string name, CorrectLevel level)
        {
            Name = name;
            Level = level;
        }
    }

    public enum CorrectLevel
    {
        Direct,
        Generic,
        Partial
    }
}
