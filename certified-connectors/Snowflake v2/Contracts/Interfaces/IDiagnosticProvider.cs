// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for diagnostic provider. Exposes diagnostic operations on a connector.
    /// </summary>
    public interface IDiagnosticProvider
    {
        /// <summary>
        /// Execute TestConnection
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>A task representing the operation</returns>
        Task<HttpResponseMessage> TestConnectionAsync(HttpRequestMessage request);
    }
}
