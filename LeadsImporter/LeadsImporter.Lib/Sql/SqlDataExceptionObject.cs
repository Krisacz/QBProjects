    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsImporter.Lib.Sql
{
    public class SqlDataExceptionObject
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Type { get; private set; }
        public string LeadId { get; private set; }
        public string CustomerId { get; private set; }
        public string LenderId { get; private set; }
        public DateTime LoanDate { get; private set; }
        public DateTime LeadCreated { get; private set; }
        public string ExceptionType { get; private set; }
        public string ExceptionDescription { get; private set; }

        public SqlDataExceptionObject(int id, DateTime dateTime, string type, string leadId, string customerId, string lenderId, DateTime loanDate, DateTime leadCreated, string exceptionType, string exceptionDescription)
        {
            Id = id;
            DateTime = dateTime;
            Type = type;
            LeadId = leadId;
            CustomerId = customerId;
            LenderId = lenderId;
            LoanDate = loanDate;
            LeadCreated = leadCreated;
            ExceptionType = exceptionType;
            ExceptionDescription = exceptionDescription;
        }

        //Constructor for insert
        public SqlDataExceptionObject(string type, string leadId, string customerId, string lenderId, DateTime loanDate, DateTime leadCreated, string exceptionType, string exceptionDescription)
        {
            Type = type;
            LeadId = leadId;
            CustomerId = customerId;
            LenderId = lenderId;
            LoanDate = loanDate;
            LeadCreated = leadCreated;
            ExceptionType = exceptionType;
            ExceptionDescription = exceptionDescription;
        }
    }
}
