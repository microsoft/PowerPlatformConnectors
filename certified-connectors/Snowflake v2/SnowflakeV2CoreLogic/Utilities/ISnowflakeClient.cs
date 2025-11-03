// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Utilities
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Models.ConnectorModels;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;

    /// <summary>
    /// Interface for Snowflake client
    /// </summary>
    public interface ISnowflakeClient
    {
        Task<SnowflakeTableData> CallAPIAsync(
            HttpClient? client,
            string sqlStatement,
            string endpoint,
            SnowflakeRequestBindings? requestBindings = null,
            SnowflakeConnectionParameters? perRequestConnectionParameters = null,
            RequestParameters? requestParameters = null,
            bool isSerializerSettings = false);

        Task<SnowflakeAPIResponseModel> ExecuteSqlStatementAsync(
            HttpClient? client,
            ExecuteSqlStatementModel fullAPIRequestPayload,
            HeaderParameters headerParameters,
            QueryParameters queryParams,
            string endpoint);

        Task<SnowflakeAPIResponseModel> GetResultsAsync(
            HttpClient? client,
            string statementHandle,
            HeaderParameters headerParameters,
            QueryParameters queryParams);

        Task<SnowflakeAPIResponseModel> CancelRequestAsync(
            HttpClient? client,
            string statementHandle,
            HeaderParameters headerParameters,
            QueryParameters queryParams);
    }
}