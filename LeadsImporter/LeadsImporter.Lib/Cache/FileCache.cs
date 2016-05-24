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
            throw new NotImplementedException();
        }

        public void Store(ReportData data)
        {
            try
            {
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

        public string Get()
        {
            throw new NotImplementedException();
        }
    }
}