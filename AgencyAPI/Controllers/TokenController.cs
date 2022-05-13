using AgencyAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AgencyAPI.Models;
using DB;

namespace AgencyAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        public TokenController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        public async Task<string> Post([FromBody]UserModel usermodel)
        {
            var user = Authenticate(usermodel);
            if (user != null)
            {
                var token = Generate(user);
                return token;
            }
            return "User not found";
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                    new Claim(ClaimTypes.Email,user.emailaddress)
            };

            var token = new JwtSecurityToken(_config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(UserModel user)
        {
            DBHandler dBHandler = new DBHandler();
            var response = dBHandler.AgencyLogin(user.emailaddress);
            if (response.Rows.Count>0) 
            {
                var encryptedpassword = Citisec.EncryptString(user.password);
                var dbpassword = response.Rows[0]["password"].ToString(); //emailaddress=TestBeta@gmail.com,password=FintechTestB,username=TestB//
                var decrytptedpassword = Citisec.DecryptString(user.password);
                if (encryptedpassword.Equals(dbpassword)) /*encryptedpassword,dbpassword,decrytptedpassword*/
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            //var currentuser = Login.emailaddress.FirstOrDefault(o => o.emailaddress.ToLower() == user.emailaddress.ToLower() && o.Password == user.password);
            //if (currentuser != null)
            //{
            //    return currentuser;
            //}
            return null;
        }
    }
}