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

    /// <summary>
    /// Class for datasets metadata data provider
    /// </summary>
    public class SnowflakeDataSetsMetadataProvider : IDataSetsMetadataProvider
    {
        private readonly ILogger logger;

        public SnowflakeDataSetsMetadataProvider(
            ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets datasets metadata
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>A task representing the operation</returns>
        public async Task<DataSetsMetadata> GetDataSetsMetadataAsync(
            HttpRequestMessage request)
        {
            DataSetsMetadata dataSetsMetadata = new DataSetsMetadata
            {
                TabularDataSetsMetadata = new TabularDataSetsMetadata()
                {
                    Source = DataSetsMetadataSource.Singleton,
                    UrlEncoding = DataSetsMetadataUrlEncoding.Single,
                },
            };

            logger.LogInformation("Completed GET of datasets metadata for Snowflake.");
            return await Task.FromResult(dataSetsMetadata).ConfigureAwait(false);
        }
    }
}