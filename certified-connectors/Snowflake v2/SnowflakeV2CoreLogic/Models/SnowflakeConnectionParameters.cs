// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Models
{
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;

    public class SnowflakeConnectionParameters
    {
        public string Server { get; set; } = null;

        public string Database { get; set; } = null;

        public string Role { get; set; } = null;

        public string Warehouse { get; set; } = null;

        public string Schema { get; set; } = null;

        public string AuthType { get; set; } = "OAUTH";

        public AuthenticationType AuthenticationType { get; set; }

        public IToken Token { get; set; }
    }
}
