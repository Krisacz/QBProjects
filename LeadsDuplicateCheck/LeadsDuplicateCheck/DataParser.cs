using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsDuplicateCheck
{
    public class DataParser
    {
        private readonly ILogger _logger;
        private readonly Settings _settings;

        public DataParser(ILogger logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        #region PARSE TO AQUARIUMLEADDATA
        public List<AquariumLeadData> ParseToAquariumLeadData(Dictionary<string, ReportData> reportDatas)
        {
            try
            {
                _logger.AddInfo($"DataParser >> ParseToAquariumLeadData: Parsing {reportDatas.Keys.Count} reports, {reportDatas.Values.Count} rows of data total...");

                var aquariumLeads = new List<AquariumLeadData>();

                foreach (var reportData in reportDatas)
                {
                    var type = reportData.Key;
                    var data = reportData.Value;
                    var id = data.QueryId;
                    var reportSettings = GetReportSettingsForType(type);

                    foreach (var dataRow in data.Rows)
                    {
                        var reportType = type;
                        var reportId = id;
                        var leadId = GetValueForColumn(GetIndexForColumn(data.Headers, reportSettings.LeadIdColumnName), dataRow);
                        var customerId = GetValueForColumn(GetIndexForColumn(data.Headers, reportSettings.CustomerIdColumnName), dataRow);
                        var surname = GetValueForColumn(GetIndexForColumn(data.Headers, reportSettings.LastNameColumnName), dataRow);
                        var postcode = GetValueForColumn(GetIndexForColumn(data.Headers, reportSettings.PostcodeColumnName), dataRow);
                        var dob = GetValueForColumn(GetIndexForColumn(data.Headers, reportSettings.DobColumnName), dataRow);
                        var loanDate = GetValueForColumn(GetIndexForColumn(data.Headers, reportSettings.LoanDateColumnName), dataRow);
                        var rawData = dataRow.Data;
                        var aquariumLeadData = new AquariumLeadData(_logger).Parse(reportType, reportId, leadId, customerId, surname, postcode, dob, loanDate, rawData);
                        aquariumLeads.Add(aquariumLeadData);
                    }
                }

                return aquariumLeads;
            }
            catch (Exception ex)
            {
                _logger.AddError("DataParser >> ParseToAquariumLeadData", ex);
            }

            return null;
        }
        #endregion

        #region HELP METHODS
        private static int GetIndexForColumn(IReadOnlyList<string> headers, string columnName)
        {
            for (var i = 0; i < headers.Count; i++)
            {
                if (headers[i] == columnName) return i;
            }

            return -1;
        }

        private static string GetValueForColumn(int index, ReportDataRow dataRow)
        {
            return dataRow.Data[index];
        }

        private ReportSetting GetReportSettingsForType(string type)
        {
            return _settings.ReportIds.First(x => x.Type == type);
        }
        #endregion
    }
}
