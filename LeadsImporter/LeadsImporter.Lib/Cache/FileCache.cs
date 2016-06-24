using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Cache
{
    public class FileCache : ICache
    {
        private readonly ILogger _logger;
        private readonly string _tempCachePath = @"Temp\\";

        public FileCache(ILogger logger)
        {
            _logger = logger;
        }

        #region CLEAR
        public void Clear()
        {
            try
            {
                _logger.AddDetailedLog($"FileCache >>> Clear: Clearing all cache data...");
                var di = new DirectoryInfo(_tempCachePath);
                foreach (var file in di.GetFiles()) file.Delete();
                foreach (var dir in di.GetDirectories()) dir.Delete(true);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Clear:", ex);
            }
        }
        #endregion

        #region STORE
        public void Store(string type, ReportData data)
        {
            try
            {
                _logger.AddDetailedLog($"FileCache >>> Store: Caching data for {type}...");
                Directory.CreateDirectory(_tempCachePath);
                var serializer = new XmlSerializer(typeof(ReportData));
                var xmlWriterSettings = new XmlWriterSettings {Indent = true, IndentChars = "  ", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace };
                using (var stringWriter = new StringWriter())
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, data);
                    var xml = stringWriter.ToString();
                    var fileName = $"{type}.xml";
                    var fullPath = Path.Combine(_tempCachePath, fileName);
                    File.WriteAllText(fullPath, xml);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Store[{type}]:", ex);
            }
        }

        public void StoreExceptions(string type, ReportDataExceptions exceptions)
        {
            try
            {
                _logger.AddDetailedLog($"FileCache >>> Store: Caching data for {type}...");
                Directory.CreateDirectory(_tempCachePath);
                var serializer = new XmlSerializer(typeof(ReportDataExceptions));
                var xmlWriterSettings = new XmlWriterSettings { Indent = true, IndentChars = "  ", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace };
                using (var stringWriter = new StringWriter())
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, exceptions);
                    var xml = stringWriter.ToString();
                    var fileName = $"{type}_exceptions.xml";
                    var fullPath = Path.Combine(_tempCachePath, fileName);
                    File.WriteAllText(fullPath, xml);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Store[{type}]:", ex);
            }
        }

        #endregion

        #region GET
        public ReportData Get(string type)
        {
            try
            {
                _logger.AddDetailedLog($"FileCache >>> Get: Getting data for {type}...");
                ReportData data = null;
                var xmlSerializer = new XmlSerializer(typeof(ReportData));
                var fileName = $"{type}.xml";
                var fullPath = Path.Combine(_tempCachePath, fileName);
                var streamReader = new StreamReader(fullPath);
                data = (ReportData)xmlSerializer.Deserialize(streamReader);
                streamReader.Close();
                return data;
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Get[{type}]:", ex);
            }

            return null;
        }

        public ReportDataExceptions GetExceptions(string type)
        {
            try
            {
                _logger.AddDetailedLog($"FileCache >>> Get: Getting data for {type}...");
                ReportDataExceptions data = null;
                var xmlSerializer = new XmlSerializer(typeof(ReportDataExceptions));
                var fileName = $"{type}_exceptions.xml";
                var fullPath = Path.Combine(_tempCachePath, fileName);
                var streamReader = new StreamReader(fullPath);
                data = (ReportDataExceptions)xmlSerializer.Deserialize(streamReader);
                streamReader.Close();
                return data;
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> GetExceptions[{type}]:", ex);
            }

            return null;
        }

        #endregion
    }
}