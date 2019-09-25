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
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Linq;


namespace TickerUpdater.Configuration
{
    static class TwilioConfig
    {
        public static TwilioConf Credentials(CloudFoundryServicesOptions cs)
        {
           
            var tempTwilio = cs.ServicesList.Where(o => o.Name.Equals("Twilio")).FirstOrDefault().Credentials;
            return new TwilioConf()
            {
                Token = tempTwilio.Where(o => o.Key.Equals("SMSKeyToken")).FirstOrDefault().Value.Value,
                From = tempTwilio.Where(o => o.Key.Equals("SMSFrom")).FirstOrDefault().Value.Value,
                Account = tempTwilio.Where(o => o.Key.Equals("SMSAccount")).FirstOrDefault().Value.Value
            };
        }
    }
}
