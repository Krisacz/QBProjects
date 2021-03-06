﻿using System.IO;
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
    public class WinServiceAppController : IAppController
    {
        private IExecuter _executer;

        public WinServiceAppController()
        {
            
        }

        public void Start()
        {
            Init();
            _executer.Start();
        }

        public void Stop()
        {
            _executer.Stop();
        }

        private void Init()
        {
            var logger = new FileLogger();
            var settings = SettingsReader.Read(logger);
            logger.EnableDetailedLog = settings.DetailedLog;

            var reportsSettings = new ReportsSettings(logger).ReadAll();
            var reportDataManager = new ReportDataManager(logger, reportsSettings);
            var dataAccessor = new AquariumWebService(logger, settings);
            var sqlManager = new SqlManager(logger, settings);
            var cache = new InMemoryCache(logger);
            var sqlDataChecker = new SqlDataChecker(reportDataManager, logger, cache);
            var sqlDataUpdater = new SqlDataUpdater(sqlManager, logger);
            var charactersValidator = new CharactersValidator(logger).Read();
            var validator = new Validator(logger, reportDataManager, sqlDataChecker, charactersValidator, cache).Read();
            var flowManager = new FlowManager(cache, dataAccessor, sqlManager, reportDataManager, sqlDataChecker, sqlDataUpdater,
                validator, logger);
            _executer = new TimerExecuter(logger, settings, flowManager);
        }
    }
}