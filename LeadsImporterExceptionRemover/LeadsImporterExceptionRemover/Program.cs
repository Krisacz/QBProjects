using System;
using System.Collections.Generic;

namespace LeadsImporterExceptionRemover
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //Prep
            var logger = new ConsoleLogger();
            var settings = SettingsReader.Read(logger);
            var sqlManager = new SqlManager(logger, settings);

            //SQL check
            if (!sqlManager.SqlConnectionCheck())
            {
                Console.WriteLine("SQL Connection error!");
                Console.ReadKey();
                return;
            }
            
            Console.Clear();
            var customerId = string.Empty;
            while (!ValidateCustomerId(customerId))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Aquarium CustomerId: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                customerId = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine($"Fetching exceptions for CustomerId = {customerId}...");
            var list = sqlManager.GetSpecificException(customerId);
            Console.WriteLine($"Found {list.Count} exception(s).");
            if (list.Count > 0)
            {
                Console.WriteLine();
                RenderExceptionsList(list);
                Console.WriteLine();

                var choice = string.Empty;
                while (!ValidateIndexChoice(choice, list.Count))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Please select ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("exception number ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("to remove: ");
                    choice = Console.ReadLine();
                }

                if (choice != null)
                {
                    var index = int.Parse(choice);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Console.WriteLine("Selected exception:");
                    RenderException(list[index], index);
                    
                    var answer = string.Empty;
                    while (!ValidateAnswer(answer))
                    {
                        Console.Write($"Is this correct? (");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[Y] - REMOVE EXCEPTION");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" [N] - CANCEL/EXIT): ");
                        answer = Console.ReadLine();
                    }

                    if (answer != null && answer.ToLower() == "y")
                    {
                        var id = list[index].Id;
                        sqlManager.RemoveSpecificException(id);
                        Console.WriteLine("Exception removed.");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Press any key to exit.");
            Console.ReadKey();
        }

        #region INPUT VALIDATION
        private static bool ValidateAnswer(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer)) return false;
            if (answer.Length != 1) return false;
            if (answer.ToLower() != "y" && answer.ToLower() != "n") return false;
            return true;
        }

        private static bool ValidateIndexChoice(string choice, int count)
        {
            if (string.IsNullOrWhiteSpace(choice)) return false;
            int index;
            if (!int.TryParse(choice, out index)) return false;
            return index >= 0 && index <= count - 1;
        }

        private static bool ValidateCustomerId(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return false;
            if (customerId.Length != 8) return false;
            int dummy;
            return int.TryParse(customerId, out dummy);
        }
        #endregion

        #region RENDER
        private static void RenderExceptionsList(IReadOnlyList<SqlDataExceptionObject> list)
        {
            for (var index = 0; index < list.Count; index++)
            {
                var e = list[index];
                RenderException(e, index);
            }
        }

        private static void RenderException(SqlDataExceptionObject exception, int index)
        {
            //Counter
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{index}) ");

            //Type
            WriteValue("Type", exception.Type, 4);
            //LeadId
            WriteValue("LeadId", exception.LeadId, 8);
            //LenderId
            WriteValue("LenderId", exception.LenderId, 6);
            //Loan Date
            WriteValue("Loan Date", exception.LoanDate.ToString("dd-MM-yyyy"), 10);
            //lead Created
            WriteValue("Lead Created", exception.LeadCreated.ToString("dd-MM-yyyy"), 10);
            //Exception Type
            WriteValue("Exception Type", exception.ExceptionType, 10);
            //Exception Desc
            WriteValue("Exception Desc", exception.ExceptionDescription, 0);

            //End
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        private static void WriteValue(string desc, string value, int minLen)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{desc}: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            var correctedValue = string.IsNullOrWhiteSpace(value) ? new string(' ', minLen) : value;
            Console.Write($"{correctedValue}\t");
        }
        #endregion
    }
}
