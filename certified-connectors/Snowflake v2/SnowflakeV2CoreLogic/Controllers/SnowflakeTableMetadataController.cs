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
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic.Exceptions;

    /// <summary>
    /// Contains actions for metadata-level operations.
    /// </summary>
    public class SnowflakeTableMetadataController : ApiController
    {
        private readonly ITableMetadataProvider tableMetadataProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeTableMetadataController"/> class
        /// </summary>
        /// <param name="tableMetadataProvider">table metadata provider</param>
        /// <param name="logger">logger</param>
        public SnowflakeTableMetadataController(
            ITableMetadataProvider tableMetadataProvider,
            ILogger logger)
        {
            this.tableMetadataProvider = tableMetadataProvider ?? throw new ArgumentNullException(nameof(tableMetadataProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get metadata of a table
        /// </summary>
        /// <param name="dataset">dataset name</param>
        /// <param name="table">table name</param>
        /// <returns>
        /// table metadata
        /// </returns>
        [HttpGet]
        [Route("$metadata.json/datasets/{dataset}/tables/{table}")]
        [ResponseType(typeof(TableMetadata))]
        public async Task<IHttpActionResult> GetTableMetadataAsync(
            string dataset,
            string table)
        {
            string decodedDataset = HttpUtility.UrlDecode(dataset)?.Trim();
            string decodedTable = HttpUtility.UrlDecode(table)?.Trim();

            if (string.IsNullOrEmpty(decodedDataset))
            {
                logger.LogError(string.Format(
                    Resource.RequiredParameterIsMissed,
                    "dataset",
                    nameof(GetTableMetadataAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeDataSetMissing));
            }

            if (string.IsNullOrEmpty(decodedTable))
            {
                logger.LogError(string.Format(
                    Resource.RequiredParameterIsMissed,
                    "table",
                    nameof(GetTableMetadataAsync)));

                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        Resource.SnowflakeTableNameMissing));
            }

            logger.LogInformation("Get metadata started for table: _");

            // Execute operation
            try
            {
                TableMetadata metadata = await tableMetadataProvider.GetTableAsync(Request, decodedDataset, decodedTable, TableOperation.Unknown).ConfigureAwait(false);

                if (metadata == null)
                {
                    logger.LogError($"Table: '_' is not found in the connection.");
                    return NotFound();
                }

                return Ok(metadata);
            }
            finally
            {
                logger.LogInformation("Get metadata ended for table: _");
            }
        }
    }
}