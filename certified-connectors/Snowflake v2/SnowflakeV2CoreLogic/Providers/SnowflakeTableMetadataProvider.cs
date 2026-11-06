// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Utilities;

    /// <summary>
    /// Implements operations performed to interact with metadata on snowflake tables.
    /// </summary>
    public class SnowflakeTableMetadataProvider : ITableMetadataProvider
    {
        private readonly SnowflakeDBOperations snowflakeDBOperations;
        private readonly SnowflakeConnectionParametersProvider snowflakeConnectionParametersProvider;
        private readonly ILogger logger;

        public SnowflakeTableMetadataProvider(
            SnowflakeDBOperations sfDBOperationsClient,
            SnowflakeConnectionParametersProvider snowflakeConnectionParametersProvider,
            ILogger logger)
        {
            snowflakeDBOperations = sfDBOperationsClient ?? throw new ArgumentNullException(nameof(sfDBOperationsClient));
            this.snowflakeConnectionParametersProvider = snowflakeConnectionParametersProvider ?? throw new ArgumentNullException(nameof(snowflakeConnectionParametersProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task<TableMetadata> GetTableAsync(
            HttpRequestMessage request,
            string dataSet,
            string table)
        {
            return GetTableAsync(request, dataSet, table, TableOperation.GetItem);
        }

        /// <inheritdoc />
        public async Task<TableMetadata> GetTableAsync(
            HttpRequestMessage request,
            string dataSet,
            string table,
            TableOperation operation)
        {
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, nameof(GetTableAsync), "_", "_"));

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrEmpty(dataSet))
            {
                throw new ArgumentNullException("dataSet");
            }

            if (string.IsNullOrEmpty(table))
            {
                throw new ArgumentNullException("table");
            }

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);
            SnowflakeConnectionParametersProvider.EnsureTableWithinConnection(table, connectionParameters);

            var metadataTask = snowflakeDBOperations.GetTableMetadataAsync(table, "GET $metadata.json/datasets/{dataset}/tables/{table}", connectionParameters);
            var primaryKeyTask = snowflakeDBOperations.GetPrimaryKeyAsync(table, "GET $metadata.json/datasets/{dataset}/tables/{table}", connectionParameters);

            // Wait for the both calls to compelte
            await Task.WhenAll(metadataTask, primaryKeyTask).ConfigureAwait(true);

            // Gather the results
            var metadataResponse = metadataTask.Result;
            var primaryKeyResponse = primaryKeyTask.Result;

            logger.LogInformation(Constants.ClientSuccessMessage);

            // Now we need to map it into an OData TableMetadata object
            TableMetadata result = SnowflakeToODataHelper.TableMetadataToOdata(metadataResponse, primaryKeyResponse, table);

            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, nameof(GetTableAsync), "_", "_"));
            return result;
        }
    }
}
