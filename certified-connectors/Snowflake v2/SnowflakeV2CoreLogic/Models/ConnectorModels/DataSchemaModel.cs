// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Models.ConnectorModels
{
    using System.Collections.Generic;

    public class DataSchemaModel
    {
        public List<DataSchema?>? DataSchema { get; set; }
    }

    public class DataSchema
    {
        public string? Name { get; set; }

        public string? Database { get; set; }

        public string? Schema { get; set; }

        public string? Table { get; set; }

        public bool? Nullable { get; set; }

        public int? Precision { get; set; }

        public int? Scale { get; set; }

        public int? ByteLength { get; set; }

        public string? Collation { get; set; }

        public int? Length { get; set; }

        public string? Type { get; set; }
    }
}