using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgencyAPI.Models;
using System.Text;

namespace AgencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyLogin : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<JObject> Post(Login login)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {

                var response = dBHandler.AgencyLogin(login.emailaddress);
                if (response.Rows.Count > 0)
                {
                    var encryptedpassword = Citisec.EncryptString(login.password);
                    var dbpassword = response.Rows[0]["password"].ToString(); //emailaddress=michellemukiri169@gmail.com,password=1022P#lease//
                    var decrytptedpassword = Citisec.DecryptString(login.password);
                    if (dbpassword.Equals(encryptedpassword)) /*encryptedpassword,dbpassword,decrytptedpassword*/
                    {
                        string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                        string sRandomOTP = GenerateRandomOTP(8, saAllowedCharacters);
                        var update_otp_response = dBHandler.AgencyOTP(login.emailaddress, sRandomOTP);
                        response_json.Add("RESPONSECODE", "00");
                        response_json.Add("RESPONSEMESSAGE", "Success");
                    }
                    else
                    {
                        response_json.Add("RESPONSECODE", "01");
                        response_json.Add("RESPONSEMESSAGE", "Invalid credentials");
                    }
                       
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }
        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string otp = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(6, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                otp += sTempChars;

            }

            return otp;
        }
    }
}
