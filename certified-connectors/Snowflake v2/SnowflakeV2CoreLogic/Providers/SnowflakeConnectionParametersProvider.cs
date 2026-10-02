// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Exceptions;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Utilities;

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

            if (connectionParametersProvider.PropertyExists(Constants.UseCaseInsensitiveFilters))
            {
                connectionParameters.UseCaseInsensitiveFilters = connectionParametersProvider.GetProperty<bool>(Constants.UseCaseInsensitiveFilters);
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

        /// <summary>
        /// Combines the server and database of the dataset with the ones configured on the connection.
        /// A value present on only one side is used as is. A value present on both sides must match,
        /// otherwise the request is rejected. An empty or "default" dataset part counts as absent.
        /// </summary>
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

            snowflakeConnectionParameters.Server = CoalesceDatasetValue(decodedServer, snowflakeConnectionParameters.Server, StringComparison.OrdinalIgnoreCase, "server");
            snowflakeConnectionParameters.Database = CoalesceDatasetValue(decodedDatabase, snowflakeConnectionParameters.Database, StringComparison.Ordinal, "database");

            return snowflakeConnectionParameters;
        }

        /// <summary>
        /// Rejects a table name whose database or schema qualifier does not match the database and
        /// schema in use. Qualifiers with nothing to compare against are let through.
        /// </summary>
        public static void EnsureTableWithinConnection(
            string table,
            SnowflakeConnectionParameters snowflakeConnectionParameters)
        {
            try
            {
                table.EnsureQualifiedIdentifierWithinScope(snowflakeConnectionParameters.Database, snowflakeConnectionParameters.Schema, "Table Name");
            }
            catch (ArgumentException ex)
            {
                throw CreateBadRequestException(ex.Message);
            }
        }

        private static string CoalesceDatasetValue(
            string datasetValue,
            string connectionValue,
            StringComparison comparison,
            string name)
        {
            if (string.IsNullOrWhiteSpace(datasetValue) || string.Equals(datasetValue, StringConstants.DefaultDataSet, StringComparison.OrdinalIgnoreCase))
            {
                return connectionValue;
            }

            if (string.IsNullOrWhiteSpace(connectionValue))
            {
                return datasetValue;
            }

            if (!string.Equals(datasetValue, connectionValue, comparison))
            {
                throw CreateBadRequestException($"The dataset {name} does not match the {name} configured on the connection.");
            }

            return connectionValue;
        }

        private static HttpResponseException CreateBadRequestException(string message)
        {
            return new HttpResponseException(SnowflakeHttpException.CreateHttpResponseMessage(HttpStatusCode.BadRequest, message));
        }
    }
}
