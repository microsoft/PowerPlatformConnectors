// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.OData;
    using System.Web.OData.Routing;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic.Exceptions;

    /// <summary>
    /// Contains actions for dataset-level operations.
    /// </summary>
    public class SnowflakeTableController : ODataController
    {
        private readonly ITableProvider tableProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeTableController"/> class
        /// </summary>
        /// <param name="tableProvider">table provider</param>
        /// <param name="logger">logger</param>
        public SnowflakeTableController(
            ITableProvider tableProvider,
            ILogger logger)
        {
            this.tableProvider = tableProvider ?? throw new ArgumentNullException(nameof(tableProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Lists tables exposed by the connection
        /// </summary>
        /// <param name="dataset">dataset name</param>
        /// <returns>A task representing the operation</returns>
        [HttpGet]
        [ODataRoute("datasets({dataset})/tables")]
        [ResponseType(typeof(TablesList))]
        public async Task<IHttpActionResult> GetTablesAsync(
            [FromODataUri] string dataset)
        {
            var decodedDataset = HttpUtility.UrlDecode(dataset)?.Trim();

            if (string.IsNullOrEmpty(decodedDataset))
            {
                logger.LogError(string.Format(
                    Resource.RequiredParameterIsMissed,
                    "dataset",
                    nameof(GetTablesAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeDataSetMissing));
            }

            logger.LogInformation("List tables started");

            // Execute operation
            try
            {
                TableCollection result = await tableProvider.ListTablesAsync(Request, decodedDataset).ConfigureAwait(false);
                return Ok(result);
            }
            finally
            {
                logger.LogInformation("List tables ended");
            }
        }
    }
}