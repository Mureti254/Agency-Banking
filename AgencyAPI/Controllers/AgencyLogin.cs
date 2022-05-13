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
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AgencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyLogin : ControllerBase
    {
        private IConfiguration _config;
        public AgencyLogin(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<JObject> Post(Login login)
        {
            //var user = Authenticate(login);
            //if (user != null)
            //{
            //    var token = Generate(user);
            //    return (JObject)token;
            //}
            //return (JObject)"User not found";
            
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {

                var response = dBHandler.AgencyLogin(login.emailaddress);
                if (response.Rows.Count > 0)
                {
                    var encryptedpassword = Citisec.EncryptString(login.password);
                    var dbpassword = response.Rows[0]["password"].ToString(); //emailaddress=TestBeta@gmail.com,password=FintechTestB,username=TestB//
                    var decrytptedpassword = Citisec.DecryptString(login.password);
                    if (encryptedpassword.Equals(dbpassword)) /*encryptedpassword,dbpassword,decrytptedpassword*/
                    {
                        string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                        string sRandomOTP = GenerateRandomOTP(8, saAllowedCharacters);
                        var update_otp_response = dBHandler.AgencyOTP(login.emailaddress, sRandomOTP);
                        var message = "Your OTP is " + sRandomOTP;
                        var update_email_response = dBHandler.AgencyEmailScheduler(login.emailaddress, "One Time Password", message,"","email",sRandomOTP);
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

        //private string Generate(UserModel user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Email,user.emailaddress)
        //    };

        //    var token = new JwtSecurityToken(_config["JWT:Issuer"],
        //        _config["JWT:Audience"],
        //        claims,
        //        expires: DateTime.Now.AddMinutes(10),
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        //private UserModel Authenticate(Login login)
        //{
        //    var currentuser = AgentStaff.emailaddress.FirstOrDefault(o => o.emailaddress.ToLower() == login.emailaddress.ToLower() && o.Password == login.password);
        //    if (currentuser != null)
        //    {
        //        return currentuser;
        //    }
        //    return null;
        //}

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
