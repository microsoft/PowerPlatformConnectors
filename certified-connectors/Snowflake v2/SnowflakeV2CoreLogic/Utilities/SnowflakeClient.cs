// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Utilities
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Exceptions;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Models.ConnectorModels;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Providers;

    public class SnowflakeClient : ISnowflakeClient
    {
        private readonly SnowflakeConnectionParametersProvider connectionParametersProvider;
        private readonly ILogger logger;
        private SnowflakeConnectionParameters connectionDetails;
        private JsonSerializerSettings serializerSettings;

        public SnowflakeClient(SnowflakeConnectionParametersProvider connectionParametersProvider, ILogger logger)
        {
            this.connectionParametersProvider = connectionParametersProvider ?? throw new ArgumentNullException(nameof(logger));
            connectionDetails = connectionParametersProvider.GetConnectionParameters();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
            };
        }

        public async Task<SnowflakeTableData> CallAPIAsync(
            HttpClient? client,
            string statement,
            string endpoint,
            SnowflakeRequestBindings? requestBindings = null,
            SnowflakeConnectionParameters? perRequestConnectionParameters = null,
            RequestParameters? requestParameters = null,
            bool isSerializerSettings = false)
        {
            logger.LogInformation("CallAPIAsync entered");

            if (connectionDetails.AuthenticationType == AuthenticationType.AADUserDelegated)
            {
                throw new Exception("Cannot use Tabular calls with UserDelegated authentication type. Please use Service Principle Auth.");
            }

            // Use the perReqestConnectionParameters if they are provided, otherwise use the connectionDetails
            var connectionDetailsForRequest = perRequestConnectionParameters ?? connectionDetails;

            string validatedServer = connectionDetailsForRequest.Server.EnsureValidSnowflakeUrl("Server");

            // Create the API endpoint
            var apiURL = $"https://{validatedServer}/api/v2/statements";

            var sfRequestBody = new SnowflakeRequestPostBody();
            
            // Add version comment to the statement with specific endpoint info
            sfRequestBody.statement = AddVersionComment(statement, endpoint);
            
            sfRequestBody.bindings = requestBindings?.bindings;
            sfRequestBody.parameters = requestParameters;

            // Map all the data into the payload object
            sfRequestBody.warehouse = connectionDetailsForRequest.Warehouse;
            sfRequestBody.role = connectionDetailsForRequest.Role;
            sfRequestBody.database = connectionDetailsForRequest.Database;
            sfRequestBody.schema = connectionDetailsForRequest.Schema;
            sfRequestBody.parameters = requestParameters;

            // Create the payload for the HTTP request
            var postBodyRequestData = isSerializerSettings ? Newtonsoft.Json.JsonConvert.SerializeObject(sfRequestBody, serializerSettings) : Newtonsoft.Json.JsonConvert.SerializeObject(sfRequestBody);
            var postBodyContent = new StringContent(postBodyRequestData, System.Text.Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiURL)
            {
                Content = postBodyContent,
            };

            // Make a post request to the URI with the payload
            return await SendHTTPRequestAsync<SnowflakeTableData>(request, client).ConfigureAwait(true);
        }

        public async Task<SnowflakeAPIResponseModel> ExecuteSqlStatementAsync(
            HttpClient? client,
            ExecuteSqlStatementModel fullAPIRequestPayload,
            HeaderParameters? headerParameters,
            QueryParameters? queryParameters,
            string endpoint)
        {
            logger.LogInformation("ExecuteSqlStatementAsync entered");

            var urlBase = $"https://{headerParameters?.Instance}/api/v2/statements";

            var apiURL = AddQueryParametersToUrl(urlBase, queryParameters);

            if (!string.IsNullOrEmpty(fullAPIRequestPayload.statement))
            {
                fullAPIRequestPayload.statement = AddVersionComment(fullAPIRequestPayload.statement, endpoint);
            }

            // Create the payload for the HTTP request
            var postBodyRequestData = JsonConvert.SerializeObject(fullAPIRequestPayload, serializerSettings);
            var postBodyContent = new StringContent(postBodyRequestData, System.Text.Encoding.UTF8, "application/json");

            // Create the request message with the post body content
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiURL);
            request.Content = postBodyContent;

            return await SendHTTPRequestAsync<SnowflakeAPIResponseModel>(request, client).ConfigureAwait(true);
        }

        public async Task<SnowflakeAPIResponseModel> GetResultsAsync(
            HttpClient? client,
            string statementHandle,
            HeaderParameters? headerParameters,
            QueryParameters? queryParameters)
        {
            logger.LogInformation("GetResultsAsync entered");

            // Set the url including the statement handle
            var urlBase = $"https://{headerParameters?.Instance}/api/v2/statements/{statementHandle}";

            var apiURL = AddQueryParametersToUrl(urlBase, queryParameters);

            // Create the request message
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiURL);

            return await SendHTTPRequestAsync<SnowflakeAPIResponseModel>(request, client).ConfigureAwait(true);
        }

        public async Task<SnowflakeAPIResponseModel> CancelRequestAsync(
            HttpClient? client,
            string statementHandle,
            HeaderParameters? headerParameters,
            QueryParameters? queryParameters)
        {
            logger.LogInformation("CancelRequestAsync entered");

            var urlBase = $"https://{headerParameters?.Instance}/api/v2/statements/{statementHandle}/cancel";

            var apiURL = AddQueryParametersToUrl(urlBase, queryParameters);

            // Create the request message with the post body content
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiURL);

            // The API is technically a post, but there is no payload required
            var postBodyContent = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
            request.Content = postBodyContent;

            return await SendHTTPRequestAsync<SnowflakeAPIResponseModel>(request, client).ConfigureAwait(true);
        }

        private async Task<T> SendHTTPRequestAsync<T>(
            HttpRequestMessage request,
            HttpClient? client)
        {
            logger.LogInformation("SendHTTPRequestAsync entered");

            // Make sure we have a valid client to make a request
            client.EnsureNotNull("Snowflake HTTPClient");

            // Add the required headers before making any outbound requests
            request.Headers.Add(Constants.SnowflakeHttpHeaderTokenType, connectionDetails.AuthType);
            request.Headers.Add(Constants.SnowflakeHttpHeaderAuthorization, $"Bearer {connectionDetails.Token.AccessToken}");
            request.Headers.Add(Constants.SnowflakeHttpHeaderAccept, "*/*");
            request.Headers.Add(Constants.SnowflakeHttpHeaderAgent, "Microsoft_Dataverse/9.1.0");

            // Send the request and wait for a response
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var response = await client.SendAsync(request).ConfigureAwait(true);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Check if we got a successful response
            if (response.IsSuccessStatusCode)
            {
                string responseContent;

                if (response.Content.Headers.ContentEncoding.Contains("gzip", StringComparer.InvariantCultureIgnoreCase))
                {
                    logger.LogInformation($"Decoding response with Content-Encoding: {string.Join(",", response.Content.Headers.ContentEncoding)}", nameof(SendHTTPRequestAsync));
                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                    using (var reader = new StreamReader(gzipStream))
                    {
                        responseContent = await reader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

                // Throw an exception if the response is null or empty string
                responseContent.EnsureNotWhiteSpace(responseContent);

                try
                {
                    // Deserialize the response content to the specified type
#pragma warning disable CS8603 // Possible null reference return.
                    return JsonConvert.DeserializeObject<T>(responseContent, serializerSettings);
#pragma warning restore CS8603 // Possible null reference return.
                }
                catch (Exception)
                {
                    throw new Exception("Failed to deserialize snowflake API response");
                }
            }
            else
            {
                // Throw an exception if the response is not successful (Should we be throwing an exception here?)
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                // It feels cleaner to have a separate SnowflakeErrorResponseModel, but technically it's just a subset of the SnowflakeAPIResponseModel.
                // We could alternatively use that and only read the error related fields we care about.
                var errorData = JsonConvert.DeserializeObject<SnowflakeErrorResponseModel>(responseContent, serializerSettings);
                throw new SnowflakeHttpException(response.StatusCode, errorData);
            }
        }

        private Uri AddQueryParametersToUrl(
            string urlBase,
            QueryParameters? queryParameters)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            // If queryParameters.requestId is not null then add it to the query string collection
            if (!string.IsNullOrWhiteSpace(queryParameters?.RequestId))
            {
                queryString.Add("requestId", queryParameters?.RequestId);
            }

            // nullable
            if (queryParameters?.Nullable != null)
            {
                queryString.Add("nullable", queryParameters.Nullable.ToString());
            }

            if (queryParameters?.AsyncExecution != null)
            {
                queryString.Add("async", queryParameters.AsyncExecution.ToString());
            }

            if (queryParameters?.Partition != null)
            {
                queryString.Add("partition", queryParameters.Partition.ToString());
            }

            // Append the query string to the url
            return new Uri(urlBase + "?" + queryString.ToString());
        }

        /// <summary>
        /// Adds version comment to SQL statement for query history tracking
        /// </summary>
        /// <param name="sqlStatement">The original SQL statement</param>
        /// <param name="operation">Optional operation name for additional context</param>
        /// <returns>SQL statement with version comment at the end before semicolon</returns>
        public static string AddVersionComment(string? sqlStatement = null, string? operation = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStatement))
                return sqlStatement ?? string.Empty;

            var operationInfo = !string.IsNullOrEmpty(operation) ? $" - {operation}" : "";
            var versionComment = $"\n-- {Constants.ConnectorName} v{Constants.ConnectorVersion}{operationInfo}\n";

            // Trim the SQL statement
            var trimmedSql = sqlStatement!.Trim();

            // Check if the query ends with a semicolon
            if (trimmedSql.EndsWith(";"))
            {
                // Insert comment before the semicolon
                var queryWithoutSemicolon = trimmedSql.Substring(0, trimmedSql.Length - 1);
                return queryWithoutSemicolon + versionComment + ";";
            }
            else
            {
                // Add comment at the end and add semicolon
                return trimmedSql + versionComment + ";";
            }
        }
    }
}
