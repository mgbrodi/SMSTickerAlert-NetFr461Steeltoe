// Copyright 2019 Maria Gabriella Brodi.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using ModelsCommons.Model;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TickerUpdater.Util
{
    public static class HTTPhandy
    {
        public static void SendSMS(TwilioConf twilioConfig, TickerSMSSettings ticker, Decimal current)
        {
            try
            {
                TwilioClient.Init(twilioConfig.Account, twilioConfig.Token);

                var message = MessageResource.Create(
                    body: ticker.TickerName + " - " + current + " - Thresholds high: " + ticker.High + " low: " + ticker.Low,
                    from: new Twilio.Types.PhoneNumber(twilioConfig.From),
                    to: new Twilio.Types.PhoneNumber(ticker.Mobile)
                );

                Console.WriteLine(message.Sid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        

        public static Decimal TickerFromAPI(FinProvider finProvider, string name)
        {
            Decimal current = 0;
            try
            {
                if (name != null && name.Length > 0)
                {
                    var url = finProvider.Url.Replace(
                        finProvider.Replace, name);

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

                        //Match @"<meta name=""price"" content=""(\d+,?\d+\.\d+)"">",

                        Match match = Regex.Match(responseFromServer, finProvider.Match, RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            Decimal.TryParse(match.Groups[1].Value, out current);
                        }
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return current;
        }
    }
}