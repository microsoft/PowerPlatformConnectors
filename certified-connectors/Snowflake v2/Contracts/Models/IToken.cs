// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models
{
    using System;

    /// <summary>
    /// Token interface
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Access token
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Token type
        /// </summary>
        string TokenType { get; }

        /// <summary>
        /// UId
        /// </summary>
        string UId { get; }

        /// <summary>
        /// Refresh change stamp
        /// </summary>
        string RefreshChangeStamp { get; }

        /// <summary>
        /// Token acquire time
        /// </summary>
        DateTime TokenAcquireTime { get; }
    }
}
