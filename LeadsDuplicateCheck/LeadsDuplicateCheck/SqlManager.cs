using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LeadsDuplicateCheck
{
    public class SqlManager
    {
        private readonly ILogger _logger;
        private readonly Settings _settings;

        public SqlManager(ILogger logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public List<ProclaimClaimData> GetAllProclaimData()
        {
            _logger.AddInfo("SqlManager >>> GetAllProclaimData: Getting Proclaim data...");

            var list = new List<ProclaimClaimData>();

            try
            {
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("GetProclaimData", conn) { CommandType = CommandType.StoredProcedure };
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var pd = new ProclaimClaimData(_logger)
                                .Parse(rdr["CaseRef"].ToString(), rdr["FirstName"].ToString(), rdr["Surname"].ToString(), rdr["Postcode"].ToString(),
                                    rdr["DOB"].ToString(), rdr["URSCLoanDate"].ToString(), rdr["RPPILoanDate"].ToString(), rdr["Lender"].ToString());
                            list.Add(pd);
                        }
                    }
                }
                _logger.AddInfo($"SqlManager >>> GetAllProclaimData: Retrieved {list.Count} claim(s).");
                return list;
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> GetAllProclaimData:", ex);
            }

            return null;
        }
    }
}
