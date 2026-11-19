# Power MCP

**Model Context Protocol for Power Platform Custom Connectors**

Power MCP enables you to build MCP server functionality directly into Power Platform custom connectors, allowing Copilot Studio agents to use MCP tools without requiring external servers.

## Features

- **Pure MCP Server**: Expose tools, resources, and prompts via standard MCP JSON-RPC protocol
- **No External Server**: Everything runs within the Power Platform custom connector
- **Copilot Studio Ready**: Copilot Studio handles orchestration, Power MCP provides the tools

## Quick Start

### 1. Import the Connector

Import this connector from GitHub into Power Platform.

### 2. Update Server Info

Update `GetServerInfo()` with your server details:

```csharp
private JObject GetServerInfo() => new JObject
{
    ["name"] = "my-mcp-server",
    ["version"] = "1.0.0",
    ["title"] = "My MCP Server",
    ["description"] = "Description of what this server does",
    ["websiteUrl"] = "https://example.com"
};
```

### 3. Define Your Tools

Add your tools to `GetDefinedTools()`:

```csharp
private JArray GetDefinedTools() => new JArray
{
    new JObject
    {
        ["name"] = "get_customer",
        ["description"] = "Retrieves customer information by ID",
        ["inputSchema"] = new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject
            {
                ["customerId"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "The customer ID to look up"
                }
            },
            ["required"] = new JArray { "customerId" }
        }
    }
};
```

### 4. Implement Tool Handlers

Add a case to `ExecuteToolByName()`:

```csharp
private async Task<JObject> ExecuteToolByName(string toolName, JObject arguments)
{
    return toolName switch
    {
        "get_customer" => await ExecuteGetCustomerTool(arguments),
        _ => throw new Exception($"Unknown tool: {toolName}")
    };
}
```

Then implement your tool logic:

```csharp
private async Task<JObject> ExecuteGetCustomerTool(JObject arguments)
{
    var customerId = arguments?["customerId"]?.ToString() 
        ?? throw new ArgumentException("customerId is required");
    
    var request = new HttpRequestMessage(HttpMethod.Get, 
        $"https://api.example.com/customers/{customerId}");
    
    var response = await this.Context.SendAsync(request, this.CancellationToken);
    var body = await response.Content.ReadAsStringAsync();
    
    if (!response.IsSuccessStatusCode)
        throw new HttpRequestException($"API error: {response.StatusCode}");
    
    return JObject.Parse(body);
}
```

### 5. Define Resources (Optional)

Add static resources to `GetDefinedResources()`:

```csharp
private JArray GetDefinedResources() => new JArray
{
    new JObject
    {
        ["uri"] = "https://api.example.com/config.json",
        ["name"] = "Configuration",
        ["description"] = "Application configuration settings",
        ["mimeType"] = "application/json"
    }
};
```

Add URI templates to `GetDefinedResourceTemplates()`:

```csharp
private JArray GetDefinedResourceTemplates() => new JArray
{
    new JObject
    {
        ["uriTemplate"] = "https://api.example.com/users/{userId}",
        ["name"] = "User Profile",
        ["description"] = "Access user profile by ID",
        ["mimeType"] = "application/json"
    }
};
```

### 6. Define Prompts (Optional)

Add prompts to `GetDefinedPrompts()`:

```csharp
private JArray GetDefinedPrompts() => new JArray
{
    new JObject
    {
        ["name"] = "analyze_data",
        ["description"] = "Analyze data with specified format",
        ["arguments"] = new JArray
        {
            new JObject { ["name"] = "source", ["description"] = "Data source", ["required"] = true },
            new JObject { ["name"] = "format", ["description"] = "Output format", ["required"] = false }
        }
    }
};
```

### 7. Save and Test

Save your connector and test with the MCP protocol methods.

## MCP Protocol Support

Based on [MCP Specification 2025-11-25](https://modelcontextprotocol.io/specification/2025-11-25).

| Method | Supported | Notes |
|--------|-----------|-------|
| `initialize` | ✅ | Returns server capabilities and info |
| `initialized` | ✅ | Lifecycle acknowledgment |
| `ping` | ✅ | Connection health check |
| `tools/list` | ✅ | Lists available tools (with pagination) |
| `tools/call` | ✅ | Executes tools, returns `isError` on failure |
| `resources/list` | ✅ | Lists available resources (with pagination) |
| `resources/templates/list` | ✅ | Lists URI templates (with pagination) |
| `resources/read` | ✅ | Reads HTTP/HTTPS resources |
| `prompts/list` | ✅ | Lists available prompts (with pagination) |
| `prompts/get` | ⚠️ | Skeleton - requires implementation |
| `completion/complete` | ✅ | Argument autocompletion |
| `logging/setLevel` | ✅ | Controls log verbosity (RFC 5424 levels) |
| `notifications/cancelled` | ✅ | Acknowledged (no-op in stateless) |
| `resources/subscribe` | ❌ | Not supported (requires SSE) |
| `notifications/*` | ❌ | Not supported (requires SSE) |

## Request Examples

### Initialize

```json
{
    "jsonrpc": "2.0",
    "method": "initialize",
    "params": {
        "protocolVersion": "2025-11-25",
        "capabilities": {},
        "clientInfo": { "name": "MyClient", "version": "1.0.0" }
    },
    "id": 1
}
```

### List Tools

```json
{
    "jsonrpc": "2.0",
    "method": "tools/list",
    "id": 2
}
```

### Call Tool

```json
{
    "jsonrpc": "2.0",
    "method": "tools/call",
    "params": {
        "name": "get_customer",
        "arguments": {
            "customerId": "CUST-123"
        }
    },
    "id": 3
}
```

### List Resource Templates

```json
{
    "jsonrpc": "2.0",
    "method": "resources/templates/list",
    "id": 4
}
```

### Completion

```json
{
    "jsonrpc": "2.0",
    "method": "completion/complete",
    "params": {
        "ref": { "type": "ref/prompt", "name": "analyze_data" },
        "argument": { "name": "format", "value": "js" }
    },
    "id": 5
}
```

## Files

| File | Description |
|------|-------------|
| `script.csx` | Main connector code with MCP implementation |
| `apiDefinition.swagger.json` | OpenAPI definition for the connector |
| `apiProperties.json` | Connector properties and connection parameters |

## Resources

- [MCP Specification](https://spec.modelcontextprotocol.io/)
- [Power Platform Custom Connectors](https://learn.microsoft.com/en-us/connectors/custom-connectors/)
- [Copilot Studio](https://learn.microsoft.com/en-us/microsoft-copilot-studio/)


## Transport Layer

Power MCP uses **basic HTTP request/response** transport, not the full Streamable HTTP transport defined in the MCP specification. This is because Power Platform custom connectors don't support Server-Sent Events (SSE) or persistent connections.

### What This Means

| Feature | Streamable HTTP | Power MCP |
|---------|-----------------|-----------|
| Request/Response | ✅ | ✅ |
| Server-Sent Events (SSE) | ✅ | ❌ |
| Server-initiated notifications | ✅ | ❌ |
| Streaming responses | ✅ | ❌ |
| Persistent connections | ✅ | ❌ |

### Practical Impact

- **No `list_changed` notifications**: Clients won't be notified when tools/resources/prompts change. Instead, clients should call `tools/list`, `resources/list`, or `prompts/list` to refresh.
- **No `resources/subscribe`**: Real-time resource updates aren't supported.
- **Stateless**: Each request is independent. No session state is maintained between calls.
- **MCP-compliant data layer**: All JSON-RPC messages follow the MCP specification exactly—only the transport is simplified.

## License

MIT
