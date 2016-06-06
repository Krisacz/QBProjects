using System;
using System.Collections.Generic;
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
            //var consoleLogger = new ConsoleLogger();
            //var settings = SettingsReader.Read(consoleLogger);
            //var sql = new SqlManager(consoleLogger, settings);

            //sql.InsertRecord("URSC", "666", "666", "666", new DateTime(2016, 1, 1), new DateTime(2016, 1, 1));
            //sql.InsertException("RPPI", "666", "666", "666", new DateTime(2016, 1, 1), new DateTime(2016, 1, 1), "DUPLICATE", "6");
            //var a = sql.GetAllData(); 
            //var c = 1;
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
            //var ws = new AquariumWebService(consoleLogger, settings);

            /*
            var reportData1 = new ReportData();
            reportData1.Headers = new List<string> {"AAA", "CCC", "BBB"};
            reportData1.Rows = new List<ReportDataRow>();
            reportData1.Rows.Add(new ReportDataRow() {Data = new List<string>() { "ID2", "MRow2-2", "MRow2-3"} });
            reportData1.Rows.Add(new ReportDataRow() {Data = new List<string>() { "ID3", "MRow3-2", "MRow3-3"} });
            reportData1.Rows.Add(new ReportDataRow() {Data = new List<string>() { "ID1", "MRow1-2", "MRow1-3"} });
            reportData1.Settings = new ReportSettings()
            {
                AquariumQueryId = "0",
                ExecutionSequnece = 1,
                Type = "TEST",
                ProclaimDropPath = @"C:\",
                LeadIdColumnName = "AAA",
                CustomerIdColumnName = "AAA",
                LenderIdColumnName = "AAA",
                LoanDateColumnName = "AAA",
                LeadCreatedColumnName = "AAA"
            };

            var reportData2 = new ReportData();
            reportData2.Headers = new List<string> {"AAA", "XXX", "ZZZ"};
            reportData2.Rows = new List<ReportDataRow>();
            reportData2.Rows.Add(new ReportDataRow() { Data = new List<string>() { "ID1", "MRow1-4", "MRow1-5" } });
            reportData2.Rows.Add(new ReportDataRow() { Data = new List<string>() { "ID3", "MRow3-4", "MRow3-5" } });
            reportData2.Rows.Add(new ReportDataRow() { Data = new List<string>() { "ID2", "MRow2-4", "MRow2-5" } });
            reportData2.Settings = new ReportSettings()
            {
                AquariumQueryId = "1",
                ExecutionSequnece = 2,
                Type = "TEST",
                ProclaimDropPath = @"C:\",
                LeadIdColumnName = "AAA",
                CustomerIdColumnName = "AAA",
                LenderIdColumnName = "AAA",
                LoanDateColumnName = "AAA",
                LeadCreatedColumnName = "AAA"
            };
            
            reportData1.Join(reportData2);
            var a = 1;
            */
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
