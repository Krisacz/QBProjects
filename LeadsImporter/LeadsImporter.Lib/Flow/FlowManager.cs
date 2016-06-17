using System;
using System.Collections.Generic;
using System.IO;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.DataAccessor;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Sql;
using LeadsImporter.Lib.Validation;

namespace LeadsImporter.Lib.Flow
{
    public class FlowManager : IFlowManager
    {
        private readonly ICache _cache;
        private readonly IDataAccessor _dataAccessor;
        private readonly SqlManager _sqlManager;
        private readonly ReportDataManager _reportDataManager;
        private readonly SqlDataChecker _sqlDataChecker;
        private readonly SqlDataUpdater _sqlDataUpdater;
        private readonly Validator _validator;
        private readonly ILogger _logger;

        private List<SqlDataExceptionObject> _sqlExceptions;
        private List<SqlDataObject> _allData;

        public FlowManager(ICache cache, IDataAccessor dataAccessor, SqlManager sqlManager, 
            ReportDataManager reportDataManager, SqlDataChecker sqlDataChecker, SqlDataUpdater sqlDataUpdater, Validator validator, ILogger logger)
        {
            _cache = cache;
            _dataAccessor = dataAccessor;
            _sqlManager = sqlManager;
            _sqlDataChecker = sqlDataChecker;
            _reportDataManager = reportDataManager;
            _sqlDataUpdater = sqlDataUpdater;
            _validator = validator;
            _logger = logger;
        }

        #region INIT
        public void Init()
        {
            try
            {
                _cache.Clear();
                _sqlExceptions = _sqlManager.GetAllExceptions();
                _allData = _sqlManager.GetAllData();
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> Init: {ex.Message}");
            }
        }
        #endregion

        #region PROCESS REPORTS
        public void ProcessReports()
        {
            try
            {
                var types = _reportDataManager.GetReportTypes();

                foreach (var type in types)
                {
                    ProcessReport(type);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> ProcessReports: {ex.Message}");
            }
        }
        
        private void ProcessReport(string type)
        {
            try
            {
                var sequences = _reportDataManager.GetSequencesCountForType(type);
                ReportData firstReportData = null;
                for (var s = 1; s <= sequences; s++)
                {
                    var queryId = _reportDataManager.GetReportQueryId(type, s);

                    //First in the sequence
                    if (s == 1)
                    {
                        var unvalidatedFirstReportData = _dataAccessor.GetReportData(queryId);
                        firstReportData = _validator.ValidateReport(unvalidatedFirstReportData, _sqlExceptions);
                    }
                    //any sequential
                    else
                    {
                        var unvalidatedReportData = _dataAccessor.GetReportData(queryId);
                        var reportData = _validator.ValidateReport(unvalidatedReportData, _sqlExceptions);
                        _reportDataManager.Join(firstReportData, reportData);
                    }
                }
                _cache.Store(type, firstReportData);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> ProcessReport[{type}]: {ex.Message}");
            }
        }
        #endregion

        #region SQL CHECK
        public void SqlCheck()
        {
            try
            {
                var types = _reportDataManager.GetReportTypes();
                foreach (var type in types) CheckData(type);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> SqlCheck: {ex.Message}");
            }
        }

        private void CheckData(string type)
        {
            try
            {
                var reportData = _cache.Get(type);
                var duplicates = new List<SqlDataExceptionObject>();
                var reportDataWithoutExceptions = _sqlDataChecker.RemoveExceptions(reportData, _sqlExceptions);
                var newReportData = _sqlDataChecker.RemoveExistingData(reportDataWithoutExceptions, _allData, duplicates);
                var newReportDataWithoutDuplicates = _sqlDataChecker.RemoveDuplicates(newReportData, duplicates);
                
                _sqlDataUpdater.SubmitNewExceptions(duplicates);
                var newSqlData = SqlConverter.GetReportDataAsSqlDataObject(newReportDataWithoutDuplicates, _reportDataManager, _logger);
                _sqlDataUpdater.SubmitNewData(newSqlData);

                _cache.Store(type, newReportDataWithoutDuplicates);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> CheckData[{type}]: {ex.Message}");
            }
        }
        #endregion

        #region OUTPUT
        //TODO ADD TYPE AS A PREFIX TO OUTPUT FILE
        public void Output()
        {
            try
            {
                var types = _reportDataManager.GetReportTypes();
                foreach (var type in types)
                {
                    SaveReport(type);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> Output: {ex.Message}");
            }
        }

        private void SaveReport(string type)
        {
            try
            {
                var reportData = _cache.Get(type);
                var csv = new List<string>();
                if(reportData.Rows.Count == 0) return;

                foreach (var reportDataRow in reportData.Rows)
                {
                    var line = string.Join(",", reportDataRow.Data);
                    csv.Add(line);
                }

                var fileName = $"import_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.csv";
                var pathRoot = _reportDataManager.GetOutputPath(reportData);
                var fullPath = Path.Combine(pathRoot, fileName);

                File.WriteAllLines(fullPath, csv);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> SaveReport[{type}]: {ex.Message}");
            }
        }
        #endregion
    }
}