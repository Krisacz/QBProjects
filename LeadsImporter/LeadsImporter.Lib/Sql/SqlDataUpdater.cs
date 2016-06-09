using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Sql
{
    public class SqlDataUpdater
    {
        private readonly SqlManager _sqlManager;
        private readonly ILogger _logger;

        public SqlDataUpdater(SqlManager sqlManager, ILogger logger)
        {
            _sqlManager = sqlManager;
            _logger = logger;
        }

        #region SUBMIT NEW DATA
        public void SubmitNewData(IEnumerable<SqlDataObject> newData)
        {
            try
            {
                foreach (var sqlDataObject in newData)
                {
                    _sqlManager.InsertRecord(sqlDataObject);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlDataUpdater >>> SubmitNewData: {ex.Message}");
            }
        }
        #endregion

        #region SUBMIT NEW EXCEPTIONS
        public void SubmitNewExceptions(IEnumerable<SqlDataExceptionObject> exceptions)
        {
            try
            {
                foreach (var exceptionObject in exceptions)
                {
                    _sqlManager.InsertException(exceptionObject);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlDataUpdater >>> SubmitNewExceptions: {ex.Message}");
            }
        }
        #endregion
    }
}