using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Setting;

namespace LeadsImporter.Lib.Cache
{
    public class FileCache : ICache
    {
        private readonly ILogger _logger;
        private readonly Settings _settings;

        public FileCache(ILogger logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        #region CLEAR
        public void Clear()
        {
            try
            {
                _logger.AddInfo($"FileCache >>> Clear: Clearing all cache data...");
                var di = new DirectoryInfo(_settings.TempCachePath);
                foreach (var file in di.GetFiles()) file.Delete();
                foreach (var dir in di.GetDirectories()) dir.Delete(true);
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Clear: {ex.Message}");
            }
        }
        #endregion

        #region STORE
        public void Store(string type, ReportData data)
        {
            try
            {
                _logger.AddInfo($"FileCache >>> Store: Caching data...");
                Directory.CreateDirectory(_settings.TempCachePath);
                var serializer = new XmlSerializer(typeof(ReportData));
                var xmlWriterSettings = new XmlWriterSettings {Indent = true, IndentChars = "  ", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace };
                using (var stringWriter = new StringWriter())
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, data);
                    var xml = stringWriter.ToString();
                    var fileName = $"{type}.xml";
                    var fullPath = Path.Combine(_settings.TempCachePath, fileName);
                    File.WriteAllText(fullPath, xml);
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
                ReportData data = null;
                var xmlSerializer = new XmlSerializer(typeof(ReportData));
                var fileName = $"{type}.xml";
                var fullPath = Path.Combine(_settings.TempCachePath, fileName);
                var streamReader = new StreamReader(fullPath);
                data = (ReportData)xmlSerializer.Deserialize(streamReader);
                streamReader.Close();
                return data;
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