// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    using System;
    using Microsoft.Extensions.Logging;

    internal class LatencyLogger : IDisposable
    {
        private readonly DateTime startTime;
        private readonly string operationName;
        private readonly ILogger logger;

        public LatencyLogger(
            string operationName,
            ILogger logger)
        {
            startTime = DateTime.UtcNow;
            this.operationName = operationName;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose()
        {
            var endTime = DateTime.UtcNow;
            var latency = endTime - startTime;
            logger.LogInformation($"Operation: {operationName}, Latency: {latency.TotalMilliseconds} ms");
        }
    }
}