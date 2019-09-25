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
using SMSTickerAlert.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;


namespace SMSTickerAlert
{
    public partial class _Default : Page
    {
        public TickerContext _db { get; set; }

        public IQueryable<Ticker> GetTickers()
        {
           
            IQueryable<Ticker> query = _db.Tickers;
            return query;
        }

        public List<TickerSMSSettings> SelectTickers()
        {
            using (TickerSMSSettingsActions smsTickers = new TickerSMSSettingsActions(_db))
            {
                return smsTickers.GetTickerSMSSettings("");
            }
        }

        public void SMSTickerAlertGrid_DeleteItem(string mobile)
        {
            using (TickerSMSSettingsActions smsTickers = new TickerSMSSettingsActions(_db))
            {
                smsTickers.DeleteTickerSMSSettings(mobile);
            }

        }
    }
}