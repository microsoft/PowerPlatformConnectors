// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Microsoft.Extensions.Logging;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Exceptions;
    using SnowflakeV2CoreLogic.Models.ConnectorModels;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Providers;
    using SnowflakeV2CoreLogic.Utilities;

    /// <summary>
    /// Controller to support SQL API operations
    /// </summary>
    public class SnowflakeSQLController : ApiController
    {
        private readonly ISnowflakeSQLOperationsProvider sqlOperationsProvider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnowflakeSQLController"/> class.
        /// </summary>
        /// <param name="sqlOperationsProvider">sql operations provider</param>
        /// <param name="logger">logger</param>
        public SnowflakeSQLController(
            ISnowflakeSQLOperationsProvider sqlOperationsProvider,
            ILogger logger)
        {
            this.sqlOperationsProvider = sqlOperationsProvider ?? throw new ArgumentNullException(nameof(sqlOperationsProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("sql")]
        [ResponseType(typeof(SnowflakeAPIResponseModel))]
        public async Task<HttpResponseMessage> ExecuteSQLStatementAsync(
            [FromBody] ExecuteSqlStatementModel payload,
            [FromUri] QueryParameters queryParams)
        {
            try
            {
                logger.LogInformation("Calling ExecuteSQLStatementAsync");
                payload.EnsureNotNull("Execute SQL Statement payload is null. Please specify a parameter.");

                var headerParameters = ExtractParametersFromHeader(Request.Headers);

                var response = await sqlOperationsProvider.ExecuteSQLStatementAsync(Request, payload, headerParameters, queryParams).ConfigureAwait(true);

                return Request.CreateResponse(
                   statusCode: HttpStatusCode.OK,
                   value: response,
                   configuration: Configuration);
            }
            catch (ArgumentNullException age)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{Constants.Http400ResponseMessage} - {age.Message}", age);
            }
            catch (SnowflakeHttpException sfe)
            {
                return Request.CreateErrorResponse(sfe.HTTPStatusCode, sfe.Message);
            }
            catch (ArgumentException ae)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{Constants.Http400ResponseMessage} - {ae.Message}", ae);
            }
        }

        [HttpPost]
        [Route("sql/{statementHandle}")]
        [ResponseType(typeof(SnowflakeAPIResponseModel))]
        public async Task<HttpResponseMessage> GetResultsAsync(
            [FromBody] DataSchemaModel? schema,
            string statementHandle,
            [FromUri] QueryParameters queryParams)
        {
            try
            {
                logger.LogInformation("Calling GetResultsAsync");
                statementHandle.EnsureNotNull("Statement Handle");

                var headerParameters = ExtractParametersFromHeader(Request.Headers);

                var response = await sqlOperationsProvider.GetResultsAsync(Request, schema, statementHandle, headerParameters, queryParams).ConfigureAwait(true);

                return Request.CreateResponse(
                   statusCode: HttpStatusCode.OK,
                   value: response,
                   configuration: Configuration);
            }
            catch (ArgumentNullException age)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{Constants.Http400ResponseMessage} - {age.Message}", age);
            }
            catch (SnowflakeHttpException sfe)
            {
                return Request.CreateErrorResponse(sfe.HTTPStatusCode, sfe.Message);
            }
            catch (ArgumentException ae)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{Constants.Http400ResponseMessage} - {ae.Message}", ae);
            }
        }

        [HttpPost]
        [Route("sql/{statementHandle}/cancel")]
        [ResponseType(typeof(SnowflakeAPIResponseModel))]
        public async Task<HttpResponseMessage> CancelAsync(
            string statementHandle,
            [FromUri] QueryParameters queryParams)
        {
            try
            {
                statementHandle.EnsureNotNull("Statement Handle can not be null.");

                var headerParameters = ExtractParametersFromHeader(Request.Headers);

                var response = await sqlOperationsProvider.CancelRequestAsync(Request, statementHandle, headerParameters, queryParams).ConfigureAwait(true);

                return Request.CreateResponse(
                   statusCode: HttpStatusCode.OK,
                   value: response,
                   configuration: Configuration);
            }
            catch (ArgumentNullException age)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{Constants.Http400ResponseMessage} - {age.Message}", age);
            }
            catch (SnowflakeHttpException sfe)
            {
                return Request.CreateErrorResponse(sfe.HTTPStatusCode, sfe.Message);
            }
            catch (ArgumentException ae)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"{Constants.Http400ResponseMessage} - {ae.Message}", ae);
            }
        }

        private HeaderParameters ExtractParametersFromHeader(
            HttpRequestHeaders headers)
        {
            var instance = headers.TryGetValues("Instance", out var instanceValues) ? instanceValues.First() : string.Empty;
            var accept = (headers.TryGetValues("Accept", out var acceptValues) ? acceptValues.First() : null).EnsureNotNull("Accept header");

            ValidateInstanceUrl(instance);

            return new HeaderParameters
            {
                Instance = instance,
                Accept = accept,
                ContentType = "application/json",
            };
        }

        private static void ValidateInstanceUrl(
            string instanceUrl)
        {
            // Validate that the instance is not null or empty
            if (string.IsNullOrEmpty(instanceUrl))
            {
                throw new ArgumentNullException(nameof(instanceUrl));
            }

            // Validate that the instance does not have http or https in front of it
            if (instanceUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || instanceUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"The Instance parameter cannot contain http or https. Please remove it and try again.");
            }

            // Validate that the instance does not have a trailing slash
            if (instanceUrl.EndsWith("/"))
            {
                throw new ArgumentException($"The Instance parameter cannot contain a trailing slash. Please remove it and try again.");
            }
        }
    }
}