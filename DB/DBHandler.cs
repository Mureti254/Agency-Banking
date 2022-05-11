using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DB
{

    public class DBHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IOptions<DbHandlerModel> appSettings;
        //public DBHandler()
        //{
        //    Crypto citisec = new Crypto();
        //    //string decryptpass = citisec.DecryptString(appSettings.Value.DbServer.ToString());
        //    systemconnection = "Data Source=" + appSettings.Value.DbServer.ToString() + ";Database=" + appSettings.Value.DbSource.ToString() + ";User ID=" + appSettings.Value.DbUser.ToString() + ";Password=" + appSettings.Value.DbPass.ToString() + ";";
        //}

        public string systemconnection;
        public DBHandler()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            //systemconnection = String.Format("Server={0};Database={1};User ID={2};Password={3};", "DITPC005\\SQL2019", "AgencyBanking", "sa", "pass@word1");
            //systemconnection = String.Format("Server={0};Database={1};User ID={2};Password={3};", "10.110.0.35\\SQL2019", "AgencyBanking", "sa", "2sYstemmaster!");

            //String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User ID={1};Password={2};", @"C:\Test\Test.mdb", _name, _pass)

            //systemconnection = "Data Source=" + root.GetSection("ConnectSettings:DbServer").ToString() + ";Database=" + root.GetSection("ConnectSettings:DbSource").ToString() + ";User ID=" + root.GetSection("ConnectSettings:DbUser").ToString() + ";Password=" + root.GetSection("ConnectSettings:DbPass").ToString() + ";";

            systemconnection = root.GetConnectionString("SystemConnection");
        }

        

        #region Databases
        public enum DataBaseObject
        {
            HostDB,
            BrokerDB
        }

        public string GetDataBaseConnection(DataBaseObject databaseobject)
        {
           
            string connection_string = systemconnection;//ConfigurationManager.ConnectionStrings["SystemConnection"].ToString();
            switch (databaseobject)
            {
                case DataBaseObject.BrokerDB:
                    connection_string = "";//ConfigurationManager.ConnectionStrings["BrokerConnection"].ToString();
                    break;

                default:
                    connection_string = systemconnection;//ConfigurationManager.ConnectionStrings["SystemConnection"].ToString();
                    break;
            }
            return connection_string;
        }
        #endregion

        #region Adhoc
        public DataTable GetAdhocData(string sql, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }
            return dt;
        }
        
        public string GetXML(string sql, DataBaseObject database = DataBaseObject.HostDB)
        {
            string dt = " ";

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connect))
                    {
                        connect.Open();
                        dt = (string)cmd.ExecuteScalar();
                        connect.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public DataTable GetXML2(string sql, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connect))
                    {
                        connect.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            dt.Load(reader);
                        }
                        connect.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public DataTable GetBridgeRecord(string RRN, 
            DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("GET_BRIDGE_LOG", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@RRN", RRN);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable GetRecords(string module, string param1 = "", string param2 = "", 
            DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", module);
                            cmd.Parameters.AddWithValue("@param1", param1);
                            cmd.Parameters.AddWithValue("@param2", param2);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable GetRecordsById(string module, int id, string param1 = "", string param2 = "", DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records_by_id", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", module);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@param1", param1);
                            cmd.Parameters.AddWithValue("@param2", param2);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public void AgencyUpdateComplaint(int v1, int v2, string resolved, string remarks, string complaintdesc)
        {
            throw new NotImplementedException();
        }

        public string GetScalarItem(string sql, DataBaseObject database = DataBaseObject.HostDB)
        {
            string scalaritem = "";

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand command = new SqlCommand(sql, connect))
                    {
                        connect.Open();
                        scalaritem = (string)(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                scalaritem = "";
            }
            return scalaritem;
        }

        public bool DeleteRecord(int id, string module, DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_records", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@recordid", id);
                        cmd.Parameters.AddWithValue("@module", module);
                        cmd.Parameters.AddWithValue("@deleted_by", 0);
                        i = cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ApproveRecord(int id, int approved_by, string module, DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_records", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@recordid", id);
                        cmd.Parameters.AddWithValue("@approved_by", approved_by);
                        cmd.Parameters.AddWithValue("@module", module);
                        i = cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool AddAuditTrail(AuditTrailModel mymodel, DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_audit_trail", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@in_user_name", mymodel.user_name);
                        cmd.Parameters.AddWithValue("@in_action_type", mymodel.action_type);
                        cmd.Parameters.AddWithValue("@in_action_description", mymodel.action_description);
                        cmd.Parameters.AddWithValue("@in_page_accessed", mymodel.page_accessed);
                        cmd.Parameters.AddWithValue("@in_client_ip_address", mymodel.client_ip_address);
                        cmd.Parameters.AddWithValue("@in_session_id", mymodel.session_id);

                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public DateTime AddToken(TokenModel mymodel, DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                DateTime expirydate = DateTime.Now;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_token", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@expiry", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@api_user_name", mymodel.api_user_name);
                        cmd.Parameters.AddWithValue("@token", mymodel.token);
                        expirydate = (DateTime)cmd.ExecuteScalar();
                    }
                }

                return expirydate;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return DateTime.Now;
            }
        }

        public bool UpdateAPICallResponse(APICallResponseModel mymodel, DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_api_call_response", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@reference_number", mymodel.reference_number);
                        cmd.Parameters.AddWithValue("@response", mymodel.response);
                        cmd.Parameters.AddWithValue("@response_desc", mymodel.response_desc);
                        cmd.Parameters.AddWithValue("@third_party", mymodel.third_party);

                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public DataTable tangazo_validate_api_consumer(string strusername, string strpassword, 
            string strtransaction_type, string strexternal_ref_num, string ip_address, string token = ""
            )
        {
            DataBaseObject database = DataBaseObject.HostDB;
            DataTable datatable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand command = new SqlCommand("validate_api_consumer", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@api_user", strusername);
                        command.Parameters.AddWithValue("@api_password", strpassword);
                        command.Parameters.AddWithValue("@external_ref_num", strexternal_ref_num);
                        command.Parameters.AddWithValue("@transaction_type", strtransaction_type);
                        command.Parameters.AddWithValue("@apicallerip", ip_address);
                        command.Parameters.AddWithValue("@token", token);
                        SqlDataAdapter dataadapter = new SqlDataAdapter();
                        dataadapter.SelectCommand = command;

                        dataadapter.Fill(datatable);
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new Exception("validate_api_consumer: " + ex.Message);
            }
            return datatable;
        }

        public async Task<DataTable> validate_api_consumer(string strusername, string strpassword, 
            string strtransaction_type, string strexternal_ref_num, string ip_address, string token = "",
            DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable datatable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand command = new SqlCommand("validate_api_consumer", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@api_user", strusername);
                        command.Parameters.AddWithValue("@api_password", strpassword);
                        command.Parameters.AddWithValue("@external_ref_num", strexternal_ref_num);
                        command.Parameters.AddWithValue("@transaction_type", strtransaction_type);
                        command.Parameters.AddWithValue("@apicallerip", ip_address);
                        command.Parameters.AddWithValue("@token", token);
                        SqlDataAdapter dataadapter = new SqlDataAdapter();
                        dataadapter.SelectCommand = command;

                        dataadapter.Fill(datatable);
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new Exception("validate_api_consumer: " + ex.Message);
            }
            return datatable;
        }

        #endregion

        #region bridge_log
        public bool bridge_savelog(LogModel mymodel, DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_savelog", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MsgID", mymodel.MsgID);
                        cmd.Parameters.AddWithValue("@PAN", mymodel.PAN);
                        cmd.Parameters.AddWithValue("@ProCode", mymodel.ProCode);
                        cmd.Parameters.AddWithValue("@TransAmt", mymodel.TransAmt);
                        cmd.Parameters.AddWithValue("@Transdt", mymodel.Transdt);
                        cmd.Parameters.AddWithValue("@AuditNo", mymodel.AuditNo);
                        cmd.Parameters.AddWithValue("@TransTime", mymodel.TransTime);
                        cmd.Parameters.AddWithValue("@MerchantTyp", mymodel.Fld18);
                        cmd.Parameters.AddWithValue("@Fld22", mymodel.Fld22);
                        cmd.Parameters.AddWithValue("@Fld23", mymodel.Fld23);
                        cmd.Parameters.AddWithValue("@Fld25", mymodel.Fld25);
                        cmd.Parameters.AddWithValue("@Fld26", mymodel.Fld26);
                        cmd.Parameters.AddWithValue("@Fld28", mymodel.Fld28);
                        cmd.Parameters.AddWithValue("@Fld30", mymodel.Fld30);
                        cmd.Parameters.AddWithValue("@Fld32", mymodel.Fld32);
                        cmd.Parameters.AddWithValue("@Fld33", mymodel.Fld33);
                        cmd.Parameters.AddWithValue("@Fld35", mymodel.Fld35);
                        cmd.Parameters.AddWithValue("@RefNo", mymodel.RefNo);
                        cmd.Parameters.AddWithValue("@ServiceCd", mymodel.ServiceCode);
                        cmd.Parameters.AddWithValue("@TerminalID", mymodel.TerminalID);
                        cmd.Parameters.AddWithValue("@CodeID", mymodel.CodeID);
                        cmd.Parameters.AddWithValue("@LocationName", mymodel.LocationName);
                        cmd.Parameters.AddWithValue("@CurrencyCode", mymodel.CurrencyCode);
                        cmd.Parameters.AddWithValue("@Remarks", mymodel.Remarks);
                        cmd.Parameters.AddWithValue("@Fld59", mymodel.Fld59);
                        cmd.Parameters.AddWithValue("@From_ac", mymodel.From_ac);
                        cmd.Parameters.AddWithValue("@To_acc", mymodel.To_acc);
                        i = (int)cmd.ExecuteNonQuery();

                        
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<ParametersModel> GetParameters()
        {
            List<ParametersModel> recordlist = new List<ParametersModel>();

            try
            {
                var da = new DBHandler();
                DataTable dt = new DataTable();

                dt = da.GetRecords("parameters");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new ParametersModel
                    {
                        id = Convert.ToInt32(dr["Id"]),
                        item_key = Convert.ToString(dr["item_key"]),
                        item_value = Convert.ToString(dr["item_value"])
                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("Clearing Server", "Get parameters", ex.InnerException.ToString() + "]");
            }

            return recordlist;
        }

        public bool add_alert(string alert_type="",string recipient="",
            string subject="",string email_body="",string attachment="",string logo="",
            DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {

                int i = 0;
                int alert_id = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_alert", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@alert_type", alert_type);
                        cmd.Parameters.AddWithValue("@recipient", recipient);
                        cmd.Parameters.AddWithValue("@subject", subject);
                        cmd.Parameters.AddWithValue("@email_body", email_body);
                        cmd.Parameters.AddWithValue("@attachment", attachment);
                        cmd.Parameters.AddWithValue("@logo", logo);
                        //cmd.Parameters["@ID"].Direction = ParameterDirection.Output;
                        i = (int)cmd.ExecuteNonQuery();
                        //alert_id = Convert.ToInt32(Convert.ToString(cmd.Parameters["@ID"].Value));
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ALERT", "ERROR",
                    "add_alert: " + ex.ToString());
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool bridge_savelog_param(logBridgeModel mymodel,string switch_name,string messageType,
            string TransType, DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
              
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    switch (switch_name)
                    {
                        case "POSTILION":
                            using (SqlCommand cmd = new SqlCommand("sp_savelog", connect))
                            {
                                connect.Open();
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@MsgID", mymodel.Fld37);
                                cmd.Parameters.AddWithValue("@PAN", mymodel.Fld2);
                                cmd.Parameters.AddWithValue("@ProCode", mymodel.Fld3);
                                cmd.Parameters.AddWithValue("@TransAmt", mymodel.Fld4);
                                cmd.Parameters.AddWithValue("@Transdt", mymodel.Fld7);
                                cmd.Parameters.AddWithValue("@AuditNo", mymodel.Fld11);
                                cmd.Parameters.AddWithValue("@TransTime", mymodel.Fld12);
                                cmd.Parameters.AddWithValue("@MerchantTyp", mymodel.Fld18);
                                cmd.Parameters.AddWithValue("@Fld22", mymodel.Fld22);
                                cmd.Parameters.AddWithValue("@Fld23", mymodel.Fld23);
                                cmd.Parameters.AddWithValue("@Fld25", mymodel.Fld25);
                                cmd.Parameters.AddWithValue("@Fld26", mymodel.Fld26);
                                cmd.Parameters.AddWithValue("@Fld28", mymodel.Fld28);
                                cmd.Parameters.AddWithValue("@Fld30", mymodel.Fld30);
                                cmd.Parameters.AddWithValue("@Fld32", mymodel.Fld32);
                                cmd.Parameters.AddWithValue("@Fld33", mymodel.Fld33);
                                cmd.Parameters.AddWithValue("@Fld35", mymodel.Fld35);
                                cmd.Parameters.AddWithValue("@RefNo", mymodel.Fld37);
                                cmd.Parameters.AddWithValue("@ServiceCd", mymodel.Fld40);
                                cmd.Parameters.AddWithValue("@TerminalID", mymodel.Fld41);
                                cmd.Parameters.AddWithValue("@CodeID", mymodel.Fld42);
                                cmd.Parameters.AddWithValue("@LocationName", mymodel.Fld43);
                                cmd.Parameters.AddWithValue("@CurrencyCode", mymodel.Fld49);
                                cmd.Parameters.AddWithValue("@Remarks", mymodel.Fld56);
                                cmd.Parameters.AddWithValue("@Fld59", mymodel.Fld59);
                                cmd.Parameters.AddWithValue("@From_ac", mymodel.Fld102);
                                cmd.Parameters.AddWithValue("@To_acc", mymodel.Fld103);
                                i = (int)cmd.ExecuteNonQuery();
                            }
                            break;
                        case "SMARTVISA":
                            using (SqlCommand cmd = new SqlCommand("sp_savelog", connect))
                            {
                                connect.Open();
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@MsgID", mymodel.Fld37);
                                cmd.Parameters.AddWithValue("@Fld2", mymodel.Fld2);
                                cmd.Parameters.AddWithValue("@Fld3", mymodel.Fld3);
                                cmd.Parameters.AddWithValue("@Fld4", mymodel.Fld4);
                                cmd.Parameters.AddWithValue("@Fld5", mymodel.Fld5);
                                cmd.Parameters.AddWithValue("@Fld6", mymodel.Fld6);
                                cmd.Parameters.AddWithValue("@Fld9", mymodel.Fld9);
                                cmd.Parameters.AddWithValue("@Fld10", mymodel.Fld10);
                                cmd.Parameters.AddWithValue("@Fld11", mymodel.Fld11);
                                cmd.Parameters.AddWithValue("@Fld12", mymodel.Fld12);
                                cmd.Parameters.AddWithValue("@Fld14", mymodel.Fld14);
                                cmd.Parameters.AddWithValue("@Fld15", mymodel.Fld15);
                                cmd.Parameters.AddWithValue("@Fld18", mymodel.Fld18);
                                cmd.Parameters.AddWithValue("@Fld22", mymodel.Fld22);
                                cmd.Parameters.AddWithValue("@Fld32", mymodel.Fld32);
                                cmd.Parameters.AddWithValue("@Fld37", mymodel.Fld37);
                                cmd.Parameters.AddWithValue("@Fld41", mymodel.Fld41);
                                cmd.Parameters.AddWithValue("@Fld42", mymodel.Fld42);
                                cmd.Parameters.AddWithValue("@Fld43", mymodel.Fld43);
                                cmd.Parameters.AddWithValue("@Fld48", mymodel.Fld48);
                                cmd.Parameters.AddWithValue("@Fld49", mymodel.Fld49);
                                cmd.Parameters.AddWithValue("@Fld50", mymodel.Fld50);
                                cmd.Parameters.AddWithValue("@Fld51", mymodel.Fld51);
                                cmd.Parameters.AddWithValue("@Fld52", mymodel.Fld52);
                                cmd.Parameters.AddWithValue("@Fld54", mymodel.Fld54);
                                cmd.Parameters.AddWithValue("@Fld61", mymodel.Fld61);
                                cmd.Parameters.AddWithValue("@Fld63", mymodel.Fld63);
                                cmd.Parameters.AddWithValue("@Fld64", mymodel.Fld64);
                                cmd.Parameters.AddWithValue("@Fld91", mymodel.Fld91);
                                cmd.Parameters.AddWithValue("@Fld95", mymodel.Fld95);
                                cmd.Parameters.AddWithValue("@Fld100", mymodel.Fld100);
                                cmd.Parameters.AddWithValue("@Fld102", mymodel.Fld102);
                                cmd.Parameters.AddWithValue("@Fld103", mymodel.Fld103);
                                cmd.Parameters.AddWithValue("@Fld127", mymodel.Fld127);
                                cmd.Parameters.AddWithValue("@MsgType", messageType);
                                cmd.Parameters.AddWithValue("@TransType",TransType);
                                i = (int)cmd.ExecuteNonQuery();
                                break;
                            }
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ISO", "ERROR",
                    "bridge_savelog_param: " + ex.ToString());
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool bridge_updatelog(string external_num,string fld38,string fld39,string amountblno="",
            string customer_number="", string flexrefnumber = "",string fccflexrefnumber="", DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_updatelog", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MsgID", external_num);                    
                        cmd.Parameters.AddWithValue("@Fld38", fld38);
                        cmd.Parameters.AddWithValue("@Fld39", fld39);
                        cmd.Parameters.AddWithValue("@AmntBlkNo", amountblno);
                        cmd.Parameters.AddWithValue("@CustNo", customer_number);
                        if (!flexrefnumber.Equals(""))
                        {
                            cmd.Parameters.AddWithValue("@flexrefnumber", flexrefnumber);
                        }
                        if (!fccflexrefnumber.Equals(""))
                        {
                            cmd.Parameters.AddWithValue("@fccflexrefnumber", fccflexrefnumber);
                        }
                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ISO", "ERROR",
                    "bridge_updatelog: " + ex.ToString());
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool bridge_updateClearinglog(string external_num, string clearAmt, string clearResp,string feeAmt,
            string feeResp, string customer_number = "",string flexrefnumber=""
            ,string fccflexrefnumber=""
            ,string feeflexrefnumber = ""
            ,string feefccflexrefnumber = ""
            , DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_update_clearlog", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@MsgID", external_num);
                        cmd.Parameters.AddWithValue("@clearAmount", clearAmt);
                        cmd.Parameters.AddWithValue("@clearResp", clearResp);
                        cmd.Parameters.AddWithValue("@CustNo", customer_number);
                        cmd.Parameters.AddWithValue("@feeAmount", feeAmt);
                        cmd.Parameters.AddWithValue("@feeResp", feeResp);
                        cmd.Parameters.AddWithValue("@flexrefnumber", flexrefnumber);
                        cmd.Parameters.AddWithValue("@fccflexrefnumber", fccflexrefnumber);
                        cmd.Parameters.AddWithValue("@feeflexrefnumber", feeflexrefnumber);
                        cmd.Parameters.AddWithValue("@feefccflexrefnumber", feefccflexrefnumber);
                     
                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_updateClearinglog: " + ex.ToString());
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool bridge_update_flexreference(string TransactionId,string FlexReferenceNumber,string TransactionType=""
            , DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_flex_reference", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TransactionId", TransactionId);
                        cmd.Parameters.AddWithValue("@FlexReferenceNumber", FlexReferenceNumber);
                        cmd.Parameters.AddWithValue("@TransactionType", TransactionType);

                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_update_flexreference: " + ex.ToString());
                return false;
            }
        }

        public DataTable bridge_check_transaction_fee_status(string TransactionId, string TransactionType = "",
             DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("Check_Transaction_Fee_status", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@TransactionId", TransactionId);
                            cmd.Parameters.AddWithValue("@TransactionType", TransactionType);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public bool bridge_insertClearRecords(string trans_type,string ref_no,string accnum,string card_num,
            string c_amnt,string f_amnt,string currency,string trans_date,string remarks,
            string iso_trans_refno="",
            string fee_code="",string billing_amount="",string billing_currency="", DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_clear_file_data", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@trans_type", trans_type);
                        cmd.Parameters.AddWithValue("@trans_id", ref_no);
                        cmd.Parameters.AddWithValue("@acc_num", accnum);
                        cmd.Parameters.AddWithValue("@card_num", card_num);
                        cmd.Parameters.AddWithValue("@c_amnt", c_amnt);
                        cmd.Parameters.AddWithValue("@f_amnt", f_amnt);
                        cmd.Parameters.AddWithValue("@c_currency", currency);
                        cmd.Parameters.AddWithValue("@trans_date", trans_date);
                        cmd.Parameters.AddWithValue("@remarks", remarks);
                        cmd.Parameters.AddWithValue("@iso_trans_refno", iso_trans_refno);
                        cmd.Parameters.AddWithValue("@fee_code", fee_code);
                        cmd.Parameters.AddWithValue("@billing_amount", billing_amount);
                        cmd.Parameters.AddWithValue("@billing_currency", billing_currency);
                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "insertClearRecords ProcessRequest: "+ex.Message+" - "+ex.StackTrace);
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        public bool bridge_insertClearFiles(string file_type, string file_name,int total_records, DataBaseObject database = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_clear_files", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@file_type", file_type);
                        cmd.Parameters.AddWithValue("@file_name", file_name);
                        cmd.Parameters.AddWithValue("@total_rec", total_records);
                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "insertClearFiles ProcessRequest: " + ex.Message + " - " + ex.StackTrace);
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region LicenseCenter
        public bool checklicense(string strTxnDate)
        {
            bool response = false;
            try
            {
                Crypto cs = new Crypto();
                //string year_ = Convert.ToString(DateTime.Now.Year);
                //strTxnDate = year_.Substring(0, 2) + strTxnDate.Substring(0, 2) + "-" + strTxnDate.Substring(2, 2) +"-" + strTxnDate.Substring(4, 2);

                string appPath = "";

                string licensedt = "";
                appPath = Directory.GetCurrentDirectory();// @"C:\ESB\certificates\";
                appPath = appPath + "\\ESBLicense.enc.cfg";
                //FileLogHandler.log_message_fields("ESB", "ERROR", "License path" + appPath);
                licensedt = File.ReadAllText(appPath);                
                string licensedate = cs.DecryptLicense(licensedt);
                string getdatepart = licensedate.Split(":")[1].ToString();
                DateTime decrypteddate = Convert.ToDateTime(getdatepart);

                TimeSpan ts = default(TimeSpan);

                ts = decrypteddate.Subtract(Convert.ToDateTime(strTxnDate));

                long prd = ts.Days;

                if (prd <= 0)
                {
                    response = false;
                }
                else if(prd ==60)
                {
                    response = true;
                }
                else if(prd ==30)
                {
                    response = true;
                }
                else if(prd ==5)
                {
                    response = true;
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ESB", "ERROR", "License Error -02" + ex.Message);
            }
            return response;
        }
        #endregion

        #region Agency Banking
        public DataTable AgencyParameters(string itemname, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "parameter");
                            cmd.Parameters.AddWithValue("@param1", itemname);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        
        public DataTable AgencyParameters(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "parameter");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyParameters(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "parameter");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddParameter(string item_key,string item_name, bool is_encrypted, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_parameter", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@item_key", item_key);
                            cmd.Parameters.AddWithValue("@item_name", item_name);
                            cmd.Parameters.AddWithValue("@is_encrypted", is_encrypted);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyDeleteParameter(int parameterid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_parameter", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@parameterid", parameterid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //parameter
        public DataTable AgencyAgentDevice(int agentid, int deviceid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "AgentDevice");
                            cmd.Parameters.AddWithValue("@param1", agentid);
                            cmd.Parameters.AddWithValue("@param1", deviceid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAgentDevice(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "AgentDevice");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAgentDevice(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "AgentDevice");
                            cmd.Parameters.AddWithValue("@param1", id );
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddAgentDevice(int agentid, int deviceid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_parameter", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentid", agentid);
                            cmd.Parameters.AddWithValue("@deviceid", deviceid);
                          

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyDeleteAgentDevice(int agentserviceid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_agent_device", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentserviceid", agentserviceid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        //agentdevice

        public DataTable AgencyAgents(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agents");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAgent(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agents");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyAgentsUserSuperAgent(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agents");
                            cmd.Parameters.AddWithValue("@param1", "");
                            cmd.Parameters.AddWithValue("@param2", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencySuperAgents(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "superagents");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencySuperAgent(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "superagents");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAgents(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agents");
                            cmd.Parameters.AddWithValue("@param1", id+"");
                            cmd.Parameters.AddWithValue("@param2", "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddAgent(int superagentid, string businessname, string emailaddress, string phone, string address,
            string bizregcert, string bizlicense, string financialstatement, string goodconductcert, int businesstypeid,
            int ownershiptypeid, string alternative_emailaddress, string alternative_phonenumber, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_agent", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@superagentid", superagentid);
                            cmd.Parameters.AddWithValue("@businessname", businessname);
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@bizregcert", bizregcert);
                            cmd.Parameters.AddWithValue("@bizlicense", bizlicense);
                            cmd.Parameters.AddWithValue("@financialstatement", financialstatement);
                            cmd.Parameters.AddWithValue("@goodconductcert", goodconductcert);
                            cmd.Parameters.AddWithValue("@businesstypeid", businesstypeid);
                            cmd.Parameters.AddWithValue("@ownershiptypeid", ownershiptypeid);
                            cmd.Parameters.AddWithValue("@alternative_emailaddress", alternative_emailaddress);
                            cmd.Parameters.AddWithValue("@alternative_phonenumber", alternative_phonenumber);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateAgent(int superagentid,int agentid, string businessname, string emailaddress, string phone, string address, string bizregcert,
            string bizlicense, string financialstatement, string goodconductcert, int businesstypeid, int ownershiptypeid,string alternative_emailaddress,string alternative_phonenumber, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_agent", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@superagentid", superagentid);
                            cmd.Parameters.AddWithValue("@agentid", agentid);
                            cmd.Parameters.AddWithValue("@businessname", businessname);
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@bizregcert", bizregcert);
                            cmd.Parameters.AddWithValue("@bizlicense", bizlicense);
                            cmd.Parameters.AddWithValue("@financialstatement", financialstatement);
                            cmd.Parameters.AddWithValue("@goodconductcert", goodconductcert);
                            cmd.Parameters.AddWithValue("@businesstypeid", businesstypeid);
                            cmd.Parameters.AddWithValue("@ownershiptypeid", ownershiptypeid);
                            cmd.Parameters.AddWithValue("@alternative_emailaddress", alternative_emailaddress);
                            cmd.Parameters.AddWithValue("@alternative_phonenumber", alternative_phonenumber);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddAgentApproval(int agentholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_created_agent", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentholderid", agentholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteAgent(int agentid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_agent", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentid", agentid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateAgentApproval(int agentid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_update_agent", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentid", agentid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }    //agent

        public DataTable AgencyAgentService(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agentservice");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAgentService(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agentservice");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            cmd.Parameters.AddWithValue("@param2", "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddAgentService(string name, string servicecode, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("adding_agent_service", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@servicecode", servicecode);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateAgentService(int agentserviceid, string name = "", string servicecode = "", DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_agent_service", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentserviceid", agentserviceid);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@servicecode", servicecode);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddAgentServiceApproval(int agentserviceholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_create_agent_service", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentserviceholderid", agentserviceholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteAgentService(int agentserviceid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_agent_service", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentserviceid", agentserviceid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateAgentServiceApproval(int agentserviceid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_update_agent_service", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentserviceid", agentserviceid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        } //agentservice

        public DataTable AgencyOwnershipTypes(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "ownershiptypes");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyOwnershipType(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "ownershiptypes");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddOwnershipType(string name, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_ownershiptype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@name", name);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyUpdateOwnershiptype(int ownershiptypeid, string name, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_ownershiptype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ownershiptypeid", ownershiptypeid);
                            cmd.Parameters.AddWithValue("@name", name);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteOwnershiptype(int ownershiptypeid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_ownershiptype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ownershiptypeid", ownershiptypeid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        //ownershiptype


        public DataTable AgencyBusinessTypes(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "businesstypes");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        
        public DataTable AgencyBusinessType(int id,DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "businesstypes");
                            cmd.Parameters.AddWithValue("@param1", id+"");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddBusinessType( string businesstypename = "", DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_businesstype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@name", businesstypename);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateBusinessType(int businesstypeid, string businesstypename, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_businesstype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@businesstypeid", businesstypeid);
                            cmd.Parameters.AddWithValue("@name", businesstypename);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteBusinessType(int businesstypeid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_businesstype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@businesstypeid", businesstypeid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //businesstype//

        public DataTable AgencyProfileTypes( DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "profiletype");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyProfilesUnderProfileType(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "profiletype");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddProfileType( string name, DataBaseObject database = DataBaseObject.HostDB )
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_profiletype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@name", name);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateProfileType( int id, string name, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_profiletype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@profiletypeid", id);
                            cmd.Parameters.AddWithValue("@name", name);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddProfileTypeApproval(int profiletypeholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_created_profiletype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@profiletypeholderid", profiletypeholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateProfileTypeApproval(int profiletypeholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_updated_profiletype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@profiletypeholderid", profiletypeholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyDeleteProfileType(int profiletypeid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_profiletype", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@profiletypeid", profiletypeid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        //profiletype//

        public DataTable AgencyProfiles( DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "profiles");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyProfile(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "profiles");
                            cmd.Parameters.AddWithValue("@param1", id+"");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddProfile(string name , int profiletype_id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_profile", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@profiletypeid", profiletype_id);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateProfile(int id, string name, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_profile", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@profileid", id);
                            cmd.Parameters.AddWithValue("@name", name);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

       
public DataTable AgencyAddProfileApproval(int profileholderid = 0, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_created_profile", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@profileholderid", profileholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateProfileApproval(int profileholderid = 0, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_updated_profile", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@profileholderid", profileholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteProfile(int profileid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_profile", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@profileid", profileid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //profile




        public DataTable AgencyBanks(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "banks");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyBank(int id,DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "banks");
                            cmd.Parameters.AddWithValue("@param1", id+"");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddBank( string name, string swiftcode , string bankcode , string banklogo, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_bank", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@name", name);
                             cmd.Parameters.AddWithValue("@bankcode", bankcode);
                            cmd.Parameters.AddWithValue("@swiftcode", swiftcode);
                            cmd.Parameters.AddWithValue("@banklogo", banklogo);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteBank(int bankid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_bank", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@bankid", bankid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateBank(int id, string name, string bankcode, string swiftcode, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_bank", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@bankid", id);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@bankcode", bankcode);
                            cmd.Parameters.AddWithValue("@swiftcode", swiftcode);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //bank//

        public DataTable AgencyBankBranches(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "bankbranches");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyBankBranch(int id,DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "bankbranches");
                            cmd.Parameters.AddWithValue("@param1", id+"");
                            cmd.Parameters.AddWithValue("@param2",  "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddBankBranch(string bankbranchname, string bankbranchcode, int bankid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_bank_branch", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@name", bankbranchname);
                            cmd.Parameters.AddWithValue("@branchcode", bankbranchcode);
                            cmd.Parameters.AddWithValue("@bankid", bankid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyBankBranchUnderBank(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "bankbranches");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            cmd.Parameters.AddWithValue("@param2", "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateBankBranch(int id, string name,string branchcode, int bankid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_bank_branch", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@branchcode", branchcode);
                            cmd.Parameters.AddWithValue("@bankid", bankid);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }



        public DataTable AgencyDeleteBankBranch(int bankbranchid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_bank_branch", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@bankbranchid", bankbranchid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        //bank branch

        public DataTable AgencyReasons(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "reasons");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyReasons(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "reasons");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddReason(string name , DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_reasons", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@name", name);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateReason(int id, string name, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_reasons", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@name", name);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteReason(int reasonid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_reason", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@reasonid", reasonid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //Reason//

        public DataTable AgencyReasonType(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "Reasontype");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyReasonType(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "Reasontype");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddReasonType(string name ,  DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_reason_type", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@name", name);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateReasonType(int id , string name,  DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_reason_type", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.Parameters.AddWithValue("@name", name);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteReasontype(int reasontypeid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_reason_type", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@reasontypeid", reasontypeid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //reason type//


        public DataTable AgencyDevices(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "Device");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDevice(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "Device");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddDevice(string serialnumber,string device_name,string device_model,string firmware_version, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_device", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@serialnumber", serialnumber);
                            cmd.Parameters.AddWithValue("@device_name", device_name);
                            cmd.Parameters.AddWithValue("@device_model", device_model);
                            cmd.Parameters.AddWithValue("@firmware_version", firmware_version);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateDevice(int deviceid , string serialnumber,string device_name, string device_model, string firmware_version, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_device", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@deviceid", deviceid);
                            cmd.Parameters.AddWithValue("@serialnumber", serialnumber);
                            cmd.Parameters.AddWithValue("@device_name", device_name);
                            cmd.Parameters.AddWithValue("@device_model", device_model);
                            cmd.Parameters.AddWithValue("@firmware_version", firmware_version);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddDeviceApproval(int deviceholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_device", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@deviceholderid", deviceholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdatedDeviceApproval(int deviceholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_updated_device", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@deviceholderid", deviceholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteDevice(int deviceid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_device", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@deviceid", deviceid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        //device


        public DataTable AgencyComplaint(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "complaint");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyComplaint(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "complaint");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddComplaint(int outletid,string customerphonenumber,bool resolved, string remarks, string complaintdescription, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_complaint", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@customerphonenumber", customerphonenumber);
                            cmd.Parameters.AddWithValue("@resolved", resolved);
                            cmd.Parameters.AddWithValue("@remarks", remarks);
                            cmd.Parameters.AddWithValue("@complaintdescription", complaintdescription);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateComplaint(string customerphonenumber, bool resolved, string remarks, string complaintdescription, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_complaint", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@customerphonenumber", customerphonenumber);
                            cmd.Parameters.AddWithValue("@resolved", resolved);
                            cmd.Parameters.AddWithValue("@remarks", remarks);
                            cmd.Parameters.AddWithValue("@complaintdescription", complaintdescription);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteComplaint(int complaintid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_complaint", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@complaintid", complaintid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        //complaint

        public DataTable AgencyAgentStaff(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agentstaff");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAgentStaff(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                           cmd.Parameters.AddWithValue("@module", "agentstaff");
                            cmd.Parameters.AddWithValue("@param1", id+"");
                            cmd.Parameters.AddWithValue("@param2", "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddAgentStaff(string firstname, string lastname, string surname, string phone, string emailaddress, string username, string password,
            string otp, string alternative_phonenumber, string alternative_emailaddress,int profileid, int agentid, int agentoutletid, int statusid = 1, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_agent_staff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@firstname", firstname);
                            cmd.Parameters.AddWithValue("@lastname", lastname);
                            cmd.Parameters.AddWithValue("@surname", surname);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@otp", otp);
                            cmd.Parameters.AddWithValue("@profileid", profileid);
                            cmd.Parameters.AddWithValue("@statusid", statusid);
                            cmd.Parameters.AddWithValue("@agentid", agentid);
                            cmd.Parameters.AddWithValue("@agentoutletid", agentoutletid);
                            cmd.Parameters.AddWithValue("@alternative_phonenumber", alternative_phonenumber);
                            cmd.Parameters.AddWithValue("@alternative_emailaddress", alternative_emailaddress);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }



        public DataTable AgencyUpdateAgentStaff(int agentstaffid, string firstname, string lastname, string surname, string phone, string emailaddress,
            string username, string password , string alternative_emailaddress, string alternative_phonenumber,  int profileid , int agentid, int agentoutletid,  DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_agent_staff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", agentstaffid);
                            cmd.Parameters.AddWithValue("@firstname", firstname);
                            cmd.Parameters.AddWithValue("@lastname", lastname);
                            cmd.Parameters.AddWithValue("@surname", surname);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@alternative_emailaddress", alternative_emailaddress);
                            cmd.Parameters.AddWithValue("@alternative_phonenumber", alternative_phonenumber);
                            cmd.Parameters.AddWithValue("@profileid", profileid);
                            cmd.Parameters.AddWithValue("@agentid", agentid);
                            cmd.Parameters.AddWithValue("@agentoutletid", agentoutletid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt; 
        }

        public DataTable AgencyAddAgentStaffApproval(int agentstaffid , bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_create_agentstaff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentstaffid", agentstaffid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteAgentStaff(int agentstaffid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_agent_staff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentstaffid", agentstaffid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyUpdateAgentStaffApproval(int agentstaffid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_updated_agentstaff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentstaffid", agentstaffid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //agent staff
        public DataTable AgencyBankStaff(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "bankstaff");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyBankStaffByEmailAddress(string emailaddress,DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("bankstaff_login_by_emailaddress", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        } 
        

        public DataTable AgencyBankStaff(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "bankstaff");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddBankStaff( string firstname, string lastname, string surname, string phone, string emailaddress, string username, 
            string password, string alternative_emailaddress, string alternative_phonenumber, int profileid, int bankid, int statusid = 1,DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_bank_staff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            
                            cmd.Parameters.AddWithValue("@firstname", firstname);
                            cmd.Parameters.AddWithValue("@lastname", lastname);
                            cmd.Parameters.AddWithValue("@surname", surname);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@profileid", profileid);
                            cmd.Parameters.AddWithValue("@bankid", bankid);
                            cmd.Parameters.AddWithValue("@alternative_emailaddress", alternative_emailaddress);
                            cmd.Parameters.AddWithValue("@alternative_phonenumber", alternative_phonenumber);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public DataTable AgencyAddBankStaffApproval(int bankstaffholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_created_bank_staff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@bankstaffholderid", bankstaffholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateBankStaffApproval(int bankstaffholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_updated_bankstaff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@bankstaffholderid", bankstaffholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteBankStaff(int bankstaffid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_bank_staff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@bankstaffid", bankstaffid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyUpdateBankStaff(int bankstaffid, string firstname, string lastname, string surname, string phone, string emailaddress,
            string username, string password, string alternative_phonenumber, string alternative_emailaddress, int bankid,int profileid, int statusid=1 ,DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_bank_staff", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@bankstaffid", bankstaffid);
                            cmd.Parameters.AddWithValue("@firstname", firstname);
                            cmd.Parameters.AddWithValue("@lastname", lastname);
                            cmd.Parameters.AddWithValue("@surname", surname);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);
                            cmd.Parameters.AddWithValue("@bankid", bankid);
                            cmd.Parameters.AddWithValue("@profileid", profileid);
                            cmd.Parameters.AddWithValue("@statusid", statusid);
                            cmd.Parameters.AddWithValue("@alternative_phonenumber", alternative_phonenumber);
                            cmd.Parameters.AddWithValue("@alternative_emailaddress", alternative_emailaddress);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        //bankstaff


        public DataTable AgencyOwnershiptype( DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "ownershiptypes");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyOwnershiptype(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "ownershiptypes");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyLogin(string emailaddress, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("Login", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                           
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //email check

        public DataTable AgencyOTP(string emailaddress, string otp, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("otp_update", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@otp", otp);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //otp

        public DataTable AgencyAgentOutlet(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agentoutlet");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAgentOutlet(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "agentoutlet");
                            cmd.Parameters.AddWithValue("@param1", id+"");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyAddAgentOutlet(string name, string phone, string emailaddress, int agentid,int agentoutletid, Double latitude,
            Double longitude,int cash_deposit_limit,int operatingdeviceid, int outlet_activity_status = 6, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_agentoutlet", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;


                            
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@agentid", agentid);
                            cmd.Parameters.AddWithValue("@agentoutletid", agentoutletid);
                            cmd.Parameters.AddWithValue("@latitude", latitude);
                            cmd.Parameters.AddWithValue("@longitude", longitude);
                            cmd.Parameters.AddWithValue("@cash_deposit_limit", cash_deposit_limit);
                            cmd.Parameters.AddWithValue("@operating_device_id", operatingdeviceid);
                           
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public DataTable AgencyAddAgentOutletApproval(int agentoutletid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_create_agentoulet", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentoutletid", agentoutletid);
                            cmd.Parameters.AddWithValue("@approve", approve);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyUpdateAgentOutletApproval(int agentoutletholderid, bool approve = false, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("approve_reject_update_agentoutlet", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentoutletholderid", agentoutletholderid);
                            cmd.Parameters.AddWithValue("@approve", approve);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteAgentOutlet(int agentoutletid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_agentoutlet", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentoutletid", agentoutletid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyUpdateAgentOutlet(string name, string phone, string emailaddress, int agentid, Double latitude, Double longitude,
            int agentoutletid,
             int cash_deposit_limit, int operatingdeviceid, int outlet_activity_status = 6, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_agentoutlet", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@agentid", agentid);
                            cmd.Parameters.AddWithValue("@agentoutletid", agentoutletid);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@phone", phone);
                            cmd.Parameters.AddWithValue("@latitude", latitude);
                            cmd.Parameters.AddWithValue("@longitude", longitude);
                            cmd.Parameters.AddWithValue("@cash_deposit_limit", cash_deposit_limit);
                            cmd.Parameters.AddWithValue("@operating_device", operatingdeviceid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyEmailScheduler(string RecipientEmail, string subject, string message, string SenderEmail, string messagetype,
            string otp, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("emailscheduler_inserts", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@RecipientEmail", RecipientEmail);
                            cmd.Parameters.AddWithValue("@subject", subject);
                            cmd.Parameters.AddWithValue("@message", message);
                            cmd.Parameters.AddWithValue("@SenderEmail", SenderEmail);
                            cmd.Parameters.AddWithValue("@messagetype", messagetype);
                            cmd.Parameters.AddWithValue("@otp", otp);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyPasswordChanges(string emailaddress, string newpassword, string confirmedpassword, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("password_change", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@emailaddress", emailaddress);
                            cmd.Parameters.AddWithValue("@newpassword", newpassword); 
                            cmd.Parameters.AddWithValue("@confirmedpassword", confirmedpassword);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyMenuAccessItem(DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "menu_access_item");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyMenuAccessItem(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "menu_access_item");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddMenuAccessItem(string name, string link, int profileid, int parentmenuid, int menuid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("insert_menu_access_item", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@menu_access_name", name);
                            cmd.Parameters.AddWithValue("@link", link);
                            cmd.Parameters.AddWithValue("@profileid", profileid);
                            cmd.Parameters.AddWithValue("@parentmenuid", parentmenuid);
                            cmd.Parameters.AddWithValue("@menuid", menuid);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteMenuAccessItem(int menuaccessitemid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_menu_access_item", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@menuaccessitemid", menuaccessitemid);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyMenuItem( DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "menu_item");
                           
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public DataTable AgencyMenuItem(int id, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", "menu_item");
                            cmd.Parameters.AddWithValue("@param1", id + "");
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyAddMenuItem(string name,string link,string icon, int parentmenuid,DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("insert_menu", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@menuname", name);
                            cmd.Parameters.AddWithValue("@link", link);
                            cmd.Parameters.AddWithValue("@icon", icon);
                            cmd.Parameters.AddWithValue("@parentmenuid", parentmenuid);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public DataTable AgencyDeleteMenuItem(int menuitemid, DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("delete_menu_item", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@menuitemid", menuitemid);

                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public DataTable AgencyProfileMenuSelect(string profilename,DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("Menu_by_profile_select", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@profilename", profilename);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ClearingServer", "ERROR",
                    "bridge_check_transaction_fee_status: " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        #endregion
    }
    public class Crypto
    {
        private static readonly string _key = "FinFlex2030ESBFutureisNow";
        internal const string Inputkey = "2020E30S46-6346-B9F2-A2E8-002F9I6N9VA2";

        public string DecryptString(string Message)
        {
            string result;
            try
            {
                string s = "FintechESB";
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(s));
                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDESCryptoServiceProvider.Key = key;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
                byte[] array = Convert.FromBase64String(Message);
                byte[] bytes;
                try
                {
                    ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
                    bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                }
                finally
                {
                    tripleDESCryptoServiceProvider.Clear();
                    mD5CryptoServiceProvider.Clear();
                }
                result = uTF8Encoding.GetString(bytes);
            }
            catch (Exception var_8_88)
            {
                result = "";
            }
            return result;
        }

        public  string DecryptLicense(string data)
        {
            string response = "";
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(data);

                var aes = GenRijManaged(_key);
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);               

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                response= streamReader.ReadToEnd();
                            }
                        }
                    }
                

            }
            catch (Exception ex)
            {
                FileLogHandler.log_message_fields("ESB", "ERROR", "License Error -01"+ex.Message);
            }
            return response;
        }

        public RijndaelManaged GenRijManaged(string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(Inputkey, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
    }
    public class AuditTrailModel
    {
        public int id { get; set; }

        public string user_name { get; set; }

        public string action_type { get; set; }

        public string action_description { get; set; }

        public string page_accessed { get; set; }

        public string client_ip_address { get; set; }

        public string session_id { get; set; }
    }

    public class TokenModel
    {
        public string api_user_name { get; set; }
        public string token { get; set; }
        public DateTime expiry { get; set; }
    }

    public class APICallResponseModel
    {
        public string reference_number { get; set; }
        public string response { get; set; }

        public string response_desc { get; set; }

        public string third_party { get; set; }
    }

    public class DbHandlerModel
    {
        public string DbServer { get; set; }

        public string DbSource { get; set; }
        public string DbUser { get; set; }
        public string DbPass { get; set; }

    }

    public class LogModel
    {
        public string MsgID { get; set; }
        public string PAN { get; set; }
        public string ProCode { get; set; }
        public string TransAmt { get; set; }
        public string Transdt { get; set; }
        public string AuditNo { get; set; }
        public string TransTime { get; set; }
        public string RefNo { get; set; }
        public string ServiceCode { get; set; }
        public string TerminalID { get; set; }
        public string CodeID { get; set; }
        public string LocationName { get; set; }
        public string CurrencyCode { get; set; }
        public string Remarks { get; set; }
        public string From_ac { get; set; }
        public string To_acc { get; set; }
        public string Fld123 { get; set; }
        public string Fld127 { get; set; }
        public string Fld6 { get; set; }
        public string Fld13 { get; set; }
        public string Fld14 { get; set; }
        public string Fld15 { get; set; }
        public string Fld17 { get; set; }
        public string Fld18 { get; set; }
        public string Fld19 { get; set; }
        public string Fld22 { get; set; }
        public string Fld23 { get; set; }
        public string Fld25 { get; set; }
        public string Fld26 { get; set; }
        public string Fld27 { get; set; }
        public string Fld28 { get; set; }
        public string Fld30 { get; set; }
        public string Fld32 { get; set; }
        public string Fld33 { get; set; }
        public string Fld35 { get; set; }
        public string Fld48 { get; set; }
        public string Fld54 { get; set; }
        public string Fld56 { get; set; }
        public string Fld59 { get; set; }
        public string Fld60 { get; set; }
        public string Fld70 { get; set; }
        public string Fld90 { get; set; }
        public string Fld95 { get; set; }
        public string Fld100 { get; set; }
        public string respCode { get; set; }
        public string AuthCode { get; set; }
    }

    public class logBridgeModel
    {
        public string Fld2 { get; set; }
        public string Fld3 { get; set; }
        public string Fld4 { get; set; }
        public string Fld5 { get; set; }
        public string Fld6 { get; set; }
        public string Fld7 { get; set; }
        public string Fld9 { get; set; }
        public string Fld10 { get; set; }
        public string Fld11 { get; set; }
        public string Fld12 { get; set; }
        public string Fld14 { get; set; }
        public string Fld15 { get; set; }
        public string Fld19 { get; set; }
        public string Fld37 { get; set; }
        public string Fld40 { get; set; }
        public string Fld41 { get; set; }
        public string Fld42 { get; set; }
        public string Fld43 { get; set; }
        public string Fld49 { get; set; }
        public string Fld50 { get; set; }
        public string Fld51 { get; set; }
        public string Fld52 { get; set; }
        public string Fld56 { get; set; }
        public string Fld61 { get; set; }
        public string Fld63 { get; set; }
        public string Fld64 { get; set; }
        public string Fld91 { get; set; }
        public string Fld95 { get; set; }
        public string Fld100 { get; set; }
        public string Fld102 { get; set; }
        public string Fld103 { get; set; }
        public string Fld123 { get; set; }
        public string Fld127 { get; set; }
        public string Fld18 { get; set; }
        public string Fld22 { get; set; }
        public string Fld23 { get; set; }
        public string Fld24 { get; set; }
        public string Fld25 { get; set; }
        public string Fld26 { get; set; }
        public string Fld28 { get; set; }
        public string Fld30 { get; set; }
        public string Fld32 { get; set; }
        public string Fld33 { get; set; }
        public string Fld35 { get; set; }
        public string Fld48 { get; set; }
        public string Fld54 { get; set; }
        public string Fld59 { get; set; }
        public string Fld39 { get; set; }
        public string Fld38 { get; set; }
    }
    
    public class EmailBodyModel
    {

    }
}
