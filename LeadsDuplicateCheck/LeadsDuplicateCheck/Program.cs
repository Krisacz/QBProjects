using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LeadsDuplicateCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            var start = DateTime.Now;

            var logger = new ConsoleLogger();                                           //Console error/info Logger
            var setting = SettingsReader.Read(logger);                                  //Settings 
            var aquarium = new AquariumWebService(logger, setting);                     //Aquarium ws wrapper
            var aquariumDataFlat = aquarium.GetAllAquariumData();                       //Simplified flat (raw) data from Aquarium 
            var dataParser = new DataParser(logger, setting);                           //Data parser
            var aquariumData = dataParser.ParseToAquariumLeadData(aquariumDataFlat);    //Aquarium data (to be user in Deduper)
            var sqlManager = new SqlManager(logger, setting);                           //Sql wrapper to retrieve Proclaim data
            var proclaimData = sqlManager.GetAllProclaimData();                         //Proclaim data (to be user in Deduper)
            var deduper = new Deduper(logger);                                          //Deduper
            var duplicates = deduper.GetDuplicates(aquariumData, proclaimData);         //Get duplicates
            var outputWriter = new OutputWriter(logger, setting);                       //File output writer
            outputWriter.WriteAll(duplicates, aquariumDataFlat);                        //Write all duplicated in csv file

            var end = DateTime.Now;


            //TODO its not fully done needs to be checked and etc
            Console.Clear();
            Console.WriteLine("===== SUMMARY =====");
            Console.Write($"Aquarium reports ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(aquariumDataFlat.Keys.Count);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" with total of ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(aquariumDataFlat.Values.Count);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" records.");

            Console.Write($"Proclaim records ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(proclaimData.Count);
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write($"Possible duplicates: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(duplicates.Count);
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.WriteLine();
            Console.WriteLine("====================");
            Console.WriteLine($"Completed! [Total duration: {end-start}]");
            Console.ReadKey();
        }
    }
}
