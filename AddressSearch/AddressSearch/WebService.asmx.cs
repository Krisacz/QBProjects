using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AddressSearch
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetFullAddress(string number, string postcode)
        {
            const int MaxNumberLength = 4;
            const int MaxPostcodeLength = 8;
            var result = string.Empty;

            //Check input paramaeters length
            if (number != null && number.Length > MaxNumberLength) result = string.Format("ERROR: Number length > {0}", MaxNumberLength);
            if (postcode != null && postcode.Length > MaxPostcodeLength) result = string.Format("ERROR: Postcode length > {0}", MaxPostcodeLength);

            //If no errors - query SQL
            if (!result.StartsWith("ERROR")) result = SqlAccess.GetFullAddress(number ?? string.Empty, postcode.ToUpper());

            //If no errors - format output
            if (!result.StartsWith("ERROR"))
            {
                var fullAddress = string.Empty;
                var parts = result.Split('|');
                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    if (!string.IsNullOrWhiteSpace(part))
                    {
                        fullAddress += part;
                        if (i < parts.Length - 1) fullAddress += Environment.NewLine;
                    }                    
                }
                result = fullAddress;
            }

            //Last check
            if (string.IsNullOrWhiteSpace(result)) result = string.Format("ERROR: Incorrect number/postcode.");

            //Return
            return result;
        }
    }
}
