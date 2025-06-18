// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.OData;
    using System.Web.OData.Query;
    using System.Web.OData.Routing;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic.Exceptions;

    /// <summary>
    /// Contains actions for item-level operations.
    /// </summary>
    public class SnowflakeTableDataController : ODataController
    {
        private readonly ITableDataProvider<Item> tableDataProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeTableDataController"/> class
        /// </summary>
        /// <param name="tableDataProvider">table data provider</param>
        /// <param name="logger">logger</param>
        public SnowflakeTableDataController(
            ITableDataProvider<Item> tableDataProvider,
            ILogger logger)
        {
            this.tableDataProvider = tableDataProvider ?? throw new ArgumentNullException(nameof(tableDataProvider));
            this.logger = logger;
        }

        /// <summary>
        /// Lists items present in table
        /// </summary>
        /// <param name="dataset">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="options">OData query options</param>
        /// <returns>A task representing the operation</returns>
        [HttpGet]
        [ODataRoute("datasets({dataset})/tables({table})/items")]
        [ResponseType(typeof(ItemsList))]
        public async Task<IHttpActionResult> GetItemsAsync(
            [FromODataUri] string dataset,
            [FromODataUri] string table,
            ODataQueryOptions<Item> options)
        {
            var decodedDataset = HttpUtility.UrlDecode(dataset)?.Trim();
            var decodedTable = HttpUtility.UrlDecode(table)?.Trim();

            if (string.IsNullOrEmpty(decodedDataset))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "dataset",
                    nameof(GetItemsAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeDataSetMissing));
            }

            if (string.IsNullOrEmpty(decodedTable))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "table",
                    nameof(GetItemsAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeTableNameMissing));
            }

            logger.LogInformation("List items started for table: _");

            try
            {
                IHttpActionResult result = Ok(await tableDataProvider.ListItemsAsync(Request, decodedDataset, decodedTable, options).ConfigureAwait(false));

                return result;
            }
            finally
            {
                logger.LogInformation("List items ended for table: _");
            }
        }

        /// <summary>
        /// Gets an item from table
        /// </summary>
        /// <param name="dataset">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="id">item key</param>
        /// <returns>A task representing the operation</returns>
        [HttpGet]
        [ODataRoute("datasets({dataset})/tables({table})/items({id})")]
        [ResponseType(typeof(Item))]
        public async Task<IHttpActionResult> GetItemAsync(
            [FromODataUri] string dataset,
            [FromODataUri] string table,
            [FromODataUri] string id)
        {
            var decodedDataset = HttpUtility.UrlDecode(dataset)?.Trim();
            var decodedTable = HttpUtility.UrlDecode(table)?.Trim();
            var decodedId = HttpUtility.UrlDecode(id)?.Trim();

            if (string.IsNullOrEmpty(decodedDataset))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "dataset",
                    nameof(GetItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeDataSetMissing));
            }

            if (string.IsNullOrEmpty(decodedTable))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "table",
                    nameof(GetItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeTableNameMissing));
            }

            if (string.IsNullOrEmpty(decodedId))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "id",
                    nameof(GetItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeItemIdMissing));
            }

            logger.LogInformation("Get item started for table: _ and id: _");

            try
            {
                // Execute operation
                Item result = await tableDataProvider.GetItemAsync(Request, decodedDataset, decodedTable, decodedId).ConfigureAwait(false);
                return Ok(result);
            }
            finally
            {
                logger.LogInformation("Get item ended for table: _ and id: _");
            }
        }

        /// <summary>
        /// Creates a new item into a table
        /// </summary>
        /// <param name="dataset">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="item">new item</param>
        /// <returns>A task representing the operation</returns>
        [HttpPost]
        [ODataRoute("datasets({dataset})/tables({table})/items")]
        [ResponseType(typeof(Item))]
        public async Task<IHttpActionResult> PostItemAsync(
            [FromODataUri] string dataset,
            [FromODataUri] string table,
            Item item)
        {
            var decodedDataset = HttpUtility.UrlDecode(dataset)?.Trim();
            var decodedTable = HttpUtility.UrlDecode(table)?.Trim();

            if (string.IsNullOrEmpty(decodedDataset))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "dataset",
                    nameof(PostItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeDataSetMissing));
            }

            if (string.IsNullOrEmpty(decodedTable))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "table",
                    nameof(PostItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeTableNameMissing));
            }

            if (item == null)
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "item",
                    nameof(PostItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeItemMissing));
            }

            logger.LogInformation("Create item started for table: _");

            try
            {
                // Execute operation
                CreatedItem<Item> createdItem = await tableDataProvider.CreateItemAsync(Request, decodedDataset, decodedTable, item).ConfigureAwait(false);

                return Created<Item>(GenerateLocationHeader(Request, createdItem.Id), createdItem.Item);
            }
            finally
            {
                logger.LogInformation("Create item ended for table: _");
            }
        }

        /// <summary>
        /// Patches a specific item in the table
        /// </summary>
        /// <param name="dataset">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="id">item key</param>
        /// <param name="item">item to be updated</param>
        /// <returns>A task representing the operation</returns>
        [HttpPatch]
        [HttpPut]
        [ODataRoute("datasets({dataset})/tables({table})/items({id})")]
        [ResponseType(typeof(Item))]
        public async Task<IHttpActionResult> PatchItemAsync(
            [FromODataUri] string dataset,
            [FromODataUri] string table,
            [FromODataUri] string id,
            Item item)
        {
            var decodedDataset = HttpUtility.UrlDecode(dataset)?.Trim();
            var decodedTable = HttpUtility.UrlDecode(table)?.Trim();
            var decodedId = HttpUtility.UrlDecode(id)?.Trim();

            if (string.IsNullOrEmpty(decodedDataset))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "dataset",
                    nameof(PatchItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeDataSetMissing));
            }

            if (string.IsNullOrEmpty(decodedTable))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "table",
                    nameof(PatchItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeTableNameMissing));
            }

            if (string.IsNullOrEmpty(decodedId))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "id",
                    nameof(PatchItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeItemIdMissing));
            }

            if (item == null)
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "item",
                    nameof(PatchItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeItemMissing));
            }

            logger.LogInformation("Patch item started for table: _ and id: _");

            try
            {
                // Execute operation
                Item result = await tableDataProvider.PatchItemAsync(Request, decodedDataset, decodedTable, decodedId, item).ConfigureAwait(false);
                return Ok(result);
            }
            finally
            {
                logger.LogInformation("Patch item ended for table: _ and id: _");
            }
        }

        /// <summary>
        /// Deletes a specific item in the table
        /// </summary>
        /// <param name="dataset">dataset name</param>
        /// <param name="table">table name</param>
        /// <param name="id">item key</param>
        /// <returns>A task representing the operation</returns>
        [HttpDelete]
        [ODataRoute("datasets({dataset})/tables({table})/items({id})")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteItemAsync(
            [FromODataUri] string dataset,
            [FromODataUri] string table,
            [FromODataUri] string id)
        {
            var decodedDataset = HttpUtility.UrlDecode(dataset)?.Trim();
            var decodedTable = HttpUtility.UrlDecode(table)?.Trim();
            var decodedId = HttpUtility.UrlDecode(id)?.Trim();

            if (string.IsNullOrEmpty(decodedDataset))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "dataset",
                    nameof(DeleteItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeDataSetMissing));
            }

            if (string.IsNullOrEmpty(decodedTable))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "table",
                    nameof(DeleteItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeTableNameMissing));
            }

            if (string.IsNullOrEmpty(decodedId))
            {
                logger.LogError(string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.RequiredParameterIsMissed,
                    "id",
                    nameof(DeleteItemAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeItemIdMissing));
            }

            logger.LogInformation("Delete item started for table: _ and id: _");

            try
            {
                // Execute operation
                await tableDataProvider.DeleteItemAsync(Request, decodedDataset, decodedTable, decodedId).ConfigureAwait(false);
                return Ok();
            }
            finally
            {
                logger.LogInformation("Delete item ended for table: _ and id: _");
            }
        }

        private static Uri GenerateLocationHeader(
            HttpRequestMessage request,
            string createdItemId)
        {
            TryGetHeader(
                request,
                SnowflakeV2CoreLogic.Constants.HeaderApimReferrer,
                out string referrerHeader);

            if (!string.IsNullOrEmpty(referrerHeader))
            {
                referrerHeader = HttpUtility.UrlDecode(referrerHeader);
            }

            var referrerHeaderUrl = referrerHeader;

            if (!Uri.TryCreate(referrerHeaderUrl, UriKind.Absolute, out Uri requestUri))
            {
                requestUri = request.RequestUri;
            }

            return new Uri(requestUri, createdItemId);
        }

        private static bool TryGetHeader(
            HttpRequestMessage request,
            string key,
            out string value)
        {
            value = null;
            if (request.Headers != null && request.Headers.TryGetValues(key, out IEnumerable<string> headerValues) && headerValues.Any())
            {
                value = headerValues.First();
                return true;
            }

            return false;
        }
    }
}