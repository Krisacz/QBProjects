using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LeadsDuplicateCheck
{
    public class OutputWriter
    {
        private readonly ConsoleLogger _logger;
        private readonly Settings _setting;

        public OutputWriter(ConsoleLogger logger, Settings setting)
        {
            _logger = logger;
            _setting = setting;
        }

        public void WriteAll(List<Duplicate> duplicates, Dictionary<string, ReportData> aquariumDataFlat)
        {
            try
            {
                var additionalHeaders = new List<string>()
                {
                    string.Empty,
                    "Proclaim-CaseRef",
                    "Proclaim-FirstName",
                    "Proclaim-Surname",
                    "Proclaim-Postcde",
                    "Proclaim-DOB",
                    "Proclaim-Lender",
                    "Proclaim-URSCLoanDate",
                    "Proclaim-RPPILoanDate",
                };
                
                var types = _setting.ReportIds.Select(x => x.Type).ToList();
                foreach (var type in types)
                {
                    if(duplicates.All(x => x.AquariumLeadData.ReportType != type)) continue;

                    var csvLines = new List<string>();
                    var headers = aquariumDataFlat[type].Headers;
                    var headersLine = $"{ListToString(headers)},{ListToString(additionalHeaders)}";
                    csvLines.Add(headersLine);

                    foreach (var duplicate in duplicates)
                    {
                        if (duplicate.AquariumLeadData.ReportType != type) continue;
                        var dataLine = $"{ListToString(duplicate.AquariumLeadData.RawData)},{string.Empty},{ProclaimDataToString(duplicate.ProclaimClaimData)}";
                        csvLines.Add(dataLine);
                    }

                    var fileName = $"{type}_{DateTime.Now.ToString("dd-MM-yyyy HHmmss")}.csv";
                    File.WriteAllLines(fileName, csvLines);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError("OutputWriter >> WriteAll", ex);
            }
        }

        #region HELP METHODS
        private static string ProclaimDataToString(ProclaimClaimData p)
        {
            var str = string.Empty;
            str += p.CaseRef;
            str += "," + p.FirstName;
            str += "," + p.Surname;
            str += "," + p.Postcode;
            str += "," + DateNullToString(p.Dob);
            str += "," + p.Lender;
            str += "," + DateNullToString(p.UrscLoanDate);
            str += "," + DateNullToString(p.RppiLoanDate);

            return str;
        }

        private static string DateNullToString(DateTime? dt)
        {
            if (dt == null) return string.Empty;
            var v = dt.Value;
            return v.ToString("dd/MM/yyyy");
        }

        private static string ListToString(IEnumerable<string> list)
        {
            return string.Join(",", list);
        }
        #endregion
    }
}