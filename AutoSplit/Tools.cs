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
            var json = HttpPost("http://localhost:10332", "{'jsonrpc': '2.0','method': 'getbalance','params': ['c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b'],'id': 1}");
            return (decimal)JObject.Parse(json)["result"]["confirmed"];
        }

        public static bool Send(string to, decimal value)
        {
            var json = HttpPost("http://localhost:10332", $@"
            {{
                'jsonrpc': '2.0',
                'method': 'sendtoaddress',
                'params': [
                    'c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b',
                    '{to}',
                    {value}
                ],
                'id': 1
            }}");
            var a = JObject.Parse(json)["result"];
            return a != null;
        }



        public static bool Send(string to, decimal value, string changeAddress)
        {
            var json = HttpPost("http://localhost:10332", $@"
            {{
                'jsonrpc': '2.0',
                'method': 'sendmany',
                'params': [
                    [
                        {{
                            'asset': 'c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b',
                            'value': {value},
                            'address': '{to}'
                        }}
                    ],
                    0,
                    '{changeAddress}'
                ],
                'id': 1
            }}");
            var a = JObject.Parse(json)["result"];
            return a != null;
        }
    }

}
