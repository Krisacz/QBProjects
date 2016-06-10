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
                        firstReportData = _validator.ValidateReport(unvalidatedFirstReportData);
                    }
                    //any sequential
                    else
                    {
                        var unvalidatedReportData = _dataAccessor.GetReportData(queryId);
                        var reportData = _validator.ValidateReport(unvalidatedReportData);
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
                var exceptions = _sqlManager.GetAllExceptions();
                var allData = _sqlManager.GetAllData();
                var types = _reportDataManager.GetReportTypes();
                foreach (var type in types) CheckData(type, exceptions, allData);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> SqlCheck: {ex.Message}");
            }
        }

        private void CheckData(string type, List<SqlDataExceptionObject> exceptions, IEnumerable<SqlDataObject> allData)
        {
            try
            {
                var reportData = _cache.Get(type);
                var reportDataWithoutExceptions = _sqlDataChecker.RemoveExceptions(reportData, exceptions);
                var duplicates = _sqlDataChecker.GetNewDuplicates(reportDataWithoutExceptions, allData);
                var reportDataWithoutDuplicatesAndExceptions = _sqlDataChecker.GetDuplicatesInNewDataSet(reportDataWithoutExceptions, duplicates);
                _sqlDataUpdater.SubmitNewExceptions(duplicates);
                var newData = GetReportDataAsSqlDataObject(reportDataWithoutDuplicatesAndExceptions);
                _sqlDataUpdater.SubmitNewData(newData);
                _cache.Store(type, reportDataWithoutDuplicatesAndExceptions);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> CheckData[{type}]: {ex.Message}");
            }
        }

        private IEnumerable<SqlDataObject> GetReportDataAsSqlDataObject(ReportData reportData)
        {
            try
            {
                var list = new List<SqlDataObject>();
                foreach (var reportDataRow in reportData.Rows)
                {
                    var type = _reportDataManager.GetReportType(reportData);
                    var leadId = _reportDataManager.GetValueForLeadId(reportData, reportDataRow);
                    var customerId = _reportDataManager.GetValueForCustomerId(reportData, reportDataRow);
                    var lenderId = _reportDataManager.GetValueForLenderId(reportData, reportDataRow);
                    var loanDate = _reportDataManager.GetValueForLoanDate(reportData, reportDataRow);
                    var leadCreated = _reportDataManager.GetValueForLeadCreated(reportData, reportDataRow);

                    list.Add(new SqlDataObject(type, leadId, customerId, lenderId, loanDate, leadCreated));
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> GetReportDataAsSqlDataObject: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region OUTPUT
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
                foreach (var reportDataRow in reportData.Rows)
                {
                    var line = string.Join(",", reportDataRow.Data);
                    csv.Add(line);
                }
                csv.Add(string.Empty);

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