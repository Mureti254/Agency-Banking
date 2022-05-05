using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace DB
{
    public static class HttpClientHelper
    {
        public class HttpHandler
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();

            public string HttpClientPost(string url, Newtonsoft.Json.Linq.JObject request_data)
            {
                string result = null;

                try
                {
                    var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpRequest.Method = "post";
                    httpRequest.ContentType = "application/json";
                    using (var dataStream = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        dataStream.Write(request_data);
                        dataStream.Flush();
                        dataStream.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Console.WriteLine(ex.Message);
                    Newtonsoft.Json.Linq.JObject error_data = new Newtonsoft.Json.Linq.JObject
                    {
                        { "exception", ex.Message }
                    };
                    return error_data.ToString();
                }

                return result;
            }

            public string HttpClientPostJson(string url, IEnumerable<KeyValuePair<string, string>> headerValueCollection, Newtonsoft.Json.Linq.JObject request_data)
            {
                string result = null;

                try
                {
                    var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpRequest.Method = "post";
                    httpRequest.ContentType = "application/json";

                    foreach (KeyValuePair<string, string> entry in headerValueCollection)
                    {
                        // Add header populated with entry.Value or entry.Key
                        httpRequest.Headers.Add(entry.Key, entry.Value);
                    }

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

                    using (var dataStream = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        dataStream.Write(request_data);
                        dataStream.Flush();
                        dataStream.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Console.WriteLine(ex.Message);
                    Newtonsoft.Json.Linq.JObject error_data = new Newtonsoft.Json.Linq.JObject
                    {
                        { "exception", ex.Message }
                    };
                    return error_data.ToString();
                }

                return result;
            }

            public string HttpClientPostForm(string url, IEnumerable<KeyValuePair<string, string>> headerValueCollection, string request_data)
            {
                string result = null;

                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                    var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpRequest.Method = "post";

                    foreach (KeyValuePair<string, string> entry in headerValueCollection)
                    {
                        // Add header populated with entry.Value or entry.Key
                        httpRequest.Headers.Add(entry.Key, entry.Value);
                    }

                    httpRequest.ContentType = "application/x-www-form-urlencoded";
                    httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    using (var dataStream = new StreamWriter(httpRequest.GetRequestStream(), System.Text.Encoding.ASCII))
                    {
                        dataStream.Write(request_data);
                        dataStream.Flush();
                        dataStream.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Console.WriteLine(ex.Message);
                    Newtonsoft.Json.Linq.JObject error_data = new Newtonsoft.Json.Linq.JObject
                    {
                        { "exception", ex.Message }
                    };
                    return error_data.ToString();
                }

                return result;
            }

            public string HttpClientGet(string url, IEnumerable<KeyValuePair<string, string>> headerValueCollection, string request_data, string content_type = "application/json")
            {
                string result = null;

                try
                {
                    var httpRequest = (HttpWebRequest)WebRequest.Create(url + request_data);
                    httpRequest.Method = "get";

                    foreach (KeyValuePair<string, string> entry in headerValueCollection)
                    {
                        // Add header populated with entry.Value or entry.Key
                        httpRequest.Headers.Add(entry.Key, entry.Value);
                    }

                    //httpRequest.ContentType = content_type;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    Console.WriteLine(ex.Message);
                    Newtonsoft.Json.Linq.JObject error_data = new Newtonsoft.Json.Linq.JObject
                    {
                        { "exception", ex.Message }
                    };
                    return error_data.ToString();
                }

                return result;
            }

            public HttpWebRequest CreateSOAPWebRequest(string serverendpoint, string soapaction)
            {
                //Making Web Request
                HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(serverendpoint);
                //SOAPAction
                Req.Headers.Add(soapaction);
                //Content_type
                Req.ContentType = "text/xml; charset=\"utf-8\"";
                Req.Accept = "text/xml";
                //HTTP method
                Req.Method = "POST";
                //return HttpWebRequest
                return Req;
            }
        }
    }
}