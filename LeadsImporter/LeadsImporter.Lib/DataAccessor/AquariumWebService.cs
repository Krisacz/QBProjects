using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Xml;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Setting;

namespace LeadsImporter.Lib.DataAccessor
{
    public class AquariumWebService : IDataAccessor
    {
        private readonly ILogger _logger;
        private readonly Settings _settings;

        public AquariumWebService(ILogger logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        #region FLOW
        public ReportData GetReportData(int reportId)
        {
            try
            {
                _logger.AddInfo($"AquariumWebService >>> GetReportData[{reportId}]: Starting AquariumWebService calls...");
                var response = GetReport(reportId);
                var headers = GetReportHeaders(response);
                var dataRows = GetReportDataRows(response);
                _logger.AddInfo($"AquariumWebService >>> GetReportData[{reportId}]: Received {headers.Count} headers and {dataRows.Count} data rows.");
                return new ReportData() {QueryId =  reportId, Headers = headers, Rows = dataRows};
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> GetReportData:", ex);
            }

            return null;
        }

        private string GetReport(int queryId)
        {
            try
            {
                //Logon
                _logger.AddDetailedLog($"AquariumWebService >>> GetReport[{queryId}]: Calling Logon method...");
                var logonResponse = CallLogon();
                var sessionKey = GetSessionKey(logonResponse);

                //Report
                _logger.AddDetailedLog($"AquariumWebService >>> GetReport[{queryId}]: Calling GetReport method...");
                var reportResponse = CallRunReport(sessionKey, queryId.ToString());
                return reportResponse;
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> GetReport[{queryId}]:", ex);
            }

            return null;
        }
        #endregion

        #region WS CALLs

        public string CallLogon()
        {
            try
            {
                var username = _settings.AquariumUsername;
                var password = _settings.AquariumPassword;
                var xml = LogonXml(username, password);
                var soapEnvelopeXml = CreateSoapEnvelope(xml);
                var webRequest = CreateWebRequest(_settings.AquariumLogonUrl, _settings.AquariumLogonAction);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                var asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string soapResult;
                using (var webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (var rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }
                }

                return soapResult;
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> CallLogon:", ex);
            }

            return null;
        }

        private string CallRunReport(string sessionKey, string queryId)
        {
            try
            {
                var username = _settings.AquariumUsername;
                var xml = RunReportXml(username, sessionKey, queryId);
                var soapEnvelopeXml = CreateSoapEnvelope(xml);
                var webRequest = CreateWebRequest(_settings.AquariumRunReportUrl, _settings.AquariumRunReportAction);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                var asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                string soapResult;
                using (var webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (var rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }
                }

                return soapResult;
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> CallRunReport:", ex);
            }

            return null;
        }
        #endregion

        #region CONSTRUCTING SOAP REQUEST
        private XmlDocument CreateSoapEnvelope(string xml)
        {
            try
            {
                var soapEnvelop = new XmlDocument();
                soapEnvelop.LoadXml(xml);
                return soapEnvelop;
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> CreateSoapEnvelope:", ex);
            }

            return null;
        }

        private HttpWebRequest CreateWebRequest(string url, string action)
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add("SOAPAction", action);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                return webRequest;
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> CreateWebRequest:", ex);
            }

            return null;
        }

        private void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            try
            {
                using (var stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> InsertSoapEnvelopeIntoWebRequest:", ex);
            }
        }
        #endregion

        #region XMLs
        private static string LogonXml(string username, string password)
        {
            var xml = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">";
            xml += @"<soap:Body>";
            xml += @"<Logon xmlns=""https://www.aquarium-software.com/AquariumSDK/"">";
            xml += @"<username>" + SecurityElement.Escape(username) + "</username>";
            xml += @"<password>" + SecurityElement.Escape(password) + "</password>";
            xml += @"</Logon>";
            xml += @"</soap:Body>";
            xml += @"</soap:Envelope>";

            return xml;
        }

        private static string GetReportXml(string username, string sessionKey, string queryId)
        {
            var xml = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">";
            xml += @"<soap:Header>";
            xml += @"<SessionDetails xmlns=""https://www.aquarium-software.com/AquariumSDK/"">";
            xml += @"<Username>" + SecurityElement.Escape(username) + "</Username>";
            xml += @"<SessionKey>" + SecurityElement.Escape(sessionKey) + "</SessionKey>";
            xml += @"<ThirdPartySystemId>0</ThirdPartySystemId>";
            xml += @"</SessionDetails>";
            xml += @"</soap:Header>";
            xml += @"<soap:Body>";
            xml += @"<GetReport xmlns=""https://www.aquarium-software.com/AquariumSDK/"">";
            xml += @"<queryID>" + queryId + "</queryID>";
            xml += @"</GetReport>";
            xml += @"</soap:Body>";
            xml += @"</soap:Envelope>";

            return xml;
        }

        private static string RunReportXml(string username, string sessionKey, string queryId)
        {
            var xml = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">";
            xml += @"<soap:Header>";
            xml += @"<SessionDetails xmlns=""https://www.aquarium-software.com/AquariumSDK/"">";
            xml += @"<Username>" + SecurityElement.Escape(username) + "</Username>";
            xml += @"<SessionKey>" + SecurityElement.Escape(sessionKey) + "</SessionKey>";
            xml += @"<ThirdPartySystemId>0</ThirdPartySystemId>";
            xml += @"</SessionDetails>";
            xml += @"</soap:Header>";
            xml += @"<soap:Body>";
            xml += @"<RunReport xmlns=""https://www.aquarium-software.com/AquariumSDK/"">";
            xml += @"<queryID>" + queryId + "</queryID>";
            xml += @"<parameters/>";
            xml += @"</RunReport>";
            xml += @"</soap:Body>";
            xml += @"</soap:Envelope>";

            return xml;
        }
        #endregion

        #region XML PARSERS

        public string GetSessionKey(string response)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(response);
                var soapBody = xmlDocument.GetElementsByTagName("SessionKey")[0];
                var innerObject = soapBody.InnerXml;
                return innerObject;
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> GetSessionKey:", ex);
            }

            return null;
        }

        private List<string> GetReportHeaders(string response)
        {
            try
            {
                _logger.AddInfo($"AquariumWebService >>> GetReportHeaders: Extracting headers...");
                var columns = new List<string>();
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(response);
                var soapBody = xmlDocument.GetElementsByTagName("Columns")[0];
                var innerObject = soapBody.ChildNodes;
                foreach (XmlNode i in innerObject) columns.Add(i.InnerText);
                return columns;
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> GetReportHeaders:", ex);
            }

            return null;
        }

        private List<ReportDataRow> GetReportDataRows(string response)
        {
            try
            {
                _logger.AddInfo($"AquariumWebService >>> GetReportDataRows: Extracting data rows...");
                var dataRows = new List<ReportDataRow>();
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(response);
                var soapBody = xmlDocument.GetElementsByTagName("Rows")[0];
                var innerObject = soapBody.ChildNodes;
                foreach (XmlNode i in innerObject)
                {
                    var data = new List<string>();
                    foreach (XmlNode d in i.ChildNodes)
                    {
                        data.Add(d.InnerText);
                    }
                    dataRows.Add(new ReportDataRow() {Data = data});
                }
                return dataRows;
            }
            catch (Exception ex)
            {
                _logger.AddError($"AquariumWebService >>> GetReportDataRows:", ex);
            }

            return null;
        }
        #endregion
    }
}
