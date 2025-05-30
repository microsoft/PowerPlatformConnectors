// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants
{
    /// <summary>
    /// Defines the sort type
    /// </summary>
    public static class SortType
    {
        /// <summary>
        /// No sorting
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Ascending order
        /// </summary>
        public const string Ascending = "asc";

        /// <summary>
        /// Descending order
        /// </summary>
        public const string Descending = "desc";

        /// <summary>
        /// Both Ascending and descending
        /// </summary>
        public const string AscendingAndDescending = "asc,desc";
    }
}
