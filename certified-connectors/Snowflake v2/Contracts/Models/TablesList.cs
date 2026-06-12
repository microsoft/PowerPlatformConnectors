// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a list of tables.
    /// </summary>
    /// <remarks>
    /// All the items in the list comes under 'value' node in case of OData
    /// </remarks>
    public class TablesList
    {
        /// <summary>
        /// List of Tables
        /// </summary>
        public List<Table> value { get; set; }
    }
}
