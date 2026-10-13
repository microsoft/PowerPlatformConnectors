using System;
using System.IO;
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

        if ("DownloadDocument".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            var format = query["format"];

            // format is a connector choice, not a Penneo query parameter.
            this.Context.Request.RequestUri = RemoveQueryParameter(this.Context.Request.RequestUri, "format");

            // json asks for the base64 JSON body. Anything else, including "default",
            // sends no Accept header so Penneo returns the file with its own content type.
            this.Context.Request.Headers.Remove("Accept");
            if (string.IsNullOrEmpty(format) || "json".Equals(format, StringComparison.OrdinalIgnoreCase))
            {
                this.Context.Request.Headers.TryAddWithoutValidation("Accept", "application/json");
            }
        }

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
