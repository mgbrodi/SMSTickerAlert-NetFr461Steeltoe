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

using Autofac;
using Autofac.Integration.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelsCommons.Model;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Common.Logging.Autofac;
using Steeltoe.Common.Options.Autofac;
using System.Data;


namespace SMSTickerAlert.App_Start
{
    public class ContainerConfig
    {
        public static ContainerProvider Register(IConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterConfiguration(config);
            builder.RegisterLogging(config);
            builder.RegisterOptions();

            builder.RegisterSqlServerConnection(ApplicationConfig.Configuration);
            builder.Register(ctx =>
            {
                var connString = ctx.Resolve<IDbConnection>().ConnectionString;
                return new TickerContext(connString);
            })
                .SingleInstance();

            builder.Register(ctx => ApplicationConfig.LoggerProvider)
                .AsSelf()
                .As<ILoggerProvider>()
                .SingleInstance();

            builder.RegisterAssemblyModules(typeof(Global).Assembly);
            var container = builder.Build();

            ManagementConfigExtensions.ConfigureManagementActuators(
               ApplicationConfig.Configuration,
               ApplicationConfig.LoggerProvider,
               null,
               ApplicationConfig.LoggerFactory);

            ManagementConfigExtensions.Start();
            ManagementConfigExtensions.UseCloudFoundryMetricsExporter(ApplicationConfig.Configuration, ApplicationConfig.LoggerFactory);

            return new ContainerProvider(container);
        }
    }
}