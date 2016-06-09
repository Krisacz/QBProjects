using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Executer;
using LeadsImporter.Lib.Flow;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Setting;
using LeadsImporter.Lib.Sql;
using LeadsImporter.Lib.Validation;
using LeadsImporter.Lib.WebService;

namespace LeadsImporter.Lib.AppController
{
    public class WinServiceAppController : IAppController
    {
        private readonly IExecuter _executer;

        public WinServiceAppController()
        {
            var logger = new FileLogger();
            var settings = SettingsReader.Read(logger);
            var reportsSettings = new ReportsSettings(logger, settings).ReadAll();
            var reportDataManager = new ReportDataManager(logger, reportsSettings);
            var cache = new FileCache(logger, settings);
            var dataAccessor = new AquariumWebService(logger, settings);
            var sqlManager = new SqlManager(logger, settings);
            var sqlDataUpdater = new SqlDataUpdater(sqlManager, logger);
            var validator = new Validator(logger, settings, reportsSettings, reportDataManager, sqlDataUpdater).Read();
            var sqlDataChecker = new SqlDataChecker(reportsSettings, reportDataManager, logger);
            var flowManager = new FlowManager(cache, reportsSettings, dataAccessor, sqlManager, reportDataManager, sqlDataChecker, sqlDataUpdater, validator, logger);
            _executer = new TimerExecuter(logger, settings, flowManager);
        }

        public void Start()
        {
            _executer.Start();
        }

        public void Stop()
        {
            _executer.Stop();
        }
    }
}