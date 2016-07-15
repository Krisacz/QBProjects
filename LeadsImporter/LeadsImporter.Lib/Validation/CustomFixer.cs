using System.Collections.Generic;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Validation
{
    public static class CustomFixer
    {
        public static IEnumerable<string> Fix(List<string> dataRow, string type)
        {
            var afterFix1 = DateExecutedFix(dataRow, type);
            //any other fixes if needed e.g. var afterFix2 = FixSomething(afterFix1, type);
            //....
            return afterFix1 ?? dataRow;
        }

        private static List<string> DateExecutedFix(IEnumerable<string> dataRow, string type)
        {
            var fixedDataRow = new List<string>(dataRow);
            int index;
            switch (type.ToLower())
            {
                case "ursc": index = 77; break;
                case "rppi": index = 64; break;
                default: return null;
            }
            if (fixedDataRow.Count < index) return null;
            var oldValue = fixedDataRow[index];
            if (oldValue.Length < 10) return null;

            var d = oldValue.Substring(8, 2);
            var m = oldValue.Substring(5, 2);
            var y = oldValue.Substring(0, 4);

            var newValue = $"{d}/{m}/{y}";
            fixedDataRow[index] = newValue;

            return fixedDataRow;
        }
    }
}
