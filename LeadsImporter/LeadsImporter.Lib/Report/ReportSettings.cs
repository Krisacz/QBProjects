namespace LeadsImporter.Lib.Report
{
    public class ReportSettings
    {
        public string Type { get; set; }
        public int AquariumQueryId { get; set; }
        public int ExecutionSequnece { get; set; }

        public string LeadIdColumnName { get; set; }
        public string CustomerIdColumnName { get; set; }
        public string LenderIdColumnName { get; set; }
        public string LoanDateColumnName { get; set; }
        public string LeadCreatedColumnName { get; set; }
        public string ProclaimDropPath { get; set; }
    }
}
