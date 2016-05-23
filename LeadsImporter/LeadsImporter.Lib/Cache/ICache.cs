using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsImporter.Lib.Cache
{
    public interface ICache
    {
        void Clear();
        void Store(string data);
        string Get();
    }
}
