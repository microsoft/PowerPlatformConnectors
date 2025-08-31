// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.OData;
    using System.Web.OData.Routing;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Controller implementing dataSet-level operations.
    /// </summary>
    public class SnowflakeDataSetController : ODataController
    {
        private readonly IDataSetProvider datasetProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeDataSetController"/> class
        /// </summary>
        /// <param name="dataSetProvider">datasets provider</param>
        /// <param name="logger">logger</param>
        public SnowflakeDataSetController(
            IDataSetProvider dataSetProvider,
            ILogger logger)
        {
            datasetProvider = dataSetProvider ?? throw new ArgumentNullException(nameof(dataSetProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ODataRoute("datasets")]
        [ResponseType(typeof(DataSetsList))]
        public async Task<IHttpActionResult> GetDataSetsAsync()
        {
            logger.LogInformation("List datasets started");

            // Execute operation
            try
            {
                DataSetCollection result = await datasetProvider.ListDataSetsAsync(Request).ConfigureAwait(false);
                return Ok(result);
            }
            finally
            {
                logger.LogInformation("List datasets ended");
            }
        }
    }
}