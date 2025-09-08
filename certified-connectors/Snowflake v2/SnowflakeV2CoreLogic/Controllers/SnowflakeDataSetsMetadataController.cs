// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Controller implementing datasets metadata API
    /// </summary>
    public class SnowflakeDataSetsMetadataController : ApiController
    {
        private readonly IDataSetsMetadataProvider dataSetsMetadataProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeDataSetsMetadataController"/> class
        /// </summary>
        /// <param name="dataSetsMetadataProvider">datasets metadata provider</param>
        /// <param name="logger">logger</param>
        public SnowflakeDataSetsMetadataController(
            IDataSetsMetadataProvider dataSetsMetadataProvider,
            ILogger logger)
        {
            this.dataSetsMetadataProvider = dataSetsMetadataProvider ?? throw new ArgumentNullException(nameof(dataSetsMetadataProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("$metadata.json/datasets")]
        [ResponseType(typeof(DataSetsMetadata))]
        public async Task<IHttpActionResult> GetDataSetsMetadataAsync()
        {
            logger.LogInformation("Get datasets metadata started");

            // Execute operation
            try
            {
                DataSetsMetadata metadata = await dataSetsMetadataProvider.GetDataSetsMetadataAsync(Request).ConfigureAwait(false);
                return Ok(metadata);
            }
            finally
            {
                logger.LogInformation("Get datasets metadata ended");
            }
        }
    }
}
