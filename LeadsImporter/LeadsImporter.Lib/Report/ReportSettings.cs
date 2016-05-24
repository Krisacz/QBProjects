namespace LeadsImporter.Lib.Report
{
    public class ReportSettings
    {
        public string Type { get; set; }
        public string AquariumQueryId { get; set; }
        public int ExecutionSequnece { get; set; }

        public string LeadIdColumnName { get; set; }
        public string ClientIdColumnName { get; set; }
        public string LenderIdColumnName { get; set; }
        public string DateOfCreditColumnName { get; set; }
        public string DateTimeLeadCreatedColumnName { get; set; }
        public string ProclaimDropPath { get; set; }
    }
}
