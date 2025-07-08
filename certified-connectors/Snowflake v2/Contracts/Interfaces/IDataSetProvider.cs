// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;

    /// <summary>
    /// Interface for dataset provider
    /// </summary>
    public interface IDataSetProvider
    {
        /// <summary>
        /// Lists datasets exposed by the connection
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>A task representing the operation</returns>
        Task<DataSetCollection> ListDataSetsAsync(HttpRequestMessage request);
    }
}
