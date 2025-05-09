// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
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

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);

            SnowflakeTableData? queryResponse = null;

            queryResponse = await snowflakeDBOperations.ListAllItemsAsync(table, options, connectionParameters).ConfigureAwait(true);
            var numberOfRecordsResponse = await snowflakeDBOperations.GetNumberOfRecordsInTableAsync(table, connectionParameters).ConfigureAwait(true);

            logger.LogDebug(Constants.ClientSuccessMessage);

            if (queryResponse != null)
            {
                var response = queryResponse.ToListOfItems();
                var numberOfRecordsAvailable = int.Parse(numberOfRecordsResponse.Data?[0][0].ToString() ?? "0");
                int numberOfRowsReturned = queryResponse.ResultSetMetaData?.NumRows ?? 0;

                Uri? nextUrl = SnowflakeToODataHelper.GenerateNextLink(snowflakeConnectionParametersProvider.GetReferralUrl(), options, numberOfRowsReturned, numberOfRecordsAvailable);

                if (nextUrl != null)
                {
                    request.ODataProperties().NextLink = nextUrl;
                }

                return response;
            }

            // Return an empty list
            return new List<Item>();
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

            // First we need to resolve the primarKey since we were only given an ID
            SnowflakeTableData? primaryKeyData = null;
            SnowflakeTableData? itemsResponse = null;

            primaryKeyData = await snowflakeDBOperations.GetPrimaryKeyAsync(table, connectionParameters).ConfigureAwait(true);

            string? primaryKeyColumn = null;
            try
            {
                // Grab the first element and look for the column_name property, which will have a value that aligns to the primary key column name.
                primaryKeyColumn = primaryKeyData?.ToGenericDictionaryList()[0]["COLUMN_NAME"].ToString();
            }
            catch (Exception)
            {
                // Unable to get the primary key
                string errorMessage = $"Unable to determine primary key from table";
                throw new Exception(string.Format(CultureInfo.InvariantCulture, Constants.GenericLoggerMessage, methodName, errorMessage));
            }

            // Now that we have a primary key, we can construct the select query
            itemsResponse = await snowflakeDBOperations.GetItemFromTableAsync(table, primaryKeyColumn, id, connectionParameters).ConfigureAwait(true);

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
                throw new Exception($"Multiple items returned when querying by primary key {primaryKeyColumn}");
            }
            else
            {
                // TODO: What should we return for an emtpy set?
                return new Item();
            }
        }

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

            // Construct the body of the insert request
            var data = await snowflakeDBOperations.InsertRecordAsync(table, item, connectionParameters).ConfigureAwait(true);

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

            SnowflakeConnectionParameters connectionParameters = snowflakeConnectionParametersProvider.GetConnectionParameters();
            connectionParameters = SnowflakeConnectionParametersProvider.UpdateConnParametersToUseDataset(request, dataSet, connectionParameters);

            // First we need to resolve the primarKey since we were only given an ID
            SnowflakeTableData? primaryKeyData = await snowflakeDBOperations.GetPrimaryKeyAsync(table, connectionParameters).ConfigureAwait(true);

            string? primaryKeyColumn = null;
            try
            {
                // Grab the first element and look for the column_name property, which will have a value that aligns to the primary key column name.
                primaryKeyColumn = primaryKeyData?.ToGenericDictionaryList()[0]["COLUMN_NAME"].ToString();
            }
            catch (Exception)
            {
                // Unable to get the primary key
                throw new Exception($"Unable to determine primary key from {table}");
            }

            // Now that we have a primary key, we can construct the select query
            SnowflakeTableData updatedItemResponse = await snowflakeDBOperations.UpdateItemAsync(table, primaryKeyColumn, id, item, connectionParameters).ConfigureAwait(true);

            // Convert the response into a list of OData Items
            var items = updatedItemResponse.ToListOfItems();

            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.FinishedMethodLoggerMessage, methodName, "_", "_"));

            // there should only be one item returned from a GetItem query
            if (items.Count == 1)
            {
                return items[0];
            }
            else if (items.Count > 1)
            {
                // We should have more than 1 item when querying by primaryKey
                throw new Exception($"Multiple items returned when updating by primary key {primaryKeyColumn}");
            }
            else
            {
                // TODO: What should we return for an emtpy set?
                return new Item();
            }
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

            // First we need to resolve the primarKey since we were only given an ID
            SnowflakeTableData? primaryKeyData = await snowflakeDBOperations.GetPrimaryKeyAsync(table, connectionParameters).ConfigureAwait(true);

            string? primaryKeyColumn;
            try
            {
                // Grab the first element and look for the column_name property, which will have a value that aligns to the primary key column name.
                primaryKeyColumn = primaryKeyData?.ToGenericDictionaryList()[0]["COLUMN_NAME"].ToString();
            }
            catch (Exception)
            {
                // Unable to get the primary key
                string errorMessage = $"Unable to determine primary key from {table}";

                throw new Exception(string.Format(CultureInfo.InvariantCulture, Constants.GenericLoggerMessage, methodName, errorMessage));
            }

            // Now that we have a primary key, we can construct the select query
            SnowflakeTableData deletedItemResponse = await snowflakeDBOperations.DeleteItemAsync(table, primaryKeyColumn, id, connectionParameters).ConfigureAwait(true);

            // Convert the response into a list of OData Items
            var items = deletedItemResponse.ToListOfItems();

            // there should only be one item returned from a GetItem query
            if (items.Count > 1)
            {
                // We should have more than 1 item when querying by primaryKey
                string errorMessage = $"Multiple items returned when deleting by primary key {primaryKeyColumn}";

                throw new Exception(string.Format(CultureInfo.InvariantCulture, Constants.GenericLoggerMessage, methodName, errorMessage));
            }
        }
    }
}