using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LeadsImporter.Lib.Log;
using LeadsImporter.Lib.Setting;

namespace LeadsImporter.Lib.Sql
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

        #region GET ALL EXCEPTIONS
        public List<SqlDataExceptionObject> GetAllExceptions()
        {
            var list = new List<SqlDataExceptionObject>();

            try
            {
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("GetAllExceptions", conn) { CommandType = CommandType.StoredProcedure };
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new SqlDataExceptionObject(
                                (int) rdr["Id"],
                                (DateTime) rdr["DateTime"],
                                rdr["Type"].ToString(),
                                rdr["LeadId"].ToString(),
                                rdr["CustomerId"].ToString(),
                                rdr["LenderId"].ToString(),
                                (DateTime) rdr["LoanDate"],
                                (DateTime) rdr["LeadCreated"],
                                rdr["ExceptionType"].ToString(),
                                rdr["ExceptionDescription"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> GetAllExceptions:", ex);
            }
            
            return list;
        }
        #endregion

        #region GET ALL DATA
        public List<SqlDataObject> GetAllData()
        {
            var list = new List<SqlDataObject>();

            try
            {
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("GetAllData", conn) { CommandType = CommandType.StoredProcedure };
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new SqlDataObject(
                                (int)rdr["Id"],
                                (DateTime)rdr["DateTime"],
                                rdr["Type"].ToString(),
                                rdr["LeadId"].ToString(),
                                rdr["CustomerId"].ToString(),
                                rdr["LenderId"].ToString(),
                                (DateTime)rdr["LoanDate"],
                                (DateTime)rdr["LeadCreated"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> GetAllData:", ex);
            }

            return list;
        }
        #endregion

        #region DUPLICATES CHECK (NOT NEEDED?)
        public int? DuplicatesCheck(string customerId, string lenderId, DateTime loanDate)
        {
            try
            {
                _logger.AddInfo($"SqlManager >>> DuplicatesCheck: Checking for duplicates (CustomerId: {customerId}, LenderId: {lenderId}, Loan Date: {loanDate.ToString("yyyy-MM-dd")}...");

                int? result = null;
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    using (var cmd = new SqlCommand("DuplicatesCheck", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Prep parameters
                        cmd.Parameters.Add("@CustomerId", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@LenderId", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@LoanDate", SqlDbType.Date);
                        cmd.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;

                        //Set parameters
                        cmd.Parameters["@CustomerId"].Value = customerId;
                        cmd.Parameters["@LenderId"].Value = lenderId;
                        cmd.Parameters["@LoanDate"].Value = loanDate;

                        //Open connection and execute stored procedure
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        //Read output value from @NewId
                        result = Convert.ToInt32(cmd.Parameters["@Result"].Value);
                        conn.Close();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> DuplicatesCheck:", ex);
            }

            return null;
        }
        #endregion

        #region INSERT RECORD
        public void InsertRecord(SqlDataObject data)
        {
            try
            {
                if(_settings.SupressSqlUpdates) return;
                _logger.AddInfo($"SqlManager >>> InsertRecord: Inserting new data row...");
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    using (var cmd = new SqlCommand("InsertRecord", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Prep parameters
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@LeadId", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@CustomerId", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@LenderId", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@LoanDate", SqlDbType.Date);
                        cmd.Parameters.Add("@LeadCreated", SqlDbType.Date);

                        //Set parameters
                        cmd.Parameters["@Type"].Value = data.Type;
                        cmd.Parameters["@LeadId"].Value = data.LeadId;
                        cmd.Parameters["@CustomerId"].Value = data.CustomerId;
                        cmd.Parameters["@LenderId"].Value = data.LenderId;
                        cmd.Parameters["@LoanDate"].Value = data.LoanDate;
                        cmd.Parameters["@LeadCreated"].Value = data.LoanDate;

                        //Open connection and execute stored procedure
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        //Close connection
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> InsertRecord:", ex);
            }
        }
        #endregion

        #region INSERT EXCEPTION
        public void InsertException(SqlDataExceptionObject exception)
        {
            try
            {
                if (_settings.SupressSqlUpdates) return;
                _logger.AddInfo($"SqlManager >>> InsertException: Inserting new exception row...");
                using (var conn = new SqlConnection(_settings.SqlConnectionString))
                {
                    using (var cmd = new SqlCommand("InsertException", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //Prep parameters
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@LeadId", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@CustomerId", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@LenderId", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@LoanDate", SqlDbType.Date);
                        cmd.Parameters.Add("@LeadCreated", SqlDbType.Date);
                        cmd.Parameters.Add("@ExceptionType", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@ExceptionDescription", SqlDbType.VarChar);

                        //Set parameters
                        cmd.Parameters["@Type"].Value = exception.Type;
                        cmd.Parameters["@LeadId"].Value = exception.LeadId;
                        cmd.Parameters["@CustomerId"].Value = exception.CustomerId;
                        cmd.Parameters["@LenderId"].Value = exception.LenderId;
                        cmd.Parameters["@LoanDate"].Value = exception.LoanDate;
                        cmd.Parameters["@LeadCreated"].Value = exception.LeadCreated;
                        cmd.Parameters["@ExceptionType"].Value = exception.ExceptionType;
                        cmd.Parameters["@ExceptionDescription"].Value = exception.ExceptionDescription;

                        //Open connection and execute stored procedure
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        //Close connection
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"SqlManager >>> InsertException:", ex);
            }
        }
        #endregion
    }
}
