// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Column metadata (capabilities)
    /// </summary>
    [DataContract]
    public class ColumnCapabilitiesMetadata
    {
        /// <summary>
        /// Filter functions
        /// </summary>
        [DataMember(Name = "filterFunctions", EmitDefaultValue = false)]
        public CapabilityFilterFunction[] FilterFunctions { get; set; }
    }
}
