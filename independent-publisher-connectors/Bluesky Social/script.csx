using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

public class Script : ScriptBase
{
    private static string cachedAccessToken = null;
    private static DateTime tokenExpiration = DateTime.MinValue;
    private static readonly SemaphoreSlim tokenLock = new SemaphoreSlim(1, 1); //Prevents race conditions

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var _logger = this.Context.Logger;
        var request = this.Context.Request;
        var operationId = this.Context.OperationId;

        _logger.LogInformation("Received request: {Method} {Uri} {OperationId}", request.Method, request.RequestUri, operationId);

        var queryParams = HttpUtility.ParseQueryString(request.RequestUri.Query);
        string pdsHost = queryParams["pdshost"];
        string blueskyID = queryParams["blueskyid"];
        string blueskyPassword = queryParams["blueskypassword"];

        if (string.IsNullOrEmpty(pdsHost) || string.IsNullOrEmpty(blueskyID) || string.IsNullOrEmpty(blueskyPassword))
        {
            _logger.LogError("Missing required query parameters.");
            return BuildErrorResponse(HttpStatusCode.BadRequest, "Missing required query parameters.");
        }

        _logger.LogInformation("Extracted query parameters: pdshost={pdsHost}, blueskyid={blueskyID}");

        //Thread-safe token handling
        await tokenLock.WaitAsync();
        try
        {
            if (!string.IsNullOrEmpty(cachedAccessToken) && tokenExpiration > DateTime.UtcNow)
            {
                _logger.LogInformation("Using cached access token.");
                return await ForwardRequestWithTokenAsync(cachedAccessToken, pdsHost, request);
            }

            //Refresh token if expired
            var newToken = await GetNewAccessTokenAsync(pdsHost, blueskyID, blueskyPassword);
            if (string.IsNullOrEmpty(newToken))
            {
                return BuildErrorResponse(HttpStatusCode.Unauthorized, "Failed to retrieve a valid access token.");
            }

            return await ForwardRequestWithTokenAsync(newToken, pdsHost, request);
        }
        finally
        {
            tokenLock.Release();
        }
    }

    private async Task<string> GetNewAccessTokenAsync(string pdsHost, string blueskyID, string blueskyPassword)
    {
        var _logger = this.Context.Logger;
        var sessionURL = $"https://{pdsHost}/xrpc/com.atproto.server.createSession";
        _logger.LogInformation("Session URL: {SessionURL}", sessionURL);

        var requestBody = JsonConvert.SerializeObject(new { identifier = blueskyID, password = blueskyPassword });
        var sessionRequest = new HttpRequestMessage(HttpMethod.Post, sessionURL)
        {
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
        };

        var sessionResponse = await this.Context.SendAsync(sessionRequest, this.CancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Received session response: {StatusCode}", sessionResponse.StatusCode);

        if (!sessionResponse.IsSuccessStatusCode)
        {
            return null;
        }

        var sessionContent = await sessionResponse.Content.ReadAsStringAsync();
        var sessionData = JsonConvert.DeserializeObject<SessionResponse>(sessionContent);

        if (string.IsNullOrEmpty(sessionData?.AccessJwt))
        {
            _logger.LogError("Failed to extract accessJwt.");
            return null;
        }

        cachedAccessToken = sessionData.AccessJwt;
        tokenExpiration = DateTime.UtcNow.AddMinutes(4.5); //Slight buffer to avoid expiry edge cases

        _logger.LogInformation("Extracted and cached accessJwt. Token expiration set to {TokenExpiration}", tokenExpiration);
        return cachedAccessToken;
    }

    private async Task<HttpResponseMessage> ForwardRequestWithTokenAsync(string accessJwt, string pdsHost, HttpRequestMessage request)
    {
        var _logger = this.Context.Logger;
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessJwt);

        var uriBuilder = new UriBuilder(request.RequestUri)
        {
            Host = pdsHost
        };
        var updatedQueryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
        updatedQueryParams.Remove("pdshost");
        updatedQueryParams.Remove("blueskyid");
        updatedQueryParams.Remove("blueskypassword");
        uriBuilder.Query = updatedQueryParams.ToString();
        request.RequestUri = uriBuilder.Uri;

        _logger.LogInformation("Forwarding request to: {Uri}", request.RequestUri);
        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            return HandleApiError(response);
        }

        _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);
        return response;
    }

    private HttpResponseMessage HandleApiError(HttpResponseMessage response)
    {
        var _logger = this.Context.Logger;
        var statusCode = response.StatusCode;

        switch (statusCode)
        {
            case HttpStatusCode.NotFound:
                _logger.LogError("404 Not Found: The requested resource does not exist.");
                return BuildErrorResponse(HttpStatusCode.NotFound, "The requested resource was not found.");

            case (HttpStatusCode)529:
                _logger.LogError("429 Too Many Requests: Rate limit exceeded.");
                return BuildErrorResponse((HttpStatusCode)529, "Too many requests. Try again later.");

            case HttpStatusCode.Forbidden:
                _logger.LogError("403 Forbidden: You do not have permission to access this resource.");
                return BuildErrorResponse(HttpStatusCode.Forbidden, "Access denied: You do not have permission.");

            case HttpStatusCode.BadGateway:
                _logger.LogError("502 Bad Gateway: Temporary API issue.");
                return BuildErrorResponse(HttpStatusCode.BadGateway, "Temporary server issue. Try again later.");

            default:
                _logger.LogError($"Unexpected error: {statusCode}");
                return BuildErrorResponse(statusCode, $"Unexpected API error: {statusCode}");
        }
    }

    private HttpResponseMessage BuildErrorResponse(HttpStatusCode statusCode, string message)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent($"{{ \"error\": \"{message}\" }}", Encoding.UTF8, "application/json")
        };
    }

    private class SessionResponse
    {
        [JsonProperty("accessJwt")]
        public string AccessJwt { get; set; }
    }
}
