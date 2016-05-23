using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using LeadsImporter.Lib.AppController;
using LeadsImporter.Lib.Aquarium;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Settings;
using Topshelf;

namespace LeadsImporter.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var settings = SettingsReader.Read(logger);
            var ws = new WebService(settings, logger);

            ws.GetReport();

            Console.WriteLine();
            Console.WriteLine("Done!");
            Console.ReadKey();

            //Run(true);
        }

        #region RUN
        private static void Run(bool debug)
        {
            if (!debug)
            {
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
            }
            else
            {
                #region WHILE JUST USING A CONSOLE WITHOUT TOP SHELD            

                Console.WriteLine("Press Escape to Stop & Exit.");
                Console.WriteLine("=============================");
                Console.WriteLine();

                var a = new ConsoleAppController();
                a.Start();

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
        #endregion
    }
}
