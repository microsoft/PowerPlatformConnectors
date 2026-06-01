// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;

    /// <summary>
    /// Interface for table metadata provider
    /// </summary>
    public interface ITableMetadataProvider
    {
        /// <summary>
        /// Gets metadata for a specific table
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <param name="table">table name</param>
        /// <returns>A task representing the operation</returns>
        Task<TableMetadata> GetTableAsync(HttpRequestMessage request, string dataSet, string table);

        /// <summary>
        /// Gets metadata for a specific table in the context of a particular operation
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="operation">operation name</param>
        /// <returns>A task representing the operation</returns>
        Task<TableMetadata> GetTableAsync(HttpRequestMessage request, string dataSet, string table, TableOperation operation);
    }
}
