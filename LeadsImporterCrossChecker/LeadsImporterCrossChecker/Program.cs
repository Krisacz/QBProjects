using System;
using System.Collections.Generic;
using System.IO;

namespace LeadsImporterCrossChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filesExist = File.Exists("a.txt") && File.Exists("s.txt");
            if (!filesExist && (args.Length <= 0 || args[0].ToLower() != "manual"))
            {
                var logger = new ConsoleLogger();
                var settings = SettingsReader.Read(logger);

                //Get SQL leads
                var sql = new SqlManager(logger, settings);
                File.WriteAllLines("s.txt", sql.GetAllLeads());

                //Get Aquarium leads
                var aquarium = new AquariumWebService(logger, settings);
                File.WriteAllLines("a.txt", aquarium.GetAllLeads());
            }
            Console.Clear();

            //Init
            Console.ForegroundColor = ConsoleColor.White;

            //Read data            
            var aLeads = ReadDataSet("a.txt", "Aquarium");
            var sLeads = ReadDataSet("s.txt", "SQL");            
            var pLeads = ReadDataSet("p.txt", "Proclaim");
            Console.WriteLine();
            Console.WriteLine("(Press any key to continue)");
            Console.WriteLine();
            Console.ReadKey();

            //Checks            
            var aquariumNotIn = Check(aLeads, "Aquarium", sLeads, "SQL", pLeads, "Proclaim");
            var sqlNotIn = Check(sLeads, "SQL", aLeads, "Aquarium", pLeads, "Proclaim");
            var proclaimNotIn = Check(pLeads, "Proclaim", aLeads, "Aquarium", sLeads, "SQL");

            //Validation
            Validations(aquariumNotIn, sqlNotIn, proclaimNotIn);

            //File Output
            Console.WriteLine();
            FileOutput(aquariumNotIn, sqlNotIn, proclaimNotIn);

            Console.WriteLine();
            Console.WriteLine("==============================");
            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        #region READ DATA SET       
        private static List<string> ReadDataSet(string fileName, string name)
        {
            Console.Write("Reading {0} data{1}", name, new string('.', 10-name.Length));
            var fileContent = File.ReadAllLines(fileName);
            var dataSet = new List<string>();
            foreach (var x in fileContent) if (!string.IsNullOrWhiteSpace(x)) dataSet.Add(x);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\tFound {0} Lead(s).", dataSet.Count);
            Console.ForegroundColor = ConsoleColor.White;
            return dataSet;
        }
        #endregion

        #region CHECK        
        private static CheckOutput Check(List<string> mainSet, string mainSetNane, List<string> checkSet1, string checkSet1Name, List<string> checkSet2, string checkSet2Name)
        {
            var output = new CheckOutput(mainSet);
            Console.WriteLine("{0} Check:", mainSetNane);
            foreach (var m in mainSet)
            {
                if (!InDataSet(m, checkSet1)) output.Set1.Add(m);
                if (!InDataSet(m, checkSet2)) output.Set2.Add(m);
            }

            Console.ForegroundColor = output.Set1.Count > 0 ? ConsoleColor.Yellow : ConsoleColor.Green;
            Console.WriteLine("\t>> {0} not in {1}", output.Set1.Count, checkSet1Name);
            Console.ForegroundColor = ConsoleColor.White;

            Console.ForegroundColor = output.Set2.Count > 0 ? ConsoleColor.Yellow : ConsoleColor.Green;
            Console.WriteLine("\t>> {0} not in {1}", output.Set2.Count, checkSet2Name);
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();

            return output;
        }
        #endregion

        #region IN DATA SET       
        private static bool InDataSet(string s, List<string> dataSet)
        {
            foreach (var x in dataSet)
            {
                if (string.Equals(s, x)) return true;
            }
            return false;
        }
        #endregion

        #region VALIDAIONS
        private static void Validations(CheckOutput aquarium, CheckOutput sql, CheckOutput proclaim)
        {
            const int offset = 75;

            //===== Rules =====
            Console.WriteLine("----- Validations -----");

            //1. All leads from Aquarium needs to be in the SQL
            var t1 = "Test 1 - All leads from Aquarium are in the SQL";
            Console.Write(t1 + new string('.', offset - t1.Length) + " ");
            Console.ForegroundColor = aquarium.Set1.Count > 0 ? ConsoleColor.Red : ConsoleColor.Green;
            var test1Correct = aquarium.Set1.Count > 0 ? "FAILED" : "PASSED";
            Console.WriteLine(test1Correct);
            Console.ForegroundColor = ConsoleColor.White;

            //2. All leads from Aquarium that are not in Proclaim needs to be in SQL:
            var t2 = "Test 2 - All leads from Aquarium that are not in Proclaim are in the SQL";
            Console.Write(t2 + new string('.', offset - t2.Length) + " ");
            var correct = true;
            foreach (var x in aquarium.Set2) if (!InDataSet(x, aquarium.Main)) correct = false;
            Console.ForegroundColor = correct ? ConsoleColor.Green : ConsoleColor.Red;
            var test2Correct = aquarium.Set1.Count > 0 ? "PASSED" : "FAILED";
            Console.WriteLine(test2Correct);
            Console.ForegroundColor = ConsoleColor.White;

            //3. All leads from SQL needs to be in the Aquarium
            var t3 = "Test 3 - All leads from SQL are in the Aquarium";
            Console.Write(t3 + new string('.', offset - t3.Length) + " ");
            Console.ForegroundColor = sql.Set1.Count > 0 ? ConsoleColor.Red : ConsoleColor.Green;
            var test3Correct = sql.Set1.Count > 0 ? "FAILED" : "PASSED";
            Console.WriteLine(test3Correct);
            Console.ForegroundColor = ConsoleColor.White;

            //4. All leads from Proclaim needs to be in the SQL
            var t4 = "Test 4 - All leads from Proclaim are in the SQL";
            Console.Write(t4 + new string('.', offset - t4.Length) + " ");
            Console.ForegroundColor = proclaim.Set2.Count > 0 ? ConsoleColor.Red : ConsoleColor.Green;
            var test4Correct = proclaim.Set2.Count > 0 ? "FAILED" : "PASSED";
            Console.WriteLine(test4Correct);
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion

        #region FILE OUTPUT
        private static void FileOutput(CheckOutput aquarium, CheckOutput sql, CheckOutput proclaim)
        {
            Console.Write("Outputing files...");
            File.WriteAllLines("_Aquarium-NotIn-SQL.txt", aquarium.Set1);
            File.WriteAllLines("_Aquarium-NotIn-Proclaim.txt", aquarium.Set2);

            File.WriteAllLines("_SQL-NotIn-Aquarium.txt", sql.Set1);
            File.WriteAllLines("_SQL-NotIn-Proclaim.txt", sql.Set2);

            File.WriteAllLines("_Proclaim-NotIn-Aquarium.txt", proclaim.Set1);
            File.WriteAllLines("_Proclaim-NotIn-SQL.txt", proclaim.Set2);
            Console.WriteLine("Completed!");
        }
        #endregion

        #region CheckOutput HELP CLASS        
        private class CheckOutput
        {
            public List<string> Main { get; private set; }
            public List<string> Set1 { get; private set; }
            public List<string> Set2 { get; private set; }

            public CheckOutput(List<string> main)
            {
                Main = main;
                Set1 = new List<string>();
                Set2 = new List<string>();
            }
        }
        #endregion
    }
}
