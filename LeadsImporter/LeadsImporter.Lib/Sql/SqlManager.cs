﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LeadsImporter.Lib.Log;

namespace LeadsImporter.Lib.Sql
{
    public class SqlManager
    {
        private readonly ILogger _logger;
        private readonly Settings.Settings _settings;

        public SqlManager(ILogger logger, Settings.Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

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
                _logger.AddError($"SqlManager >>> GetAllExceptions: {ex.Message}");
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
                _logger.AddError($"SqlManager >>> GetAllData: {ex.Message}");
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
                _logger.AddError($"SqlManager >>> DuplicatesCheck: {ex.Message}");
            }

            return null;
        }
        #endregion

        #region INSERT RECORD
        public void InsertRecord(string type, string leadId, string customerId, string lenderId, DateTime loanDate, DateTime leadCreated)
        {
            try
            {
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
                        cmd.Parameters["@Type"].Value = type;
                        cmd.Parameters["@LeadId"].Value = leadId;
                        cmd.Parameters["@CustomerId"].Value = customerId;
                        cmd.Parameters["@LenderId"].Value = lenderId;
                        cmd.Parameters["@LoanDate"].Value = loanDate;
                        cmd.Parameters["@LeadCreated"].Value = leadCreated;

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
                _logger.AddError($"SqlManager >>> InsertRecord: {ex.Message}");
            }
        }
        #endregion

        #region INSERT EXCEPTION
        public void InsertException(string type, string leadId, string customerId, string lenderId, DateTime loanDate, DateTime leadCreated, string exceptionType, string exceptionDescription)
        {
            try
            {
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
                        cmd.Parameters["@Type"].Value = type;
                        cmd.Parameters["@LeadId"].Value = leadId;
                        cmd.Parameters["@CustomerId"].Value = customerId;
                        cmd.Parameters["@LenderId"].Value = lenderId;
                        cmd.Parameters["@LoanDate"].Value = loanDate;
                        cmd.Parameters["@LeadCreated"].Value = leadCreated;
                        cmd.Parameters["@ExceptionType"].Value = exceptionType;
                        cmd.Parameters["@ExceptionDescription"].Value = exceptionDescription;

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
                _logger.AddError($"SqlManager >>> InsertException: {ex.Message}");
            }
        }
        #endregion
    }
}
