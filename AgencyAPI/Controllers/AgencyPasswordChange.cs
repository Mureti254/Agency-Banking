using AgencyAPI.Models;
using DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    public class AgencyPasswordChange : ControllerBase
    {

        // POST api/<AgencyPasswordChange>
        [HttpPost("PasswordChange")]
        public async Task<JObject> Post(PasswordChange passwordchange)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "{", "}", "?", ".", ",", ":", ":", "~" };
                string sRandomPass = GenerateRandomPass(8, saAllowedCharacters);
                var encryptedpassword = Citisec.EncryptString(passwordchange.confirmedpassword);

                var response = dBHandler.AgencyPasswordChanges(passwordchange.emailaddress, encryptedpassword, encryptedpassword);
                if (response.Rows.Count > 0)
                {
                    var confirmpassword = (passwordchange.newpassword);
                    {

                        response_json.Add("RESPONSECODE", "00");
                        response_json.Add("RESPONSEMESSAGE", "Password change successful");
                    }

                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Password doesn't match!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }
        private string GenerateRandomPass(int iOTPLength, string[] saAllowedCharacters)
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
