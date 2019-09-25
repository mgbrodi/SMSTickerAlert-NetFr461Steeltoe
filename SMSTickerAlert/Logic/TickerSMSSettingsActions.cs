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
using System.Collections.Generic;
using System.Linq;


namespace SMSTickerAlert.Logic
{
    public class TickerSMSSettingsActions: IDisposable
    {

        private TickerContext _db;

        public TickerSMSSettingsActions(TickerContext db)
        {
            _db = db;
        }

        public void AddUpdateTickerSMSSettings(TickerSMSSettings ticker)
        {
            
            var tickerSMSSettings = _db.TickerSMSSettings.SingleOrDefault(
                    c => c.Mobile == ticker.Mobile);
            if (tickerSMSSettings == null)
            {
                _db.TickerSMSSettings.Add(ticker);
            }
            else
            {
                tickerSMSSettings.TickerName = ticker.TickerName;
                tickerSMSSettings.High = ticker.High;
                tickerSMSSettings.Low = ticker.Low;
                tickerSMSSettings.TickerDate = ticker.TickerDate;
            }
            _db.SaveChanges();
        }


        public List<TickerSMSSettings> GetTickerSMSSettings(string ticker)
        {
            if (ticker.Length > 0)
            {
                return _db.TickerSMSSettings.Where(
                    c => c.TickerName.Equals(ticker)).ToList();
            }
            else
            {
                return _db.TickerSMSSettings.ToList();
            }
        }

        public void DeleteTickerSMSSettings(string mobile)
        {
            var tickerSMSSettings = _db.TickerSMSSettings.SingleOrDefault(
                c => c.Mobile == mobile);
            _db.TickerSMSSettings.Remove(tickerSMSSettings);
            _db.SaveChanges();
        }


        public void Dispose()
        {
            if (_db != null)
            {
                _db = null;
            }
        }
    }
}