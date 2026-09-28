// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Implements operations performed on DataSets.
    /// </summary>
    public class SnowflakeDataSetProvider : IDataSetProvider
    {
        private readonly ILogger logger;
        private readonly SnowflakeConnectionParametersProvider connectionParametersProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeDataSetProvider"/> class.
        /// </summary>
        /// <param name="logger">logger</param>
        public SnowflakeDataSetProvider(
            ILogger logger,
            SnowflakeConnectionParametersProvider connectionParametersProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.connectionParametersProvider = connectionParametersProvider;
        }

        /// <inheritdoc />
        public Task<DataSetCollection> ListDataSetsAsync(
            HttpRequestMessage request)
        {
            logger.LogInformation("Initiating GET of datasets for Snowflake.");

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var connectionParameters = connectionParametersProvider.GetConnectionParameters();
            var dataSetCollection = new DataSetCollection
            {
                new DataSet()
                {
                    Name = $"{connectionParameters.Server},{connectionParameters.Database}",
                    DisplayName = "dataset",
                },
            };

            logger.LogInformation("Completed GET of datasets for Snowflake.");
            return Task.FromResult(dataSetCollection);
        }
    }
}