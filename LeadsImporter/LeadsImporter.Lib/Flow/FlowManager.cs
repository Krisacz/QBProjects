using System;
using LeadsImporter.Lib.Aquarium;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Sql;

namespace LeadsImporter.Lib.Flow
{
    public class FlowManager : IFlowManager
    {
        private readonly ICache _cache;
        private readonly ReportsSettings _reportsSettings;
        private readonly AquariumWebService _webService;
        private readonly SqlManager _sqlManager;
        private SqlDataChecker _slqDataChecker;

        public FlowManager(ICache cache, ReportsSettings reportsSettings, AquariumWebService webService, SqlManager sqlManager)
        {
            _cache = cache;
            _reportsSettings = reportsSettings;
            _webService = webService;
            _sqlManager = sqlManager;
        }

        public void Init()
        {
            _cache.Clear();
        }

        public void GetReportsData()
        {
            var types = _reportsSettings.GetTypes();

            foreach (var type in types)
            {
                var sequences = _reportsSettings.GetSequencesPerType(type);
                ReportData firstReportData = null;
                for (var s = 1; s <= sequences; s++)
                {
                    var reportSettings = _reportsSettings.GetReportSettings(type, s);
                    //First in the sequence
                    if (s == 1)
                    {
                        firstReportData = _webService.GetReportData(reportSettings.AquariumQueryId, reportSettings);
                    }
                    //any sequential
                    else
                    {
                        var reportData = _webService.GetReportData(reportSettings.AquariumQueryId, reportSettings);
                        ReportDataManager.Join(firstReportData, reportData);
                    }
                }
                _cache.Store(type, firstReportData);
            }
        }

        public void SqlCheck()
        {
            var exceptions = _sqlManager.GetAllExceptions();
            var allData = _sqlManager.GetAllData();
            var types = _reportsSettings.GetTypes();

            foreach (var type in types)
            {
                var reportData = _cache.Get(type);
                _slqDataChecker.RemoveExceptions(reportData, exceptions);
                var duplictes = _slqDataChecker.GetNewDuplicates(reportData, allData);
                _sqlDataUpdater.SubmitNewDuplicates(duplictes);
                _sqlDataUpdater.SubmitNewData(reportData);
            }
        }

        public void Output()
        {
            throw new NotImplementedException();
        }

        public void End()
        {
            throw new NotImplementedException();
        }
    }
}