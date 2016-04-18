using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProclaimAuditConverter
{
    public class Change
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string By { get; set; }
        public string Source { get; set; }
        public string NewValue { get; set; }

        public Change(DateTime date, TimeSpan time, string by, string source, string newValue)
        {
            Date = date;
            Time = time;
            By = by;
            Source = source;
            NewValue = newValue;
        }
    }
}
