using System;
using IMRR.Lib;
using Topshelf;

namespace IMRR.ConsoleTester
{

    #region WRAPPER CLASS
    public class WinService
    {
        private readonly FileWatcher _fileWatcher;

        public WinService()
        {
            var logger = new FileLogger();
            var settings = SettingsReader.Read(logger);
            _fileWatcher = new FileWatcher(logger, settings);
        }

        public void Start()
        {
            _fileWatcher.Start();
        }

        public void Stop()
        {
            _fileWatcher.Stop();
        }
    }
    #endregion

    class Program
    {
        private static bool _debug = true;    

        static void Main(string[] args)
        {
            if(!_debug)
            {            
                #region TOP SHELF
                HostFactory.Run(x =>
                {
                    x.Service<WinService>(s =>
                    {
                        s.ConstructUsing(name => new WinService());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });
                    x.RunAsLocalSystem();

                    x.SetDescription("Incoming Mail Reader and Renamer service.");
                    x.SetDisplayName("IMRR");
                    x.SetServiceName("IMRR");
                });   
                #endregion
            }
            else
            { 
                #region WHILE JUST USING A CONSOLE WITHOUT TOP SHELD            
                Console.WriteLine("Press Escape to Stop & Exit.");
                Console.WriteLine("=============================");
                Console.WriteLine();

                //var logger = new FileLogger();
                var logger = new ConsoleLogger();
                var settings = SettingsReader.Read(logger);
                var fileWatcher = new FileWatcher(logger, settings);
                fileWatcher.Start();

                do
                {
                    while (!Console.KeyAvailable)
                    {
                        //Continue processing until ESC pressed
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);             
                #endregion
            }
        }
    }
}
