using System;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Cache
{
    //TODO InMemoryCache implementation
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