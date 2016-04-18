using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace ProclaimAuditConverter
{
    [DelimitedRecord(",")]
    public class AuditRecord
    {
        [FieldQuoted()]
        public string CaseRef;

        [FieldQuoted()]
        public string FieldName;

        [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
        [FieldQuoted()]
        public DateTime Date;

        [FieldConverter(typeof(TimeConverter))]
        [FieldQuoted()]
        public TimeSpan Time;

        [FieldQuoted()]
        public string By;

        [FieldQuoted()]
        public string Source;

        [FieldQuoted()]
        public string NewValue;
    }
}

public class TimeConverter : ConverterBase
{
    public override string FieldToString(object from)
    {
        return base.FieldToString(from);
    }

    public override object StringToField(string from)
    {
        var parts = from.Split(':');
        var hours = int.Parse(parts[0]);
        var minutes = int.Parse(parts[1]);
        var seconds = int.Parse(parts[2]);
        return new TimeSpan(hours, minutes, seconds);
    }
}