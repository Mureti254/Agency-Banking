﻿//using AgencyAPI.Models;
//using DB;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace AgencyAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AgencyMenuItem : ControllerBase
//    {
//        // GET: api/<MenuController>
//        [HttpGet("GetMenuItems")]
//        public async Task<JObject> Get()
//        {
//            JObject response_json = new JObject();
//            try
//            {
//                DBHandler dBHandler = new DBHandler();
//                var response = dBHandler.AgencyMenuItem();

//                if (response.Rows.Count > 0)
//                {
//                    JArray children = new JArray();
//                    int i = 0;
//                    foreach (DataRow row in response.Rows)
//                    {
//                        JObject child = new JObject();
//                        foreach (DataColumn col in response.Columns)
//                        {
//                            child.Add(col.ColumnName, response.Rows[i][col].ToString());
//                        }
//                        children.Add(child);
//                        i++;
//                    }
//                    response_json.Add("RESPONSECODE", "00");
//                    response_json.Add("RESPONSEMESSAGE", "Success!");
//                    response_json.Add("DATA", children);
//                }
//                else
//                {
//                    response_json.Add("RESPONSECODE", "01");
//                    response_json.Add("RESPONSEMESSAGE", "Failed to get menus!");
//                }
//            }
//            catch (Exception ex)
//            {
//                response_json.Add("RESPONSECODE", "01");
//                response_json.Add("RESPONSEMESSAGE", ex.Message);
//            }

//            return response_json;
//        }

//        // GET api/<MenuController>/5
//        [HttpGet("GetMenuItem/{id}")]

//        public async Task<JObject> Get(int id)
//        {
//            DBHandler dBHandler = new DBHandler();
//            JObject response_json = new JObject();
//            try
//            {
//                var response = dBHandler.AgencyMenuItem(id);
//                if (response.Rows.Count > 0)
//                {
//                    JObject child = new JObject();
//                    foreach (DataColumn col in response.Columns)
//                    {

//                        child.Add(col.ColumnName, response.Rows[0][col].ToString());
//                    }
//                    JToken b = JToken.FromObject(response.Rows[0]);
//                    response_json.Add("RESPONSECODE", "00");
//                    response_json.Add("RESPONSEMESSAGE", "Success!");
//                    response_json.Add("DATA", child);
//                }
//                else
//                {
//                    response_json.Add("RESPONSECODE", "01");
//                    response_json.Add("RESPONSEMESSAGE", "Failed to get menu item!");
//                }
//            }
//            catch (Exception ex)
//            {
//                response_json.Add("RESPONSECODE", "01");
//                response_json.Add("RESPONSEMESSAGE", ex.Message);
//            }

//            return response_json;
//        }

//        // POST api/<MenuController>
//        [HttpPost("CreateMenuItem")]
//        public async Task<JObject> Post(Menus menus)
//        {
//            DBHandler dBHandler = new DBHandler();
//            JObject response_json = new JObject();
//            try
//            {
//                var response = dBHandler.AgencyAddMenuItem(menus.name, menus.link, menus.icon, menus.parentmenuid);
//                if (response.Rows.Count > 0)
//                {
//                    response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
//                    response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
//                }
//                else
//                {
//                    response_json.Add("RESPONSECODE", "01");
//                    response_json.Add("RESPONSEMESSAGE", "Failed to add!");
//                }
//            }
//            catch (Exception ex)
//            {
//                response_json.Add("RESPONSECODE", "01");
//                response_json.Add("RESPONSEMESSAGE", ex.Message);
//            }

//            return response_json;
//        }

//        // DELETE api/<MenuController>/5

//        [HttpDelete("DeleteMenuItem/{id}")]
//        public async Task<JObject> Delete(int id)
//        {
//            DBHandler dBHandler = new DBHandler();
//            JObject response_json = new JObject();
//            try
//            {
//                var response = dBHandler.AgencyDeleteMenuItem(id);
//                if (response.Rows.Count > 0)
//                {
//                    response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
//                    response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
//                }
//                else
//                {
//                    response_json.Add("RESPONSECODE", "01");
//                    response_json.Add("RESPONSEMESSAGE", "Failed to delete!");
//                }
//            }
//            catch (Exception ex)
//            {
//                response_json.Add("RESPONSECODE", "01");
//                response_json.Add("RESPONSEMESSAGE", ex.Message);
//            }

//            return response_json;
//        }
//    }
//}
