// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Models.ConnectorModels
{
    public class HeaderParameters
    {
        public string? Instance { get; set; }

        public string? ContentType { get; set; }

        public string? Accept { get; set; }
    }
}