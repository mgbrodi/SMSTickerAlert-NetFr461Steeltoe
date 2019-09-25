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
using System;
using System.Linq;
using System.Web.ModelBinding;


namespace SMSTickerAlert.TickerAlert
{
    public partial class SMSEdit : System.Web.UI.Page
    {
        public TickerContext _db { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {           
            string tickerName = Request.QueryString["id"];
            if (tickerName == null || tickerName.Length == 0)
            {
                Response.Redirect("/Default.aspx");
            }
            if (txtMobile.Text.Length < 4)
            {
                GetConfig(tickerName);
            }
        }

        public void GetConfig([QueryString("id")] string tickerName)
        {
 
            Ticker ticker = _db.Tickers.Where(o => o.TickerName.Equals(tickerName)).FirstOrDefault();
            lblCurrent.Text = string.Format("$ {0:#,##0.00}", double.Parse(ticker.Current.ToString()));
            lblLastRead.Text = ticker.LastRead.ToString();
            lblTicker.Text = tickerName;
            txtHigh.Text = (ticker.Current + 0.5m).ToString();
            txtLow.Text = (ticker.Current - 0.5m).ToString(); 
        }

        protected void Subscribe_Click(object sender, EventArgs e)
        {
            if (txtMobile.Text.Length < 4 || txtHigh.Text.Length==0 || txtLow.Text.Length == 0)
            {
                lblError.Visible = true;
            }
            else
            {
                using (TickerSMSSettingsActions smsLogic = new TickerSMSSettingsActions(_db))
                {
                    smsLogic.AddUpdateTickerSMSSettings(
                    new TickerSMSSettings()
                    {
                        TickerName = Request.QueryString["id"],
                        TickerDate = DateTime.Parse(lblLastRead.Text),
                        High = Decimal.Parse(txtHigh.Text.Replace("$ ","")),
                        Low = Decimal.Parse(txtLow.Text.Replace("$ ", "")),
                        Mobile = "+1"+txtMobile.Text
                    });
                }
            }
            Response.Redirect("/Default.aspx");
        }
    }    
}