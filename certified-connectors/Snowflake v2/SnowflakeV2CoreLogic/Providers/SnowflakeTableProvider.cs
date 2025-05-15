// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.OData.Query;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;

    /// <summary>
    /// Implements the operations performed on Snowflake tables.
    /// </summary>
    public class SnowflakeTableProvider : ITableProvider
    {
        private readonly SnowflakeDBOperations snowflakeDBOperations;
        private readonly SnowflakeConnectionParametersProvider snowflakeConnectionParametersProvider;
        private readonly ILogger logger;

        public SnowflakeTableProvider(
            SnowflakeDBOperations sfDBOperationsClient,
            SnowflakeConnectionParametersProvider snowflakeConnectionParametersProvider,
            ILogger logger)
        {
            snowflakeDBOperations = sfDBOperationsClient ?? throw new ArgumentNullException(nameof(sfDBOperationsClient));
            this.snowflakeConnectionParametersProvider = snowflakeConnectionParametersProvider ?? throw new ArgumentNullException(nameof(snowflakeConnectionParametersProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<TableCollection> ListTablesAsync(
            HttpRequestMessage request,
            string dataSet,
            string operation,
            ODataQueryOptions<Table> options)
        {
            return await ListTablesAsync(request, dataSet).ConfigureAwait(false);
        }

        public async Task<TableCollection> ListTablesAsync(
            HttpRequestMessage request,
            string dataSet)
        {
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, nameof(ListTablesAsync), "_", "_"));

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrEmpty(dataSet))
            {
                throw new ArgumentNullException("dataSet");
            }

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);

            logger.LogInformation("Send request to Snowflake to get all tables.");

            SnowflakeTableData data = await snowflakeDBOperations.GetTablesForSchemaAsync(connectionParameters).ConfigureAwait(true);

            logger.LogInformation(Constants.ClientSuccessMessage);

            var result = new TableCollection();
            if (data != null)
            {
                foreach (Dictionary<string, object> rowData in data.ToGenericDictionaryList())
                {
                    // Get the value associated with the table name key
                    string tableName = (string)rowData["TABLE_NAME"];

                    result.Add(new Table
                    {
                        DisplayName = tableName,
                        Name = tableName,
                    });
                }
            }

            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.FinishedMethodLoggerMessage, nameof(ListTablesAsync), "_", "_"));

            return result;
        }
    }
}
