namespace LeadsImporter.Lib.Report
{
    public class ReportSettings
    {
        public string Type { get; set; }
        public string AquariumQueryId { get; set; }
        public string ExecutionSequnece { get; set; }

        public int LeadIdIndex { get; set; }
        public int ClientIdIndex { get; set; }
        public int LenderIdIndex { get; set; }
        public int DateOfCreditIndex { get; set; }
        public int LoanAmountIndex { get; set; }
        public int DateTimeLeadCreateIndex { get; set; }
        public string ProclaimDropPath { get; set; }
    }
}
