using System;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    private static readonly HashSet<string> LegacyAuthOperations = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "GetCaseFileDetails",
        "DownloadDocument"
    };

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.Request.Headers.TryGetValues("Authorization", out var authHeaderValues))
        {
            var authHeader = authHeaderValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader))
            {
                var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? authHeader.Substring(7)
                    : authHeader;

                this.Context.Request.Headers.Remove("Authorization");
                this.Context.Request.Headers.Add("X-Auth-Token", token);

                if (LegacyAuthOperations.Contains(this.Context.OperationId))
                {
                    this.Context.Request.Headers.TryAddWithoutValidation("Authorization", "JWT");
                }
            }
        }

        var returnPdf = false;

        if ("DownloadDocument".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            returnPdf = "pdf".Equals(query["format"], StringComparison.OrdinalIgnoreCase);

            // "format" is handled here, Penneo always returns the base64 encoded JSON representation.
            this.Context.Request.RequestUri = RemoveQueryParameter(this.Context.Request.RequestUri, "format");
        }

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (returnPdf && response.IsSuccessStatusCode)
        {
            response = await ConvertToPdfResponse(response).ConfigureAwait(continueOnCapturedContext: false);
        }

        return response;
    }

    private async Task<HttpResponseMessage> ConvertToPdfResponse(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

        string content;
        try
        {
            content = (string)JObject.Parse(responseString)["content"];
        }
        catch (Exception)
        {
            return response;
        }

        if (string.IsNullOrEmpty(content))
        {
            return response;
        }

        byte[] pdf;
        try
        {
            pdf = Convert.FromBase64String(content);
        }
        catch (FormatException)
        {
            return response;
        }

        response.Content = new ByteArrayContent(pdf);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        return response;
    }

    private static Uri RemoveQueryParameter(Uri uri, string name)
    {
        var query = HttpUtility.ParseQueryString(uri.Query);

        if (query[name] == null)
        {
            return uri;
        }

        query.Remove(name);

        return new UriBuilder(uri) { Query = query.ToString() }.Uri;
    }
}
