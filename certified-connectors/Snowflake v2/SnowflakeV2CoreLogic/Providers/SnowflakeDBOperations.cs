// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.OData.Query;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.QueryOptions;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Exceptions;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Utilities;

    public class SnowflakeDBOperations
    {
        private readonly ISnowflakeClient snowflakeClient;
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public SnowflakeDBOperations(
            ISnowflakeClient snowflakeClient,
            HttpClient httpClient,
            ILogger logger)
        {
            this.snowflakeClient = snowflakeClient.EnsureNotNull("Snowflake Client");
            this.httpClient = httpClient.EnsureNotNull("HTTPClient");
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SnowflakeTableData?> GetTableMetadataAsync(
            string tableName)
        {
            SnowflakeTableData? metadataResponse = null;

            using (var latencyLogger = new LatencyLogger(Constants.GetObjectAsync, logger))
            {
                var metadataStatement = $"SELECT * FROM information_schema.columns where TABLE_NAME=?";

                // Add request bindings
                SnowflakeRequestBindings metaDataBindings = new SnowflakeRequestBindings();
                metaDataBindings.AddTextBinding(1, tableName);

                // Fetch the metadata
                metadataResponse = await snowflakeClient.CallAPIAsync(httpClient, metadataStatement, metaDataBindings).ConfigureAwait(true);
            }

            return metadataResponse;
        }

        public async Task<SnowflakeTableData?> GetPrimaryKeyAsync(
            string tableName,
            SnowflakeConnectionParameters? connectionParameters = null)
        {
            SnowflakeTableData? primaryKeyResponse = null;
            using (var latencyLogger = new LatencyLogger(Constants.GetObjectAsync, logger))
            {
                var primaryKeyStatement = $"WITH t AS (select get_ddl(?,?) tbl_ddl), t1 AS (SELECT POSITION('primary key (', tbl_ddl) + 13 pos, SUBSTR(tbl_ddl, pos, POSITION(')', tbl_ddl, pos) - pos ) str FROM t) SELECT x.value column_name, x.index ordinal_position FROM t1, LATERAL SPLIT_TO_TABLE(t1.str, ',') x;";
                var primaryKeyBindings = new SnowflakeRequestBindings();
                primaryKeyBindings.AddTextBinding(1, "TABLE");
                primaryKeyBindings.AddTextBinding(2, tableName);

                primaryKeyResponse = await snowflakeClient.CallAPIAsync(httpClient, primaryKeyStatement, primaryKeyBindings, connectionParameters).ConfigureAwait(true);
            }

            return primaryKeyResponse;
        }

        public async Task<SnowflakeTableData?> GetTablesForSchemaAsync(
            SnowflakeConnectionParameters connectionParameters)
        {
            SnowflakeTableData? snowflakeTableData = null;

            using (var latencyLogger = new LatencyLogger(Constants.GetTablesForSchemaAsync, logger))
            {
                string sqlCommand = $"SELECT table_name FROM information_schema.tables WHERE TABLE_SCHEMA = ?";

                // Add request bindings
                SnowflakeRequestBindings stmtBindings = new SnowflakeRequestBindings();
                stmtBindings.AddTextBinding(1, connectionParameters.Schema);

                // Fetch the metadata
                snowflakeTableData = await snowflakeClient.CallAPIAsync(httpClient, sqlCommand, stmtBindings).ConfigureAwait(true);
            }

            return snowflakeTableData;
        }

        public async Task<SnowflakeTableData?> ListAllItemsAsync(
            string table,
            ODataQueryOptions? options = null,
            SnowflakeConnectionParameters? connectionParameters = null)
        {
            SnowflakeTableData? snowflakeTableData = null;

            var fieldsToSelect = "*";
            string? orderBy = null;
            var top = "51";
            var skip = "0";
            var filter = string.Empty;

            if (options != null)
            {
                QueryOptions? queryOptions;

                try
                {
                    queryOptions = QueryOptions.Parse(options);
                }
                catch (ArgumentException ex)
                {
                    logger.LogError(ex, "Error parsing query options");
                    throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        ex.Message));
                }

                fieldsToSelect = queryOptions.Select != null ? queryOptions.Select : "*";
                orderBy = options.OrderBy != null ? options.OrderBy.RawValue : null;
                top = queryOptions.IsTopSet ? queryOptions.Top.ToString() : Constants.DefaultNumberOfRowsToReturn.ToString();
                skip = queryOptions.Skip.ToString();
                filter = ConvertODataFilterToSql(options);
            }

            using (var latencyLogger = new LatencyLogger(Constants.ListAllItemsAsync, logger))
            {
                // This select statement needs to be transformed using the options
                var stmt = string.Empty;

                if (!string.IsNullOrEmpty(filter))
                {
                    if (orderBy != null)
                    {
                        stmt = string.Format(
                            CultureInfo.InvariantCulture,
                            QueryConstants.SelectItemsQueryWithFilterAndOrderBy,
                            fieldsToSelect,
                            table,
                            filter,
                            orderBy,
                            top,
                            skip);
                    }
                    else
                    {
                        stmt = string.Format(
                            CultureInfo.InvariantCulture,
                            QueryConstants.SelectItemsQueryWithFilter,
                            fieldsToSelect,
                            table,
                            filter,
                            top,
                            skip);
                    }
                }
                else
                {
                    if (orderBy != null)
                    {
                        stmt = string.Format(
                            CultureInfo.InvariantCulture,
                            QueryConstants.SelectItemsQueryWithoutFilter,
                            fieldsToSelect,
                            table,
                            orderBy,
                            top,
                            skip);
                    }
                    else
                    {
                        stmt = string.Format(
                            CultureInfo.InvariantCulture,
                            QueryConstants.SelectItemsQueryWithoutFilterAndOrderBy,
                            fieldsToSelect,
                            table,
                            top,
                            skip);
                    }
                }

                // Add request bindings
                SnowflakeRequestBindings stmtBindings = new SnowflakeRequestBindings();
                stmtBindings.AddTextBinding(1, table);

                // Fetch the metadata
                snowflakeTableData = await snowflakeClient.CallAPIAsync(httpClient, stmt, stmtBindings, connectionParameters).ConfigureAwait(true);
            }

            return snowflakeTableData;
        }

        public string ConvertODataFilterToSql(ODataQueryOptions options)
        {
            if (options?.Filter != null)
            {
                var filterClause = options.Filter.FilterClause;
                var sqlConverter = new ODataToSqlParser();
                return sqlConverter.ParseFilterToSql(filterClause);
            }

            // If no filter is provided, return an empty string
            return string.Empty;
        }

        public async Task<SnowflakeTableData?> GetItemFromTableAsync(
            string tableName,
            string? fieldToQuery,
            string itemId,
            SnowflakeConnectionParameters? connectionParameters = null)
        {
            SnowflakeTableData? data = null;

            // Check the table name and field name adhere to the Snowflake schema
            tableName.EnsureValidSnowflakeIdentifier("Table Name");
            fieldToQuery.EnsureValidSnowflakeIdentifier("Field Name");

            using (var latencyLogger = new LatencyLogger(Constants.GetObjectAsync, logger))
            {
                var query = $"SELECT * FROM {tableName} where {fieldToQuery}=?";

                // Add request bindings
                SnowflakeRequestBindings queryBindings = new SnowflakeRequestBindings();
                queryBindings.AddTextBinding(1, itemId);

                // Fetch the metadata
                data = await snowflakeClient.CallAPIAsync(httpClient, query, queryBindings, connectionParameters).ConfigureAwait(true);
            }

            return data;
        }

        public async Task<SnowflakeTableData?> InsertRecordAsync(
            string table,
            Item dataToInsert,
            SnowflakeConnectionParameters? connectionParameters = null)
        {
            SnowflakeTableData? data = null;

            using (var latencyLogger = new LatencyLogger(Constants.GetObjectAsync, logger))
            {
                // Create an insert statement
                var columns = dataToInsert.DynamicProperties.Keys;
                var values = dataToInsert.DynamicProperties.Values;

                var valuesPlaceholders = new StringBuilder();

                for (int i = 0; i < values.Count; i++)
                {
                    valuesPlaceholders.Append("?");

                    if (i != values.Count - 1)
                    {
                        valuesPlaceholders.Append(",");
                    }
                }

                var columnsString = $"{string.Join(",", columns)} ";
                var query = $"INSERT INTO {table} ({columnsString}) VALUES ({valuesPlaceholders})";

                // Create the request bindings
                SnowflakeRequestBindings queryBindings = new SnowflakeRequestBindings();
                int bindingsCounter = 1;
                foreach (var value in values)
                {
                    queryBindings.AddBinding(bindingsCounter, value);
                    bindingsCounter++;
                }

                // Fetch the metadata
                data = await snowflakeClient.CallAPIAsync(httpClient, query, queryBindings, connectionParameters).ConfigureAwait(true);
            }

            return data;
        }

        internal async Task<SnowflakeTableData> UpdateItemAsync(
            string table,
            string? primaryKeyColumn,
            string id,
            Item item,
            SnowflakeConnectionParameters connectionParameters)
        {
            SnowflakeTableData? data = null;
            var sqlUpdateString = new StringBuilder();
            SnowflakeRequestBindings queryBindings = new SnowflakeRequestBindings();

            using (var latencyLogger = new LatencyLogger(Constants.UpdateItemAsync, logger))
            {
                // Create an insert statement
                var columns = item.DynamicProperties.Keys.ToArray();
                var values = item.DynamicProperties.Values.ToArray();

                if (columns.Length != values.Length)
                {
                    throw new InvalidOperationException("The number of columns and values do not match");
                }

                // Loop through all the columns to create the update string
                var bindingCounter = 1;
                for (int i = 0; i < columns.Length; i++)
                {
                    sqlUpdateString.Append($"{columns[i]} = ?");

                    // Add a comma if we still have more columns to add
                    if (i != columns.Length - 1)
                    {
                        sqlUpdateString.Append(", ");
                    }

                    // Add the value to the query bindings
                    queryBindings.AddBinding(bindingCounter++, values[i]);
                }

                // Add the primary key binding
                queryBindings.AddBinding(bindingCounter, id);

                var query = $"UPDATE {table} SET {sqlUpdateString} WHERE {primaryKeyColumn} = ?";

                // Fetch the metadata
                data = await snowflakeClient.CallAPIAsync(httpClient, query, queryBindings, connectionParameters).ConfigureAwait(true);
            }

            return data;
        }

        internal async Task<SnowflakeTableData> DeleteItemAsync(
            string table,
            string? primaryKeyColumn,
            string id,
            SnowflakeConnectionParameters connectionParameters)
        {
            SnowflakeTableData? data = null;

            using (var latencyLogger = new LatencyLogger(Constants.DeleteItemAsync, logger))
            {
                var query = $"DELETE FROM {table} where {primaryKeyColumn}=?";

                // Add request bindings
                SnowflakeRequestBindings queryBindings = new SnowflakeRequestBindings();
                queryBindings.AddBinding(1, id);

                // Fetch the metadata
                data = await snowflakeClient.CallAPIAsync(httpClient, query, queryBindings, connectionParameters).ConfigureAwait(true);
            }

            return data;
        }

        internal async Task<SnowflakeTableData> GetNumberOfRecordsAvailableInTableAsync(
            string table,
            ODataQueryOptions options,
            SnowflakeConnectionParameters connectionParameters)
        {
            var query = "SELECT COUNT(*) FROM " + table;

            if (options != null)
            {
                QueryOptions? queryOptions;

                try
                {
                    queryOptions = QueryOptions.Parse(options);
                }
                catch (ArgumentException ex)
                {
                    throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        ex.Message));
                }

                // Apply OData `$filter` conditions, ignore `$select` and `$orderby`
                string filterText = string.Empty;
                if (options.Filter != null)
                {
                    filterText = ConvertODataFilterToSql(options);
                    query = $"SELECT COUNT(*) FROM {table} WHERE {filterText}";
                }   
            }

            SnowflakeRequestBindings queryBindings = new SnowflakeRequestBindings();
            var data = await snowflakeClient.CallAPIAsync(httpClient, query, queryBindings, connectionParameters).ConfigureAwait(true);
            return data;
        }

        internal async Task<SnowflakeTableData> GetInformationScehmaAsync(
            SnowflakeConnectionParameters connectionParameters)
        {
            string role = connectionParameters.Role;
            string warehouse = connectionParameters.Warehouse;
            string database = connectionParameters.Database;
            string schema = connectionParameters.Schema;

            if (string.IsNullOrEmpty(role.Trim()) || string.IsNullOrEmpty(warehouse.Trim()) || string.IsNullOrEmpty(schema.Trim()))
            {
                var queryWithoutValidation = $"SELECT * FROM information_schema.columns";
                SnowflakeRequestBindings queryBindings = new SnowflakeRequestBindings();
                var dataWithoutValidation = await snowflakeClient.CallAPIAsync(httpClient, queryWithoutValidation).ConfigureAwait(true);
                return dataWithoutValidation;
            }

            var queryWithSnowflakeConfigValidation = $"USE ROLE \"{role}\";USE WAREHOUSE \"{warehouse}\";USE DATABASE \"{database}\";USE SCHEMA \"{schema}\";SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{schema}';";
            RequestParameters requestParameters = new RequestParameters
            {
                MULTI_STATEMENT_COUNT = 5,
            };
            var dataWithValidation = await snowflakeClient.CallAPIAsync(httpClient, queryWithSnowflakeConfigValidation, null, null, requestParameters, true).ConfigureAwait(true);
            return dataWithValidation;
        }
    }
}