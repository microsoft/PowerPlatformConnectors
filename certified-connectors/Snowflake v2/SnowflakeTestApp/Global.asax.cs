// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeTestApp
{
    using System.Web;
    using System.Web.Http;

    /// <summary>
    /// WebApi application
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        /// Method for performing Tasks on application start
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
