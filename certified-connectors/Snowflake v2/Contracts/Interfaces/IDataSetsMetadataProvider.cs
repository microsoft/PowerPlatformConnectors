// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;

    /// <summary>
    /// Interface for datasets metadata provider
    /// </summary>
    public interface IDataSetsMetadataProvider
    {
        /// <summary>
        /// Gets datasets metadata
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>A task representing the operation</returns>
        Task<DataSetsMetadata> GetDataSetsMetadataAsync(HttpRequestMessage request);
    }
}
