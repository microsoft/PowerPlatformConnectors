public class Script : ScriptBase
{
    // The Wavelength API is one MCP endpoint (JSON-RPC 2.0). Each connector operation
    // has its own path in the OpenAPI definition. This script turns each operation into
    // a JSON-RPC "tools/call" request to /api/mcp and returns the tool result as clean JSON.

    private const string McpPath = "/api/mcp";
    private const string AppFlowUrl = "https://wavelengthflow.com/app/f/";
    private const int TriggerPageSize = 100;
    private const int RetryAfterSeconds = 60;

    private static readonly string[] KnownOperations = new[]
    {
        "ListFlows", "CreateFlowFromText", "SearchFlows", "GetFlow", "ExportFlow",
        "SummarizeFlow", "SendFlowToChannel", "ListChannels", "ListFolders", "OnFlowChanged"
    };

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        string op = ResolveOperationId(this.Context.OperationId);
        try
        {
            switch (op)
            {
                case "ListFlows": return await this.ListFlows().ConfigureAwait(false);
                case "CreateFlowFromText": return await this.CreateFlowFromText().ConfigureAwait(false);
                case "SearchFlows": return await this.SearchFlows().ConfigureAwait(false);
                case "GetFlow": return await this.GetFlow().ConfigureAwait(false);
                case "ExportFlow": return await this.ExportFlow().ConfigureAwait(false);
                case "SummarizeFlow": return await this.SummarizeFlow().ConfigureAwait(false);
                case "SendFlowToChannel": return await this.SendFlowToChannel().ConfigureAwait(false);
                case "ListChannels": return await this.PassThrough("list_channels", new JObject()).ConfigureAwait(false);
                case "ListFolders": return await this.PassThrough("list_folders", new JObject()).ConfigureAwait(false);
                case "OnFlowChanged": return await this.OnFlowChanged().ConfigureAwait(false);
                default: return ErrorResponse(HttpStatusCode.BadRequest, "unknown_operation", "Unknown operation ID '" + op + "'.");
            }
        }
        catch (ToolException ex)
        {
            return ErrorResponse(ex.Status, ex.Code, ex.Message);
        }
    }

    // In some regions the operation ID arrives base64 encoded. Accept the decoded value
    // only when it is a known operation, so a plain ID that is also valid base64 still works.
    private static string ResolveOperationId(string raw)
    {
        if (Array.IndexOf(KnownOperations, raw) >= 0) return raw;
        try
        {
            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(raw));
            if (Array.IndexOf(KnownOperations, decoded) >= 0) return decoded;
        }
        catch (FormatException) { }
        return raw;
    }

    // ---------- Operations ----------

    private async Task<HttpResponseMessage> ListFlows()
    {
        var q = Query();
        var args = new JObject();
        CopyString(q, "query", args, "query");
        CopyString(q, "folder_id", args, "folder_id");
        CopyInt(q, "limit", args, "limit");
        JObject result = await this.CallTool("list_flows", args).ConfigureAwait(false);
        return Ok(new JObject { ["flows"] = MapFlowList(result["flows"] as JArray) });
    }

    private async Task<HttpResponseMessage> CreateFlowFromText()
    {
        JObject body = await this.ReadBody().ConfigureAwait(false);
        string text = (string)body["text"];
        if (string.IsNullOrWhiteSpace(text))
        {
            return ErrorResponse(HttpStatusCode.BadRequest, "missing_text", "The Text parameter is required.");
        }
        var args = new JObject { ["text"] = text };
        foreach (string key in new[] { "title", "direction", "folder_id", "team_id", "org_id" })
        {
            string v = (string)body[key];
            if (!string.IsNullOrWhiteSpace(v)) args[key] = v;
        }
        JObject r = await this.CallTool("create_flow_from_text", args).ConfigureAwait(false);

        string flowId = FirstString(r, "flow_id", "new_flow_id", "id");
        JToken labels = r["node_labels"] ?? r["labels"] ?? r["nodes"];
        var labelArray = new JArray();
        if (labels is JArray)
        {
            foreach (JToken t in (JArray)labels)
            {
                if (t.Type == JTokenType.String) labelArray.Add(t);
                else if (t is JObject && t["label"] != null) labelArray.Add((string)t["label"]);
            }
        }
        var output = new JObject
        {
            ["flow_id"] = flowId,
            ["title"] = FirstString(r, "title"),
            ["node_count"] = FirstInt(r, "node_count", "nodes_count", "node_total") ?? (labels is JArray ? (int?)((JArray)labels).Count : null),
            ["edge_count"] = FirstInt(r, "edge_count", "edges_count", "edge_total"),
            ["node_labels"] = labelArray,
            ["open_url"] = FirstString(r, "open_url", "url", "link") ?? (flowId != null ? AppFlowUrl + flowId : null)
        };
        return Ok(RemoveNulls(output));
    }

    private async Task<HttpResponseMessage> SearchFlows()
    {
        var q = Query();
        string query = Get(q, "query");
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
        {
            return ErrorResponse(HttpStatusCode.BadRequest, "invalid_query", "The Search text must have at least 2 characters.");
        }
        var args = new JObject { ["query"] = query.Trim() };
        CopyInt(q, "limit", args, "limit");
        JObject r = await this.CallTool("search_flows", args).ConfigureAwait(false);
        var matches = new JArray();
        foreach (JToken m in (r["matches"] as JArray) ?? new JArray())
        {
            matches.Add(new JObject
            {
                ["id"] = m["id"],
                ["title"] = m["title"],
                ["updated_at"] = m["updated_at"],
                ["folder_id"] = m["folder_id"],
                ["shared"] = m["shared"],
                ["matched_in"] = m["matched_in"] ?? new JArray(),
                ["matched_nodes"] = m["matched_nodes"] ?? new JArray(),
                ["transcript_snippet"] = m["transcript_snippet"],
                ["open_url"] = m["open_url"]
            });
        }
        return Ok(new JObject
        {
            ["query"] = r["query"] ?? query,
            ["total"] = r["total"] ?? matches.Count,
            ["matches"] = matches
        });
    }

    private async Task<HttpResponseMessage> GetFlow()
    {
        string flowId = PathSegmentAfter("flows");
        var q = Query();
        var args = new JObject { ["flow_id"] = flowId };
        if (IsTrue(Get(q, "include_transcript"))) args["include_transcript"] = true;
        JObject r = await this.CallTool("get_flow", args).ConfigureAwait(false);
        r.Remove("flow");
        r["flow_id"] = flowId;
        return Ok(r);
    }

    private async Task<HttpResponseMessage> ExportFlow()
    {
        string flowId = PathSegmentAfter("flows");
        string format = Get(Query(), "format") ?? "mermaid";
        JObject r = await this.CallTool("export_flow", new JObject { ["flow_id"] = flowId, ["format"] = format }).ConfigureAwait(false);
        string content;
        if (format == "json")
        {
            var graph = new JObject { ["title"] = r["title"], ["graph_nodes"] = r["graph_nodes"], ["graph_edges"] = r["graph_edges"] };
            content = graph.ToString(Newtonsoft.Json.Formatting.None);
        }
        else
        {
            content = (string)r[format];
        }
        return Ok(new JObject { ["flow_id"] = flowId, ["format"] = format, ["content"] = content });
    }

    private async Task<HttpResponseMessage> SummarizeFlow()
    {
        string flowId = PathSegmentAfter("flows");
        var q = Query();
        var args = new JObject { ["flow_id"] = flowId, ["format"] = Get(q, "format") ?? "systems_in_scope" };
        CopyString(q, "audience", args, "audience");
        JObject r = await this.CallTool("summarize_flow", args).ConfigureAwait(false);
        return Ok(new JObject { ["flow_id"] = flowId, ["format"] = r["format"], ["text"] = r["text"] });
    }

    private async Task<HttpResponseMessage> SendFlowToChannel()
    {
        string flowId = PathSegmentAfter("flows");
        JObject body = await this.ReadBody().ConfigureAwait(false);
        var args = new JObject { ["flow_id"] = flowId, ["channel_id"] = (string)body["channel_id"] };
        if (body["summary_lines"] is JArray && ((JArray)body["summary_lines"]).Count > 0)
        {
            args["summary_lines"] = body["summary_lines"];
        }
        JObject r = await this.CallTool("send_flow_to_channel", args).ConfigureAwait(false);
        return Ok(new JObject
        {
            ["flow_id"] = r["flow_id"] ?? flowId,
            ["channel_id"] = r["channel_id"] ?? args["channel_id"],
            ["sent"] = r["sent"],
            ["status"] = r["status"],
            ["link"] = r["link"]
        });
    }

    private async Task<HttpResponseMessage> PassThrough(string tool, JObject args)
    {
        JObject r = await this.CallTool(tool, args).ConfigureAwait(false);
        return Ok(r);
    }

    // Polling trigger. The "since" query value is the trigger state.
    // First run: no state. Save the newest updated_at as the state and do not fire.
    // Later runs: return flows with updated_at after the state, newest state in Location.
    private async Task<HttpResponseMessage> OnFlowChanged()
    {
        var q = Query();
        var args = new JObject { ["limit"] = TriggerPageSize };
        CopyString(q, "folder_id", args, "folder_id");
        JObject r = await this.CallTool("list_flows", args).ConfigureAwait(false);
        JArray flows = MapFlowList(r["flows"] as JArray);

        DateTimeOffset newest = DateTimeOffset.MinValue;
        foreach (JToken f in flows)
        {
            DateTimeOffset t;
            if (TryParseTime(f["updated_at"], out t) && t > newest) newest = t;
        }

        string sinceRaw = Get(q, "since");
        DateTimeOffset since;
        if (string.IsNullOrWhiteSpace(sinceRaw) || !DateTimeOffset.TryParse(sinceRaw, out since))
        {
            DateTimeOffset start = newest == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : newest;
            return this.TriggerResponse(HttpStatusCode.Accepted, null, start);
        }

        var changed = new JArray();
        foreach (JToken f in flows)
        {
            DateTimeOffset t;
            if (TryParseTime(f["updated_at"], out t) && t > since) changed.Add(f);
        }
        if (changed.Count == 0)
        {
            return this.TriggerResponse(HttpStatusCode.Accepted, null, since);
        }
        // Oldest first, so runs start in the order of the changes.
        var ordered = new JArray(changed.OrderBy(f => { DateTimeOffset t; TryParseTime(f["updated_at"], out t); return t; }));
        return this.TriggerResponse(HttpStatusCode.OK, ordered, newest > since ? newest : since);
    }

    private HttpResponseMessage TriggerResponse(HttpStatusCode status, JArray items, DateTimeOffset state)
    {
        var response = new HttpResponseMessage(status);
        if (items != null) response.Content = CreateJsonContent(items.ToString(Newtonsoft.Json.Formatting.None));
        var uri = new UriBuilder(this.Context.Request.RequestUri);
        var query = HttpUtility.ParseQueryString(uri.Query);
        query["since"] = state.ToUniversalTime().ToString("o");
        uri.Query = query.ToString();
        response.Headers.Location = uri.Uri;
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(RetryAfterSeconds));
        return response;
    }

    // ---------- MCP call ----------

    private async Task<JObject> CallTool(string tool, JObject arguments)
    {
        var rpc = new JObject
        {
            ["jsonrpc"] = "2.0",
            ["id"] = 1,
            ["method"] = "tools/call",
            ["params"] = new JObject { ["name"] = tool, ["arguments"] = arguments }
        };

        Uri incoming = this.Context.Request.RequestUri;
        var target = new UriBuilder(incoming.Scheme, incoming.Host, incoming.Port, McpPath).Uri;
        var request = new HttpRequestMessage(HttpMethod.Post, target);
        if (this.Context.Request.Headers.Authorization != null)
        {
            request.Headers.Authorization = this.Context.Request.Headers.Authorization;
        }
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        request.Content = CreateJsonContent(rpc.ToString(Newtonsoft.Json.Formatting.None));

        HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        string raw = response.Content == null ? "" : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ToolException(HttpStatusCode.Unauthorized, "unauthorized", "The Wavelength sign-in expired. Fix the connection and try again.");
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new ToolException(response.StatusCode, "http_error", "The Wavelength API returned HTTP " + (int)response.StatusCode + ".");
        }

        JObject envelope = ParseRpc(raw, response.Content != null && response.Content.Headers.ContentType != null ? response.Content.Headers.ContentType.MediaType : null);
        if (envelope["error"] != null)
        {
            throw new ToolException(HttpStatusCode.BadRequest, "rpc_error", (string)envelope["error"]["message"] ?? "The Wavelength API returned an error.");
        }
        var result = envelope["result"] as JObject;
        if (result == null)
        {
            throw new ToolException(HttpStatusCode.BadGateway, "empty_result", "The Wavelength API returned no result.");
        }
        string text = null;
        var content = result["content"] as JArray;
        if (content != null && content.Count > 0) text = (string)content[0]["text"];

        if (result["isError"] != null && (bool)result["isError"])
        {
            throw new ToolException(HttpStatusCode.BadRequest, "tool_error", string.IsNullOrEmpty(text) ? "The Wavelength tool returned an error." : text);
        }

        // content[0].text holds the tool result as JSON. structuredContent is the fallback.
        if (!string.IsNullOrEmpty(text))
        {
            try
            {
                JToken parsed = ParseJson(text);
                if (parsed is JObject) return (JObject)parsed;
                return new JObject { ["result"] = parsed };
            }
            catch (JsonReaderException) { }
        }
        var structured = result["structuredContent"] as JObject;
        if (structured != null)
        {
            structured.Remove("flow");
            return structured;
        }
        return new JObject { ["result"] = text };
    }

    // Accepts a JSON body or a Server-Sent Events body with "data:" lines.
    private static JObject ParseRpc(string raw, string mediaType)
    {
        string trimmed = (raw ?? "").Trim();
        if (mediaType == "text/event-stream" || trimmed.StartsWith("event:") || trimmed.StartsWith("data:"))
        {
            JObject last = null;
            foreach (string line in trimmed.Split('\n'))
            {
                string l = line.Trim();
                if (!l.StartsWith("data:")) continue;
                try
                {
                    var obj = ParseJson(l.Substring(5).Trim()) as JObject;
                    if (obj == null) continue;
                    if (obj["result"] != null || obj["error"] != null) last = obj;
                }
                catch (JsonReaderException) { }
            }
            if (last != null) return last;
            throw new ToolException(HttpStatusCode.BadGateway, "bad_stream", "The Wavelength API returned a stream with no result.");
        }
        try
        {
            var envelope = ParseJson(trimmed) as JObject;
            if (envelope != null) return envelope;
            throw new ToolException(HttpStatusCode.BadGateway, "bad_json", "The Wavelength API returned a response that is not a JSON object.");
        }
        catch (JsonReaderException)
        {
            throw new ToolException(HttpStatusCode.BadGateway, "bad_json", "The Wavelength API returned a response that is not JSON.");
        }
    }

    // ---------- Helpers ----------

    // Keeps date strings as they are. The default parser turns them into DateTime values.
    private static JToken ParseJson(string json)
    {
        using (var reader = new JsonTextReader(new StringReader(json)) { DateParseHandling = DateParseHandling.None })
        {
            return JToken.ReadFrom(reader);
        }
    }

    private static JArray MapFlowList(JArray flows)
    {
        var list = new JArray();
        if (flows == null) return list;
        foreach (JToken f in flows)
        {
            list.Add(new JObject
            {
                ["id"] = f["id"],
                ["title"] = f["title"],
                ["updated_at"] = f["updated_at"],
                ["folder_id"] = f["folder_id"],
                ["is_public"] = f["is_public"],
                ["shared"] = f["shared"],
                ["role"] = f["role"],
                ["open_url"] = f["open_url"]
            });
        }
        return list;
    }

    private Dictionary<string, string> Query()
    {
        var parsed = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string key in parsed.AllKeys)
        {
            if (key != null) dict[key] = parsed[key];
        }
        return dict;
    }

    private async Task<JObject> ReadBody()
    {
        if (this.Context.Request.Content == null) return new JObject();
        string s = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(s)) return new JObject();
        try { var o = ParseJson(s) as JObject; return o ?? new JObject(); }
        catch (JsonReaderException) { throw new ToolException(HttpStatusCode.BadRequest, "bad_body", "The request body is not valid JSON."); }
    }

    private string PathSegmentAfter(string segment)
    {
        string[] parts = this.Context.Request.RequestUri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parts.Length - 1; i++)
        {
            if (parts[i] == segment) return Uri.UnescapeDataString(parts[i + 1]);
        }
        throw new ToolException(HttpStatusCode.BadRequest, "missing_flow_id", "The Flow ID parameter is required.");
    }

    private static string Get(Dictionary<string, string> q, string key)
    {
        string v;
        return q.TryGetValue(key, out v) ? v : null;
    }

    private static void CopyString(Dictionary<string, string> q, string from, JObject args, string to)
    {
        string v = Get(q, from);
        if (!string.IsNullOrWhiteSpace(v)) args[to] = v;
    }

    private static void CopyInt(Dictionary<string, string> q, string from, JObject args, string to)
    {
        int n;
        if (int.TryParse(Get(q, from), out n)) args[to] = n;
    }

    private static bool IsTrue(string v)
    {
        return string.Equals(v, "true", StringComparison.OrdinalIgnoreCase) || v == "1";
    }

    private static string FirstString(JObject o, params string[] keys)
    {
        foreach (string k in keys)
        {
            JToken t = o[k];
            if (t != null && t.Type != JTokenType.Null && t.Type != JTokenType.Object && t.Type != JTokenType.Array) return (string)t;
        }
        return null;
    }

    private static int? FirstInt(JObject o, params string[] keys)
    {
        foreach (string k in keys)
        {
            JToken t = o[k];
            if (t != null && t.Type == JTokenType.Integer) return (int)t;
        }
        return null;
    }

    private static bool TryParseTime(JToken token, out DateTimeOffset value)
    {
        value = DateTimeOffset.MinValue;
        if (token == null || token.Type == JTokenType.Null) return false;
        if (token.Type == JTokenType.Date)
        {
            value = token.ToObject<DateTimeOffset>();
            return true;
        }
        return DateTimeOffset.TryParse((string)token, out value);
    }

    // Drops null values, so an action output never has a null where the schema expects a number.
    private static JObject RemoveNulls(JObject o)
    {
        foreach (var p in o.Properties().ToList())
        {
            if (p.Value == null || p.Value.Type == JTokenType.Null) p.Remove();
        }
        return o;
    }

    private static HttpResponseMessage Ok(JObject body)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(body.ToString(Newtonsoft.Json.Formatting.None));
        return response;
    }

    private static HttpResponseMessage ErrorResponse(HttpStatusCode status, string code, string message)
    {
        var response = new HttpResponseMessage(status);
        response.Content = CreateJsonContent(new JObject
        {
            ["error"] = new JObject { ["code"] = code, ["message"] = message }
        }.ToString(Newtonsoft.Json.Formatting.None));
        return response;
    }

    private class ToolException : Exception
    {
        public HttpStatusCode Status { get; private set; }
        public string Code { get; private set; }
        public ToolException(HttpStatusCode status, string code, string message) : base(message)
        {
            this.Status = status;
            this.Code = code;
        }
    }
}
