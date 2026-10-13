using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    private const string CaseFileOrigin = "microsoft";

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

        if ("CreateCaseFile".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            await SetCaseFileOrigin().ConfigureAwait(continueOnCapturedContext: false);
        }

        var returnFile = false;

        if ("DownloadDocument".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            var format = query["format"];
            // "pdf" remains accepted so flows saved before the file option keep returning the raw file.
            returnFile = "file".Equals(format, StringComparison.OrdinalIgnoreCase)
                || "pdf".Equals(format, StringComparison.OrdinalIgnoreCase);

            // "format" is handled here. Penneo returns the base64 encoded JSON representation,
            // and the bytes themselves say whether the file is a PDF or a ZIP.
            this.Context.Request.RequestUri = RemoveQueryParameter(this.Context.Request.RequestUri, "format");
        }

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (returnFile && response.IsSuccessStatusCode)
        {
            response = await ConvertToFileResponse(response).ConfigureAwait(continueOnCapturedContext: false);
        }

        return response;
    }

    private async Task SetCaseFileOrigin()
    {
        if (this.Context.Request.Content == null)
        {
            return;
        }

        var body = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

        JObject payload;
        try
        {
            // DateParseHandling.None keeps date-like strings (titles, metadata) byte-for-byte on re-serialization.
            using (var reader = new JsonTextReader(new StringReader(body)) { DateParseHandling = DateParseHandling.None })
            {
                payload = JObject.Load(reader);
            }
        }
        catch (JsonReaderException)
        {
            return;
        }

        if (!(payload["caseFile"] is JObject caseFile))
        {
            return;
        }

        caseFile["origin"] = CaseFileOrigin;

        this.Context.Request.Content = new StringContent(payload.ToString(Newtonsoft.Json.Formatting.None), Encoding.UTF8, "application/json");
    }

    private async Task<HttpResponseMessage> ConvertToFileResponse(HttpResponseMessage response)
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

        byte[] file;
        try
        {
            file = Convert.FromBase64String(content);
        }
        catch (FormatException)
        {
            return response;
        }

        response.Content = new ByteArrayContent(file);
        response.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeFor(file));

        return response;
    }

    private static string MediaTypeFor(byte[] content)
    {
        if (StartsWithAscii(content, "%PDF"))
        {
            return "application/pdf";
        }

        if (StartsWithAscii(content, "PK"))
        {
            return "application/zip";
        }

        return "application/octet-stream";
    }

    private static bool StartsWithAscii(byte[] content, string prefix)
    {
        if (content == null || content.Length < prefix.Length)
        {
            return false;
        }

        for (var i = 0; i < prefix.Length; i++)
        {
            if (content[i] != (byte)prefix[i])
            {
                return false;
            }
        }

        return true;
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
