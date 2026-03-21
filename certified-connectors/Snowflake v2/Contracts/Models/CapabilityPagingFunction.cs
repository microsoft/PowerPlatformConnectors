// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Server-driven paging functions that could be made available via OData
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CapabilityPagingFunction
    {
        /// <summary>
        /// $top paging option
        /// </summary>
        [EnumMember(Value = "top")]
        Top,

        /// <summary>
        /// $skiptoken paging option
        /// </summary>
        [EnumMember(Value = "skiptoken")]
        SkipToken,
    }
}
