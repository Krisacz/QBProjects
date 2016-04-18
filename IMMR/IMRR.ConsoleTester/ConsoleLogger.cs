using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMRR.Lib;

namespace IMRR.ConsoleTester
{
    class ConsoleLogger : ILogger
    {
        public void AddEmptyLine()
        {
            Console.WriteLine();
        }

        public void AddError(string error)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("{0} [ERROR] \t{1}", dt, error);
        }

        public void AddInfo(string info)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("{0} [INFO] \t{1}", dt, info);
        }
    }
}
