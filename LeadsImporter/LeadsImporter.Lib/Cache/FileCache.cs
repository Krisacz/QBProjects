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
        private readonly Settings.Settings _settings;

        public FileCache(ILogger logger, Settings.Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

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
                _logger.AddError($"FileCache >>> Store: {ex.Message}");
            }
        }

        public void Store(ReportData data)
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
                    var fullPath = Path.Combine(_settings.TempCachePath, "temp.xml");
                    File.WriteAllText(fullPath, xml);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Store: {ex.Message}");
            }
        }

        public ReportData Get(string path)
        {
            try
            {
                _logger.AddInfo($"FileCache >>> Get[{path}]: Getting data...");
                ReportData data = null;
                var xmlSerializer = new XmlSerializer(typeof(ReportData));
                var streamReader = new StreamReader(path);
                data = (ReportData)xmlSerializer.Deserialize(streamReader);
                streamReader.Close();
                return data;
            }
            catch (Exception ex)
            {
                _logger.AddError($"FileCache >>> Get[{path}]: {ex.Message}");
            }

            return null;
        }
    }
}