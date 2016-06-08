using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeadsImporter.Lib.Aquarium;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Sql;
using LeadsImporter.Lib.Validation;

namespace LeadsImporter.Lib.Flow
{
    //TODO Add logging
    public class FlowManager : IFlowManager
    {
        private readonly ICache _cache;
        private readonly ReportsSettings _reportsSettings;
        private readonly AquariumWebService _webService;
        private readonly SqlManager _sqlManager;
        private readonly ReportDataManager _reportDataManager;
        private readonly SqlDataChecker _slqDataChecker;
        private readonly SqlDataUpdater _sqlDataUpdater;
        private readonly Validator _validator;

        public FlowManager(ICache cache, ReportsSettings reportsSettings, AquariumWebService webService, SqlManager sqlManager, ReportDataManager reportDataManager, SqlDataChecker slqDataChecker, SqlDataUpdater sqlDataUpdater, Validator validator)
        {
            _cache = cache;
            _reportsSettings = reportsSettings;
            _webService = webService;
            _sqlManager = sqlManager;
            _slqDataChecker = slqDataChecker;
            _reportDataManager = reportDataManager;
            _sqlDataUpdater = sqlDataUpdater;
            _validator = validator;
        }

        public void Init()
        {
            _cache.Clear();
        }

        public void ProcessReports()
        {
            var types = _reportsSettings.GetTypes();

            foreach (var type in types)
            {
                ProcessReport(type);
            }
        }

        private void ProcessReport(string type)
        {
            var sequences = _reportsSettings.GetSequencesPerType(type);
            ReportData firstReportData = null;
            for (var s = 1; s <= sequences; s++)
            {
                var reportSettings = _reportsSettings.GetReportSettings(type, s);
                //First in the sequence
                if (s == 1)
                {
                    var unvalidatedFirstReportData = _webService.GetReportData(reportSettings.AquariumQueryId);
                    firstReportData = _validator.ValidateReport(unvalidatedFirstReportData);
                }
                //any sequential
                else
                {
                    var unvalidatedReportData = _webService.GetReportData(reportSettings.AquariumQueryId);
                    var reportData = _validator.ValidateReport(unvalidatedReportData);
                    _reportDataManager.Join(firstReportData, reportData);
                }
            }
            _cache.Store(type, firstReportData);
        }

        public void SqlCheck()
        {
            var exceptions = _sqlManager.GetAllExceptions();
            var allData = _sqlManager.GetAllData();
            var types = _reportsSettings.GetTypes();
            foreach (var type in types) CheckData(type, exceptions, allData);
        }

        private void CheckData(string type, List<SqlDataExceptionObject> exceptions, List<SqlDataObject> allData)
        {
            var reportData = _cache.Get(type);
            _slqDataChecker.RemoveExceptions(reportData, exceptions);
            var duplicates = _slqDataChecker.GetNewDuplicates(reportData, allData);
            _sqlDataUpdater.SubmitNewExceptions(duplicates);
            _sqlDataUpdater.SubmitNewData(reportData);
            _cache.Store(type, reportData);
        }

        public void Validate()
        {
            var types = _reportsSettings.GetTypes();
            foreach (var type in types)
            {
                CleanReport(type);
            }
        }

        private void CleanReport(string type)
        {
            //_validator.
            var reportData = _cache.Get(type);
        }

        public void Output()
        {
            var types = _reportsSettings.GetTypes();
            foreach (var type in types)
            {
                SaveReport(type);
            }
        }

        private void SaveReport(string type)
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

        public void End()
        {
            throw new NotImplementedException();
        }
    }
}