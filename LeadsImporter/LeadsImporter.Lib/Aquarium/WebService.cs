using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;

namespace LeadsImporter.Lib.Aquarium
{
    public class WebService
    {
        private readonly Settings.Settings _settings;
        private readonly ILogger _logger;

        public WebService(Settings.Settings settings, ILogger logger)
        {
            _settings = settings;
            _logger = logger;
        }

        #region FLOW
        public void GetReport()
        {
            try
            {
                //var response = GetReport("38375");//RPPI
                var response = GetReport("38379");//URSC
                var headers = GetReportHeaders(response);
                var dataRows = GetReportDataRows(response);
                var report = new ReportData(headers, dataRows);
            }
            catch (Exception ex)
            {
                _logger.AddError($"WebService >>> GetReport: {ex.Message}");
            }
        }

        private string GetReport(string queryId)
        {
            try
            {
                //Logon
                var logonResponse = CallLogon();
                var sessionKey = GetSessionKey(logonResponse);

                //Report
                var reportResponse = CallRunReport(sessionKey, queryId);
                return reportResponse;
            }
            catch (Exception ex)
            {
                _logger.AddError($"WebService >>> GetReport[{queryId}]: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region WS CALLs
        private string CallLogon()
        {
            try
            {
                const string url = "https://aqnet.aquarium-software.com/AquariumSDK/UserManagement.asmx";
                const string action = "https://www.aquarium-software.com/AquariumSDK/Logon";
                var username = _settings.AquariumUsername;
                var password = _settings.AquariumPassword;
                var xml = LogonXml(username, password);
                var soapEnvelopeXml = CreateSoapEnvelope(xml);
                var webRequest = CreateWebRequest(url, action);
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
                _logger.AddError($"WebService >>> CallLogon: {ex.Message}");
            }

            return null;
        }

        private string CallGetReports(string sessionKey, string queryId)
        {
            try
            {
                const string url = "https://aqnet.aquarium-software.com/AquariumSDK/ReportManagement.asmx";
                const string action = "https://www.aquarium-software.com/AquariumSDK/RunReport";
                var username = _settings.AquariumUsername;
                var xml = GetReportXml(username, sessionKey, queryId);
                var soapEnvelopeXml = CreateSoapEnvelope(xml);
                var webRequest = CreateWebRequest(url, action);
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
                _logger.AddError($"WebService >>> CallGetReports: {ex.Message}");
            }

            return null;
        }

        private string CallRunReport(string sessionKey, string queryId)
        {
            try
            {
                const string url = "https://aqnet.aquarium-software.com/AquariumSDK/ReportManagement.asmx";
                const string action = "https://www.aquarium-software.com/AquariumSDK/RunReport";
                var username = _settings.AquariumUsername;
                var xml = RunReportXml(username, sessionKey, queryId);
                var soapEnvelopeXml = CreateSoapEnvelope(xml);
                var webRequest = CreateWebRequest(url, action);
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
                _logger.AddError($"WebService >>> CallRunReport: {ex.Message}");
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
                _logger.AddError($"WebService >>> CreateSoapEnvelope: {ex.Message}");
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
                _logger.AddError($"WebService >>> CreateWebRequest: {ex.Message}");
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
                _logger.AddError($"WebService >>> InsertSoapEnvelopeIntoWebRequest: {ex.Message}");
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
        private string GetSessionKey(string response)
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
                _logger.AddError($"WebService >>> GetSessionKey: {ex.Message}");
            }

            return null;
        }

        private List<string> GetReportHeaders(string response)
        {
            try
            {
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
                _logger.AddError($"WebService >>> GetColumns: {ex.Message}");
            }

            return null;
        }

        private List<ReportDataRow> GetReportDataRows(string response)
        {
            try
            {
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
                    dataRows.Add(new ReportDataRow(data));
                }
                return dataRows;
            }
            catch (Exception ex)
            {
                _logger.AddError($"WebService >>> GetColumns: {ex.Message}");
            }

            return null;
        }
        #endregion
    }
}
