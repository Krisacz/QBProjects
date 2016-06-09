using System;
using System.Timers;
using LeadsImporter.Lib.Flow;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Setting;
using static System.Double;
using Timer = System.Timers.Timer;

namespace LeadsImporter.Lib.Executer
{
    public class TimerExecuter : IExecuter
    {
        private readonly ILogger _logger;
        private readonly Timer _timer;
        private readonly IFlowManager _flowManager;

        public TimerExecuter(ILogger logger, Settings settings, IFlowManager flowManager)
        {
            _logger = logger;
            _flowManager = flowManager;

            _timer = new Timer();
            _timer.Elapsed += Execute;
            _timer.Interval = Parse(settings.PoolingTimeInSec) * 1000;
        }

        #region START
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
        #endregion

        #region STOP
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
        #endregion

        #region EXECUTE
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

                _flowManager.Init();
                _flowManager.ProcessReports();
                _flowManager.SqlCheck();
                _flowManager.Output();

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
        #endregion
    }
}