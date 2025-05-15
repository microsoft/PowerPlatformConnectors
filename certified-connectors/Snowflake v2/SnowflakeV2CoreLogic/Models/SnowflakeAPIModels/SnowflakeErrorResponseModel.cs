// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Models.SnowflakeAPIModels
{
    public class SnowflakeErrorResponseModel
    {
        public string? Message { get; set; }

        public string? Code { get; set; }

        public string? SqlState { get; set; }

        public string? StatementHandle { get; set; }
    }
}