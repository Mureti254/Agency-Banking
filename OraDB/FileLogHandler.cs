using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OraDB
{
    public class FileLogHandler
    {
        // public static IConfiguration _configuration;

        public static string errorpath,logpath;
        public FileLogHandler()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            errorpath = root.GetSection("Data:Errorlogger").ToString();
            logpath = root.GetSection("Data:Infologger").ToString();
        }
        public static void log_message_fields(string module, string messagetype, string message)
        {
            string strFileName = null;
            
            StreamWriter f;
            DBHandler dbhandler = new DBHandler();
            string filename = "_"+DateTime.Now.ToString("yyyy_MM_dd");

            try
            {
                string logfilespath = dbhandler.GetRecords("parameters", "LOG_FILES_PATH").Rows[0]["item_value"].ToString();
              
                if (messagetype == "ERROR") 
                {

                    strFileName = logfilespath + module + filename+ "_Errorlog.txt";//Program.configuration.GetSection("Data").GetSection("Errorlogger").Value ?? string.Empty;
                }
                else
                {
                    strFileName = logfilespath + module + filename + "_Infolog.txt"; //Program.configuration.GetSection("Data").GetSection("Errorlogger").Value ?? string.Empty;
                }
                                
                f = new StreamWriter(strFileName, true);
                f.WriteLine(message);
                f.Close();
            }
            catch (Exception ex)
            {
                throw ex; //through to avoid system running without logs
            }
        }

       
    }
}
