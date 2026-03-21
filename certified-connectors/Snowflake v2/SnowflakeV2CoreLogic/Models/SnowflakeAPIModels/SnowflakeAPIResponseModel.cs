// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Models.SnowflakeAPIModels
{
    using System.Collections.Generic;

    public class SnowflakeAPIResponseModel
    {
        public ResultSetMetaData? ResultSetMetaData { get; set; }

        public List<List<object>>? Data { get; set; }

        public string? Code { get; set; }

        public string? StatementStatusUrl { get; set; }

        public string? RequestId { get; set; }

        public string? SqlState { get; set; }

        public string? StatementHandle { get; set; }

        public string? Message { get; set; }

        public long CreatedOn { get; set; }

        public List<string>? StatementHandles { get; set; }
    }

    public class ResultSetMetaData
    {
        public int NumRows { get; set; }

        public string? Format { get; set; }

        public List<PartitionInfo>? PartitionInfo { get; set; }

        public List<RowType>? RowType { get; set; }
    }

    public class PartitionInfo
    {
        public int RowCount { get; set; }

        public int UncompressedSize { get; set; }
    }

    public class RowType
    {
        public string? Name { get; set; }

        public string? Database { get; set; }

        public string? Schema { get; set; }

        public string? Table { get; set; }

        public bool? Nullable { get; set; }

        public int? ByteLength { get; set; }

        public int? Precision { get; set; }

        public int? Scale { get; set; }

        public object? Collation { get; set; }

        public string? Type { get; set; }

        public int? Length { get; set; }
    }
}