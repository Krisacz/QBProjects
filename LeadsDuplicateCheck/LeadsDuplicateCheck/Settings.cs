using System.Collections.Generic;

namespace LeadsDuplicateCheck
{
    public class Settings
    {
        public string SqlConnectionString { get; private set; }
        public string AquariumUsername { get; private set; }
        public string AquariumPassword { get; private set; }
        public string AquariumLogonUrl { get; private set; }
        public string AquariumLogonAction { get; private set; }
        public string AquariumRunReportUrl { get; private set; }
        public string AquariumRunReportAction { get; private set; }
        public List<ReportSetting> ReportIds { get; private set; } 

        public Settings(string sqlConnectionString, string aquariumUsername, string aquariumPassword, string aquariumLogonUrl, 
            string aquariumLogonAction, string aquariumRunReportUrl, string aquariumRunReportAction, List<ReportSetting> reportIds)
        {
            SqlConnectionString = sqlConnectionString;
            AquariumUsername = aquariumUsername;
            AquariumPassword = aquariumPassword;
            AquariumLogonUrl = aquariumLogonUrl;
            AquariumLogonAction = aquariumLogonAction;
            AquariumRunReportUrl = aquariumRunReportUrl;
            AquariumRunReportAction = aquariumRunReportAction;
            ReportIds = reportIds;
        }
    }
}
