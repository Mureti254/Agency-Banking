using AgencyAPI.Models;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AgencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyOTP : ControllerBase
    {
        [HttpPost("OTP")]
        public async Task<JObject> Post(Login login)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

                var response = dBHandler.AgencyLogin(login.emailaddress);
                if (response.Rows.Count > 0)
                {
                    var db_otp = response.Rows[0]["otp"].ToString();
                    var db_otp_date = Convert.ToDateTime(response.Rows[0]["otptime"].ToString());

                    var time_difference = (DateTime.Now - db_otp_date).Minutes;

                    if (db_otp.Equals(login.otp) && (time_difference<5))
                    {
                        JObject child = new JObject();
                        foreach (DataColumn col in response.Columns)
                        {
                            child.Add(col.ColumnName, response.Rows[0][col].ToString());
                        }

                        response_json.Add("RESPONSECODE", "00");
                        response_json.Add("RESPONSEMESSAGE", "Success");
                        response_json.Add("DATA", child);
                    }
                    else
                    {
                        response_json.Add("RESPONSECODE", "01");
                        response_json.Add("RESPONSEMESSAGE", "Failed");
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
    }
}