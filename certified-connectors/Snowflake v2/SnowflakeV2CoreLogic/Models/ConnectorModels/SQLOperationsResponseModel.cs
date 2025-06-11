// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Models.ConnectorModels
{
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class SQLOperationsResponseModel
    {
        public List<Partition>? Partitions { get; set; }

        public List<DataSchema>? Schema { get; set; }

        public List<JObject>? Data { get; set; }

        public Metadata? Metadata { get; set; }
    }

    public class Partition
    {
        public int? RowCount { get; set; }

        public int? UncompressedSize { get; set; }

        public int? CompressedSize { get; set; }
    }

    public class Metadata
    {
        public int? Rows { get; set; }

        public string? Format { get; set; }

        public string? Code { get; set; }

        public string? StatementStatusUrl { get; set; }

        public string? RequestId { get; set; }

        public string? SqlState { get; set; }

        public string? StatementHandle { get; set; }

        public List<string>? StatementHandles { get; set; }

        public string? CreatedOn { get; set; }
    }
}