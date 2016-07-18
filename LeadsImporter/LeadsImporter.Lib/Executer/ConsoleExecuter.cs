using System;
using LeadsImporter.Lib.Flow;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Executer
{
    public class ConsoleExecuter : IExecuter
    {
        private readonly ILogger _logger;
        private readonly IFlowManager _flowManager;

        public ConsoleExecuter(ILogger logger, IFlowManager flowManager)
        {
            _logger = logger;
            _flowManager = flowManager;
        }

        public void Start()
        {
            _logger.AddInfo("ConsoleExecuter >>> Start: Starting...");
        }

        public void Stop()
        {
            _logger.AddInfo("ConsoleExecuter >>> Stop: Stopped.");
        }

        public void Execute()
        {
            try
            {
                _logger.AddInfo("TimerExecuter >>> Execute: Executing...");

                _flowManager.Init();
                _flowManager.Process();
                _flowManager.Output();

                _logger.AddInfo("TimerExecuter >>> Execute: Finished!");
            }
            catch (Exception ex)
            {
                _logger.AddError($"TimerExecuter >>> Execute:", ex);
            }
        }
    }
}