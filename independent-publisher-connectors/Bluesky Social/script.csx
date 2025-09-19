//Script provided by Troy Taylor MVP, troystaylor: https://github.com/troystaylor/Bluesky-Connector/blob/main/script.csx
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var _logger = this.Context.Logger;
        var request = this.Context.Request;
        var operationId = this.Context.OperationId;
        _logger.LogInformation("Received request: {Method} {Uri} {OperationId}", request.Method, request.RequestUri, operationId);

        var queryParams = HttpUtility.ParseQueryString(request.RequestUri.Query);
        var pdsHost = queryParams["pdshost"];
        var blueskyID = queryParams["identifier"];
        var blueskyPassword = queryParams["password"];
        _logger.LogInformation("Extracted query parameters: pdshost={pdsHost}, blueskyid={blueskyID}", pdsHost, blueskyID);

        var sessionURL = $"https://{pdsHost}/xrpc/com.atproto.server.createSession";
        _logger.LogInformation("Session URL: {SessionURL}", sessionURL);

        var requestBodyDictionary = new Dictionary<string, string>
        {
            { "identifier", blueskyID },
            { "password", blueskyPassword },
            { "pdshost", pdsHost },
        };

        var json = JsonConvert.SerializeObject(requestBodyDictionary);
        _logger.LogInformation("Session request body: {Json}", json);

        var sessionRequest = new HttpRequestMessage(HttpMethod.Post, sessionURL)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var sessionResponse = await this.Context.SendAsync(sessionRequest, this.CancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Received session response: {StatusCode}", sessionResponse.StatusCode);

        if (string.Equals(operationId, "CreateSession", StringComparison.OrdinalIgnoreCase))
        {
            return sessionResponse;
        }

        var sessionContent = await sessionResponse.Content.ReadAsStringAsync();
        _logger.LogInformation("Session response content: {SessionContent}", sessionContent);

        var sessionData = JsonConvert.DeserializeObject<Dictionary<string, object>>(sessionContent);
        var accessJwt = sessionData["accessJwt"].ToString();
        _logger.LogInformation("Extracted accessJwt: {AccessJwt}", accessJwt);

        this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessJwt);

        var uriBuilder = new UriBuilder(request.RequestUri)
        {
            Host = pdsHost
        };
        var updatedQueryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
        updatedQueryParams.Remove("pdshost");
        updatedQueryParams.Remove("identifier");
        updatedQueryParams.Remove("password");
        uriBuilder.Query = updatedQueryParams.ToString();
        this.Context.Request.RequestUri = uriBuilder.Uri;

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);
        return response;
    }
}