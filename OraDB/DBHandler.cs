using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OraDB
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
            //systemconnection = String.Format("Server={0};Database={1};User ID={2};Password={3};", "DITLPT018\\SQL2019", "ESB_DB", "sa", "sYstem@123!#");

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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand(sql, connect))
                    {
                        using (OracleDataAdapter sd = new OracleDataAdapter(cmd))
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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand(sql, connect))
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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand(sql, connect))
                    {
                        connect.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
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
        public DataTable GetRecords(string module, string param1 = "", string param2 = "", DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand("get_records", connect))
                    {
                        using (OracleDataAdapter sd = new OracleDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@module", module);
                            cmd.Parameters.Add("@param1", param1);
                            cmd.Parameters.Add("@param2", param2);
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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand("get_records_by_id", connect))
                    {
                        using (OracleDataAdapter sd = new OracleDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@module", module);
                            cmd.Parameters.Add("@id", id);
                            cmd.Parameters.Add("@param1", param1);
                            cmd.Parameters.Add("@param2", param2);
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

        public string GetScalarItem(string sql, DataBaseObject database = DataBaseObject.HostDB)
        {
            string scalaritem = "";

            try
            {
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand command = new OracleCommand(sql, connect))
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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand("delete_records", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@recordid", id);
                        cmd.Parameters.Add("@module", module);
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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand("approve_records", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@recordid", id);
                        cmd.Parameters.Add("@approved_by", approved_by);
                        cmd.Parameters.Add("@module", module);
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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand("add_audit_trail", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@in_user_name", mymodel.user_name);
                        cmd.Parameters.Add("@in_action_type", mymodel.action_type);
                        cmd.Parameters.Add("@in_action_description", mymodel.action_description);
                        cmd.Parameters.Add("@in_page_accessed", mymodel.page_accessed);
                        cmd.Parameters.Add("@in_client_ip_address", mymodel.client_ip_address);
                        cmd.Parameters.Add("@in_session_id", mymodel.session_id);

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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand("add_token", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@expiry", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@api_user_name", mymodel.api_user_name);
                        cmd.Parameters.Add("@token", mymodel.token);
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
                using (OracleConnection connect = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand cmd = new OracleCommand("update_api_call_response", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@reference_number", mymodel.reference_number);
                        cmd.Parameters.Add("@response", mymodel.response);
                        cmd.Parameters.Add("@response_desc", mymodel.response_desc);
                        cmd.Parameters.Add("@third_party", mymodel.third_party);

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

        public async Task<DataTable> validate_api_consumer(string strusername, string strpassword, string strtransaction_type, string strexternal_ref_num, string ip_address, string token = "", DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable datatable = new DataTable();
            try
            {
                using (OracleConnection connection = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand command = new OracleCommand("validate_api_consumer", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@api_user", strusername);
                        command.Parameters.Add("@api_password", strpassword);
                        command.Parameters.Add("@external_ref_num", strexternal_ref_num);
                        command.Parameters.Add("@transaction_type", strtransaction_type);
                        command.Parameters.Add("@apicallerip", ip_address);
                        command.Parameters.Add("@token", token);
                        OracleDataAdapter dataadapter = new OracleDataAdapter();
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

        public DataTable tangazo_validate_api_consumer(string strusername, string strpassword, string strtransaction_type, string strexternal_ref_num, string ip_address, string token = "", DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable datatable = new DataTable();
            try
            {
                using (OracleConnection connection = new OracleConnection(GetDataBaseConnection(database)))
                {
                    using (OracleCommand command = new OracleCommand("validate_api_consumer", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@api_user", strusername);
                        command.Parameters.Add("@api_password", strpassword);
                        command.Parameters.Add("@external_ref_num", strexternal_ref_num);
                        command.Parameters.Add("@transaction_type", strtransaction_type);
                        command.Parameters.Add("@apicallerip", ip_address);
                        command.Parameters.Add("@token", token);
                        OracleDataAdapter dataadapter = new OracleDataAdapter();
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



    }
    public class Crypto
    {
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
}

