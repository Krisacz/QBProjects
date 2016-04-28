using System;
using System.Data;
using System.Data.SqlClient;

namespace AddressSearch
{
    public class SqlAccess
    {   
        public static string GetFullAddress(string number, string postcode)
        {
            var result = string.Empty;
            try
            {
                using (var conn = new SqlConnection(@"Server=192.168.2.111,1433\miwh; DataBase=RoyalMailPostCodes; User Id=kssql; Password=Kena654654;"))
                //using (var conn = new SqlConnection(@"Server=192.168.2.111,1433\miwh; DataBase=RoyalMailPostCodes; Trusted_Connection=True;"))
                {
                    conn.Open();
                    var cmd = new SqlCommand("GetFullAddress", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.Add(new SqlParameter("@Number", number));
                    cmd.Parameters.Add(new SqlParameter("@Postcode", postcode));

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result = rdr["Result"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("ERROR: {0}", ex.Message);
            }
            
            return result;
        }
    }
}
