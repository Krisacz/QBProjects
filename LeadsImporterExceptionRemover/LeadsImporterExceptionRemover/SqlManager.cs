using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LeadsImporterExceptionRemover
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

        #region SLQ CONNECTION CHECK
        public bool SqlConnectionCheck()
        {
            try
            {
                using (var connection = new SqlConnection(_settings.SqlConnectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region GET SPECIFIC EXCEPTION
        public List<SqlDataExceptionObject> GetSpecificException(string customerId)
        {
            var list = new List<SqlDataExceptionObject>();

            try
            {
                //_logger.AddInfo($"SqlManager >>> GetSpecificException: Getting exception(s) for CustomerId: {customerId}...");
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    using (var cmd = new SqlCommand("GetSpecificException", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Prep parameters
                        cmd.Parameters.Add("@CustomerId", SqlDbType.VarChar, 50);
                        //Set parameters
                        cmd.Parameters["@CustomerId"].Value = customerId;
                        
                        //Open connection and execute stored procedure
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                list.Add(new SqlDataExceptionObject(
                                    (int)rdr["Id"],
                                    (DateTime)rdr["DateTime"],
                                    rdr["Type"].ToString(),
                                    rdr["LeadId"].ToString(),
                                    rdr["CustomerId"].ToString(),
                                    rdr["LenderId"].ToString(),
                                    (DateTime)rdr["LoanDate"],
                                    (DateTime)rdr["LeadCreated"],
                                    rdr["ExceptionType"].ToString(),
                                    rdr["ExceptionDescription"].ToString()));
                            }
                        }
                        conn.Close();
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> GetSpecificException:", ex);
            }

            return null;
        }
        #endregion

        #region REMOVE SPECIFIC EXCEPTION
        public void RemoveSpecificException(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    using (var cmd = new SqlCommand("RemoveSpecificException", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Prep parameters
                        cmd.Parameters.Add("@Id", SqlDbType.Int);
                        //Set parameters
                        cmd.Parameters["@Id"].Value = id;
                        
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> RemoveSpecificException:", ex);
            }
        }
        #endregion
    }
}
