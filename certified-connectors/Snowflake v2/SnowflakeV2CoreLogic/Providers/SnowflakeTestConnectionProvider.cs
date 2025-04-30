// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic.Exceptions;
    using SnowflakeV2CoreLogic.Models;

    /// <summary>
    /// Diagnostic Provider for Snowflake
    /// </summary>
    public class SnowflakeTestConnectionProvider : IDiagnosticProvider
    {
        private readonly SnowflakeConnectionParametersProvider snowflakeConnectionParametersProvider;
        private readonly ILogger logger;
        private SnowflakeDBOperations snowflakeDBOperations;

        public SnowflakeTestConnectionProvider(
           SnowflakeDBOperations sfDBOperationsClient,
           SnowflakeConnectionParametersProvider sfconnectionParametersProvider,
           ILogger logger)
        {
            snowflakeDBOperations = sfDBOperationsClient ?? throw new ArgumentNullException(nameof(sfDBOperationsClient));
            snowflakeConnectionParametersProvider = sfconnectionParametersProvider ?? throw new ArgumentNullException(nameof(snowflakeConnectionParametersProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Execute TestConnection
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>
        /// A task representing the operation
        /// </returns>
        public async Task<HttpResponseMessage> TestConnectionAsync(
            HttpRequestMessage request)
        {
            try
            {
                var connParam = snowflakeConnectionParametersProvider.GetConnectionParameters();

                // For User Delegated Auth, we return 200 OK without any checks
                // because we don't have snowflake instance
                // information at the time of connection creation.
                if (connParam.AuthenticationType == AuthenticationType.AADUserDelegated)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }

                // Try and query the schema table for Serivce Principal auth
                await snowflakeDBOperations.GetInformationScehmaAsync(connParam).ConfigureAwait(true);

                logger.LogInformation("Test connection succeeded");
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (SnowflakeHttpException snowflakeException)
            {
                logger.LogInformation($"Test connection failed: '{snowflakeException}");
                var errorMessage = string.IsNullOrWhiteSpace(snowflakeException.Message) ? "Unknown error" : snowflakeException.Message;
                throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, Resource.TestConnectionFailedSnowflakeException, errorMessage), snowflakeException);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Test connection failed: '{ex}");
                throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, Resource.TestConnectionFailed, ex.Message), ex);
            }
        }
    }
}