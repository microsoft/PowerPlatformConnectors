using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Power Automate connector script
/// </summary>
public class Script : ScriptBase
{
    /// <summary>
    ///EqualsSeparator
    /// </summary>
    private static readonly char[] EqualsSeparator = new[] { '=' };
    private static readonly string SegmentPatternWithRecord = "Record";
    private static readonly string ServiceDomain = "siw_api/";
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (Context!.OperationId == "RawRequest")
        {
            return await HandleRawRequestAsync().ConfigureAwait(false);
        }

        return await HandleNonRawRequestsAsync().ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> HandleRawRequestAsync()
    {
        var request = Context!.Request;
        if(request?.Content == null)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Content not defined on request")
            };
        }
        var content = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

        var obj = JObject.Parse(content);
        var body = GetValue<string>(obj, "body");

        var verb = GetValue<string>(obj, "verb");

        var url = $"{request.RequestUri}{ServiceDomain}{GetValue<string>(obj, "relativeUrl")?.TrimStart('/')}";

        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent($"Invalid Url Format '{url}'")
            };
        }

        if (!string.IsNullOrEmpty(body))
        {
            request.Content = CreateJsonContent(body);
        }

        if (string.IsNullOrEmpty(verb))
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Verb must be provided.")
            };
        }

        url = AppendQueryParamsRawRequest(url, obj);

        request.Method = new HttpMethod(verb);
        request.RequestUri = new Uri(url);

        foreach (var token in GetValue<JArray>(obj, "headers") ?? new JArray())
        {
            var item = token.Value<JObject>();

            var key = GetValue<string>(item, "headerKey");
            if (key == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = CreateJsonContent("Headers are invalid.")
                };
            }

            var value = GetValue<string>(item, "headerValue");
            // Need to to add without validation because the etag causes it to fail
            request.Headers.TryAddWithoutValidation(key, value);
        }

        return await Context.SendAsync(request, CancellationToken).ConfigureAwait(false);
    }

    private static string AppendQueryParamsRawRequest(string url, JObject obj)
    {
        var tokens = GetValue<JArray>(obj, "query") ?? new JArray();
        if(tokens.Count > 0)
        {
            url += "?";
        }

        foreach (var token in tokens)
        {
            var item = token.Value<JObject>();

            var key = GetValue<string>(item, "queryKey");
            var value = GetValue<string>(item, "queryValue");

            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(key))
            {
                url += $"{key}={value}&";
            }
        }

        if (url.EndsWith("&"))
        {
            url = url.TrimEnd('&');
        }

        if (url.EndsWith("?"))
        {
            url = url.TrimEnd('?');
        }

        return url;
    }

    private async Task<HttpResponseMessage> HandleNonRawRequestsAsync()
    {
        var request = Context?.Request;
        if (request == null)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Request context is not available.")
            };
        }

        var uri = request.RequestUri;
        if (uri == null)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Request URI is not available.")
            };
        }

        try
        {
            var queryParams = ExtractQueryParams(uri);
            var verb = GetLastSegment(uri);
            var route = DecodeQueryParam(queryParams, "route");
            var version = DecodeQueryParam(queryParams, "version");

            var url = BuildUrl(uri, route);

            var content = await request.Content!.ReadAsStringAsync().ConfigureAwait(false);
            var obj = JObject.Parse(content);

            url = ReplaceRouteValues(url, obj);

            url = AppendQueryParams(url, obj);

            AddHeadersToRequest(request, obj, version);

            request.RequestUri = new Uri(url);
            request.Method = new HttpMethod(verb);

            var body = GetValue<JObject>(obj, "body");

            request.Content = CreateJsonContent(JsonConvert.SerializeObject(body));
        }
        catch (InvalidOperationException ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent(ex.Message)
            };
        }

        return await Context!.SendAsync(request, CancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// ExtractQueryParams
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    private static Dictionary<string, string> ExtractQueryParams(Uri uri)
    {
        if (uri == null || string.IsNullOrEmpty(uri.Query))
        {
            return new Dictionary<string, string>();
        }

        return uri.Query.TrimStart('?')
        .Split('&')
        .Where(part => !string.IsNullOrEmpty(part) && part.Contains('='))
        .Select(part => part.Split(EqualsSeparator, 2))
        .ToDictionary(
            part => part[0],
            part => part.Length > 1 ? part[1] : string.Empty,
            StringComparer.OrdinalIgnoreCase);
    }
    /// <summary>
    /// GetLastSegment
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    private static string GetLastSegment(Uri uri)
    {
        if (uri.Segments.Length == 0)
        {
            throw new InvalidOperationException("The URI does not contain any segments.");
        }
        string lastSegment = uri.Segments[uri.Segments.Length - 1];

        if (lastSegment.Contains(SegmentPatternWithRecord))
        {
            return lastSegment.Split(new[] { SegmentPatternWithRecord }, StringSplitOptions.None)[0];
        }

        throw new InvalidOperationException("The last segment does not match the expected pattern.");
    }
    /// <summary>
    /// DecodeQueryParam
    /// </summary>
    /// <param name="queryParams"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static string DecodeQueryParam(Dictionary<string, string>? queryParams, string key)
    {
        return HttpUtility.UrlDecode(queryParams?[key])!;
    }
    /// <summary>
    /// BuildUrl
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="route"></param>
    /// <returns></returns>
    private static string BuildUrl(Uri uri, string route)
    {
        if (uri.Segments.Length == 0)
        {
            throw new InvalidOperationException("The URI does not contain any segments.");
        }
        string lastSegment = uri.Segments[uri.Segments.Length - 1];
        return uri.ToString()
                .Replace(lastSegment, ServiceDomain + route?.TrimStart('/'))
                .Split('?')[0];
    }
    /// <summary>
    /// ReplaceRouteValues
    /// </summary>
    /// <param name="part"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static string ReplaceRouteValues(string part, JObject obj)
    {
        part = HttpUtility.UrlDecode(part);

        var routeValues = GetValue<JObject>(obj, "Path") ?? new JObject();

        foreach (var routeValue in routeValues)
        {
            part = part.Replace($"{{{routeValue.Key}}}", routeValue.Value?.ToString());
        }
        return part;
    }

    /// <summary>
    /// AppendQueryParams
    /// </summary>
    /// <param name="part"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static string AppendQueryParams(string part, JObject obj)
    {
        part += "?";
        var queries = GetValue<JObject>(obj, "Query") ?? new JObject();
        foreach (var query in queries)
        {
            part += $"{query.Key}={query.Value}&";
        }

        if (part.EndsWith("&"))
        {
            part = part.TrimEnd('&');
        }

        return part;
    }

    /// <summary>
    /// AddHeadersToRequest
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="obj">Body</param>
    /// <param name="version">Version</param>
    private static void AddHeadersToRequest(HttpRequestMessage request, JObject obj, string version)
    {
        var headers = GetValue<JObject>(obj, "Header") ?? new JObject();
        foreach (var header in headers)
        {
            var value = header.Value?.Value<string>() ?? string.Empty;
            request.Headers.TryAddWithoutValidation(header.Key, value);
        }
        request.Headers.TryAddWithoutValidation("version", version);
    }
    /// <summary>
    /// GetValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static T? GetValue<T>(JObject? obj, string key) where T : class
    {
        return obj?.GetValue(key, StringComparison.InvariantCulture)?.Value<T>();
    }

}
