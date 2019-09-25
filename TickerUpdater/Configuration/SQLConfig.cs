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

using Microsoft.Extensions.Configuration;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.CloudFoundry.Connector.SqlServer;
using System.Data.SqlClient;

namespace TickerUpdater.Configuration
{
    public class SQLConfig
    {
        public static SqlConnection Connection(IConfiguration config)
        {
            SqlServerProviderConnectorOptions SqlServerConfig = new SqlServerProviderConnectorOptions(config);
            SqlServerServiceInfo info = config.GetSingletonServiceInfo<SqlServerServiceInfo>();
           
            SqlServerProviderConnectorFactory factorySQLC = new SqlServerProviderConnectorFactory(info, SqlServerConfig, typeof(SqlConnection));
            return (SqlConnection)factorySQLC.Create(null);
        }
    }
}
