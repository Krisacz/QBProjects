using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LeadsImporterCrossChecker
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

        public List<string> GetAllLeads()
        {
            var list = new List<string>();

            try
            {
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("GetAllLeadsData", conn) { CommandType = CommandType.StoredProcedure };
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read()) list.Add(rdr["LeadId"].ToString());
                    }

                    var cmd2 = new SqlCommand("GetAllLeadsExceptions", conn) { CommandType = CommandType.StoredProcedure };
                    using (var rdr = cmd2.ExecuteReader())
                    {
                        while (rdr.Read()) list.Add(rdr["LeadId"].ToString());
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> GetAllLeadsData:", ex);
            }

            return null;
        }
    }
}
