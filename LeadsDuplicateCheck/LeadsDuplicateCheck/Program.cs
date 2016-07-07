using System;
using System.Linq;

namespace LeadsDuplicateCheck
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            var start = DateTime.Now;

            var logger = new ConsoleLogger();                                           //Console error/info Logger
            var setting = SettingsReader.Read(logger);                                  //Settings 
            var aquarium = new AquariumWebService(logger, setting);                     //Aquarium ws wrapper
            var aquariumDataFlat = aquarium.GetAllAquariumData();                       //### Simplified flat (raw) data from Aquarium 
            var cleaner = new Cleaner(logger);                                          //Data cleaner
            cleaner.Cleanse(aquariumDataFlat);                                          //### Clean data
            var dataParser = new DataParser(logger, setting);                           //Data parser
            var aquariumData = dataParser.ParseToAquariumLeadData(aquariumDataFlat);    //### Aquarium data (to be user in Deduper)
            var sqlManager = new SqlManager(logger, setting);                           //Sql wrapper to retrieve Proclaim data
            var proclaimData = sqlManager.GetAllProclaimData();                         //### Proclaim data (to be user in Deduper)
            var deduper = new Deduper(logger);                                          //Deduper
            var duplicates = deduper.GetDuplicates(aquariumData, proclaimData);         //### Get duplicates
            var excluder = new Excluder(logger, setting).Read();                        //Excluder (for excluding ignored Proclaim claims)
            var withoutIgnored = excluder.Exclude(duplicates);                          //### Duplicates without excluded(ignored) claims
            var outputWriter = new OutputWriter(logger, setting);                       //File output writer
            outputWriter.WriteAll(withoutIgnored, aquariumDataFlat);                    //### Write all duplicated in csv file

            var end = DateTime.Now;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===== SUMMARY =====");
            Console.ForegroundColor = ConsoleColor.White;

            WriteSummaryLine("Aquarium Reports: ", aquariumDataFlat?.Keys.Count.ToString() ?? "0");
            WriteSummaryLine("Aquarium Total Records: ", aquariumDataFlat?.Values.Sum(x=>x.Rows.Count).ToString() ?? "0");
            WriteSummaryLine("Proclaim Records: ", proclaimData?.Count.ToString() ?? "0");
            WriteSummaryLine("Possible duplicates: ", withoutIgnored?.Count.ToString() ?? "0");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("====================");
            Console.WriteLine("Completed!");
            Console.WriteLine($"[Total duration: {end-start}]");
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("(Press any key to exit)");
            Console.ReadKey();
        }

        private static void WriteSummaryLine(string text, string value)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
