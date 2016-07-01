using System;

namespace LeadsDuplicateCheck
{
    public class ProclaimClaimData
    {
        private readonly ILogger _logger;
        public string CaseRef { get; private set; }
        public string Surname { get; private set; }
        public string Postcode { get; private set; }
        public DateTime? Dob { get; private set; }
        public DateTime? UrscLoanDate { get; private set; }
        public DateTime? RppiLoanDate { get; private set; }

        public ProclaimClaimData(ILogger logger)
        {
            _logger = logger;
        }

        public ProclaimClaimData Parse(string caseRef, string surname, string postcode, string dob, string urscLoanDate, string rppiLoanDate)
        {
            try
            {
                CaseRef = caseRef;
                Surname = surname;
                Postcode = postcode;
                Dob = TryParse(dob);
                UrscLoanDate = TryParse(urscLoanDate);
                RppiLoanDate = TryParse(rppiLoanDate);
            }
            catch (Exception ex)
            {
                _logger.AddError("ProclaimClaimData >> Parse", ex);
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
