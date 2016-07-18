using System;
using System.Collections.Generic;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Cache
{
    public class InMemoryCache : ICache
    {
        private readonly Dictionary<string, ReportData> _data = new Dictionary<string, ReportData>();
        private readonly Dictionary<string, ReportDataExceptions> _exceptions = new Dictionary<string, ReportDataExceptions>();
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
                _logger.AddDetailedLog($"InMemoryCache >>> Clear: Clearing all cache data...");
                _data.Clear();
                _exceptions.Clear();
            }
            catch (Exception ex)
            {
                _logger.AddError($"InMemoryCache >>> Store:", ex);
            }
        }
        #endregion

        #region STORE
        public void Store(string type, ReportData data)
        {
            try
            {
                //_logger.AddDetailedLog($"InMemoryCache >>> Store: Caching data for {type}...");
                if (_data.ContainsKey(type))
                {
                    _data[type] = data;
                }
                else
                {
                    _data.Add(type, data);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"InMemoryCache >>> Store[{type}]:", ex);
            }
        }

        public void StoreExceptions(string type, ReportDataExceptions exceptions)
        {
            try
            {
                //_logger.AddDetailedLog($"InMemoryCache >>> Store: Caching data for {type}...");
                if (_exceptions.ContainsKey(type))
                {
                    _exceptions[type] = exceptions;
                }
                else
                {
                    _exceptions.Add(type, exceptions);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"InMemoryCache >>> StoreExceptions[{type}]:", ex);
            }
        }

        #endregion

        #region GET
        public ReportData Get(string type)
        {
            try
            {
                _logger.AddDetailedLog($"InMemoryCache >>> Get: Getting data for {type}...");
                return _data[type];
            }
            catch (Exception ex)
            {
                _logger.AddError($"InMemoryCache >>> Get[{type}]:", ex);
            }

            return null;
        }

        public ReportDataExceptions GetExceptions(string type)
        {
            try
            {
                _logger.AddDetailedLog($"InMemoryCache >>> Get: Getting data for {type}...");
                return _exceptions[type];
            }
            catch (Exception ex)
            {
                _logger.AddError($"InMemoryCache >>> Get[{type}]:", ex);
            }

            return null;
        }

        #endregion
    }
}