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
            //TODO Console Summary 
            var end = DateTime.Now;

            Console.WriteLine();
            Console.WriteLine("====================");
            Console.WriteLine($"Completed! [Total duration: {end-start}]");
            Console.ReadKey();
        }
    }
}
