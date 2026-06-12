// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.OData.Query;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;

    /// <summary>
    /// Interface for table data provider. Exposes CRUD operations on tabular data.
    /// </summary>
    /// <typeparam name="TItem">Item entity on which the provider operates</typeparam>
    public interface ITableDataProvider<TItem>
    {
        /// <summary>
        /// Lists items present in a table
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="options">OData query options</param>
        /// <returns>A task representing the operation</returns>
        Task<IEnumerable<TItem>> ListItemsAsync(HttpRequestMessage request, string dataSet, string table, ODataQueryOptions<TItem> options);

        /// <summary>
        /// Gets a specific item present in the table
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="id">item key</param>
        /// <returns>A task representing the operation</returns>
        Task<TItem> GetItemAsync(HttpRequestMessage request, string dataSet, string table, string id);

        /// <summary>
        /// Creates a new item in the table
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="item">new item</param>
        /// <returns>A task representing the operation</returns>
        Task<CreatedItem<TItem>> CreateItemAsync(HttpRequestMessage request, string dataSet, string table, TItem item);

        /// <summary>
        /// Patches an existing item in the table
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="id">item key</param>
        /// <param name="item">item to be updated</param>
        /// <returns>A task representing the operation</returns>
        Task<TItem> PatchItemAsync(HttpRequestMessage request, string dataSet, string table, string id, TItem item);

        /// <summary>
        /// Deletes a specific item in the table
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="dataSet">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="id">item key</param>
        /// <returns>A task representing the operation</returns>
        Task DeleteItemAsync(HttpRequestMessage request, string dataSet, string table, string id);
    }
}
