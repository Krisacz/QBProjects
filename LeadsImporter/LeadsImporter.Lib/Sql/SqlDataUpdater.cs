using System.Collections.Generic;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Sql
{
    //TODO Add logging
    //TODO Implementation
    public class SqlDataUpdater
    {
        private readonly SqlManager _sqlManager;

        public SqlDataUpdater(SqlManager sqlManager)
        {
            _sqlManager = sqlManager;
        }

        public void SubmitNewData(List<SqlDataObject> newData)
        {
            foreach (var sqlDataObject in newData)
            {
                _sqlManager.InsertRecord(sqlDataObject);
            }
        }

        public void SubmitNewExceptions(List<SqlDataExceptionObject> exceptions)
        {

        }
    }
}