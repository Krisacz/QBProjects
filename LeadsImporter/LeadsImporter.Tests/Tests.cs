using System.Collections.Generic;
using System.Dynamic;
using LeadsImporter.Lib.Cache;
using LeadsImporter.Lib.DataAccessor;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Report;
using LeadsImporter.Lib.Setting;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace LeadsImporter.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestTestFramework()
        {
            var a = 100;
            var b = 200;
            var c = a + b;
            Assert.AreEqual(c,300);
        }

        #region CACHE TESTS
        [TestFixture]
        public class CacheTest
        {
            private static class TestReportDataFactory
            {
                public static ReportData Get()
                {
                    return new ReportData()
                    {
                        QueryId = 111,
                        Headers = new List<string>() { "LeadID", "CustomerID", "Lender ID", "Date of Loan", "WhenCreate", "SomeData1" },
                        Rows = new List<ReportDataRow>()
                        {
                            new ReportDataRow() { Data = new List<string>() { "111", "1111", "11111", "2016-01-25", "25/01/2016 11:11:11", "TestData1" }},
                        }
                    };
                }
            }

            [Test]
            public void FileCacheGetTest()
            {
                var consoleLogger = new ConsoleLogger();
                var fileCache = new FileCache(consoleLogger);
                var testReportData = TestReportDataFactory.Get();
                fileCache.Store("test", testReportData);

                var data = fileCache.Get("test");

                Assert.AreEqual(data.QueryId, testReportData.QueryId);

                //Fail
                //testReportData.Headers[0] = "error";
                Assert.AreEqual(data.Headers, testReportData.Headers);

                //Fail
                //testReportData.Rows[0].Data[0] = "erorr";
                Assert.AreEqual(data.Rows[0].Data, testReportData.Rows[0].Data);
            }

            [Test]
            public void InMemoryCacheGetTest()
            {
                var consoleLogger = new ConsoleLogger();
                var fileCache = new InMemoryCache(consoleLogger);
                var testReportData = TestReportDataFactory.Get();
                fileCache.Store("test", testReportData);

                var d = fileCache.Get("test");
                var fromFactory = TestReportDataFactory.Get();

                Assert.AreEqual(d.QueryId, fromFactory.QueryId);

                //Fail
                //d.Headers[0] = "error";
                Assert.AreEqual(d.Headers, fromFactory.Headers);

                //Fail
                //d.Rows[0].Data[0] = "erorr";
                Assert.AreEqual(d.Rows[0].Data, fromFactory.Rows[0].Data);
            }
        }
        #endregion

        #region DATA ACCESSOR TESTS
        [TestFixture]
        public class DataAccessorTest
        {
            private static class SettingsFactory
            {
                public static Settings Get(string login = null)
                {
                    var l = login ?? "k.sowinski@queensbeck.com";
                    return new Settings("", "", l, "Sb3T023&",
                        "https://aqnet.aquarium-software.com/AquariumSDK/UserManagement.asmx",
                        "https://www.aquarium-software.com/AquariumSDK/Logon",
                        "https://aqnet.aquarium-software.com/AquariumSDK/ReportManagement.asmx",
                        "https://www.aquarium-software.com/AquariumSDK/RunReport", true, false);
                }
            }

            [Test]
            public void AquariumLogonTest()
            {
                var logger = new ConsoleLogger();

                //Fail
                //var settings = SettingsFactory.Get("error");
                var settings = SettingsFactory.Get();

                var aquarium = new AquariumWebService(logger, settings);
                var logonResponse = aquarium.CallLogon();
                var sessionKey = aquarium.GetSessionKey(logonResponse);
                Assert.AreNotEqual(sessionKey, null);
            }
        }
        #endregion
    }
}
