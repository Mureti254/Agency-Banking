﻿using AgencyAPI.Models;
using DB;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AgencyAgentOutlets : ControllerBase
    {
        // GET: api/<AgencyAgentOutlets>
        [HttpGet("GetAgentOutlets")]
        public async Task<JObject> Get()
        {
            JObject response_json = new JObject();
            try
            {
                DBHandler dBHandler = new DBHandler();
                var response = dBHandler.AgencyAgentOutlet();

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
                    response_json.Add("RESPONSEMESSAGE", "Failed to get outlets!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        // GET api/<AgencyAgentOutlets>/5
        [HttpGet("GetAgentOutlet/{id}")]

        public async Task<JObject> Get(int id)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyAgentOutlet(id);
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
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Failed to get outlet!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        // POST api/<AgencyAgentOutlets>
        [HttpPost("CreateAgentOutlet")]
        public async Task<JObject> Post(Outlet AgentOutlet)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyAddAgentOutlet(AgentOutlet.agentid, AgentOutlet.name, AgentOutlet.emailaddress, AgentOutlet.phone,
                    AgentOutlet.latitude, AgentOutlet.longitude);
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

        // PUT api/<AgencyAgentOutlets>/5
        [HttpPut("UpdateAgentOutlet/{id}")]
        public async Task<JObject> Put(Outlet AgentOutlet)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyUpdateAgentOutlet(AgentOutlet.agentid, AgentOutlet.agentoutletid, AgentOutlet.name, AgentOutlet.emailaddress, AgentOutlet.phone,
                    AgentOutlet.latitude, AgentOutlet.longitude);
                if (response.Rows.Count > 0)
                {
                    response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
                    response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Failed to update!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        // DELETE api/<AgencyAgentOutlets>/5
        [HttpDelete("DeleteAgentOutlet/{id}")]
        public async Task<JObject> Delete(int id)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyDeleteAgentOutlet(id);
                if (response.Rows.Count > 0)
                {
                    response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
                    response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Failed to delete!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }

        [HttpPut("ApprovalOfCreatedAgentOutlet/{id}")]

        public async Task<JObject> Truefalse(int id, [FromBody] bool status)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyAddAgentOutletApproval(id, status);
                if (response.Rows.Count > 0)
                {
                    response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
                    response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Rejected!");
                }
            }
            catch (Exception ex)
            {
                response_json.Add("RESPONSECODE", "01");
                response_json.Add("RESPONSEMESSAGE", ex.Message);
            }

            return response_json;
        }
        [HttpPut("ApprovalOfUpdatedAgentOutlet/{id}")]

        public async Task<JObject> truefalse(int id, [FromBody] bool status)
        {
            DBHandler dBHandler = new DBHandler();
            JObject response_json = new JObject();
            try
            {
                var response = dBHandler.AgencyUpdateAgentOutletApproval(id, status);
                if (response.Rows.Count > 0)
                {
                    response_json.Add("RESPONSECODE", response.Rows[0]["RESPONSECODE"].ToString());
                    response_json.Add("RESPONSEMESSAGE", response.Rows[0]["RESPONSEMESSAGE"].ToString());
                }
                else
                {
                    response_json.Add("RESPONSECODE", "01");
                    response_json.Add("RESPONSEMESSAGE", "Rejected!");
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
