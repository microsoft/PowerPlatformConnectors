// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Models.SnowflakeAPIModels
{
    using System.Collections.Generic;

    public class Parameters
    {
        public string? timezone { get; set; }

        public string? query_tag { get; set; }

        public string? binary_output_format { get; set; }

        public string? date_output_format { get; set; }

        public string? time_output_format { get; set; }

        public string? timestamp_output_format { get; set; }

        public string? timestamp_ltz_output_format { get; set; }

        public string? timestamp_ntz_output_format { get; set; }

        public string? timestamp_tz_output_format { get; set; }

        public int? MULTI_STATEMENT_COUNT { get; set; }
    }

    public class ExecuteSqlStatementModel
    {
        public string? statement { get; set; }

        public int? timeout { get; set; }

        public string? database { get; set; }

        public string? schema { get; set; }

        public string? warehouse { get; set; }

        public string? role { get; set; }

        public Dictionary<string, object>? bindings { get; set; }

        public Parameters? parameters { get; set; }
    }
}
