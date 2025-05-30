// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic
{
    using System.Collections.Generic;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants;
    using SnowflakeV2CoreLogic.Utilities;

    /// <summary>
    /// Contains the string constants used in the connector.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Delimiter for the OData $select query
        /// </summary>
        public const char SelectQueryFieldListDelimiter = ',';

        /// <summary>
        /// Template to store logger message in initiating the provider methods
        /// </summary>
        public const string InitiateMethodLoggerMessage = "[Snowflake.{0}]: Started with dataset {1} and table {2}.";

        /// <summary>
        /// Template to store logger message after finishing the provider methods
        /// </summary>
        public const string FinishedMethodLoggerMessage = "[Snowflake.{0}]: Completed for dataset {1} and table {2}.";

        /// <summary>
        /// The force client success logger message
        /// </summary>
        public const string ClientSuccessMessage = "Successfully recieved response from Snowflake Client";

        public const string GenericLoggerMessage = "[Snowflake.{0}]: {1}.";

        /// <summary>
        /// The  client get object async operation name
        /// </summary>
        public const string GetObjectAsync = "SnowflakeGetObjectAsync";
        public const string GetTablesForSchemaAsync = "GetTablesForSchemaAsync";
        public const string UpdateItemAsync = "SnowflakeUpdateItemAsync";
        public const string DeleteItemAsync = "SnowflakeDeleteItemAsync";
        public const string ListAllItemsAsync = "ListAllItemsAsync";

        /// <summary>
        /// SNOWFLAKE_HTTP_HEADER_TOKEN_TOKEN given to the "Id" column in Snowflake
        /// </summary>
        public const string IdColumnName = "Id";

        /// <summary>
        /// SNOWFLAKE_HTTP_HEADER_TOKEN_TOKEN given to datasets
        /// </summary>
        public const string DataSets = "datasets";

        /// <summary>
        /// Default dataSet for Snowflake
        /// </summary>
        public const string DefaultDataSetName = "default";

        /// <summary>
        /// Default number of records to skip
        /// </summary>
        public const long Skip = 0;

        /// <summary>
        /// Default number of rows to return if no value is provided
        /// </summary>
        public const int DefaultNumberOfRowsToReturn = 51;
        public const string SFDataTypeText = "TEXT";
        public const string SFDataTypeNumber = "NUMBER";
        public const string SFDataTypeDecimal = "DECIMAL";
        public const string SFDataTypeBoolean = "BOOLEAN";
        public const string SFDataTypeFloat = "FLOAT";
        public const string SFDataTypeTime = "TIME";
        public const string SFDataTypeArray = "ARRAY";
        public const string SFDataTypeTimestampNoTimeZone = "TIMESTAMP_NTZ";
        public const string SFDataTypeTimestampLocalTimeZone = "TIMESTAMP_LTZ";
        public const string SFDataTypeTimestampWithTimezone = "TIMESTAMP_TZ";
        public const string SFDataTypeVariant = "VARIANT";
        public const string SFDataTypeGeography = "GEOGRAPHY";
        public const string SFDataTypeObject = "OBJECT";
        public const string SFDataTypeDate = "DATE";
        public const string SFDataTypeFixed = "FIXED";

        // HTTP Status Codes
        public const string Http400ResponseMessage = "Bad Request. The request payload is invalid or malformed";

        // Snowflake HTTP Headers
        public const string SnowflakeHttpHeaderTokenType = "X-Snowflake-Authorization-Token-Type";
        public const string SnowflakeHttpHeaderAuthorization = "Authorization";
        public const string SnowflakeHttpHeaderAccept = "Accept";
        public const string SnowflakeHttpHeaderAgent = "User-Agent";

        // Connection parameters
        public const string Server = "server";
        public const string Database = "database";
        public const string Schema = "schema";
        public const string Warehouse = "warehouse";
        public const string Role = "role";

        public const string HeaderApimReferrer = "x-ms-apim-referrer";

        /// <summary>
        /// The dictionary to translate the allowed snowflake type into the correct Data Type to be returned to the client as table metadata
        /// </summary>
        public static readonly IList<DataTypeMap> SnowflakeMetadataTypeToODataMapping = new List<DataTypeMap>
        {
            new DataTypeMap(SFDataTypeText, DataType.String, DataFormat.Empty),
            new DataTypeMap(SFDataTypeNumber, DataType.Integer, DataFormat.Int32),
            new DataTypeMap(SFDataTypeDecimal, DataType.Number, DataFormat.Double),
            new DataTypeMap(SFDataTypeBoolean, DataType.Boolean, DataFormat.Empty),
            new DataTypeMap(SFDataTypeFloat, DataType.Number, DataFormat.Float),
            new DataTypeMap(SFDataTypeTime, DataType.String, DataFormat.Time),
            new DataTypeMap(SFDataTypeArray, DataType.Array, DataFormat.Empty),
            new DataTypeMap(SFDataTypeTimestampNoTimeZone, DataType.String, DataFormat.DateTimeNoZone),
            new DataTypeMap(SFDataTypeTimestampLocalTimeZone, DataType.String, DataFormat.DateTime),
            new DataTypeMap(SFDataTypeTimestampWithTimezone, DataType.String, DataFormat.DateTime),
            new DataTypeMap(SFDataTypeVariant, DataType.Object, DataFormat.Empty),
            new DataTypeMap(SFDataTypeGeography, DataType.Object, DataFormat.Empty),
            new DataTypeMap(SFDataTypeObject, DataType.Object, DataFormat.Empty),
            new DataTypeMap(SFDataTypeDate, DataType.String, DataFormat.Date),
        };
    }
}