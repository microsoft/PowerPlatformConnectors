using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Custom code for the OFAC Sanctions List connector.
//
// DownloadSanctionsFile and GetLatestChanges are answered by the Sanctions List
// Service with an HTTP 302 to a time-limited storage URL on a different host
// (Amazon S3, US GovCloud). The Power Platform connector runtime does not follow
// a cross-host redirect on its own, so this script follows it and returns the
// file contents. Every other operation is forwarded unchanged.
//
// The script never returns a 3xx and never throws out of ExecuteAsync: any
// problem comes back as a 200 with a JSON body that carries a short trace, for
// diagnosis from the Test tab.
public class Script : ScriptBase
{
    private static readonly string[] KnownOperations =
    {
        "ListSanctionsLists",
        "ListSanctionsPrograms",
        "DownloadSanctionsFile",
        "GetLatestChanges",
        "GetChangeHistory",
    };

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var trace = new List<string>();
        var operationId = this.ResolveOperationId();
        trace.Add("op=" + operationId);

        try
        {
            if (operationId == "DownloadSanctionsFile" || operationId == "GetLatestChanges")
            {
                return await this.HandleRedirectingDownload(operationId, trace).ConfigureAwait(false);
            }

            if (operationId == "GetChangeHistory")
            {
                return await this.HandleChangeHistory(trace).ConfigureAwait(false);
            }

            trace.Add("forwarded");
            return await this.Context
                .SendAsync(this.Context.Request, this.CancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            trace.Add("exception=" + ex.GetType().Name + ":" + ex.Message);
            return JsonResult(new JObject
            {
                ["error"] = "custom code failed",
                ["detail"] = ex.Message,
                ["trace"] = string.Join(" | ", trace),
            });
        }
    }

    // The runtime usually hands back the plain operation id, but in some regions
    // it is base64 encoded. Only treat it as base64 when the decoded value is one
    // of our known operations - several of our ids are themselves valid base64.
    private string ResolveOperationId()
    {
        var id = this.Context.OperationId ?? string.Empty;

        if (KnownOperations.Contains(id))
        {
            return id;
        }

        try
        {
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(id));
            if (KnownOperations.Contains(decoded))
            {
                return decoded;
            }
        }
        catch (FormatException)
        {
            // not base64 - fall through
        }

        return id;
    }

    // /changes/history returns a top-level JSON array. Power Platform rejects a
    // top-level array as a response body, so wrap it as { "publications": [ ... ] }.
    private async Task<HttpResponseMessage> HandleChangeHistory(List<string> trace)
    {
        var response = await this.Context
            .SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(false);
        trace.Add("history=" + (int)response.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            return response;
        }

        var raw = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        response.Dispose();

        JArray publications;
        try
        {
            publications = string.IsNullOrWhiteSpace(raw) ? new JArray() : JArray.Parse(raw);
        }
        catch (JsonException)
        {
            trace.Add("history-body-not-array");
            publications = new JArray();
        }

        return JsonResult(new JObject { ["publications"] = publications });
    }

    private async Task<HttpResponseMessage> HandleRedirectingDownload(string operationId, List<string> trace)
    {
        var response = await this.Context
            .SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(false);
        trace.Add("hop0=" + (int)response.StatusCode);

        for (var hop = 1; hop <= 5; hop++)
        {
            var status = (int)response.StatusCode;
            if (status < 300 || status >= 400)
            {
                break;
            }

            var target = GetLocation(response);
            if (target == null)
            {
                trace.Add("no-location");
                break;
            }

            trace.Add("follow" + hop + "->" + target.Host);
            response.Dispose();

            using (var next = new HttpRequestMessage(HttpMethod.Get, target))
            {
                response = await this.Context.SendAsync(next, this.CancellationToken).ConfigureAwait(false);
            }
            trace.Add("hop" + hop + "=" + (int)response.StatusCode);
        }

        var finalStatus = (int)response.StatusCode;

        if (finalStatus >= 300 && finalStatus < 400)
        {
            var loc = GetLocation(response);
            response.Dispose();
            return JsonResult(new JObject
            {
                ["error"] = "redirect not resolved",
                ["lastStatus"] = finalStatus,
                ["lastLocation"] = loc?.AbsoluteUri,
                ["trace"] = string.Join(" | ", trace),
            });
        }

        if (!response.IsSuccessStatusCode)
        {
            trace.Add("storage-not-ok=" + finalStatus);
            return response;
        }

        var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        var mediaType = response.Content.Headers.ContentType?.MediaType;
        response.Dispose();

        var result = new HttpResponseMessage(HttpStatusCode.OK);

        if (operationId == "GetLatestChanges")
        {
            // Return the delta document as a JSON string so it satisfies the
            // { "type": "string" } response schema; parse it in a flow with xml().
            result.Content = CreateJsonContent(JsonConvert.SerializeObject(Encoding.UTF8.GetString(bytes)));
        }
        else
        {
            result.Content = new ByteArrayContent(bytes);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(
                string.IsNullOrEmpty(mediaType) ? "application/octet-stream" : mediaType);
        }

        return result;
    }

    private static Uri GetLocation(HttpResponseMessage response)
    {
        if (response.Headers.Location != null)
        {
            return response.Headers.Location;
        }

        if (response.Headers.TryGetValues("Location", out var values))
        {
            var raw = values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
            if (raw != null && Uri.TryCreate(raw, UriKind.Absolute, out var parsed))
            {
                return parsed;
            }
        }

        return null;
    }

    private static HttpResponseMessage JsonResult(JObject body)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = CreateJsonContent(body.ToString()),
        };
    }
}
