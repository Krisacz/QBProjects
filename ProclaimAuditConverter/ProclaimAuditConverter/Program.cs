using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using FileHelpers;

namespace ProclaimAuditConverter
{
    class Program
    {
        private static string _processedPath = ".\\Processed\\";
        private static string _outputPath = ".\\Output\\";

        static void Main(string[] args)
        {
            //Prep
            if(!Directory.Exists(_processedPath)) Directory.CreateDirectory(_processedPath);
            if(!Directory.Exists(_outputPath)) Directory.CreateDirectory(_outputPath);

            //Get CSV files to process
            Console.WriteLine("Scanning current directory...");
            var files = new List<string>();
            files = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory).ToList();
            var csvFiles = new List<string>();
            foreach (var file in files)
            {
                if (file.ToLower().EndsWith("csv"))
                {
                    csvFiles.Add(file);
                }            
            }
            Console.WriteLine("Found {0} csv file(s).", csvFiles.Count);
            
            //Process CSV files            
            var counter = 1;
            foreach (var csvFile in csvFiles)
            {
                var csvFileJustName = Path.GetFileName(csvFile);
                Console.WriteLine("Processing file {0}/{1} ({2})...", counter, csvFiles.Count, csvFileJustName);

                var engine = new FileHelperEngine<AuditRecord>();                
                var records = engine.ReadFile(csvFile);               
                var audit = new Audit();
                var lineCounter = 1;
                foreach (var record in records)
                {
                    Console.WriteLine("\tProcessing record {0}/{1}...", lineCounter, records.Length);
                    audit.AddCase(record);
                    lineCounter++;
                }
                Console.WriteLine("\t(Found {0} unique case(s))", audit.Cases.Count);
                Console.WriteLine();

                #region USER INPUT
                //User input - Changes type
                char changesType = new char();
                while(changesType != 'L' && changesType != 'l' && changesType != 'O' &&changesType != 'o')
                {
                    Console.Write("Output (L)atest changes or (O)ldest: ");
                    var k = Console.ReadKey();
                    changesType = k.KeyChar;
                    Console.WriteLine();
                }

                //User input - Output New Value
                char outputNewValue = new char();
                while (outputNewValue != 'Y' && outputNewValue != 'y' && outputNewValue != 'N' && outputNewValue != 'n')
                {
                    Console.Write("Output field new value? (Y/N): ");
                    var k = Console.ReadKey();
                    outputNewValue = k.KeyChar;
                    Console.WriteLine();
                }

                //User input - Output Date
                char outputDate = new char();
                while (outputDate != 'Y' && outputDate != 'y' && outputDate != 'N' && outputDate != 'n')
                {
                    Console.Write("Output date of change? (Y/N): ");
                    var k = Console.ReadKey();
                    outputDate = k.KeyChar;
                    Console.WriteLine();
                }

                //User input - Output Time
                char outputTime = new char();
                while (outputTime != 'Y' && outputTime != 'y' && outputTime != 'N' && outputTime != 'n')
                {
                    Console.Write("Output time of change? (Y/N): ");
                    var k = Console.ReadKey();
                    outputTime = k.KeyChar;
                    Console.WriteLine();
                }

                //User input - Output By
                char outputBy = new char();
                while (outputBy != 'Y' && outputBy != 'y' && outputBy != 'N' && outputBy != 'n')
                {
                    Console.Write("Output change by name? (Y/N): ");
                    var k = Console.ReadKey();
                    outputBy = k.KeyChar;
                    Console.WriteLine();
                }

                //User input - Output Change Source
                char outputSource = new char();
                while (outputSource != 'Y' && outputSource != 'y' && outputSource != 'N' && outputSource != 'n')
                {
                    Console.Write("Output source of change? (Y/N): ");
                    var k = Console.ReadKey();
                    outputSource = k.KeyChar;
                    Console.WriteLine();
                }

                //User input - File Name
                Console.Write("Output file name: ");
                var fileName = Console.ReadLine();
                fileName = ValidateFileName(fileName);
                #endregion

                //Save output file
                Console.WriteLine();
                var outputFile = SaveFile(audit, fileName, changesType == 'L' || changesType == 'l', outputNewValue == 'Y' || outputNewValue == 'y', outputDate == 'Y' || outputDate == 'y',
                    outputTime == 'Y' || outputTime == 'y', outputBy == 'Y' || outputBy == 'y', outputSource == 'Y' || outputSource == 'y');
                Console.WriteLine("Output file \"{0}\" saved in \"{1}\" folder as \"{2}\"", fileName, new DirectoryInfo(_outputPath).Name, outputFile);
                
                //Move work file                
                var newCsvFilePath = Path.Combine(_processedPath, csvFileJustName);
                var workFile = ForceFileMove(csvFile, newCsvFilePath);
                Console.WriteLine("Work file \"{0}\" saved in \"{1}\" folder as \"{2}\"", csvFileJustName, new DirectoryInfo(_processedPath).Name, workFile);


                Console.WriteLine("---------------------------------------------------------------------");
                counter++;
            }
                        
