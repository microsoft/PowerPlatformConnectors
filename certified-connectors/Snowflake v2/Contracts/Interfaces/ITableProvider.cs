// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.OData.Query;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;

    /// <summary>
    /// Interface for table provider
    /// </summary>
    public interface ITableProvider
    {
        /// <summary>
        /// Lists tables exposed by the connection
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <returns>A task representing the operation</returns>
        Task<TableCollection> ListTablesAsync(HttpRequestMessage request, string dataSet);

        /// <summary>
        /// Lists tables exposed by the connection
        /// which are applicable to the given operation
        /// and apply OData semantics to allow filtering and paging of results
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <param name="operation">operation name</param>
        /// <param name="options">OData options</param>
        /// <returns>A task representing the operation</returns>
        Task<TableCollection> ListTablesAsync(HttpRequestMessage request, string dataSet, string operation, ODataQueryOptions<Table> options);
    }
}
