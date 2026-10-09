// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.OData.Extensions;
    using System.Web.OData.Query;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Utilities;

    /// <summary>
    /// Implements operations performed on a Snowflake tables.
    /// </summary>
    public class SnowflakeTableDataProvider : ITableDataProvider<Item>
    {
        private readonly SnowflakeDBOperations snowflakeDBOperations;
        private readonly SnowflakeConnectionParametersProvider snowflakeConnectionParametersProvider;
        private readonly ILogger logger;

        public SnowflakeTableDataProvider(
            SnowflakeDBOperations sfDBOperationsClient,
            SnowflakeConnectionParametersProvider snowflakeConnectionParametersProvider,
            ILogger logger)
        {
            snowflakeDBOperations = sfDBOperationsClient ?? throw new ArgumentNullException(nameof(sfDBOperationsClient));
            this.snowflakeConnectionParametersProvider = snowflakeConnectionParametersProvider ?? throw new ArgumentNullException(nameof(snowflakeConnectionParametersProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Item>> ListItemsAsync(
            HttpRequestMessage request,
            string dataSet,
            string table,
            ODataQueryOptions<Item> options)
        {
            string methodName = nameof(ListItemsAsync);
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, methodName, "_", "_"));

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(dataSet))
            {
                throw new ArgumentNullException(nameof(dataSet));
            }

            if (string.IsNullOrEmpty(table))
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);
            SnowflakeConnectionParametersProvider.EnsureTableWithinConnection(table, connectionParameters);

            NameValueCollection queryParams = HttpUtility.ParseQueryString(request.RequestUri.Query);
            string? skipToken = queryParams["$skiptoken"];

            bool isPartitionFollowUp = SnowflakeToODataHelper.TryParsePartitionSkipToken(
                skipToken, out string? sfStatementHandle, out int partitionIndex, out int totalPartitions);

            if (isPartitionFollowUp)
            {
                logger.LogInformation($"Fetching partition {partitionIndex} for statement handle");

                var partitionResponse = await snowflakeDBOperations.FetchPartitionAsync(sfStatementHandle!, partitionIndex, connectionParameters).ConfigureAwait(true);
                logger.LogDebug(Constants.ClientSuccessMessage);

                if (partitionResponse == null)
                {
                    return new List<Item>();
                }

                int rowsReturnedInPartition = partitionResponse.Data?.Count ?? 0;

                Uri? nextUrl = SnowflakeToODataHelper.GeneratePartitionNextLink(
                    snowflakeConnectionParametersProvider.GetReferralUrl(),
                    options,
                    sfStatementHandle!,
                    partitionIndex + 1,
                    totalPartitions,
                    rowsReturnedInPartition);
                request.ODataProperties().NextLink = nextUrl;

                return partitionResponse.ToListOfItems();
            }

            bool countRequested = options.Count?.RawValue?.Equals("true", StringComparison.InvariantCultureIgnoreCase) == true;

            var dataTask = snowflakeDBOperations.ListAllItemsAsync(table, "GET datasets/{dataset}/tables/{table}/items", options, connectionParameters);
            Task<SnowflakeTableData>? countTask = countRequested
                ? snowflakeDBOperations.GetNumberOfRecordsAvailableInTableAsync(table, options, connectionParameters, "GET datasets/{dataset}/tables/{table}/items")
                : null;

            var queryResponse = await dataTask.ConfigureAwait(true);

            logger.LogDebug(Constants.ClientSuccessMessage);

            if (queryResponse == null)
            {
                return new List<Item>();
            }

            int partitionCount = queryResponse.ResultSetMetaData?.PartitionInfo?.Count ?? 1;
            string? statementHandle = queryResponse.StatementHandle;

            if (partitionCount > 1 && statementHandle != null && statementHandle.Length > 0)
            {
                int rowsReturnedInPartition = queryResponse.Data?.Count ?? 0;

                Uri? nextUrl = SnowflakeToODataHelper.GeneratePartitionNextLink(
                    snowflakeConnectionParametersProvider.GetReferralUrl(),
                    options,
                    statementHandle,
                    1,
                    partitionCount,
                    rowsReturnedInPartition);
                  request.ODataProperties().NextLink = nextUrl;
            }

            if (countRequested && countTask != null)
            {
                var numberOfRecordsResponse = await countTask.ConfigureAwait(true);
                var numberOfRecordsAvailable = int.Parse(numberOfRecordsResponse.Data?[0][0].ToString() ?? "0");
                request.ODataProperties().TotalCount = numberOfRecordsAvailable;
            }

            return queryResponse.ToListOfItems();
        }

        /// <inheritdoc />
        public async Task<Item> GetItemAsync(
            HttpRequestMessage request,
            string dataSet,
            string table,
            string id)
        {
            string methodName = nameof(GetItemAsync);
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, methodName, "_", "_"));

            request.EnsureNotNull(nameof(request));
            dataSet.EnsureNotWhiteSpace(nameof(dataSet));
            table.EnsureNotWhiteSpace(nameof(table));
            id.EnsureNotEmpty(nameof(id));

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);
            SnowflakeConnectionParametersProvider.EnsureTableWithinConnection(table, connectionParameters);

            // First we need to resolve the primarKey since we were only given an ID
            var itemKey = await ResolveItemKeyAsync(table, id, "GET datasets/{dataset}/tables/{table}/items/{id}", connectionParameters, methodName).ConfigureAwait(true);

            // Now that we have a primary key, we can construct the select query
            SnowflakeTableData? itemsResponse = await snowflakeDBOperations.GetItemFromTableAsync(table, itemKey, "GET datasets/{dataset}/tables/{table}/items/{id}", connectionParameters).ConfigureAwait(true);

            // Convert the response into a list of OData Items
            var items = itemsResponse?.ToListOfItems();

            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.FinishedMethodLoggerMessage, methodName, "_", "_"));

            // there should only be one item returned from a GetItem query
            if (items?.Count == 1)
            {
                return items[0];
            }
            else if (items?.Count > 1)
            {
                // We should have more than 1 item when querying by primaryKey
                throw new Exception($"Multiple items returned when querying by primary key {DescribeKeyColumns(itemKey)}");
            }
            return new Item();
        }

        /// <inheritdoc />
        public async Task<CreatedItem<Item>> CreateItemAsync(
            HttpRequestMessage request,
            string dataSet,
            string table,
            Item item)
        {
            string methodName = nameof(CreateItemAsync);
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, methodName, "_", "_"));

            request.EnsureNotNull(nameof(request));
            dataSet.EnsureNotWhiteSpace(nameof(dataSet));
            table.EnsureNotWhiteSpace(nameof(table));
            item.EnsureNotNull(nameof(item));

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);
            SnowflakeConnectionParametersProvider.EnsureTableWithinConnection(table, connectionParameters);

            // Construct the body of the insert request
            var data = await snowflakeDBOperations.InsertRecordAsync(table, item, "POST datasets/{dataset}/tables/{table}/items", connectionParameters).ConfigureAwait(true);

            // At this time it's unclear how to get the ID (or any info) of the created item from snowflake, so we will return the item that was created
            // https://stackoverflow.com/questions/53837950/get-identity-of-row-inserted-in-snowflake-datawarehouse/53903693#53903693
            var createdItem = new CreatedItem<Item>
            {
                Id = "-1",
                Item = new Item(),
            };
            return createdItem;
        }

        public async Task<Item> PatchItemAsync(
            HttpRequestMessage request,
            string dataSet,
            string table,
            string id,
            Item item)
        {
            string methodName = nameof(PatchItemAsync);
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, methodName, "_", "_"));

            request.EnsureNotNull(nameof(request));
            dataSet.EnsureNotWhiteSpace(nameof(dataSet));
            table.EnsureNotWhiteSpace(nameof(table));
            id.EnsureNotEmpty(nameof(id));
            item.EnsureNotNull(nameof(item));

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);
            SnowflakeConnectionParametersProvider.EnsureTableWithinConnection(table, connectionParameters);

            // First we need to resolve the primarKey since we were only given an ID
            var itemKey = await ResolveItemKeyAsync(table, id, "PATCH datasets/{dataset}/tables/{table}/items/{id}", connectionParameters, methodName).ConfigureAwait(true);

            // Now that we have a primary key, we can construct the update query
            SnowflakeTableData updatedItemResponse = await snowflakeDBOperations.UpdateItemAsync(table, itemKey, item, connectionParameters, "PATCH datasets/{dataset}/tables/{table}/items/{id}").ConfigureAwait(true);

            // Convert the response into a list of OData Items
            var items = updatedItemResponse.ToListOfItems();

            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.FinishedMethodLoggerMessage, methodName, "_", "_"));

            return items.FirstOrDefault() ?? new Item();
        }

        public async Task DeleteItemAsync(
            HttpRequestMessage request,
            string dataSet,
            string table,
            string id)
        {
            string methodName = nameof(DeleteItemAsync);
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, methodName, "_", "_"));

            request.EnsureNotNull(nameof(request));
            dataSet.EnsureNotWhiteSpace(nameof(dataSet));
            table.EnsureNotWhiteSpace(nameof(table));
            id.EnsureNotEmpty(nameof(id));

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);
            SnowflakeConnectionParametersProvider.EnsureTableWithinConnection(table, connectionParameters);

            // First we need to resolve the primarKey since we were only given an ID
            var itemKey = await ResolveItemKeyAsync(table, id, "DELETE datasets/{dataset}/tables/{table}/items/{id}", connectionParameters, methodName).ConfigureAwait(true);

            // Now that we have a primary key, we can construct the delete query
            await snowflakeDBOperations.DeleteItemAsync(table, itemKey, connectionParameters, "DELETE datasets/{dataset}/tables/{table}/items/{id}").ConfigureAwait(true);
        }

        private static string DescribeKeyColumns(IReadOnlyList<(string Column, string Value)> itemKey)
        {
            return string.Join(", ", itemKey.Select(k => k.Column));
        }

        /// <summary>
        /// Resolves the primary key columns of a table and pairs them with the values from the item id.
        /// </summary>
        /// <param name="table">The table name.</param>
        /// <param name="id">The item id.</param>
        /// <param name="endpoint">The endpoint name used for logging.</param>
        /// <param name="connectionParameters">The connection parameters.</param>
        /// <param name="methodName">The calling method name used in error messages.</param>
        /// <returns>The primary key columns, ordered by their position in the key, paired with their values.</returns>
        private async Task<IReadOnlyList<(string Column, string Value)>> ResolveItemKeyAsync(
            string table,
            string id,
            string endpoint,
            SnowflakeConnectionParameters connectionParameters,
            string methodName)
        {
            SnowflakeTableData? primaryKeyData = await snowflakeDBOperations.GetPrimaryKeyAsync(table, endpoint, connectionParameters).ConfigureAwait(true);

            IReadOnlyList<string> keyColumns = PrimaryKeyHelper.GetPrimaryKeyColumns(primaryKeyData);
            if (keyColumns.Count == 0)
            {
                // Unable to get the primary key
                string errorMessage = $"Unable to determine primary key from table";
                throw new Exception(string.Format(CultureInfo.InvariantCulture, Constants.GenericLoggerMessage, methodName, errorMessage));
            }

            return PrimaryKeyHelper.ParseItemId(id, keyColumns, table);
        }
    }
}