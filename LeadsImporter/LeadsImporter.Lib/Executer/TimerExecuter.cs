using System;
using System.Timers;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using static System.Double;

namespace LeadsImporter.Lib.Executer
{
    public class TimerExecuter : IExecuter
    {
        private readonly Settings.Settings _settings;
        private readonly ILogger _logger;
        private readonly ReportsSettings _reportsSettings;
        private readonly ICache _cache;
        private readonly Timer _timer;

        public TimerExecuter(Settings.Settings settings, ILogger logger, ReportsSettings reportsSettings, ICache cache)
        {
            try
            {
                _settings = settings;
                _logger = logger;
                _reportsSettings = reportsSettings;
                _cache = cache;

                _timer = new Timer();
                _timer.Elapsed += Execute;
                _timer.Interval = Parse(settings.PoolingTimeInSec);
            }
            catch (Exception ex)
            {
                logger.AddError($"TimerExecuter >>> Init: {ex.Message}");
            }
        }

        public void Start()
        {
            try
            {
                _logger.AddInfo("TimerExecuter >>> Start: Started executer timer.");
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
                _logger.AddInfo("TimerExecuter >>> Stop: Stopped executer timer.");
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
                _logger.AddInfo("TimerExecuter >>> Execute: Executing...");

                //TODO Do stuff
                _logger.AddInfo("TimerExecuter >>> Execute: DO STUFF");
            }
            catch (Exception ex)
            {
                _logger.AddError($"TimerExecuter >>> Execute: {ex.Message}");
            }
        }
    }
}