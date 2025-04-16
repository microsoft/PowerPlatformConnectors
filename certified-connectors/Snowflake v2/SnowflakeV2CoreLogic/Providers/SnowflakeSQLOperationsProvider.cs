// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Numerics;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Models.ConnectorModels;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Utilities;

    /// <summary>
    /// Implements operations performed on a Snowflake tables.
    /// </summary>
    public class SnowflakeSQLOperationsProvider : ISnowflakeSQLOperationsProvider
    {
        private readonly ISnowflakeClient snowflakeClient;
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public SnowflakeSQLOperationsProvider(
            ISnowflakeClient snowflakeClient,
            HttpClient httpClient,
            ILogger logger)
        {
            this.snowflakeClient = snowflakeClient ?? throw new ArgumentNullException(nameof(snowflakeClient));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SQLOperationsResponseModel> ExecuteSQLStatementAsync(
            HttpRequestMessage request,
            ExecuteSqlStatementModel payload,
            HeaderParameters headerParameters,
            QueryParameters queryParams)
        {
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, nameof(ExecuteSQLStatementAsync), "_", "_"));

            // Execute the SQL statement
            // We would normally execute this through the db operations, but that's intended to build usable sql statements from data in the request.
            // In this case, we have the entire request paylaod provided by the user, so we can bypass it and go straight to the client.
            SnowflakeAPIResponseModel apiResponse = await snowflakeClient.ExecuteSqlStatementAsync(httpClient, payload, headerParameters, queryParams).ConfigureAwait(true);

            // Need to map the response to the SQLOperationsResponseModel
            var response = MapAPIResponseToOperationResponse(apiResponse);

            return response;
        }

        public async Task<SQLOperationsResponseModel> GetResultsAsync(
            HttpRequestMessage request,
            DataSchemaModel? schema,
            string statementHandle,
            HeaderParameters headerParameters,
            QueryParameters queryParams)
        {
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, nameof(GetResultsAsync), "_", "_"));
            var response = await snowflakeClient.GetResultsAsync(httpClient, statementHandle, headerParameters, queryParams).ConfigureAwait(true);

            return MapAPIResponseToOperationResponse(response, schema);
        }

        public async Task<SnowflakeAPIResponseModel> CancelRequestAsync(
            HttpRequestMessage request,
            string statementHandle,
            HeaderParameters headerParameters,
            QueryParameters queryParams)
        {
            logger.LogInformation(string.Format(CultureInfo.InvariantCulture, Constants.InitiateMethodLoggerMessage, nameof(CancelRequestAsync), "_", "_"));
            var response = await snowflakeClient.CancelRequestAsync(httpClient, statementHandle, headerParameters, queryParams).ConfigureAwait(true);

            return response;
        }

        private SQLOperationsResponseModel MapAPIResponseToOperationResponse(
            SnowflakeAPIResponseModel apiResponse,
            DataSchemaModel? originalSchema = null)
        {
            SQLOperationsResponseModel mdl = new SQLOperationsResponseModel();

            // Map the partitions
            if (apiResponse?.ResultSetMetaData?.PartitionInfo != null)
            {
                mdl.Partitions = new List<Partition>();
                foreach (var partition in apiResponse.ResultSetMetaData.PartitionInfo)
                {
                    mdl.Partitions.Add(new Partition()
                    {
                        RowCount = partition.RowCount,
                        UncompressedSize = partition.UncompressedSize,

                        // TODO: swagger response also contains compressed size. is this missing from API response model?
                    });
                }
            }

            // Map the metadata
            mdl.Metadata = new Metadata();
            mdl.Metadata.SqlState = apiResponse?.SqlState;
            mdl.Metadata.StatementStatusUrl = apiResponse?.StatementStatusUrl;
            mdl.Metadata.Code = apiResponse?.Code;
            mdl.Metadata.Format = apiResponse?.ResultSetMetaData?.Format;
            mdl.Metadata.Rows = apiResponse?.ResultSetMetaData?.NumRows;
            mdl.Metadata.StatementHandles = apiResponse?.StatementHandles;
            mdl.Metadata.StatementStatusUrl = apiResponse?.StatementStatusUrl;
            mdl.Metadata.StatementHandle = apiResponse?.StatementHandle;
            mdl.Metadata.RequestId = apiResponse?.RequestId;
            mdl.Metadata.CreatedOn = ConvertDateTimeFromEpochToString(apiResponse?.CreatedOn);

            // Map the schema
            // If the api response contains a schema, then use that. Otherwise, use the schema provided in the request.
            if (apiResponse?.Data != null)
            {
                if (apiResponse?.ResultSetMetaData?.RowType != null)
                {
                    mdl.Schema = new List<DataSchema>();
                    foreach (var rowType in apiResponse.ResultSetMetaData.RowType)
                    {
                        mdl.Schema.Add(new DataSchema()
                        {
                            Name = rowType.Name,
                            Database = rowType.Database,
                            Schema = rowType.Schema,
                            Table = rowType.Table,
                            Nullable = rowType.Nullable,
                            ByteLength = rowType.ByteLength,
                            Precision = rowType.Precision,
                            Scale = rowType.Scale,
                            Type = rowType.Type,
                        });
                    }
                }
                else if (originalSchema != null)
                {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    mdl.Schema = originalSchema.DataSchema;
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
                }
                else
                {
                    // Throw an exception if the schema is not provided in the request or the API response
                    throw new Exception("Schema is not provided in the request or the API response");
                }

                // Merge the schema and data into a singular array of JObjects
                mdl.Data = ConvertToArrayOfRows(apiResponse?.Data, mdl.Schema);
            }

            return mdl;
        }

        private List<JObject>? ConvertToArrayOfRows(
            List<List<object>>? data,
            List<DataSchema>? schema)
        {
            List<JObject> items = new List<JObject>();

            // Loop through the metadata and create a list of indexed headers
            var columnData = schema?.ToArray();

            try
            {
                if (data != null)
                {
                    // Loop through each row in the data block
                    foreach (var row in data)
                    {
                        // Create a new item
                        JObject item = new JObject();

                        // Loop through the columns and add the data to the item
                        if (columnData != null)
                        {
                            for (int i = 0; i < columnData.Length; i++)
                            {
                                var dataType = columnData[i].Type;
                                var scale = columnData[i].Scale;
                                var precision = columnData[i].Precision;

                                // TODO convert the value to the correct format here
                                var value = FormatData(row[i], dataType, scale, precision);

                                // Cast the datatype to the correct type
#pragma warning disable CS8604 // Possible null reference argument.
                                item.Add(columnData[i].Name, value);
#pragma warning restore CS8604 // Possible null reference argument.
                            }
                        }

                        items.Add(item);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error converting data to JObject, exception = {ex.Message}");
            }
        }

        private JToken? FormatData(
            object value,
            string? dataType,
            long? scale,
            int? precision)
        {
            JToken? result = null;

            // added null string check explicitly as snowflake returns "null" if we send nullable as false
            // https://docs.snowflake.com/en/developer-guide/sql-api/reference
            if (dataType == null || value == null || value.ToString().Equals("null") || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return result;
            }

            try
            {
                switch (dataType.ToUpper())
                {
                    case Constants.SFDataTypeArray:
                        result = JArray.Parse(value.ToString());
                        break;

                    case Constants.SFDataTypeVariant:
                    case Constants.SFDataTypeObject:
                        result = JObject.Parse(value.ToString());
                        break;

                    case Constants.SFDataTypeFixed:

                        try
                        {
                            if (scale == null || scale == 0)
                            {
                                result = JToken.FromObject(BigInteger.Parse(value.ToString()));
                            }
                            else
                            {
                                result = JToken.FromObject(double.Parse(value.ToString()));
                            }
                        }
                        catch (Exception)
                        {
                            result = JToken.FromObject(0);
                        }

                        break;

                    case Constants.SFDataTypeFloat:
                        result = JToken.FromObject(float.Parse(value.ToString()));
                        break;

                    case Constants.SFDataTypeBoolean:
                        result = JToken.FromObject(bool.Parse(value.ToString()));
                        break;

                    default:
                        // convert value to a JToken
                        result = JToken.FromObject(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Invalid value for data type {dataType}, exception = {ex.Message}");
            }

            return result;
        }

        private string? ConvertDateTimeFromEpochToString(
            long? epochTime)
        {
            if (epochTime == null)
            {
                return null;
            }

            var utcTime = DateTimeOffset.FromUnixTimeMilliseconds((long)epochTime);
            return utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }
    }
}