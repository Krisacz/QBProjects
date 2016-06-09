using System;
using System.Collections.Generic;
using System.IO;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Sql;
using LeadsImporter.Lib.Validation;
using LeadsImporter.Lib.WebService;

namespace LeadsImporter.Lib.Flow
{
    public class FlowManager : IFlowManager
    {
        private readonly ICache _cache;
        private readonly ReportsSettings _reportsSettings;
        private readonly IDataAccessor _dataAccessor;
        private readonly SqlManager _sqlManager;
        private readonly ReportDataManager _reportDataManager;
        private readonly SqlDataChecker _slqDataChecker;
        private readonly SqlDataUpdater _sqlDataUpdater;
        private readonly Validator _validator;
        private readonly ILogger _logger;

        public FlowManager(ICache cache, ReportsSettings reportsSettings, IDataAccessor dataAccessor, SqlManager sqlManager, 
            ReportDataManager reportDataManager, SqlDataChecker slqDataChecker, SqlDataUpdater sqlDataUpdater, Validator validator, ILogger logger)
        {
            _cache = cache;
            _reportsSettings = reportsSettings;
            _dataAccessor = dataAccessor;
            _sqlManager = sqlManager;
            _slqDataChecker = slqDataChecker;
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
                var types = _reportsSettings.GetTypes();

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
                var sequences = _reportsSettings.GetSequencesCountForType(type);
                ReportData firstReportData = null;
                for (var s = 1; s <= sequences; s++)
                {
                    var reportSettings = _reportsSettings.GetReportSettings(type, s);
                    //First in the sequence
                    if (s == 1)
                    {
                        var unvalidatedFirstReportData = _dataAccessor.GetReportData(reportSettings.AquariumQueryId);
                        firstReportData = _validator.ValidateReport(unvalidatedFirstReportData);
                    }
                    //any sequential
                    else
                    {
                        var unvalidatedReportData = _dataAccessor.GetReportData(reportSettings.AquariumQueryId);
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
                var types = _reportsSettings.GetTypes();
                foreach (var type in types) CheckData(type, exceptions, allData);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FlowManager >>> SqlCheck: {ex.Message}");
            }
        }

        private void CheckData(string type, IEnumerable<SqlDataExceptionObject> exceptions, IEnumerable<SqlDataObject> allData)
        {
            try
            {
                var reportData = _cache.Get(type);
                var reportDataWithoutExceptions = _slqDataChecker.RemoveExceptions(reportData, exceptions);
                var duplicates = _slqDataChecker.GetNewDuplicates(reportDataWithoutExceptions, allData);
                _sqlDataUpdater.SubmitNewExceptions(duplicates);
                var newData = GetReportDataAsSqlDataObject(reportDataWithoutExceptions);
                _sqlDataUpdater.SubmitNewData(newData);
                _cache.Store(type, reportDataWithoutExceptions);
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
                    var type = _reportsSettings.GetTypeFromQueryId(reportData.QueryId);
                    var leadId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LeadIdColumnName);
                    var customerId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).CustomerIdColumnName);
                    var lenderId = _reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LenderIdColumnName);
                    var loanDate = DateTime.Parse(_reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LoanDateColumnName));
                    var leadCreated = DateTime.Parse(_reportDataManager.GetValueForColumn(reportData, reportDataRow,
                        _reportsSettings.GetReportSettings(reportData.QueryId).LeadCreatedColumnName));

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
                var types = _reportsSettings.GetTypes();
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
                var pathRoot = _reportsSettings.GetReportSettings(reportData.QueryId).ProclaimDropPath;
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