// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Models;

    public class SnowflakeConnectionParametersProvider
    {
        private static readonly IDictionary<string, AuthenticationType> AuthenticationTypeMap = new Dictionary<string, AuthenticationType>
        {
            ["oauthSP"] = AuthenticationType.AAD,
            ["oauthSPUserDelegated"] = AuthenticationType.AADUserDelegated,
        };

        private readonly IConnectionParametersProvider connectionParametersProvider;
        private readonly ILogger logger;

        public SnowflakeConnectionParametersProvider(
            IConnectionParametersProvider connectionParametersProvider,
            ILogger logger)
        {
            this.connectionParametersProvider = connectionParametersProvider ?? throw new ArgumentNullException(nameof(connectionParametersProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public SnowflakeConnectionParameters GetConnectionParameters()
        {
            var connectionParameters = new SnowflakeConnectionParameters();

            if (connectionParametersProvider.PropertyExists(Constants.Server))
            {
                connectionParameters.Server = connectionParametersProvider.GetProperty<string>(Constants.Server);
            }

            if (connectionParametersProvider.PropertyExists(Constants.Database))
            {
                connectionParameters.Database = connectionParametersProvider.GetProperty<string>(Constants.Database);
            }

            if (connectionParametersProvider.PropertyExists(Constants.Role))
            {
                connectionParameters.Role = connectionParametersProvider.GetProperty<string>(Constants.Role);
            }

            if (connectionParametersProvider.PropertyExists(Constants.Warehouse))
            {
                connectionParameters.Warehouse = connectionParametersProvider.GetProperty<string>(Constants.Warehouse);
            }

            if (connectionParametersProvider.PropertyExists(Constants.Schema))
            {
                connectionParameters.Schema = connectionParametersProvider.GetProperty<string>(Constants.Schema);
            }

            connectionParameters.AuthenticationType = GetAuthenticationType();
            connectionParameters.Token = connectionParametersProvider.GetToken();

            return connectionParameters;
        }

        public AuthenticationType GetAuthenticationType()
        {
            AuthenticationType authenticationType = AuthenticationType.AAD;

            if (connectionParametersProvider.TryGetProperty("$parameterSet", out string parameterSet))
            {
                logger.LogInformation($"Multi auth connection with $parameterSet {parameterSet}");
                if (!AuthenticationTypeMap.TryGetValue(parameterSet, out authenticationType))
                {
                    throw new Exception($"Unknown authentication type used: {parameterSet}");
                }
            }

            return authenticationType;
        }

        public Uri GetReferralUrl()
        {
            return connectionParametersProvider.GetReferrerUri();
        }

        public static SnowflakeConnectionParameters UpdateConnParametersToUseDataset(
           HttpRequestMessage request,
           string dataset,
           SnowflakeConnectionParameters snowflakeConnectionParameters)
        {
            if (string.IsNullOrWhiteSpace(dataset))
            {
                return snowflakeConnectionParameters;
            }

            if (string.Equals(dataset, StringConstants.DefaultDataSet, StringComparison.OrdinalIgnoreCase))
            {
                return snowflakeConnectionParameters;
            }

            // We need to look at the url encoded value of datasets to be able to correctly determine the server and database
            int datasetValueIndex = Array.FindIndex(request.RequestUri.Segments, t => t.Equals("datasets/", StringComparison.OrdinalIgnoreCase));

            if (request.RequestUri.Segments.Length < datasetValueIndex + 2)
            {
                return snowflakeConnectionParameters;
            }

            string datasetValue = request.RequestUri.Segments[datasetValueIndex + 1].TrimEnd(new char[] { '/' });
            var datasources = datasetValue.Split(new char[] { ',' }).ToList<string>();

            if (datasources == null || datasources.Count != 2)
            {
                throw new InvalidOperationException("Unable to parse dataset.");
            }

            string decodedServer = HttpUtility.UrlDecode(HttpUtility.UrlDecode(datasources[0]));
            string decodedDatabase = HttpUtility.UrlDecode(HttpUtility.UrlDecode(datasources[1]));

            if (snowflakeConnectionParameters.AuthenticationType == AuthenticationType.AAD)
            {
                snowflakeConnectionParameters.Server = decodedServer;
                snowflakeConnectionParameters.Database = decodedDatabase;
            }

            return snowflakeConnectionParameters;
        }
    }
}
