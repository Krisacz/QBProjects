using System;

namespace LeadsImporter.Lib.Sql
{
    public class SqlDataObject
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Type { get; private set; }
        public string LeadId { get; private set; }
        public string CustomerId { get; private set; }
        public string LenderId { get; private set; }
        public DateTime LoanDate { get; private set; }
        public DateTime LeadCreated { get; private set; }
       
        public SqlDataObject(int id, DateTime dateTime, string type, string leadId, string customerId, string lenderId, DateTime loanDate, DateTime leadCreated)
        {
            Id = id;
            DateTime = dateTime;
            Type = type;
            LeadId = leadId;
            CustomerId = customerId;
            LenderId = lenderId;
            LoanDate = loanDate;
            LeadCreated = leadCreated;
        }

        //Constructor for insert
        public SqlDataObject(string type, string leadId, string customerId, string lenderId, DateTime loanDate, DateTime leadCreated)
        {
            Type = type;
            LeadId = leadId;
            CustomerId = customerId;
            LenderId = lenderId;
            LoanDate = loanDate;
            LeadCreated = leadCreated;
        }
    }
}
