using System.Collections.Generic;
using System.Text.RegularExpressions;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Validation
{
    public static class CustomFixer
    {
        public static IEnumerable<string> Fix(IEnumerable<string> dataRow)
        {
            var fixedDataRow1 = FixDates(dataRow);
            return fixedDataRow1;
        }

        private static IEnumerable<string> FixDates(IEnumerable<string> dataRow)
        {
            var regex = new Regex(@"\d{4}-\d{2}-\d{2}");
            var regex2 = new Regex(@"(\d{2}/\d{2}/\d{4})");
            var fixedDataRow = new List<string>();

            foreach (var field in dataRow)
            {
                if (regex.IsMatch(field))
                {
                    var regexMatch = regex.Match(field);
                    var incorrectDateString = regexMatch.Value;
                    var correctedDate = CorrectDate(incorrectDateString);
                    fixedDataRow.Add(correctedDate);
                }
                else if (regex2.IsMatch(field))
                {
                    var regexMatch = regex2.Match(field);
                    var correctDateString = regexMatch.Value;
                    var correctedDate = correctDateString;
                    fixedDataRow.Add(correctedDate);
                }
                else
                {
                    fixedDataRow.Add(field);
                }
            }

            return fixedDataRow;
        }

        private static string CorrectDate(string value)
        {
            //0123456789
            //2001-01-31
            var year = value.Substring(0, 4);
            var month = value.Substring(5, 2);
            var day = value.Substring(8, 2);
            var correctedDate = $"{day}/{month}/{year}";
            return correctedDate;
        }
    }
}
