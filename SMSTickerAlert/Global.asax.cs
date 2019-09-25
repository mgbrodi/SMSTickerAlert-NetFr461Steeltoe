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

using Autofac.Integration.Web;
using SMSTickerAlert.App_Start;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;


namespace SMSTickerAlert
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        static IContainerProvider _containerProvider;
        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }

        protected void Application_Start(object sender, EventArgs e)
        {

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ApplicationConfig.RegisterConfig();

            _containerProvider = ContainerConfig.Register(ApplicationConfig.Configuration);

        }
        protected void Application_Stop()
        {
            ManagementConfigExtensions.Stop();
        }
    }
}

