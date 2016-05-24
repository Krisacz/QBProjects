using System.Runtime.InteropServices;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Executer;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Settings;
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
            var reportsSettings = new ReportsSettings(settings, logger).ReadAll();
            var cache = new InMemoryCache();
            var validator = new Validator(logger, settings).ReadAll();          
            _executer = new TimerExecuter(settings, logger, reportsSettings, cache, validator);
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
