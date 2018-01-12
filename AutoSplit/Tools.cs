using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AutoSplit
{
    public static class Tools
    {
        public static string HttpPost(string Url, string postData, int timeOut = 60000)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    WebRequest request = WebRequest.Create(Url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    request.Timeout = timeOut;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();
                    dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    return responseFromServer;
                }
                catch (WebException)
                {
                    Console.WriteLine("网络超时，重试中……");
                    continue;
                }
            }
            return "";
        }

        public static decimal GetBalance()
        {
            var json = HttpPost("http://localhost:20332", "{'jsonrpc': '2.0','method': 'getbalance','params': ['602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7'],'id': 1}");
            return (decimal)JObject.Parse(json)["result"]["confirmed"];
        }

        public static bool Send(string to, decimal value)
        {
            var json = HttpPost("http://localhost:20332", $"{{'jsonrpc': '2.0','method': 'sendtoaddress','params': ['602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7','{to}',{value}],'id': 1}}");
            var a = JObject.Parse(json)["result"];
            return a != null;
        }
    }

}
