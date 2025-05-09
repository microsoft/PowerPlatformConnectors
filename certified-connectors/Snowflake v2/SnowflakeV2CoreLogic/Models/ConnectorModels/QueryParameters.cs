// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Models.ConnectorModels
{
    public class QueryParameters
    {
        public bool? AsyncExecution { get; set; }

        public bool? Nullable { get; set; }

        public string? RequestId { get; set; }

        public int? Partition { get; set; }
    }
}