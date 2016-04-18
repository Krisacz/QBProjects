using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProclaimAuditConverter
{
    public class Audit
    {
        public List<Case> Cases { get; private set; }

        public Audit()
        {
            Cases = new List<Case>();
        }

        public void AddCase(AuditRecord record)
        {
            var caseExistIndex = CaseExist(record.CaseRef);
            if (caseExistIndex == null)
            {
                var newCase = new Case(record.CaseRef, record.FieldName);
                newCase.AddChange(record.Date, record.Time, record.By, record.Source, record.NewValue);
                Cases.Add(newCase);
            }
            else
            {
                var index = (int)caseExistIndex;
                Cases[index].AddChange(record.Date, record.Time, record.By, record.Source, record.NewValue);
            }
        }

        private int? CaseExist(string caseRef)
        {
            for (int i = 0; i < Cases.Count; i++)
            {
                if (Cases[i].CaseRef == caseRef) return i;
            }

            return null;
        }
    }
}
