using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using LeadsImporter.Lib.AppController;
using LeadsImporter.Lib.Aquarium;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Settings;
using LeadsImporter.Lib.Sql;
using Topshelf;

namespace LeadsImporter.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var consoleLogger = new ConsoleLogger();
            var settings = SettingsReader.Read(consoleLogger);
            var sql = new SqlManager(consoleLogger, settings);

            //sql.InsertRecord("URSC", "666", "666", "666", new DateTime(2016, 1, 1), new DateTime(2016, 1, 1));
            sql.InsertException("RPPI", "666", "666", "666", new DateTime(2016, 1, 1), new DateTime(2016, 1, 1), "DUPLICATE", "6");

            //var r = sql.DuplicatesCheck("666", "666", new DateTime(2016, 1, 1));
            //var a = 1;

            //ReportData d = null;

            //var consoleLogger = new ConsoleLogger();
            //var settings = SettingsReader.Read(consoleLogger);
            //var c = new FileCache(consoleLogger, settings);
            //d = c.Get("Temp\\temp.xml");
            //var a = 1;
            //var logger = new ConsoleLogger();
            //var settings = SettingsReader.Read(logger);
            //var ws = new WebService(settings, logger);

            //ws.GetReport();

            //Console.WriteLine();
            //Console.WriteLine("Done!");
            //Console.ReadKey();

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
