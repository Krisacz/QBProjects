﻿using System.Runtime.InteropServices;
using LeadsImporter.Lib.Aquarium;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Executer;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Settings;
using LeadsImporter.Lib.Sql;
using LeadsImporter.Lib.Validation;

namespace LeadsImporter.Lib.AppController
{
    public class ConsoleAppController : IAppController
    {
        private readonly IExecuter _executer;

        public ConsoleAppController()
        {
            var logger = new ConsoleLogger();
            var settings = SettingsReader.Read(logger);
            var reportsSettings = new ReportsSettings(logger, settings).ReadAll();
            var reportDataManager = new ReportDataManager(logger, reportsSettings);
            var cache = new FileCache(logger, settings);
            var sqlDataUpdater = new SqlDataUpdater();
            var validator = new Validator(logger, settings, reportsSettings, reportDataManager, sqlDataUpdater).ReadAll();
            var webService = new AquariumWebService(logger, settings, reportsSettings);          
            _executer = new TimerExecuter(logger, settings, reportsSettings, cache, validator, webService);
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
