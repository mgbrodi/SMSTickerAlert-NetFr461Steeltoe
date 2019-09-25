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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using System;
using System.IO;

namespace SMSTickerAlert.App_Start
{
    public class ApplicationConfig
    {
        public static IConfiguration Configuration { get; private set; }
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILoggerProvider LoggerProvider { get; set; }

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

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetContentRoot())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCloudFoundry();
            Configuration = builder.Build();
            ConfigureLogging();
        }

        public static string GetContentRoot()
        {
            var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }


        public static void ConfigureLogging()
        {
            LoggerProvider = new DynamicLoggerProvider(new ConsoleLoggerSettings().FromConfiguration(Configuration));
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(LoggerProvider);
        }

    }
}