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

                var response = dBHandler.AgencyPasswordChanges(passwordchange.newpassword);
                if (response.Rows.Count > 0)
                {
                    var confirmpassword = (passwordchange.newpassword);
                    {

                        var update_password_response = dBHandler.AgencyPasswordChanges(passwordchange.newpassword);
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
    }
}
