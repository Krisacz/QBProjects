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
                                    
            if (number.Length > MaxNumberLength) number = number.Substring(0, MaxNumberLength);
            if (postcode.Length > MaxPostcodeLength) postcode = postcode.Substring(0, MaxPostcodeLength);

            var result = SqlAccess.GetFullAddress(number, postcode.ToUpper());

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


            return result;
        }
    }
}
