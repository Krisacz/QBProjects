using System;
using System.Threading;
using System.Timers;
using LeadsImporter.Lib.Aquarium;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Validation;
using static System.Double;
using Timer = System.Timers.Timer;

namespace LeadsImporter.Lib.Executer
{
    public class TimerExecuter : IExecuter
    {
        private readonly ILogger _logger;
        private readonly Settings.Settings _settings;
        private readonly ReportsSettings _reportsSettings;
        private readonly ICache _cache;
        private readonly Validator _validator;
        private readonly AquariumWebService _aquariumWebService;
        private readonly Timer _timer;

        public TimerExecuter(ILogger logger, Settings.Settings settings, ReportsSettings reportsSettings, ICache cache, Validator validator, AquariumWebService aquariumWebService)
        {
            _logger = logger;
            _settings = settings;
            _reportsSettings = reportsSettings;
            _cache = cache;
            _validator = validator;
            _aquariumWebService = aquariumWebService;

            _timer = new Timer();
            _timer.Elapsed += Execute;
            _timer.Interval = Parse(settings.PoolingTimeInSec) * 1000;
        }

        public void Start()
        {
            try
            {
                _logger.AddInfo("TimerExecuter >>> Start: Starting timer...");
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.AddError($"TimerExecuter >>> Start: {ex.Message}");
            }
        }

        public void Stop()
        {
            try
            {
                _logger.AddInfo("TimerExecuter >>> Stop: Stopping timer...");
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.AddError($"TimerExecuter >>> Stop: {ex.Message}");
            }
        }

        private void Execute(object sender, ElapsedEventArgs e)
        {
            Execute();
        }

        public void Execute()
        {
            try
            {
                _timer.Enabled = false;
                _logger.AddInfo("TimerExecuter >>> Execute: (Waking up)");
                _logger.AddInfo("TimerExecuter >>> Execute: Executing...");

                //Debug
                _logger.AddInfo("TimerExecuter >>> Execute: Doing Stuff...");
                Thread.Sleep(3 * 1000);
                _logger.AddInfo("TimerExecuter >>> Execute: Doing Stuff Completed!");
                //var reportData = _aquariumWebService.GetReportData(_reportsSettings.GetReportsId()[0]);
                //_cache.Store(reportData);

                _logger.AddInfo("TimerExecuter >>> Execute: Finished!");
                _logger.AddInfo("TimerExecuter >>> Execute: (Sleeping)");
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.AddError($"TimerExecuter >>> Execute: {ex.Message}");
            }
            finally
            {
                _timer.Enabled = true;
            }
        }
    }
}