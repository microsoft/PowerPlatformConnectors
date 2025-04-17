// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    /// <summary>
    /// Dataset metadata source
    /// </summary>
    public static class DataSetsMetadataSource
    {
        /// <summary>
        /// Most recently used
        /// </summary>
        public const string Mru = "mru";

        /// <summary>
        /// List data source
        /// </summary>
        public const string List = "list";

        /// <summary>
        /// Singleton data source
        /// </summary>
        public const string Singleton = "singleton";

        /// <summary>
        /// File data source
        /// </summary>
        public const string File = "file";
    }
}
