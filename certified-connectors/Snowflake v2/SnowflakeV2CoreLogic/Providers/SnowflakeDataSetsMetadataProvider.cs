// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic.Models;

    /// <summary>
    /// Class for datasets metadata data provider
    /// </summary>
    public class SnowflakeDataSetsMetadataProvider : IDataSetsMetadataProvider
    {
        private readonly ILogger logger;
        private readonly SnowflakeConnectionParametersProvider connectionParametersProvider;

        public SnowflakeDataSetsMetadataProvider(
            ILogger logger,
            SnowflakeConnectionParametersProvider connectionParametersProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.connectionParametersProvider = connectionParametersProvider ?? throw new ArgumentNullException(nameof(connectionParametersProvider));
        }

        /// <summary>
        /// Gets datasets metadata
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>A task representing the operation</returns>
        public async Task<DataSetsMetadata> GetDataSetsMetadataAsync(
            HttpRequestMessage request)
        {
            SnowflakeConnectionParameters connectionParameters = connectionParametersProvider.GetConnectionParameters();

            DataSetsMetadata dataSetsMetadata = new DataSetsMetadata
            {
                TabularDataSetsMetadata = new TabularDataSetsMetadata()
                {
                    Source = $"{connectionParameters.Server},{connectionParameters.Database}",
                    UrlEncoding = DataSetsMetadataUrlEncoding.Single,
                },
            };

            logger.LogInformation("Completed GET of datasets metadata for Snowflake.");
            return await Task.FromResult(dataSetsMetadata).ConfigureAwait(false);
        }
    }
}