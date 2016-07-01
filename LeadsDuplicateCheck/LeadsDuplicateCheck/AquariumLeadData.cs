using System;
using System.Collections.Generic;

namespace LeadsDuplicateCheck
{
    public class AquariumLeadData
    {
        private readonly ILogger _logger;
        public string ReportType { get; private set; }
        public int ReportId { get; private set; }
        public string LeadId { get; private set; }
        public string CustomerId {get; private set; }
        public string Surname { get; private set; }
        public string Postcode { get; private set; }
        public DateTime? Dob { get; private set; }
        public DateTime? LoanDate { get; private set; }
        public List<string> RawData { get; set; }

        public AquariumLeadData(ILogger logger)
        {
            _logger = logger;
        }

        public AquariumLeadData Parse(string reportType, int reportId, string leadId, string customerId, 
            string surname, string postcode, string dob, string loanDate, List<string> rawData)
        {
            try
            {
                ReportType = reportType;
                ReportId = reportId;
                LeadId = leadId;
                CustomerId = customerId;
                Surname = surname;
                Postcode = postcode;
                Dob = TryParse(dob);
                LoanDate = TryParse(loanDate);
                RawData = rawData;
            }
            catch (Exception ex)
            {
                _logger.AddError("AquariumLeadData >> Parse", ex);
            }

            return this;
        }

        private static DateTime? TryParse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            DateTime dt;
            var success = DateTime.TryParse(input, out dt);
            if (!success || dt == DateTime.MinValue) return null;
            return dt;
        }
    }
}
