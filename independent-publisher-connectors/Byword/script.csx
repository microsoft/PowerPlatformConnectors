using Newtonsoft.Json;
using System.Collections.Generic;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var request = this.Context.Request;
        var query = new Uri(request.RequestUri.AbsoluteUri).Query;
        var queryParams = HttpUtility.ParseQueryString(query);
        var key = queryParams["key"];

        var requestBody = await request.Content.ReadAsStringAsync();
        var requestBodyDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestBody);

        var jsonObject = new Dictionary<string, string>
        {
            { "key", key }
        };

        foreach (var kvp in requestBodyDictionary)
        {
            jsonObject[kvp.Key] = kvp.Value;
        }

        var json = JsonConvert.SerializeObject(jsonObject);
        var uriBuilder = new UriBuilder(request.RequestUri);
        var queryParameters = HttpUtility.ParseQueryString(uriBuilder.Query);
        queryParameters.Remove("key");
        uriBuilder.Query = queryParameters.ToString();
        var modifiedUri = uriBuilder.Uri;

        var modifiedRequest = new HttpRequestMessage(request.Method, modifiedUri)
        {
            Content = new StringContent(json, Encoding.UTF8, request.Content.Headers.ContentType.MediaType)
        };

        var response = await this.Context.SendAsync(modifiedRequest, this.CancellationToken).ConfigureAwait(false);
        return response;
    }
}