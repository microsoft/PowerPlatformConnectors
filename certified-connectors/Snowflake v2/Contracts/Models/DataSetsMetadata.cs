// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Dataset metadata
    /// </summary>
    [DataContract]
    public class DataSetsMetadata
    {
        /// <summary>
        /// Tabular dataset metadata
        /// </summary>
        [DataMember(Name = "tabular")]
        public TabularDataSetsMetadata TabularDataSetsMetadata { get; set; }
    }
}
