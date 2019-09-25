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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ModelsCommons.Model;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.ConfigServer;
using System;
using System.Data.SqlClient;
using System.IO;

namespace TickerUpdater.Configuration
{
    public class AppConfig
    {
        public static IConfigurationRoot Configuration { get; private set; }
        public static SqlConnection TickerSqlConnection { get; private set; }
        public static FinProvider FinConfiguration { get; private set; }
        public static TwilioConf TwConfiguration { get; private set; }
        public static int Interval { get; private set; }

        public static CloudFoundryServicesOptions CloudFoundryServices
        {
            get
            {
                var opts = new CloudFoundryServicesOptions();
                var serviceSection = Configuration.GetSection(CloudFoundryServicesOptions.CONFIGURATION_PREFIX);
                serviceSection.Bind(opts);
                return opts;
            }
        }


        public static void RegisterConfig(string environment = "")
        {
            try
            {
                ILoggerFactory factoryIlog = new LoggerFactory();
                factoryIlog.AddProvider(new ConsoleLoggerProvider((category, logLevel) => logLevel >= LogLevel.Trace, false));

                // Set up configuration sources.
                var builder = new ConfigurationBuilder()
                    .SetBasePath(GetContentRoot())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .AddCloudFoundry()
                    .AddConfigServer(environment, factoryIlog);
                Configuration = builder.Build();

                TickerSqlConnection = SQLConfig.Connection(Configuration);

                TwConfiguration = TwilioConfig.Credentials(CloudFoundryServices);

                FinConfiguration = FinConfig.Provider(Configuration);
                Interval = Configuration.GetValue<int>("Interval");                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Reading Configuration: "+ex.Message);
            }
        }

        public static string GetContentRoot()
        {
            var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }

    }
}