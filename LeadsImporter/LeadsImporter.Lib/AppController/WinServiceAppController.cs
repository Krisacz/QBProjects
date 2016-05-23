﻿using System;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Executer;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Settings;

namespace LeadsImporter.Lib.AppController
{
    public class WinServiceAppController : IAppController
    {
        private readonly IExecuter _executer;

        public WinServiceAppController()
        {
            var logger = new FileLogger();
            var settings = SettingsReader.Read(logger);
            var reportsSettings = new ReportsSettings(settings, logger).ReadAll();
            var cache = new InMemoryCache();
            _executer = new TimerExecuter(settings, logger, reportsSettings, cache);
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