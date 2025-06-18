// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Utilities
{
    /// <summary>
    /// A Mapping between the Snowflake Data Type and the Connector data type and formats to expose to the client.
    /// </summary>
    public class DataTypeMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeMap"/> class.
        /// </summary>
        /// <param name="snowflakeDatatype">The Snowflake Data Type returned.</param>
        /// <param name="connectorDataType">The Connector data type to return.</param>
        /// <param name="connectorDataFormat">The Connector data format to return.</param>
        public DataTypeMap(
            string snowflakeDatatype,
            string connectorDataType,
            string connectorDataFormat)
        {
            SnowflakeDataType = snowflakeDatatype;
            ConnectorDataType = connectorDataType;
            ConnectorDataFormat = connectorDataFormat;
        }

        /// <summary>
        /// The snowflake data type returned for a field
        /// </summary>
        public string SnowflakeDataType { get; set; }

        /// <summary>
        /// The Connector data type to expose the snowflake field to the client.
        /// </summary>
        public string ConnectorDataType { get; set; }

        /// <summary>
        /// The Connector data format to expose the snowflake field to the client.
        /// </summary>
        public string ConnectorDataFormat { get; set; }
    }
}