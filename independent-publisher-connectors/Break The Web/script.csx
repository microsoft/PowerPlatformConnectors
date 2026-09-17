using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        string apiKey = null;
        
        // Get the API key from request header set by connection parameter
        if (this.Context.Request.Headers.Contains("api_key"))
        {
            apiKey = this.Context.Request.Headers.GetValues("api_key").FirstOrDefault();
            this.Context.Request.Headers.Remove("api_key");
        }

        // Handle MCP requests (before modifying the main request)
        if (this.Context.Request.RequestUri.AbsolutePath.EndsWith("/mcp/invoke"))
        {
            return await HandleMCPRequest(apiKey);
        }
        
        // Add Bearer token to Authorization header for regular API requests
        if (!string.IsNullOrEmpty(apiKey))
        {
            // Check if the API key already starts with "Bearer "
            if (apiKey.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                // If it already has "Bearer ", extract just the token part
                string token = apiKey.Substring(7); // Remove "Bearer " (7 characters)
                this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                // If no "Bearer " prefix, add it
                this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            }
        }

        // Forward the request to the backend API
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        
        return response;
    }

    private async Task<HttpResponseMessage> HandleMCPRequest(string apiKey)
    {
        try 
        {
            // Read the JSON-RPC request body
            string requestBody = await this.Context.Request.Content.ReadAsStringAsync();
            var mcpRequest = JsonConvert.DeserializeObject<JObject>(requestBody);
            
            string method = mcpRequest["method"]?.ToString();
            string id = mcpRequest["id"]?.ToString();
            var parameters = mcpRequest["params"] as JObject;
            
            JObject mcpResponse;
            
            switch (method)
            {
                case "initialize":
                    mcpResponse = HandleInitialize(id);
                    break;
                    
                case "tools/list":
                    mcpResponse = HandleToolsList(id);
                    break;
                    
                case "tools/call":
                    mcpResponse = await HandleToolsCall(id, parameters, apiKey);
                    break;
                    
                case "btw/get_trending_feed":
                    mcpResponse = await HandleGetTrendingFeed(id, apiKey);
                    break;
                    
                case "btw/search_trends":
                    mcpResponse = await HandleSearchTrends(id, parameters, apiKey);
                    break;
                    
                default:
                    mcpResponse = CreateErrorResponse(id, -32601, "Method not found");
                    break;
            }
            
            var responseContent = new StringContent(mcpResponse.ToString(), Encoding.UTF8, "application/json");
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = responseContent
            };
            
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = CreateErrorResponse(null, -32000, "Internal error: " + ex.Message);
            var responseContent = new StringContent(errorResponse.ToString(), Encoding.UTF8, "application/json");
            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = responseContent
            };
        }
    }
    
    private JObject HandleInitialize(string id)
    {
        return new JObject
        {
            ["jsonrpc"] = "2.0",
            ["id"] = id,
            ["result"] = new JObject
            {
                ["protocolVersion"] = "2024-11-05",
                ["capabilities"] = new JObject
                {
                    ["tools"] = new JObject()
                },
                ["serverInfo"] = new JObject
                {
                    ["name"] = "break-the-web-mcp-server",
                    ["version"] = "1.0.0",
                    ["description"] = "Break the Web MCP Server for trending news and current events"
                }
            }
        };
    }
    
    private JObject HandleToolsList(string id)
    {
        return new JObject
        {
            ["jsonrpc"] = "2.0",
            ["id"] = id,
            ["result"] = new JObject
            {
                ["tools"] = new JArray
                {
                    new JObject
                    {
                        ["name"] = "get_trending_feed",
                        ["description"] = "Get current trending stories from Break the Web API",
                        ["inputSchema"] = new JObject
                        {
                            ["type"] = "object",
                            ["properties"] = new JObject(),
                            ["required"] = new JArray()
                        }
                    },
                    new JObject
                    {
                        ["name"] = "search_trends", 
                        ["description"] = "Search for trending stories by query",
                        ["inputSchema"] = new JObject
                        {
                            ["type"] = "object",
                            ["properties"] = new JObject
                            {
                                ["query"] = new JObject
                                {
                                    ["type"] = "string",
                                    ["description"] = "Search query for trending topics"
                                }
                            },
                            ["required"] = new JArray { "query" }
                        }
                    }
                }
            }
        };
    }
    
    private async Task<JObject> HandleToolsCall(string id, JObject parameters, string apiKey)
    {
        try
        {
            string toolName = parameters?["name"]?.ToString();
            var arguments = parameters?["arguments"] as JObject;
            
            switch (toolName)
            {
                case "get_trending_feed":
                    return await HandleGetTrendingFeed(id, apiKey);
                    
                case "search_trends":
                    return await HandleSearchTrends(id, arguments, apiKey);
                    
                default:
                    return CreateErrorResponse(id, -32602, "Invalid tool name");
            }
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(id, -32000, "Tool execution error: " + ex.Message);
        }
    }
    
    private async Task<JObject> HandleGetTrendingFeed(string id, string apiKey)
    {
        try
        {
            // Create request to BTW API
            var request = new HttpRequestMessage(HttpMethod.Get, "https://btw.co/api/trends/feed");
            
            // Add Bearer authentication
            if (!string.IsNullOrEmpty(apiKey))
            {
                if (apiKey.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string token = apiKey.Substring(7);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                }
            }
            
            // Use the context's SendAsync method
            var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return new JObject
                {
                    ["jsonrpc"] = "2.0",
                    ["id"] = id,
                    ["result"] = new JObject
                    {
                        ["content"] = new JArray
                        {
                            new JObject
                            {
                                ["type"] = "text",
                                ["text"] = content
                            }
                        }
                    }
                };
            }
            else
            {
                return CreateErrorResponse(id, -32000, $"API Error: {response.StatusCode} - {content}");
            }
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(id, -32000, "Request failed: " + ex.Message);
        }
    }
    
    private async Task<JObject> HandleSearchTrends(string id, JObject parameters, string apiKey)
    {
        try
        {
            string query = parameters?["query"]?.ToString();
            if (string.IsNullOrEmpty(query))
            {
                return CreateErrorResponse(id, -32602, "Missing required parameter: query");
            }
            
            // Create request to BTW API
            var request = new HttpRequestMessage(HttpMethod.Post, "https://btw.co/api/trends/search");
            
            // Add Bearer authentication
            if (!string.IsNullOrEmpty(apiKey))
            {
                if (apiKey.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string token = apiKey.Substring(7);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                }
            }
            
            // Add request body
            var requestBody = new JObject { ["Query"] = query };
            request.Content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
            
            // Use the context's SendAsync method
            var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return new JObject
                {
                    ["jsonrpc"] = "2.0",
                    ["id"] = id,
                    ["result"] = new JObject
                    {
                        ["content"] = new JArray
                        {
                            new JObject
                            {
                                ["type"] = "text",
                                ["text"] = content
                            }
                        }
                    }
                };
            }
            else
            {
                return CreateErrorResponse(id, -32000, $"API Error: {response.StatusCode} - {content}");
            }
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(id, -32000, "Request failed: " + ex.Message);
        }
    }
    
    private JObject CreateErrorResponse(string id, int code, string message)
    {
        return new JObject
        {
            ["jsonrpc"] = "2.0",
            ["id"] = id,
            ["error"] = new JObject
            {
                ["code"] = code,
                ["message"] = message
            }
        };
    }
}