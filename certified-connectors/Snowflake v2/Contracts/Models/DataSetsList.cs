// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// List of datasets
    /// </summary>
    /// <remarks>
    /// All the items in the list comes under 'value' node in case of OData
    /// </remarks>
    public class DataSetsList
    {
        /// <summary>
        /// List of datasets
        /// </summary>
        public List<DataSet> value { get; set; }
    }
}
