using System.ServiceProcess;

namespace IMRR.Lib
{
    class WindowsService : ServiceBase
    {
        /// <summary>
        /// Public Constructor for WindowsService.
        /// - Put all of your Initialization code here.
        /// </summary>
        public WindowsService()
        {
            ServiceName = "IMRR";
            EventLog.Log = "Application";

            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            CanHandlePowerEvent = true;
            CanHandleSessionChangeEvent = true;
            CanPauseAndContinue = true;
            CanShutdown = true;
            CanStop = true;
        }

        /// <summary>
        /// The Main Thread: This is where your Service is Run.
        /// </summary>
        static void Main()
        {
            Run(new WindowsService());
        }

        protected override void OnStart(string[] args)
        {
            var logger = new FileLogger();
            var settings = SettingsReader.Read(logger);
            var fileWatcher = new FileWatcher(logger, settings);
            fileWatcher.Start();
            base.OnStart(args);
        }
    }
}
