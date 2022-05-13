using AgencyAPI.Models;
using DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AgencyAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AgencyParameter : ControllerBase
    {
        // GET: api/<AgencyParameter>
        [HttpGet("GetParameters")]
        public async Task<JObject> Get()
        {
            JObject response_json = new JObject();
            try
            {
                DBHandler dBHandler = new DBHandler();
                var response = dBHandler.AgencyParameters();

                if (response.Rows.Count > 0)
                {
                    JArray children = new JArray();
                    int i = 0;
                    foreach (DataRow row in response.Rows)
                    {
                        JObject child = new JObject();
                        foreach (DataColumn col in response.Columns)
                        {
                            child.Add(col.ColumnName, response.Rows[0][col].ToString());
                        }
                        children.Add(child);
                        i++;
                    }
                    response_json.Add("RESPONSECODE", "00");
                    response_json.Add("RESPONSEMESSAGE", "Success!");
                    response_json.Add("DATA", children);
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Failed to get parameters!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        // GET api/<AgencyParameter>/5
        [HttpGet("GetParameters/{id}")]
        public async Task<JObject> Get(int id)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyParameters(id);
                if (response.Rows.Count > 0)
                {
                    JObject child = new JObject();
                    foreach (DataColumn col in response.Columns)
                    {
                        child.Add(col.ColumnName, response.Rows[0][col].ToString());
                    }

                    JToken b = JToken.FromObject(response.Rows[0]);
                    response_json.Add("RESPONSECODE", "00");
                    response_json.Add("RESPONSEMESSAGE", "Success!");
                    response_json.Add("DATA", child);
                    //response_json.Add( response.Rows[0].ToString());
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Failed to get agent!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        // POST api/<AgencyParameter>
        [HttpPost("CreateParameter")]
        public async Task<JObject> Post(Parameter Parameter)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyAddParameter(Parameter.item_key, Parameter.item_name, Parameter.is_encrypted);
                if (response.Rows.Count > 0)
                {
                    response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
                    response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Failed to add!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        ////// PUT api/<AgencyParameter>/5
        //[HttpPut("Parameter/{id}")]
        //public async Task<JObject> Put(AgencyParameter)
        //{
        //    DBHandler dBHandler = new DBHandler();
        //    JObject response_json = new JObject();
        //    try
        //    {
        //        var response = dBHandler.AgencyUpdateParameter(Parameter.item_key, Parameter.item_name, Parameter.is_encrypted);
        //        if (response.Rows.Count > 0)
        //        {
        //            response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
        //            response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
        //        }
        //        else
        //        {
        //            response_json.Add("RESPONSECODE", "01");
        //            response_json.Add("RESPONSEMESSAGE", "Failed to update!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response_json.Add("RESPONSECODE", "01");
        //        response_json.Add("RESPONSEMESSAGE", ex.Message);
        //    }

        //    return response_json;
        //}

        // DELETE api/<AgencyParameter>/5
        [HttpDelete("DeleteParameter/{id}")]
        public async Task<JObject> Delete(Parameter Parameter)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyDeleteParameter(Parameter.parameterid);
                if (response.Rows.Count > 0)
                {
                    response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
                    response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Failed to deleted!");
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
