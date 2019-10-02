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
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ModelsCommons.Model
{
    public class TickerDatabaseInit : DropCreateDatabaseAlways<TickerContext>
    {
        protected override void Seed(TickerContext context)
        {
            GetTickers().ForEach(c => context.Tickers.Add(c));

        }
        private static List<Ticker> GetTickers()
        {
            var tickers = new List<Ticker> {
                new Ticker
                {
                    TickerName = "AAPL",
                    Description ="Apple",
                    LastRead=DateTime.UtcNow
                },
                new Ticker
                {
                    TickerName = "GOOG",
                    Description = "Google",
                    LastRead=DateTime.UtcNow
                },
                new Ticker
                {
                    TickerName = "PVTL",
                    Description= "Pivotal",
                    Current=0m,
                    LastRead=DateTime.UtcNow
                },
                new Ticker
                {
                    TickerName = "TSLA",
                    Description ="Tesla",
                    LastRead=DateTime.UtcNow
                },
                new Ticker
                {
                    TickerName = "AMZN",
                    Description ="Amazon",
                    LastRead=DateTime.UtcNow
                },
            };

            return tickers;
        }

    }
}
