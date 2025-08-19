// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Utilities
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.OData.Query;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;

    public static class SnowflakeToODataHelper
    {
        private const string DateTimeOutputFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
        private const string DateOutputFormat = "yyyy-MM-dd";

        // Supported filter functions
        // When new functions are added, they need to be added to the capabilityFilterFunction array
        // so that they are returned in the metadata for delegation to work correctly
        private static readonly CapabilityFilterFunction[] capabilityFilterFunction =
        {
            CapabilityFilterFunction.Equal,
            CapabilityFilterFunction.NotEqual,
            CapabilityFilterFunction.GreaterThan,
            CapabilityFilterFunction.GreaterThanOrEqual,
            CapabilityFilterFunction.LessThan,
            CapabilityFilterFunction.LessThanOrEqual,
            CapabilityFilterFunction.And,
            CapabilityFilterFunction.Or,
            CapabilityFilterFunction.Contains,
            CapabilityFilterFunction.StartsWith,
            CapabilityFilterFunction.EndsWith,
            CapabilityFilterFunction.Not,
        };

        /// <summary>
        /// Maps the table metadata response from Snowflake to an OData compatible format
        /// </summary>
        /// <returns>Table metadata</returns>
        public static TableMetadata TableMetadataToOdata(
            SnowflakeAPIResponseModel metadataResponse,
            SnowflakeAPIResponseModel primaryKeyResponse,
            string tableName)
        {
            var numRows = metadataResponse?.ResultSetMetaData?.NumRows ?? 0;

            TableMetadata metadata = new TableMetadata()
            {
                Name = tableName,
                Title = tableName,
                Permission = PermissionType.ReadWrite,
                Capabilities = CreateTableCapabilities(numRows),
            };

            // Get the indicies where the following information can be found
            int? dataTypeIndex = metadataResponse?.ResultSetMetaData?.RowType?.FindIndex(x => (x.Name != null && x.Name.Equals("DATA_TYPE", StringComparison.OrdinalIgnoreCase)));
            int? columnNameIndex = metadataResponse?.ResultSetMetaData?.RowType?.FindIndex(x => (x.Name != null && x.Name.Equals("COLUMN_NAME", StringComparison.OrdinalIgnoreCase)));
            int? isNullableIndex = metadataResponse?.ResultSetMetaData?.RowType?.FindIndex(x => (x.Name != null && x.Name.Equals("IS_NULLABLE", StringComparison.OrdinalIgnoreCase)));
            int? precisionToRightOfDecimalIndex = metadataResponse?.ResultSetMetaData?.RowType?.FindIndex(x => (x.Name != null && x.Name.Equals("NUMERIC_SCALE", StringComparison.OrdinalIgnoreCase)));
            string? primaryKey = null;

            // Throw exceptions if dataTypeIndex, columnNameIndex, or isNullableIndex are null
            if (dataTypeIndex == null || columnNameIndex == null || isNullableIndex == null)
            {
                throw new ArgumentNullException("Unable to parse table column data. dataTypeIndex, columnNameIndex, or isNullableIndex is null");
            }

            try
            {
                // Try and identify the primary key based on the response from the API
                int? primaryKeyColumnNameIndex = primaryKeyResponse?.ResultSetMetaData?.RowType?.FindIndex(x => x.Name != null && x.Name.Equals("COLUMN_NAME", StringComparison.OrdinalIgnoreCase));

                if (primaryKeyColumnNameIndex != null)
                {
                    primaryKey = (string?)primaryKeyResponse?.Data?[0][(int)primaryKeyColumnNameIndex];
                }
            }
            catch
            {
                // Do nothing,  if the primary key cannot be identified, it will be null
            }

            var properties = new JObject();
            var requiredProperties = new JArray();

            // Loop through all data elements and create the columns
            if (metadataResponse?.Data != null)
            {
                foreach (var row in metadataResponse.Data)
                {
                    // Convert the snowflake metadata type to an OData compatible type
                    var snowflakeDatatypeName = (string)row[(int)dataTypeIndex];

                    int? precisionToRightOfDecimal = null;

                    // If the datatype is a number, we need to get the precision to the right of the decimal point
                    if (snowflakeDatatypeName.Equals(Constants.SFDataTypeNumber, StringComparison.OrdinalIgnoreCase) && precisionToRightOfDecimalIndex != null)
                    {
                        // Precision will be an int
                        precisionToRightOfDecimal = int.Parse((string)row[(int)precisionToRightOfDecimalIndex]);
                    }

                    // Snowflake overlaods data types. This allows adjustments to be made to ensure the correct data type is returned.
                    snowflakeDatatypeName = AdjustSnowflakeDataType(snowflakeDatatypeName, precisionToRightOfDecimal);

                    // Maps the data type from snowflake to the correct OData data type
                    var datatype = Constants.SnowflakeMetadataTypeToODataMapping.FirstOrDefault(c => string.Equals(c.SnowflakeDataType, snowflakeDatatypeName, StringComparison.OrdinalIgnoreCase));

                    if (datatype != null)
                    {
                        var columnName = (string)row[(int)columnNameIndex];
                        var isNullable = (string)row[(int)isNullableIndex];
                        var isFieldRequired = false;

                        if (isNullable != null && isNullable.Equals("NO", StringComparison.OrdinalIgnoreCase))
                        {
                            requiredProperties.Add(new JValue(columnName));
                            isFieldRequired = true;
                        }

                        // Identify if the current column is a primary key and set the KeyType accordingly
                        var keyType = string.Equals(columnName, primaryKey, StringComparison.OrdinalIgnoreCase) ?
                                               KeyType.Primary : KeyType.None;

                        ColumnCapabilitiesMetadata columnCapabilitiesMetadata = new ColumnCapabilitiesMetadata()
                        {
                            FilterFunctions = capabilityFilterFunction,
                        };

                        JsonSerializer serializer = new JsonSerializer();
                        serializer.TypeNameHandling = TypeNameHandling.None;
                        serializer.Converters.Add(new StringEnumConverter());
                        JToken columnCapabilitiesMetadataJson = JToken.FromObject(columnCapabilitiesMetadata, serializer);

                        // Create an entry for the current column
                        var rowEntry = new JObject()
                        {
                            [SchemaPropertyConstants.Type] = datatype.ConnectorDataType,
                            [SchemaPropertyConstants.Title] = columnName,
                            [SchemaPropertyConstants.KeyOrder] = keyType == KeyType.Primary ? 1 : 0,
                            [SchemaPropertyConstants.Sort] = SortType.AscendingAndDescending,
                            [SchemaPropertyConstants.Permission] = "read-write",
                            [SchemaPropertyConstants.KeyType] = keyType,
                            [SchemaPropertyConstants.Required] = isFieldRequired,
                            [SchemaPropertyConstants.Capabilities] = columnCapabilitiesMetadataJson,
                        };

                        if (!string.IsNullOrEmpty(datatype.ConnectorDataFormat))
                        {
                            rowEntry.Add(SchemaPropertyConstants.Format, datatype.ConnectorDataFormat);
                        }

                        properties[columnName] = rowEntry;
                    }
                }
            }

            // Identify the required properties for this table with "id"
            var rowSchema = new JObject
            {
                { SchemaPropertyConstants.Type, DataType.Object },
                { SchemaPropertyConstants.Required, requiredProperties },
                { SchemaPropertyConstants.Properties, properties },
            };
            metadata.Schema.Add(SchemaPropertyConstants.Type, DataType.Array);
            metadata.Schema.Add(SchemaPropertyConstants.Items, rowSchema);

            return metadata;
        }

        public static TableCapabilitiesMetadata CreateTableCapabilities(int numRows)
        {
            // Get all possible values of CapabilityFilterFunction
            var allCapabilities = Enum.GetValues(typeof(CapabilityFilterFunction)).Cast<CapabilityFilterFunction>();

            // Find capabilities not included in capabilityFilterFunction
            var nonFilterableProperties = allCapabilities.Except(capabilityFilterFunction).Select(c => c.ToString()).ToArray();

            TableFilterRestrictionsMetadata? tableFilterRestrictionsMetadata = new TableFilterRestrictionsMetadata()
            {
                Filterable = true,
                NonFilterableProperties = nonFilterableProperties,
                RequiredProperties = null
            };

            // Identify sort restrictions
            var sortRestrictions = new TableSortRestrictionsMetadata();
            sortRestrictions.Sortable = true;

            // Identify select restrictions
            TableSelectRestrictionsMetadata selectRestrictions = new TableSelectRestrictionsMetadata();
            selectRestrictions.Selectable = true;

            TableCapabilitiesMetadata tableCapabilitiesMetadata = new TableCapabilitiesMetadata()
            {
                FilterRestrictions = tableFilterRestrictionsMetadata,
                SortRestrictions = sortRestrictions,
                SelectRestrictions = selectRestrictions,
                CountRestrictions = new TableCountRestrictionsMetadata()
                {
                    Countable = numRows > 0,
                },
                FilterFunctionSupport = capabilityFilterFunction,
            };

            return tableCapabilitiesMetadata;
        }

        /// <summary>
        /// Converts value into the correct .NET data type based on snowflake datatype mapping
        /// </summary>
        /// <param name="snowflakeDataType">The datatype in the Snowflake database/schema</param>
        /// <param name="precision">The amount of digits to the right of the decimal (if relevant), null otherwise.</param>
        /// <param name="data">The value of the data</param>
        /// <returns>Data converted to appropriate .NET data type</returns>
        public static object? CastSnowflakeDataToCorrectType(
            string snowflakeDataType,
            int? precision,
            object data)
        {
            if (data == null)
            {
                return null;
            }
            snowflakeDataType = snowflakeDataType.ToUpper();

            switch (snowflakeDataType)
            {
                case Constants.SFDataTypeFixed:
                    // If the precision is greater than 0, then it's a floating point number
                    if ((precision ?? 0) > 0)
                    {
                        return Convert.ToDecimal(data);
                    }

                    return Convert.ToInt32(data);
                case Constants.SFDataTypeDate:
                    return ConvertSnowflakeDate(data.ToString());

                case Constants.SFDataTypeTimestampNoTimeZone: // Timestamp without a time zone
                    return ConvertSnowflakeDateTimeNtzToString(data.ToString());

                // LTZ (local time zone) is stored internally in snowflake as unix timestamp (same as NTZ).
                // The local part is a view based modification based on snowflake settings which we don't have access to. We will treat this the same as NTZ.
                case Constants.SFDataTypeTimestampLocalTimeZone:
                    return ConvertSnowflakeDateTimeNtzToString(data.ToString());
                case Constants.SFDataTypeTimestampWithTimezone: // Timestamp with a time zone offset.
                    return ConvertSnowflakeDateTimeWithTzToString(data.ToString());
                default:
                    return data;
            }
        }

        public static Uri? GenerateNextLink(
            Uri referrerUri,
            ODataQueryOptions optionsFromRequest,
            int recordsFetched,
            int totalRecords)
        {
            int offset = 0;

            if (optionsFromRequest.Skip?.Value != null)
            {
                offset = (int)optionsFromRequest.Skip.Value;
            }

            int totalNumberOfRecordsRead = (int)recordsFetched + offset;

            // No more records to read
            if (totalRecords <= totalNumberOfRecordsRead)
            {
                return null;
            }

            // The format of the link should be the baseURL with original filters and adjusted offsets
            string? filter = (optionsFromRequest.Filter == null) ? null : optionsFromRequest.Filter.RawValue;
            string endpointUrl = referrerUri.GetLeftPart(UriPartial.Path);
            string? orderBy = optionsFromRequest.OrderBy != null ? optionsFromRequest.OrderBy.RawValue : null;
            string? select = optionsFromRequest.SelectExpand != null ? optionsFromRequest.SelectExpand.RawSelect : null;
            string? top = optionsFromRequest.Top != null ? optionsFromRequest.Top.RawValue : null;
            string? skip = totalNumberOfRecordsRead.ToString();

            StringBuilder nextUrl = new StringBuilder();

            bool firstOption = true;

            if (endpointUrl != null)
            {
                nextUrl.Append(endpointUrl);
                nextUrl.Append("?");
            }

            if (orderBy != null)
            {
                if (!firstOption)
                {
                    nextUrl.Append("&");
                }

                nextUrl.Append($"$orderby={orderBy}");
                firstOption = false;
            }

            if (select != null)
            {
                if (!firstOption)
                {
                    nextUrl.Append("&");
                }

                nextUrl.Append($"$select={select}");
                firstOption = false;
            }

            if (top != null)
            {
                if (!firstOption)
                {
                    nextUrl.Append("&");
                }

                nextUrl.Append($"$top={top}");
                firstOption = false;
            }

            if (skip != null)
            {
                if (!firstOption)
                {
                    nextUrl.Append("&");
                }

                nextUrl.Append($"$skip={skip}");
                firstOption = false;
            }

            if (filter != null)
            {
                if (!firstOption)
                {
                    nextUrl.Append("&");
                }

                nextUrl.Append($"$filter={filter}");
            }

            return new Uri(nextUrl.ToString());
        }

        /// <summary>
        /// Snowflake overloads different data types with the same name. This function adjusts the data type based on other columns to ensure it maps correctly
        /// with downstream clients
        /// </summary>
        /// <param name="snowflakeDatatypeName">Datatype name as returned by Snowflake</param>
        /// <param name="precisionToRightOfDecimal">Precision right of the decimal point</param>
        /// <returns>Adjust snowflake datatype</returns>
        private static string AdjustSnowflakeDataType(
            string snowflakeDatatypeName,
            int? precisionToRightOfDecimal)
        {
            var updatedDataType = snowflakeDatatypeName;

            // Snowflake uses the same data type for both fixed and floating point numbers. We can use the precision to determine if it's a fixed or floating point number
            if (precisionToRightOfDecimal != null && precisionToRightOfDecimal > 0)
            {
                updatedDataType = Constants.SFDataTypeDecimal;
            }

            return updatedDataType;
        }

        private static string ConvertSnowflakeDate(
            string numberOfDaysSinceEpoch)
        {
            // Snowflake stores the date as the number of days since epoch, so we start with epoch and add the days
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime datetime = epoch.AddDays(int.Parse(numberOfDaysSinceEpoch));
            return datetime.ToString(DateOutputFormat);
        }

        private static string ConvertSnowflakeDateTimeNtzToString(
            string snowflakeDateTime)
        {
            // Snowflake returns data as "unixTime.F9" (meaning utc.9 fractional seconds) which doesn't parse natively with .NET
            // We need to split the two parts up and parse handle them separately before recombining them
            string[] dateTimeParts = snowflakeDateTime.Split('.');

            // Convert the time since epoch to a datetimeoffset
            var parsedDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(dateTimeParts[0]));

            DateTimeOffset parsedDateTimeWithFullPrecision = ParseAndAddFractionalSecondsToDateTime(dateTimeParts[1], parsedDateTime);

            return parsedDateTimeWithFullPrecision.ToString(DateTimeOutputFormat);
        }

        /// <summary>
        /// Formats a Snowflake timestamp with time zone offset to a string in the format "yyyy-MM-dd HH:mm:ss.fffffff"
        /// </summary>
        /// <param name="snowflakeDateTime">Snowflake timestamp</param>
        /// <returns>Convert snowflake time</returns>
        private static string ConvertSnowflakeDateTimeWithTzToString(
            string snowflakeDateTime)
        {
            // Snowflake returns data as "unixTime.F9 +0000" which doesn't parse natively with .NET
            // We need to split the components two parts up and parse them separately before recombining them
            // Note: the offset is not used because snowflake stores the timestamp in UTC which is what we want to return
            string[] dateTimeParts = snowflakeDateTime.Split('.', ' ');

            // Convert the time since epoch to a datetimeoffset
            var parsedDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(dateTimeParts[0]));

            // Include the fractional seconds
            parsedDateTime = ParseAndAddFractionalSecondsToDateTime(dateTimeParts[1], parsedDateTime);

            return parsedDateTime.ToString(DateTimeOutputFormat);
        }

        /// <summary>
        /// Given a string of fractional seconds, parse it and add it to the given datetime
        /// </summary>
        /// <param name="fractionalSeconds">Fractional seconds to add</param>
        /// <param name="dateTime">DateTimeOffset to add the fractional seconds to</param>
        /// <returns>Parse datetime</returns>
        private static DateTimeOffset ParseAndAddFractionalSecondsToDateTime(
            string fractionalSeconds,
            DateTimeOffset dateTime)
        {
            // Add the nanoseconds to the parsed datetime
            var nanoseconds = long.Parse(fractionalSeconds);

            // Convert nanoseconds to Ticks (1 Tick == 100 nanoseconds)
            var ticks = nanoseconds / 100;
            var parsedDateTimeWithFractionalSeconds = dateTime.AddTicks(ticks);
            return parsedDateTimeWithFractionalSeconds;
        }
    }
}
