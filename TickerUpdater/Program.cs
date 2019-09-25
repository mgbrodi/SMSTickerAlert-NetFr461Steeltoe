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

using ModelsCommons.Model;
using System;
using System.Linq;
using System.Threading;
using TickerUpdater.Configuration;

namespace TickerUpdater
{
    class Program
    {
        static TickerContext _db;

        static void Main(string[] args)
        {
            {
                AppConfig.RegisterConfig("");
                _db = new TickerContext(AppConfig.TickerSqlConnection.ConnectionString);
               
                MainLoop();
            }
        }

        static void MainLoop()
        {
            while (true)
            {
                try
                {
                    foreach (Ticker ticker in _db.Tickers)
                    {
                        ticker.LastRead = DateTime.UtcNow;
                        ticker.Current = Util.HTTPhandy.TickerFromAPI(AppConfig.FinConfiguration,ticker.TickerName);
                        Console.WriteLine(ticker.TickerName+":"+ ticker.Current);
                    }
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                try
                {
                    var results = from t1 in _db.Tickers
                                  join t2 in _db.TickerSMSSettings on t1.TickerName equals t2.TickerName
                                  where (t1.Current <= t2.Low) || (t1.Current >= t2.High)
                                  select new
                                  {
                                      t1.TickerName,
                                      t1.Current,
                                      t2.Mobile,
                                      t2.Low,
                                      t2.High,
                                      t1.LastRead
                                  };
                    TickerSMSSettings found;
                    foreach (var smsSettings in results.ToList())
                    {
                        Console.WriteLine(smsSettings.TickerName + " " + smsSettings.Mobile + " " + smsSettings.Current);
                        found = _db.TickerSMSSettings.Where(s => s.Mobile == smsSettings.Mobile)
                   .FirstOrDefault<TickerSMSSettings>();
                        found.TickerDate = smsSettings.LastRead;
                        Util.HTTPhandy.SendSMS(AppConfig.TwConfiguration, found, smsSettings.Current);
                        _db.TickerSMSSettings.Remove(found);
                    }
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(AppConfig.Interval);
            }
        }
    }
}
