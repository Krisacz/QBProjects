using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Cache
{
    public class InMemoryCache : ICache
    {
        private readonly Dictionary<string, ReportData> _cache = new Dictionary<string, ReportData>();
        private readonly ILogger _logger;

        public InMemoryCache(ILogger logger)
        {
            _logger = logger;
        }

        #region CLEAR
        public void Clear()
        {
            try
            {
                _logger.AddInfo($"InMemoryCache >>> Clear: Clearing all cache data...");
                _cache.Clear();
            }
            catch (Exception ex)
            {
                _logger.AddError($"InMemoryCache >>> Store: {ex.Message}");
            }
        }
        #endregion

        #region STORE
        public void Store(string type, ReportData data)
        {
            try
            {
                _logger.AddInfo($"FileCache >>> Store: Caching data...");
                if (_cache.ContainsKey(type))
                {
                    _cache[type] = data;
                }
                else
                {
                    _cache.Add(type, data);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Store: {ex.Message}");
            }
        }
        #endregion

        #region GET
        public ReportData Get(string type)
        {
            try
            {
                _logger.AddInfo($"FileCache >>> Get[{type}]: Getting data...");
                return _cache[type];
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Get[{type}]: {ex.Message}");
            }

            return null;
        }
        #endregion
    }
}