using System.Collections.Generic;

namespace LeadsImporter.Lib.Validation
{
    public class Validation
    {
        public int QueryId { get; private set; }
        public string ColumnName { get; private set; }
        public bool CanBeEmpty { get; private set; }
        public FieldType FieldType { get; private set; }
        public List<string> Parameters { get; private set; }

        public Validation(int queryId, string columnName, bool canBeEmpty, FieldType fieldType, List<string> parameters)
        {
            QueryId = queryId;
            ColumnName = columnName;
            CanBeEmpty = canBeEmpty;
            FieldType = fieldType;
            Parameters = parameters;
        }
    }
}