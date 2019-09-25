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

using System.Collections.Generic;
using System.Web.Http.Description;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.CloudFoundry.Connector.Relational;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Census.Stats;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Exporter.Metrics.CloudFoundryForwarder;

namespace SMSTickerAlert.App_Start
{
    public static class ManagementConfigExtensions
    {
        public static IMetricsExporter MetricsExporter { get; set; }

        public static void Start()
        {
            DiagnosticsManager.Instance.Start();
            if (MetricsExporter != null)
            {
                MetricsExporter.Start();
            }
        }

        public static void Stop()
        {
            DiagnosticsManager.Instance.Stop();
            if (MetricsExporter != null)
            {
                MetricsExporter.Stop();
            }
        }

        private static IEnumerable<IHealthContributor> GetHealthContributors(IConfiguration configuration)
        {
            var healthContributors = new List<IHealthContributor>
                {
                    new DiskSpaceContributor(),
                    new CustomHealthContributor(),
                    RelationalHealthContributor.GetSqlServerContributor(configuration)
                };

            return healthContributors;
        }


        public static void UseCloudFoundryMetricsExporter(IConfiguration configuration, ILoggerFactory loggerFactory = null)
        {
            var options = new CloudFoundryForwarderOptions(configuration);
            MetricsExporter = new CloudFoundryForwarderExporter(options, OpenCensusStats.Instance, loggerFactory != null ? loggerFactory.CreateLogger<CloudFoundryForwarderExporter>() : null);
        }

        public static void ConfigureManagementActuators(IConfiguration configuration, ILoggerProvider dynamicLogger, IApiExplorer apiExplorer, ILoggerFactory loggerFactory = null)
        {
            ActuatorConfigurator.UseCloudFoundryActuators(configuration, dynamicLogger, GetHealthContributors(configuration), apiExplorer, loggerFactory);

        }
    }
}