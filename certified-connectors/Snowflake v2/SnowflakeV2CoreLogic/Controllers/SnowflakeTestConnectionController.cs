// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Controllers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Diagnostic controller for Snowflake
    /// </summary>
    public class SnowflakeTestConnectionController : ApiController
    {
        private readonly IDiagnosticProvider diagnosticProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeTestConnectionController"/> class
        /// </summary>
        /// <param name="diagnosticProvider">diagnostic provider</param>
        /// <param name="logger">logger</param>
        public SnowflakeTestConnectionController(
            IDiagnosticProvider diagnosticProvider,
            ILogger logger)
        {
            this.diagnosticProvider = diagnosticProvider ?? throw new ArgumentNullException(nameof(diagnosticProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Execute TestConnection operation
        /// </summary>
        /// <returns>Http response</returns>
        [HttpGet]
        [Route("testconnection")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> TestConnectionAsync()
        {
            logger.LogInformation("TestConnection started");

            // Execute operation
            try
            {
                HttpResponseMessage response = await diagnosticProvider.TestConnectionAsync(Request).ConfigureAwait(false);
                return ResponseMessage(response);
            }
            finally
            {
                logger.LogInformation("TestConnection ended");
            }
        }
    }
}