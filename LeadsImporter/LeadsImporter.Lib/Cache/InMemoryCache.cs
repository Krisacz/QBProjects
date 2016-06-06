using System;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Cache
{
    public class InMemoryCache : ICache
    {
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Store(string type, ReportData data)
        {
            throw new NotImplementedException();
        }

        public ReportData Get(string xmlPath)
        {
            throw new NotImplementedException();
        }
    }
}