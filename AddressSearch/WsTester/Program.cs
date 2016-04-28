using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WsTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var ws = new AddressSearchWS.WebService1SoapClient();
            var result = ws.GetFullAddress("89", "SK14 6JQ");
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
