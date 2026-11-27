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
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.OData.Extensions;
    using System.Web.OData.Query;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.OData.Core;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Exceptions;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Utilities;

    /// <summary>
    /// Implements operations performed on a Snowflake tables.
    /// </summary>
    public class SnowflakeTableDataProvider : ITableDataProvider<Item>
    {
        private const string InvalidSkipTokenMessage = "The $skiptoken does not refer to a partition of the statement.";

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
                skipToken, out string? sfStatementHandle, out int partitionIndex);

            if (isPartitionFollowUp)
            {
                EnsureValidQueryOptions(options);
                if (partitionIndex < 1)
                {
                    throw CreateBadRequestException(InvalidSkipTokenMessage);
                }

                logger.LogInformation($"Fetching partition {partitionIndex} for statement handle");

                var partitionResponse = await snowflakeDBOperations.FetchPartitionAsync(sfStatementHandle!, partitionIndex, connectionParameters).ConfigureAwait(true);
                logger.LogDebug(Constants.ClientSuccessMessage);

                if (partitionResponse == null)
                {
                    return new List<Item>();
                }

                return ToItemsWithNextLink(request, options, partitionResponse, sfStatementHandle, partitionIndex);
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

            if (countRequested && countTask != null)
            {
                var numberOfRecordsResponse = await countTask.ConfigureAwait(true);
                var numberOfRecordsAvailable = int.Parse(numberOfRecordsResponse.Data?[0][0].ToString() ?? "0");
                request.ODataProperties().TotalCount = numberOfRecordsAvailable;
            }

            return ToItemsWithNextLink(request, options, queryResponse, queryResponse.StatementHandle, 0);
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

        /// <summary>
        /// Converts one partition of the statement that reads a page into items, and links to what follows.
        /// While the page continues in the statement's next partition, the link reads that partition, or the rows
        /// after this one when the statement has no handle. Once the page is complete, the link reads the next page
        /// if the statement found more rows than fit in the page.
        /// </summary>
        /// <param name="request">The request, which receives the next link.</param>
        /// <param name="options">The OData options of the request.</param>
        /// <param name="partition">The rows of the partition, with the metadata of the whole statement.</param>
        /// <param name="statementHandle">The handle of the statement.</param>
        /// <param name="partitionIndex">The index of the partition.</param>
        /// <returns>The items of the partition that belong to the page.</returns>
        private List<Item> ToItemsWithNextLink(
            HttpRequestMessage request,
            ODataQueryOptions options,
            SnowflakeTableData partition,
            string? statementHandle,
            int partitionIndex)
        {
            Uri referrerUri = snowflakeConnectionParametersProvider.GetReferralUrl();
            List<PartitionInfo>? partitionInfo = partition.ResultSetMetaData?.PartitionInfo;
            int totalPartitions = partitionInfo?.Count ?? 1;
            bool isLastPartition = partitionIndex + 1 >= totalPartitions;
            int rowsInPartition = partition.Data?.Count ?? 0;
            long statementRows = partition.ResultSetMetaData?.NumRows ?? 0;
            long rowsBefore = partitionInfo?.Take(partitionIndex).Sum(p => (long)p.RowCount) ?? 0;
            long pageSize = SnowflakeToODataHelper.GetPageSize(options);
            long skip = options.Skip?.Value ?? 0;

            if (!isLastPartition && rowsBefore + rowsInPartition < pageSize)
            {
                if (!string.IsNullOrEmpty(statementHandle))
                {
                    request.ODataProperties().NextLink = SnowflakeToODataHelper.GeneratePartitionNextLink(
                        referrerUri, options, skip, statementHandle!, partitionIndex + 1, totalPartitions);
                }
                else if (rowsBefore + rowsInPartition > 0)
                {
                    // A link that does not move past any row would lead back to this request.
                    request.ODataProperties().NextLink = SnowflakeToODataHelper.GenerateNextPageLink(
                        referrerUri, options, skip + rowsBefore + rowsInPartition);
                }

                return partition.ToListOfItems();
            }

            // The page ends in this partition. Rows past it are the statement's look-ahead row, which only shows that another page follows.
            int rowsInPage = (int)Math.Max(0, Math.Min(rowsInPartition, pageSize - rowsBefore));
            if (partition.Data != null && rowsInPage < partition.Data.Count)
            {
                partition.Data.RemoveRange(rowsInPage, partition.Data.Count - rowsInPage);
            }

            if (pageSize > 0 && statementRows > pageSize)
            {
                request.ODataProperties().NextLink = SnowflakeToODataHelper.GenerateNextPageLink(referrerUri, options, skip + pageSize);
            }

            return partition.ToListOfItems();
        }

        private static void EnsureValidQueryOptions(ODataQueryOptions options)
        {
            var settings = new ODataValidationSettings { AllowedQueryOptions = AllowedQueryOptions.Supported | AllowedQueryOptions.SkipToken };
            try
            {
                options.Validate(settings);
            }
            catch (ODataException ex)
            {
                throw CreateBadRequestException(ex.Message);
            }
        }

        private static HttpResponseException CreateBadRequestException(string message)
        {
            return new HttpResponseException(SnowflakeHttpException.CreateHttpResponseMessage(HttpStatusCode.BadRequest, message));
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