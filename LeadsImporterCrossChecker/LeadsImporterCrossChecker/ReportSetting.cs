namespace LeadsImporterCrossChecker
{
    public class ReportSetting
    {
        public int Id { get; private set; }
        public string LeadIdColumnName { get; private set; }

        public ReportSetting(string setting)
        {
            var tokens = setting.Split('|');
            Id = int.Parse(tokens[0]);
            LeadIdColumnName = tokens[1];
        }
    }
}
