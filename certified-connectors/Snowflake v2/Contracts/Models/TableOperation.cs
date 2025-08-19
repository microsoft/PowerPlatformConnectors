// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    /// <summary>
    /// Operations Enumeration
    /// </summary>
    public enum TableOperation
    {
        /// <summary>
        /// Post item operation
        /// </summary>
        PostItem,

        /// <summary>
        /// Patch item operation
        /// </summary>
        PatchItem,

        /// <summary>
        /// Get item operation
        /// </summary>
        GetItem,

        /// <summary>
        /// Delete item operation
        /// </summary>
        DeleteItem,

        /// <summary>
        /// The unknown
        /// </summary>
        Unknown,
    }
}
