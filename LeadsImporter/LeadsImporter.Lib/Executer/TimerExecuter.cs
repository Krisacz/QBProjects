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
                _logger.AddError($"TimerExecuter >>> Start:", ex);
            }
        }
        #endregion

        #region STOP
        public void Stop()
        {
            try
            {
                _logger.AddInfo("TimerExecuter >>> Stop: Stopping timer...");
                _timer.Enabled = false;
            }
            catch (Exception ex)
            {
                _logger.AddError($"TimerExecuter >>> Stop:", ex);
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
                //SQL Connection check - if fails - do not proceed
                if (!_flowManager.PreCheck())
                {
                    _logger.AddError("TimerExecuter >>> Execute: SQL Connection failed!", null);
                    return;
                }

                //Process
                _timer.Enabled = false;
                _logger.AddInfo("TimerExecuter >>> Execute: (Waking up) Executing...");

                _flowManager.Init();
                _flowManager.Process();
                _flowManager.Output();

                _logger.AddInfo("TimerExecuter >>> Execute: Finished! (Sleeping...)");
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.AddError($"TimerExecuter >>> Execute:", ex);
            }
            finally
            {
                _timer.Enabled = true;
            }
        }
        #endregion
    }
}