// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants
{
    /// <summary>
    /// Supported data types for tabular data
    /// </summary>
    public static class DataType
    {
        /// <summary>
        /// The type of the column is a string.
        /// </summary>
        public const string String = "string";

        /// <summary>
        /// The type of the column is a integer or long.
        /// </summary>
        public const string Integer = "integer";

        /// <summary>
        /// The type of the column is a number.
        /// </summary>
        public const string Number = "number";

        /// <summary>
        /// The type of the column is a boolean.
        /// </summary>
        public const string Boolean = "boolean";

        /// <summary>
        /// The type of the column is an object.
        /// </summary>
        public const string Object = "object";

        /// <summary>
        /// The type of the column is an array.
        /// </summary>
        public const string Array = "array";

        /// <summary>
        /// The type of the column is an array.
        /// </summary>
        public const string DateTime = "date-time";
    }
}
