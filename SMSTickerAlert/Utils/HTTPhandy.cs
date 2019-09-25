using SMSTickerAlert.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace SMSTickerAlert.Utils
{
    public static class HTTPhandy
    {
        public static void SendSMS(ref TickerSMSSettings _ticker)
        {
            StringBuilder postData = new StringBuilder("");
            //            curl - i \
            //-X POST \
            //-H "Content-Type: application/json" \
            //-H "Accept: application/json" \
            //-H "Authorization: Y3HSdmWsR--d4kvmDv2EOA==" \
            //-d '{"content": "blah blah", "to": ["19143343722"]}' \
            //-s https://platform.clickatell.com/messages


            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["SMS:Url"]);
            // Set the Method property of the request to POST.  
            request.Method = "POST";

            // Create POST data and convert it to a byte array.  
            //postData.AppendFormat(@"{""content"":""Ticker: {0}, current: {1}, high: {2}, low: {3}"", ""to"":[""{4}""]}",
            //    _ticker.TickerName, _ticker.Current.ToString(), _ticker.High.ToString(), _ticker.Low.ToString(), "19143343722");

            postData.Append(@"{""content"":""Ticker: 0, current: , high: 1 , low: 2"", ""to"":[""19143343722""]}");
               // _ticker.TickerName);//, _ticker.Current.ToString(), _ticker.High.ToString(), _ticker.Low.ToString(), "19143343722");


            byte[] byteArray = Encoding.UTF8.GetBytes(postData.ToString());

            // Set the ContentType property of the WebRequest.  
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + ConfigurationManager.AppSettings["SMS:Token"]);

            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;

            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();

            // Get the response.  
            WebResponse response = request.GetResponse();

            // Close the response.  
            response.Close();

            _ticker.Mobile = "";
        }


        public static Decimal TickerFromAPI(string name)
        {
            Decimal current = 0;
            if (name != null && name.Length > 0)
            {
                var url = ConfigurationManager.AppSettings["FinProvider:Url"].Replace(ConfigurationManager.AppSettings["FinProvider:Replace"], name);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                WebResponse response = request.GetResponse();

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();

                    //marketwatch <==> yahoo
                    if (ConfigurationManager.AppSettings["FinProvider:Name"] == "marketwatch")
                    {
                        //<meta name="price" content="220.83">
                        Match match = Regex.Match(responseFromServer, @"<meta name=""price"" content=""(\d+,?\d+\.\d+)"">",
                            RegexOptions.IgnoreCase);

                        if (match.Success)
                        {
                            Decimal.TryParse(match.Groups[1].Value, out current);
                        }
                    }
                }

                response.Close();
            }
            return current;
        }
    }
}