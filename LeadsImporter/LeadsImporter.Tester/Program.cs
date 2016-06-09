using System;
using LeadsImporter.Lib.AppController;
using Topshelf;

namespace LeadsImporter.Tester
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Run(RunType.ConsoleTest);
        }

        #region RUN
        private static void Run(RunType runType)
        {
            switch (runType)
            {
                case RunType.TopShelf:
                        #region TOP SHELF
                        HostFactory.Run(x =>
                        {
                            x.Service<IAppController>(s =>
                            {
                                s.ConstructUsing(name => new WinServiceAppController());
                                s.WhenStarted(tc => tc.Start());
                                s.WhenStopped(tc => tc.Stop());
                            });
                            x.RunAsLocalSystem();
                            x.SetDescription("Proclaim Leads Importer");
                            x.SetDisplayName("LeadsImporter");
                            x.SetServiceName("LeadsImporter");
                        });
                        #endregion
                    break;

                case RunType.ConsoleLive:
                        #region CONSOLE
                        Console.WriteLine("Press Escape to Stop & Exit.");
                        Console.WriteLine("=============================");
                        Console.WriteLine();

                        var app = new ConsoleAppController();
                        app.Start();

                        do
                        {
                            while (!Console.KeyAvailable)
                            {
                                //Continue processing until ESC pressed
                            }
                        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                        #endregion
                    break;

                case RunType.ConsoleTest:
                        #region CONSOLE
                        Console.WriteLine("Press Escape to Stop & Exit.");
                        Console.WriteLine("=============================");
                        Console.WriteLine();

                        var app1 = new TestConsoleAppController();
                        app1.Start();

                        do
                        {
                            while (!Console.KeyAvailable)
                            {
                                //Continue processing until ESC pressed
                            }
                        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                        #endregion
                    break;

                default:    throw new ArgumentOutOfRangeException(nameof(runType), runType, null);
            }
        }
        #endregion

        private enum RunType
        {
            TopShelf,
            ConsoleLive,
            ConsoleTest
        }
    }
}
