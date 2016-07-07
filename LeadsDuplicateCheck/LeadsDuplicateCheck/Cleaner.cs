using System;
using System.Collections.Generic;

namespace LeadsDuplicateCheck
{
    public class Cleaner
    {
        private readonly ConsoleLogger _logger;

        public Cleaner(ConsoleLogger logger)
        {
            _logger = logger;
        }

        public void Cleanse(Dictionary<string, ReportData> data)
        {
            try
            {
                _logger.AddInfo("Cleaner >> Cleanse: Cleaning data...");
                foreach (var d in data)
                {
                    foreach (var r in d.Value.Rows)
                    {
                        for (var columnIndex = 0; columnIndex < r.Data.Count; columnIndex++)
                        {
                            var s = r.Data[columnIndex];
                            r.Data[columnIndex] = s.Replace(",", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError("Cleaner >> Cleanse", ex);
            }
        }
    }
}