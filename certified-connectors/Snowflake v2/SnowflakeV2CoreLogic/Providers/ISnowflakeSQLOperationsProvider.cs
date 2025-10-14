// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Providers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using SnowflakeV2CoreLogic.Models.ConnectorModels;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;

    public interface ISnowflakeSQLOperationsProvider
    {
        public Task<SQLOperationsResponseModel> ExecuteSQLStatementAsync(
            HttpRequestMessage request,
            ExecuteSqlStatementModel payload,
            HeaderParameters headerParameters,
            QueryParameters queryParams);

        public Task<SQLOperationsResponseModel> GetResultsAsync(
            HttpRequestMessage request,
            DataSchemaModel? schema,
            string statementHandle,
            HeaderParameters headerParameters,
            QueryParameters queryParams);

        public Task<SnowflakeAPIResponseModel> CancelRequestAsync(
            HttpRequestMessage request,
            string statementHandle,
            HeaderParameters headerParameters,
            QueryParameters queryParams);
    }
}