            Console.WriteLine();
            Console.WriteLine("All files processed.");
            Console.WriteLine("Press any key to EXIT.");
            Console.ReadKey();
        }

        #region HELP METHODS
        private static string ValidateFileName(string input)
        {
            var validCharacters = new List<char>(){ 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                    'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                                                    '1','2','3','4','5','6','7','8','9','0', '!', '.', '_', '-'};
            var output = "";
            foreach (char c in input)
            {
                var isValid = false;
                foreach (char vc in validCharacters)
                {
                    if (c == vc) isValid = true;
                }

                if (isValid) output += c;
            }

            return output;
        }

        private static string SaveFile(Audit audit, string fileName, bool latestChanges, bool outNew, bool outDate, bool outTime, bool outBy, bool outSource)
        { 
            var fileNameWithExt = string.Format("{0}.csv", fileName);
            var fullPath = Path.Combine(_outputPath, fileNameWithExt);
            var outputLines = new List<string>();

            foreach (var c in audit.Cases)
            {
                var change = latestChanges ? c.GetLatest() : c.GetOldest();
                outputLines.Add(ConvertChangeToString(c.CaseRef, change, outNew, outDate, outTime, outBy, outSource));    
            }            

            var file = ForceWriteAllLines(fullPath, outputLines);
            return file;
        }
               
        private static string ForceFileMove(string from, string to)
        {
            var actualFile = "";

            if (File.Exists(to))
            {
                var path = Path.GetDirectoryName(to);
                var fileName = Path.GetFileNameWithoutExtension(to);
                var ext = Path.GetExtension(to);

                var postfix = 1;
                var fullPath = string.Format("{0}\\{1}{2}{3}", path, fileName, postfix, ext);
                while (File.Exists(fullPath))
                {
                    postfix++;
                    fullPath = string.Format("{0}\\{1}{2}{3}", path, fileName, postfix, ext);
                }

                File.Move(from, fullPath);
                actualFile = string.Format("{0}{1}{2}", fileName, postfix, ext);
            }
            else
            {
                File.Move(from, to);
                actualFile = Path.GetFileName(to);
            }

            return actualFile;
        }

        private static string ForceWriteAllLines(string destination, IEnumerable<string> lines)
        {
            var actualFile = "";

            if (File.Exists(destination))
            {
                var path = Path.GetDirectoryName(destination);
                var fileName = Path.GetFileNameWithoutExtension(destination);
                var ext = Path.GetExtension(destination);

                var postfix = 1;
                var fullPath = string.Format("{0}\\{1}{2}{3}", path, fileName, postfix, ext);
                while (File.Exists(fullPath))
                {
                    postfix++;
                    fullPath = string.Format("{0}\\{1}{2}{3}", path, fileName, postfix, ext);
                }

                File.WriteAllLines(fullPath, lines);
                actualFile = string.Format("{0}{1}{2}", fileName, postfix, ext);
            }
            else
            {
                File.WriteAllLines(destination, lines);
                actualFile = Path.GetFileName(destination);
            }

            return actualFile;
        }

        private static string ConvertChangeToString(string caseRef, Change change, bool outNew, bool outDate, bool outTime, bool outBy, bool outSource)
        {
            var outputString = caseRef.Replace(Environment.NewLine, string.Empty);
            if (outNew) outputString += "," + change.NewValue.Replace(Environment.NewLine, string.Empty);
            if (outDate) outputString += "," + change.Date.ToString("dd/MM/yyyy").Replace(Environment.NewLine, string.Empty);
            if (outTime) outputString += "," + string.Format("{0}:{1}:{2}", change.Time.Hours.ToString("D2"), change.Time.Minutes.ToString("D2"), change.Time.Seconds.ToString("D2")).Replace(Environment.NewLine, string.Empty);
            if (outBy) outputString += "," + change.By.Replace(Environment.NewLine, string.Empty);
            if (outSource) outputString += "," + change.Source.Replace(Environment.NewLine, string.Empty);
            return outputString;
        }        
        #endregion
    }
}
