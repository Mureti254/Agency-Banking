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
    public class AgencyMenuAccessItem : ControllerBase
    {
        // GET: api/<AgencyMenuAccessItem>
        //[HttpGet("GetMenuAccessItems")]
        //public async Task<JObject> Get()
        //{
        //    JObject response_json = new JObject();
        //    try
        //    {
        //        DBHandler dBHandler = new DBHandler();
        //        var response = dBHandler.AgencyMenuAccessItem();

        //        if (response.Rows.Count > 0)
        //        {
        //            JArray children = new JArray();
        //            int i = 0;
        //            foreach (DataRow row in response.Rows)
        //            {
        //                JObject child = new JObject();
        //                foreach (DataColumn col in response.Columns)
        //                {
        //                    child.Add(col.ColumnName, response.Rows[i][col].ToString());
        //                }
        //                children.Add(child);
        //                i++;
        //            }
        //            response_json.Add("RESPONSECODE", "00");
        //            response_json.Add("RESPONSEMESSAGE", "Success!");
        //            response_json.Add("DATA", children);
        //        }
        //        else
        //        {
        //            response_json.Add("RESPONSECODE", "01");
        //            response_json.Add("RESPONSEMESSAGE", "Failed to get menu access items!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response_json.Add("RESPONSECODE", "01");
        //        response_json.Add("RESPONSEMESSAGE", ex.Message);
        //    }

        //    return response_json;
        //}

        // GET api/<AgencyMenuAccessItem>/5
        [HttpGet("GetMenuAccessItem/{id}")]

        public async Task<JObject> Get(int id)
        {
            //id Represents profileid
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyProfileMenuItems(id);
                if (response.Rows.Count > 0)
                {
                    if (response.Rows.Count > 0)
                    {
                        JArray children = new JArray();
                        int i = 0;
                        foreach (DataRow row in response.Rows)
                        {
                            JObject child = new JObject();
                            foreach (DataColumn col in response.Columns)
                            {
                                child.Add(col.ColumnName, response.Rows[i][col].ToString());
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
                        response_json.Add("RESPONSEMESSAGE", "Failed to get menu access items!");
                    }
                    //JObject child = new JObject();
                    //foreach (DataColumn col in response.Columns)
                    //{

                    //    child.Add(col.ColumnName, response.Rows[0][col].ToString());
                    //}
                    //JToken b = JToken.FromObject(response.Rows[0]);
                    //response_json.Add("RESPONSECODE", "00");
                    //response_json.Add("RESPONSEMESSAGE", "Success!");
                    //response_json.Add("DATA", child);
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Failed to get menu access item!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        // POST api/<AgencyMenuAccessItem>
        [HttpPost("CreateMenuAccessItem")]
        public async Task<JObject> Post([FromBody]MenuAssignment menuAssignment)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {

                var response2 = dBHandler.AgencyDeleteMenuAccessItemsUnderProfile(menuAssignment.profile.profileid);

                foreach(Menus menu in menuAssignment.menu)
                {
                    dBHandler.AgencyAddMenuAccessItem(menuAssignment.profile.profileid, menu.menuid);
                }

                response_json.Add("RESPONSECODE", "000");
                response_json.Add("RESPONSEMESSAGE", "Success");
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        // DELETE api/<AgencyMenuAccessItem>/5
        //[HttpDelete("DeleteMenuAccessItem/{id}")]
        //public async Task<JObject> Delete(int id)
        //{
        //    DBHandler dBHandler = new DBHandler();
        //    JObject response_json = new JObject();
        //    try
        //    {
        //        var response = dBHandler.AgencyDeleteMenuAccessItem(id);
        //        if (response.Rows.Count > 0)
        //        {
        //            response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
        //            response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
        //        }
        //        else
        //        {
        //            response_json.Add("RESPONSECODE", "01");
        //            response_json.Add("RESPONSEMESSAGE", "Failed to delete!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response_json.Add("RESPONSECODE", "01");
        //        response_json.Add("RESPONSEMESSAGE", ex.Message);
        //    }

        //    return response_json;
        //}
    }
}
