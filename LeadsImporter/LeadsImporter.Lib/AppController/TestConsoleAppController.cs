using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.DataAccessor;
using LeadsImporter.Lib.Executer;
using LeadsImporter.Lib.Flow;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Setting;
using LeadsImporter.Lib.Sql;
using LeadsImporter.Lib.Validation;

namespace LeadsImporter.Lib.AppController
{
    public class TestConsoleAppController : IAppController
    {
        private readonly IExecuter _executer;

        public TestConsoleAppController()
        {
            var logger = new ConsoleLogger() {EnableDetailedLog = false};
            var settings = SettingsReader.Read(logger);
            var reportsSettings = new ReportsSettings(logger).ReadAll();
            var reportDataManager = new ReportDataManager(logger, reportsSettings);
            var dataAccessor = new TestDataAccessor(logger);
            var sqlManager = new SqlManager(logger, settings);
            var sqlDataChecker = new SqlDataChecker(reportDataManager, logger);
            var sqlDataUpdater = new SqlDataUpdater(sqlManager, logger);
            var charactersValidator = new CharactersValidator(logger).Read();
            var validator = new Validator(logger, reportDataManager, sqlDataChecker, charactersValidator).Read();
            var cache = new InMemoryCache(logger);
            var flowManager = new FlowManager(cache, dataAccessor, sqlManager, reportDataManager, sqlDataChecker, sqlDataUpdater, validator, logger);
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
