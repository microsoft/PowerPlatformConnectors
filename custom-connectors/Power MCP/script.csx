using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    private JObject GetServerInfo() => new JObject
    {
        ["name"] = "power-mcp-server",
        ["version"] = "1.0.0"
        // Optional fields you can add:
        // ["title"] = "My MCP Server",
        // ["description"] = "Description of what this server does",
        // ["websiteUrl"] = "https://example.com"
    };

    private JObject GetServerCapabilities() => new JObject
    {
        ["tools"] = new JObject { ["listChanged"] = false },
        ["resources"] = new JObject { ["subscribe"] = false, ["listChanged"] = false },
        ["prompts"] = new JObject { ["listChanged"] = false },
        ["logging"] = new JObject(),
        ["completions"] = new JObject()
    };

    private string _logLevel = "info";

    private JArray GetDefinedTools() => new JArray
    {
        // Example tool definition:
        // new JObject
        // {
        //     ["name"] = "get_weather",
        //     ["description"] = "Gets current weather for a location",
        //     ["inputSchema"] = new JObject
        //     {
        //         ["type"] = "object",
        //         ["properties"] = new JObject
        //         {
        //             ["location"] = new JObject
        //             {
        //                 ["type"] = "string",
        //                 ["description"] = "City name or coordinates"
        //             }
        //         },
        //         ["required"] = new JArray { "location" }
        //     }
        // }
    };

    private async Task<JObject> ExecuteToolByName(string toolName, JObject arguments)
    {
        return toolName switch
        {
            // Add your tool handlers here:
            // "get_weather" => await ExecuteGetWeatherTool(arguments),
            // "search_data" => await ExecuteSearchDataTool(arguments),
            _ => throw new Exception($"Unknown tool: {toolName}")
        };
    }

    // Example tool implementation:
    // private async Task<JObject> ExecuteGetWeatherTool(JObject arguments)
    // {
    //     var location = arguments?["location"]?.ToString() 
    //         ?? throw new ArgumentException("location is required");
    //     
    //     var request = new HttpRequestMessage(HttpMethod.Get, 
    //         $"https://api.weather.com/current?location={Uri.EscapeDataString(location)}");
    //     
    //     var response = await this.Context.SendAsync(request, this.CancellationToken);
    //     var body = await response.Content.ReadAsStringAsync();
    //     
    //     if (!response.IsSuccessStatusCode)
    //         throw new HttpRequestException($"Weather API error: {response.StatusCode}");
    //     
    //     return JObject.Parse(body);
    // }

    private JArray GetDefinedResources() => new JArray
    {
        // Example resource:
        // new JObject
        // {
        //     ["uri"] = "https://api.example.com/config.json",
        //     ["name"] = "Configuration",
        //     ["description"] = "Application configuration settings",
        //     ["mimeType"] = "application/json"
        // }
    };

    private JArray GetDefinedResourceTemplates() => new JArray
    {
        // Example resource template (RFC 6570 URI template):
        // new JObject
        // {
        //     ["uriTemplate"] = "https://api.example.com/users/{userId}",
        //     ["name"] = "User Profile",
        //     ["description"] = "Access user profile by ID",
        //     ["mimeType"] = "application/json"
        // }
    };

    private JArray GetDefinedPrompts() => new JArray
    {
        // Example prompt:
        // new JObject
        // {
        //     ["name"] = "analyze_data",
        //     ["description"] = "Analyze data with specified format",
        //     ["arguments"] = new JArray
        //     {
        //         new JObject { ["name"] = "source", ["description"] = "Data source", ["required"] = true },
        //         new JObject { ["name"] = "format", ["description"] = "Output format", ["required"] = false }
        //     }
        // }
    };

    // ═══════════════════════════════════════════════════════════════════════════
    // ═══════════════════════════════════════════════════════════════════════════
    // 
    //  POWER MCP FRAMEWORK
    // 
    // ═══════════════════════════════════════════════════════════════════════════
    // ═══════════════════════════════════════════════════════════════════════════

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            var requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestJson = JObject.Parse(requestBody);

            if (!requestJson.ContainsKey("jsonrpc")) requestJson["jsonrpc"] = "2.0";
            return await HandleMCPRequest(requestJson).ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            return CreateErrorResponse(-32700, $"Parse error: {ex.Message}", null);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(-32603, $"Internal error: {ex.Message}", null);
        }
    }

    private async Task<HttpResponseMessage> HandleMCPRequest(JObject request)
    {
        var method = request["method"]?.ToString();
        var id = request["id"];
        var parms = request["params"] as JObject;

        return method switch
        {
            "initialize" => HandleInitialize(request, id),
            "initialized" or "ping" => CreateSuccessResponse(new JObject(), id),
            "notifications/cancelled" => CreateSuccessResponse(new JObject(), id), // Acknowledge but no-op (stateless)
            "tools/list" => HandleListWithPagination(GetDefinedTools(), "tools", parms, id),
            "tools/call" => await HandleToolsCall(parms, id).ConfigureAwait(false),
            "resources/list" => HandleListWithPagination(GetDefinedResources(), "resources", parms, id),
            "resources/templates/list" => HandleListWithPagination(GetDefinedResourceTemplates(), "resourceTemplates", parms, id),
            "resources/read" => await HandleResourcesRead(parms, id).ConfigureAwait(false),
            "prompts/list" => HandleListWithPagination(GetDefinedPrompts(), "prompts", parms, id),
            "prompts/get" => HandlePromptsGet(parms, id),
            "completion/complete" => HandleCompletionComplete(parms, id),
            "logging/setLevel" => HandleLoggingSetLevel(parms, id),
            _ => CreateErrorResponse(-32601, $"Method not found: {method}", id)
        };
    }

    private HttpResponseMessage HandleListWithPagination(JArray items, string fieldName, JObject parms, JToken id)
    {
        // Simple pagination: if cursor provided, return empty (all items returned in first page)
        var cursor = parms?["cursor"]?.ToString();
        if (!string.IsNullOrEmpty(cursor))
        {
            // Cursor provided means client wants next page - we return all in first page
            return CreateSuccessResponse(new JObject { [fieldName] = new JArray() }, id);
        }
        return CreateSuccessResponse(new JObject { [fieldName] = items }, id);
    }

    private HttpResponseMessage HandleInitialize(JObject request, JToken id)
    {
        var clientParams = request["params"] as JObject;
        var protocolVersion = clientParams?["protocolVersion"]?.ToString() ?? "2025-06-18";

        return CreateSuccessResponse(new JObject
        {
            ["protocolVersion"] = protocolVersion,
            ["capabilities"] = GetServerCapabilities(),
            ["serverInfo"] = GetServerInfo()
        }, id);
    }

    private async Task<HttpResponseMessage> HandleToolsCall(JObject parms, JToken id)
    {
        var toolName = parms?["name"]?.ToString();
        if (string.IsNullOrEmpty(toolName))
            return CreateErrorResponse(-32602, "Tool name required", id);

        var tools = GetDefinedTools();
        if (!tools.Any(t => t["name"]?.ToString() == toolName))
            return CreateErrorResponse(-32601, $"Unknown tool: {toolName}", id);

        try
        {
            var arguments = parms["arguments"] as JObject ?? new JObject();
            var result = await ExecuteToolByName(toolName, arguments).ConfigureAwait(false);

            return CreateSuccessResponse(new JObject
            {
                ["content"] = new JArray { new JObject { ["type"] = "text", ["text"] = result.ToString() } },
                ["isError"] = false
            }, id);
        }
        catch (ArgumentException ex)
        {
            // Tool execution error - return as isError: true so LLM can self-correct
            return CreateSuccessResponse(new JObject
            {
                ["content"] = new JArray { new JObject { ["type"] = "text", ["text"] = $"Invalid arguments: {ex.Message}" } },
                ["isError"] = true
            }, id);
        }
        catch (Exception ex)
        {
            // Tool execution error - return as isError: true so LLM can self-correct
            return CreateSuccessResponse(new JObject
            {
                ["content"] = new JArray { new JObject { ["type"] = "text", ["text"] = $"Tool error: {ex.Message}" } },
                ["isError"] = true
            }, id);
        }
    }

    private async Task<HttpResponseMessage> HandleResourcesRead(JObject parms, JToken id)
    {
        var uri = parms?["uri"]?.ToString();
        if (string.IsNullOrEmpty(uri))
            return CreateErrorResponse(-32602, "URI required", id);

        if (!uri.StartsWith("http://") && !uri.StartsWith("https://"))
            return CreateErrorResponse(-32602, "Only HTTP/HTTPS URIs supported", id);

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
                return CreateErrorResponse(-32000, $"Resource fetch failed: {response.StatusCode}", id);

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var mimeType = response.Content.Headers.ContentType?.MediaType ?? "text/plain";

            return CreateSuccessResponse(new JObject
            {
                ["contents"] = new JArray
                {
                    new JObject { ["uri"] = uri, ["mimeType"] = mimeType, ["text"] = content }
                }
            }, id);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(-32000, $"Resource error: {ex.Message}", id);
        }
    }

    private HttpResponseMessage HandlePromptsGet(JObject parms, JToken id)
    {
        var promptName = parms?["name"]?.ToString();
        if (string.IsNullOrEmpty(promptName))
            return CreateErrorResponse(-32602, "Prompt name required", id);

        var prompts = GetDefinedPrompts();
        if (!prompts.Any(p => p["name"]?.ToString() == promptName))
            return CreateErrorResponse(-32602, $"Unknown prompt: {promptName}", id);

        // Implement prompt building logic here when needed
        return CreateErrorResponse(-32000, $"Prompt '{promptName}' not implemented", id);
    }

    private HttpResponseMessage HandleCompletionComplete(JObject parms, JToken id)
    {
        var refObj = parms?["ref"] as JObject;
        if (refObj == null)
            return CreateErrorResponse(-32602, "ref object required", id);

        var refType = refObj["type"]?.ToString();
        var refName = refObj["name"]?.ToString();
        var argument = parms?["argument"] as JObject;
        var argName = argument?["name"]?.ToString();
        var argValue = argument?["value"]?.ToString() ?? "";

        var completions = GetCompletionsFor(refType, refName, argName, argValue);

        return CreateSuccessResponse(new JObject
        {
            ["completion"] = new JObject
            {
                ["values"] = new JArray(completions),
                ["hasMore"] = false,
                ["total"] = completions.Length
            }
        }, id);
    }

    private string[] GetCompletionsFor(string refType, string refName, string argName, string argValue)
    {
        // Override this method to provide argument completions
        // Example: return customer IDs that start with the typed value
        // 
        // if (refType == "ref/prompt" && refName == "analyze_data" && argName == "format")
        //     return new[] { "json", "csv", "xml" }.Where(v => v.StartsWith(argValue)).ToArray();
        
        return Array.Empty<string>();
    }

    private HttpResponseMessage HandleLoggingSetLevel(JObject parms, JToken id)
    {
        var level = parms?["level"]?.ToString()?.ToLowerInvariant();
        var validLevels = new[] { "debug", "info", "notice", "warning", "error", "critical", "alert", "emergency" };

        if (string.IsNullOrEmpty(level) || !validLevels.Contains(level))
            return CreateErrorResponse(-32602, $"Invalid log level. Valid: {string.Join(", ", validLevels)}", id);

        _logLevel = level;
        return CreateSuccessResponse(new JObject(), id);
    }

    private HttpResponseMessage CreateSuccessResponse(JObject result, JToken id)
    {
        var json = new JObject { ["jsonrpc"] = "2.0", ["result"] = result, ["id"] = id };
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(json.ToString());
        return response;
    }

    private HttpResponseMessage CreateErrorResponse(int code, string message, JToken id)
    {
        var json = new JObject
        {
            ["jsonrpc"] = "2.0",
            ["error"] = new JObject { ["code"] = code, ["message"] = message },
            ["id"] = id
        };
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(json.ToString());
        return response;
    }
}
