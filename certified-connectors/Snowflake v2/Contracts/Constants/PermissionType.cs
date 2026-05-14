// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants
{
    /// <summary>
    /// Defines the supported permission types
    /// </summary>
    public static class PermissionType
    {
        /// <summary>
        /// No operation allowed
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Read and Write
        /// </summary>
        public const string ReadWrite = "read-write";

        /// <summary>
        /// Read only
        /// </summary>
        public const string ReadOnly = "read-only";
    }
}
