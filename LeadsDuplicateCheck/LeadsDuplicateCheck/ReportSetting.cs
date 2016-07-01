using System;

namespace LeadsDuplicateCheck
{
    public class ReportSetting
    {
        private readonly ILogger _logger;
        public string Type { get; private set; }
        public int Id { get; private set; }
        public string LastNameColumnName { get; private set; }
        public string DobColumnName { get; private set; }
        public string PostcodeColumnName { get; private set; }
        public string LoanDateColumnName { get; private set; }
        public string LeadIdColumnName { get; private set; }
        public string CustomerIdColumnName { get; private set; }

        public ReportSetting(ILogger logger)
        {
            _logger = logger;
        }

        public ReportSetting Parse(string setting)
        {
            try
            {
                var tokens = setting.Split('|');
                Type = tokens[0];
                Id = int.Parse(tokens[1]);
                LastNameColumnName = tokens[2];
                DobColumnName = tokens[3];
                PostcodeColumnName = tokens[4];
                LoanDateColumnName = tokens[5];
                LeadIdColumnName = tokens[6];
                CustomerIdColumnName = tokens[7];
            }
            catch(Exception ex)
            {
                _logger.AddError($"ReportSetting >> Parse[{setting}", ex);
            }

            return this;
        }
    }
}
