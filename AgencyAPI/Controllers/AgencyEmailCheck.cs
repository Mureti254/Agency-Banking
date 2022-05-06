using AgencyAPI.Models;
using DB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AgencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyEmailCheck : ControllerBase
    {
        // POST api/<AgencyEmailCheck>
        [HttpPost("EmailVerification")]
        public async Task<JObject> Post(Login login)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {

                var response = dBHandler.AgencyLogin(login.emailaddress);
                if (response.Rows.Count > 0)
                {
                    var email = (login.emailaddress);
                    {
                        string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                        string sRandomOTP = GenerateRandomOTP(8, saAllowedCharacters);
                        var update_otp_response = dBHandler.AgencyOTP(login.emailaddress, sRandomOTP);
                        response_json.Add("RESPONSECODE", "00");
                        response_json.Add("RESPONSEMESSAGE", "Success"); 
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