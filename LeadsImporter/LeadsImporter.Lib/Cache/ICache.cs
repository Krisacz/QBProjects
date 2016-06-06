﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Cache
{
    public interface ICache
    {
        void Clear();
        void Store(string type, ReportData data);
        ReportData Get(string type);
    }
}
