/*
 * Universal Mermaid Connector - Optimized for Power Platform
 * Supports both REST operations and MCP protocol in a single efficient implementation
 */

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            // Check which operation was called based on the context
            var operationId = this.Context.OperationId;
            
            // Route to appropriate method based on operation
            switch (operationId)
            {
                case "ParseMermaidDiagram":
                    return await ParseMermaidDiagram().ConfigureAwait(false);
                    
                case "ConvertMermaidToFormats":
                    return await ConvertMermaidToFormats().ConfigureAwait(false);

                case "GetInvokeServer":
                case "InvokeServer":
                    // Handle MCP protocol for InvokeServer operation
                    var requestBody = await ParseRequestBodyAsync().ConfigureAwait(false);
                    var method = GetStringProperty(requestBody, "method", "");
                    if (!string.IsNullOrEmpty(method))
                    {
                        return await HandleMCPRequest(requestBody, method).ConfigureAwait(false);
                    }
                    return CreateJsonRpcErrorResponse(null, -32602, "Invalid params", "Missing method parameter");
                    
                default:
                    // Default response for health check or unknown operations
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("Universal Mermaid Connector Ready", Encoding.UTF8, "text/plain")
                    };
            }
        }
        catch (Exception ex)
        {
            return CreateJsonRpcErrorResponse(null, -32603, "Internal error", ex.Message);
        }
    }

    // REST Operations
    public async Task<HttpResponseMessage> ParseMermaidDiagram()
    {
        try
        {
            var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestBody = JObject.Parse(requestContent);
            var mermaidContent = GetStringProperty(requestBody, "mermaidContent", "");

            if (string.IsNullOrEmpty(mermaidContent))
                return CreateErrorResponse("MISSING_CONTENT", "Missing required parameter: mermaidContent");

            var parser = new MermaidParser();
            var result = parser.ParseMermaidDiagram(mermaidContent);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    success = true,
                    entities = ConvertToRESTFormat(result["entities"] as JArray),
                    diagramType = result["diagramType"],
                    relationships = result["relationships"],
                    validation = result["validation"],
                    cdmDetection = new { detectedEntities = new JArray() }
                }, Newtonsoft.Json.Formatting.Indented), Encoding.UTF8, "application/json")
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("PARSING_ERROR", ex.Message);
        }
    }

    public async Task<HttpResponseMessage> ConvertMermaidToFormats()
    {
        try
        {
            var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestBody = JObject.Parse(requestContent);
            var entities = requestBody["entities"] as JArray;
            var outputFormat = GetStringProperty(requestBody, "outputFormat", "all");

            if (entities == null || entities.Count == 0)
                return CreateErrorResponse("MISSING_ENTITIES", "Missing required parameter: entities");

            var parser = new MermaidParser();
            var parserEntities = ConvertFromRESTFormat(entities);
            var conversions = new JObject();

            // Generate requested formats - all 18 formats from Swagger
            var formats = outputFormat == "all" ? 
                new[] { "jsonSchema", "sqlDDL", "typescript", "csharp", "python", "java", "go", "rust", 
                       "graphql", "react", "vue", "swift", "dataverseSchema", "powerAppsFormulas", 
                       "postgresqlDDL", "mysqlDDL", "mongooseSchema", "markdown" } :
                new[] { outputFormat };

            // Add relationships to parser context
            var relationships = requestBody["relationships"] as JArray ?? new JArray();

            foreach (var format in formats)
            {
                conversions[format] = parser.ConvertToFormat("", format, parserEntities, relationships);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    success = true,
                    conversions
                }, Newtonsoft.Json.Formatting.Indented), Encoding.UTF8, "application/json")
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("CONVERSION_ERROR", ex.Message);
        }
    }

    // MCP Protocol Handler
    private async Task<HttpResponseMessage> HandleMCPRequest(JObject requestBody, string method)
    {
        var requestId = GetRequestId(requestBody);

        switch (method)
        {
            case "initialize":
                return CreateJsonRpcSuccessResponse(requestId, new JObject
                {
                    ["protocolVersion"] = "2024-11-05",
                    ["capabilities"] = new JObject { ["tools"] = new JObject() },
                    ["serverInfo"] = new JObject { ["name"] = "universal-mermaid-server", ["version"] = "2.0.0" }
                });

            case "notifications/initialized":
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            case "tools/list":
                return CreateJsonRpcSuccessResponse(requestId, new JObject
                {
                    ["tools"] = new JArray
                    {
                        CreateTool("parse_mermaid_diagram", "Parse Mermaid diagram and extract structured information"),
                        CreateTool("detect_diagram_type", "Detect the type of Mermaid diagram with confidence scoring"),
                        CreateTool("extract_entities", "Extract entities from diagrams with detailed field information"),
                        CreateTool("extract_relationships", "Extract relationships between entities"),
                        CreateTool("convert_to_formats", "Convert diagram to various output formats", new[] { "content", "format" }),
                        CreateTool("validate_mermaid_syntax", "Validate Mermaid diagram syntax")
                    }
                });

            case "tools/call":
                return await HandleToolsCall(requestBody, requestId).ConfigureAwait(false);

            default:
                return CreateJsonRpcErrorResponse(requestId, -32601, "Method not found", $"Unknown method '{method}'");
        }
    }

    private async Task<HttpResponseMessage> HandleToolsCall(JObject requestBody, object requestId)
    {
        try
        {
            var paramsObj = requestBody["params"] as JObject;
            var toolName = GetStringProperty(paramsObj, "name", "");
            var arguments = paramsObj["arguments"] as JObject ?? new JObject();
            var content = GetStringProperty(arguments, "content", "");

            if (string.IsNullOrEmpty(content))
                return CreateJsonRpcErrorResponse(requestId, -32602, "Invalid params", "Missing required parameter: content");

            var parser = new MermaidParser();
            object result = null;

            switch (toolName)
            {
                case "parse_mermaid_diagram":
                    result = parser.ParseMermaidDiagram(content);
                    break;
                case "detect_diagram_type":
                    result = parser.DetectDiagramType(content);
                    break;
                case "extract_entities":
                    result = parser.ExtractEntitiesForDiagram(content);
                    break;
                case "extract_relationships":
                    result = parser.ExtractRelationshipsForDiagram(content);
                    break;
                case "convert_to_formats":
                    var format = GetStringProperty(arguments, "format", "json_schema");
                    result = parser.ConvertToFormat(content, format);
                    break;
                case "validate_mermaid_syntax":
                    result = parser.ValidateMermaidSyntax(content);
                    break;
                default:
                    return CreateJsonRpcErrorResponse(requestId, -32602, "Invalid params", $"Unknown tool '{toolName}'");
            }

            return CreateJsonRpcSuccessResponse(requestId, new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = result is string ? result.ToString() : JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented)
                    }
                }
            });
        }
        catch (Exception ex)
        {
            return CreateJsonRpcErrorResponse(requestId, -32603, "Internal error", ex.Message);
        }
    }

    // Helper Methods
    private async Task<JObject> ParseRequestBodyAsync()
    {
        try
        {
            var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            return string.IsNullOrEmpty(content) ? new JObject() : JObject.Parse(content);
        }
        catch { return new JObject(); }
    }

    private JObject CreateTool(string name, string description, string[] requiredParams = null)
    {
        var properties = new JObject
        {
            ["content"] = new JObject 
            { 
                ["type"] = "string", 
                ["description"] = "Mermaid diagram content to process"
            }
        };

        if (requiredParams?.Contains("format") == true)
        {
            properties["format"] = new JObject
            {
                ["type"] = "string",
                ["description"] = "Target output format",
                ["enum"] = new JArray { "jsonSchema", "sqlDDL", "typescript", "csharp", "python", "java", "go", "rust", "graphql", "react", "vue", "swift", "markdown" }
            };
        }

        var tool = new JObject
        {
            ["name"] = name,
            ["description"] = description,
            ["inputSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = properties,
                ["required"] = new JArray(requiredParams ?? new[] { "content" })
            }
        };

        return tool;
    }

    private JArray ConvertToRESTFormat(JArray entities)
    {
        var result = new JArray();
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString() ?? "";
            
            // Clean entity name - remove any unwanted characters/newlines
            entityName = entityName.Replace("\\n", "").Replace("\\", "").Trim();
            if (entityName.Contains(" "))
            {
                entityName = entityName.Split(' ')[0]; // Take first word if multiple
            }
            
            result.Add(new JObject
            {
                ["name"] = entityName,
                ["logicalName"] = entityName.ToLower().Replace(" ", "_"),
                ["entityType"] = entityObj["entityType"]?.ToString() ?? "Entity",
                ["attributes"] = ConvertFieldsToAttributes(entityObj["attributes"] as JArray),
                ["methods"] = entityObj["methods"] ?? new JArray(),
                ["metadata"] = new JObject()
            });
        }
        return result;
    }

    private JArray ConvertFieldsToAttributes(JArray fields)
    {
        var result = new JArray();
        if (fields != null)
        {
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                result.Add(new JObject
                {
                    ["name"] = fieldObj["name"],
                    ["dataType"] = fieldObj["type"],
                    ["isPrimaryKey"] = fieldObj["isPrimaryKey"] ?? false,
                    ["isForeignKey"] = fieldObj["isForeignKey"] ?? false,
                    ["isUnique"] = fieldObj["isUnique"] ?? false,
                    ["isNullable"] = true,
                    ["constraints"] = new JArray(),
                    ["visibility"] = fieldObj["visibility"] ?? "public"
                });
            }
        }
        return result;
    }

    private JArray ConvertFromRESTFormat(JArray entities)
    {
        var result = new JArray();
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            result.Add(new JObject
            {
                ["name"] = entityObj["name"],
                ["type"] = entityObj["entityType"]?.ToString().ToLower(),
                ["fields"] = ConvertAttributesToFields(entityObj["attributes"] as JArray)
            });
        }
        return result;
    }

    private JArray ConvertAttributesToFields(JArray attributes)
    {
        var result = new JArray();
        if (attributes != null)
        {
            foreach (var attr in attributes)
            {
                var attrObj = attr as JObject;
                result.Add(new JObject
                {
                    ["name"] = attrObj["name"],
                    ["type"] = attrObj["dataType"],
                    ["isPrimaryKey"] = attrObj["isPrimaryKey"],
                    ["isForeignKey"] = attrObj["isForeignKey"],
                    ["isUnique"] = attrObj["isUnique"]
                });
            }
        }
        return result;
    }

    private string GetStringProperty(JObject obj, string property, string defaultValue)
    {
        return obj?[property]?.ToString() ?? defaultValue;
    }

    private object GetRequestId(JObject request)
    {
        return request?["id"]?.ToString();
    }

    private HttpResponseMessage CreateJsonRpcSuccessResponse(object id, JObject result)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new JObject
            {
                ["jsonrpc"] = "2.0",
                ["id"] = JToken.FromObject(id),
                ["result"] = result
            }), Encoding.UTF8, "application/json")
        };
    }

    private HttpResponseMessage CreateJsonRpcErrorResponse(object id, int code, string message, string data = null)
    {
        var error = new JObject { ["code"] = code, ["message"] = message };
        if (!string.IsNullOrEmpty(data)) error["data"] = data;

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new JObject
            {
                ["jsonrpc"] = "2.0",
                ["id"] = id != null ? JToken.FromObject(id) : null,
                ["error"] = error
            }), Encoding.UTF8, "application/json")
        };
    }

    private HttpResponseMessage CreateErrorResponse(string code, string message)
    {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new
            {
                error = new { code, message }
            }), Encoding.UTF8, "application/json")
        };
    }
}

internal class MermaidParser
{
    public JObject ParseMermaidDiagram(string content)
    {
        var result = new JObject();
        try
        {
            var diagramType = DetectDiagramType(content);
            var entities = ExtractEntitiesForDiagram(content);
            var relationships = ExtractRelationshipsForDiagram(content);
            var validation = ValidateMermaidSyntax(content);
            
            result["success"] = true;
            result["diagramType"] = diagramType["type"];
            result["entities"] = entities;
            result["relationships"] = relationships;
            result["validation"] = validation;
            result["cdmDetection"] = new JObject
            {
                ["detectedEntities"] = new JArray()
            };
        }
        catch (Exception ex)
        {
            result["success"] = false;
            result["error"] = ex.Message;
        }
        return result;
    }

    public JObject DetectDiagramType(string content)
    {
        var patterns = new Dictionary<string, string[]>
        {
            // Core diagram types
            ["erDiagram"] = new[] { "erDiagram", "||--||", "||--o{", "}o--||", "||--|{", "}|--||" },
            ["classDiagram"] = new[] { "classDiagram", "class ", "<<", ">>", " : ", " + ", " - ", " # ", "interface ", "enum " },
            ["flowchart"] = new[] { "flowchart", "graph ", "-->", "---", "-.->", "==>", "subgraph " },
            ["sequenceDiagram"] = new[] { "sequenceDiagram", "participant ", "->", "->>", "-->>", 
                "activate ", "deactivate ", "loop ", "alt ", "opt " },
            ["stateDiagram"] = new[] { "stateDiagram", "state ", "[*]", "-->", "stateDiagram-v2" },
            ["gantt"] = new[] { "gantt", "dateFormat", "title", "section", ":", "axisFormat" },
            
            // Advanced diagram types
            ["journey"] = new[] { "journey", "title", "section", "task", "score" },
            ["gitgraph"] = new[] { "gitgraph", "commit", "branch", "checkout", "merge" },
            ["pie"] = new[] { "pie", "title", "\"", ":" },
            ["quadrantChart"] = new[] { "quadrantChart", "title", "x-axis", "y-axis", "quadrant-1", "quadrant-2", "quadrant-3", "quadrant-4" },
            ["requirement"] = new[] { "requirementDiagram", "requirement", "element", 
                "functionalRequirement", "interfaceRequirement", "performanceRequirement", 
                "physicalRequirement", "designConstraint" },
            ["mindmap"] = new[] { "mindmap", "root", "))", "((", "::icon" },
            ["timeline"] = new[] { "timeline", "title", "period", ":" },
            ["sankey"] = new[] { "sankey-beta", "source", "target", "value" },
            ["xyChart"] = new[] { "xyChart-beta", "title", "x-axis", "y-axis", "line", "bar" },
            ["block"] = new[] { "block-beta", "columns", "block", "space" },
            
            // Specialized diagrams
            ["architecture"] = new[] { "architecture-beta", "service", "junction", "group" },
            ["packet"] = new[] { "packet-beta", "title", "0-15", "16-31" },
            ["c4"] = new[] { "C4Context", "C4Container", "C4Component", "Person", "System", "Container", "Component" }
        };

        var scores = new Dictionary<string, int>();
        var detectionDetails = new Dictionary<string, JArray>();
        
        foreach (var pattern in patterns)
        {
            var score = 0;
            var matchedKeywords = new JArray();
            
            foreach (var keyword in pattern.Value)
            {
                if (content.Contains(keyword))
                {
                    var keywordScore = keyword == pattern.Value[0] ? 10 : 1; // First keyword is diagram declaration
                    score += keywordScore;
                    matchedKeywords.Add(new JObject
                    {
                        ["keyword"] = keyword,
                        ["score"] = keywordScore,
                        ["position"] = content.IndexOf(keyword)
                    });
                }
            }
            
            scores[pattern.Key] = score;
            detectionDetails[pattern.Key] = matchedKeywords;
        }

        // Find best match manually instead of LINQ
        var bestMatch = new KeyValuePair<string, int>("", 0);
        foreach (var score in scores)
        {
            if (score.Value > bestMatch.Value)
            {
                bestMatch = score;
            }
        }
        var confidence = Math.Min(100, bestMatch.Value * 10);
        
        // Apply additional confidence adjustments
        if (bestMatch.Value == 0)
        {
            confidence = 0;
        }
        else if (bestMatch.Key == "flowchart" && content.Contains("flowchart"))
        {
            confidence = Math.Max(confidence, 85);
        }
        else if (bestMatch.Key == "classDiagram" && content.Contains("classDiagram"))
        {
            confidence = Math.Max(confidence, 85);
        }
        
        return new JObject
        {
            ["type"] = bestMatch.Key,
            ["confidence"] = confidence,
            ["scores"] = JObject.FromObject(scores),
            ["detectionDetails"] = JObject.FromObject(detectionDetails),
            ["metadata"] = new JObject
            {
                ["contentLength"] = content.Length,
                ["lineCount"] = content.Split('\n').Length,
                ["hasDirectives"] = content.Contains("%%"),
                ["hasComments"] = content.Contains("%%"),
                ["hasConfig"] = content.Contains("%%{config:"),
                ["version"] = DetectMermaidVersion(content)
            }
        };
    }

    private string DetectMermaidVersion(string content)
    {
        // Detect version based on syntax features
        if (content.Contains("architecture-beta") || content.Contains("packet-beta") || content.Contains("block-beta"))
            return "v10.x+";
        if (content.Contains("xyChart-beta") || content.Contains("sankey-beta"))
            return "v10.x";
        if (content.Contains("mindmap") || content.Contains("timeline") || content.Contains("quadrantChart"))
            return "v9.x";
        if (content.Contains("journey") || content.Contains("gitgraph"))
            return "v8.x";
        if (content.Contains("stateDiagram-v2"))
            return "v8.x+";
        
        return "v8.x+"; // Default to recent version
    }

    public JArray ExtractEntitiesForDiagram(string content)
    {
        var diagramType = DetectDiagramType(content)["type"].ToString();
        switch (diagramType)
        {
            case "erDiagram": return ExtractERDEntities(content);
            case "classDiagram": return ExtractClassEntities(content);
            case "flowchart": return ExtractFlowchartEntities(content);
            case "sequenceDiagram": return ExtractSequenceEntities(content);
            default: return ExtractGenericEntities(content);
        }
    }

    public JArray ExtractRelationshipsForDiagram(string content)
    {
        var diagramType = DetectDiagramType(content)["type"].ToString();
        switch (diagramType)
        {
            case "erDiagram": return ExtractERDRelationships(content);
            case "classDiagram": return ExtractClassRelationships(content);
            case "flowchart": return ExtractFlowchartRelationships(content);
            case "sequenceDiagram": return ExtractSequenceRelationships(content);
            default: return new JArray();
        }
    }

    private JArray ExtractERDEntities(string content)
    {
        var entities = new JArray();
        
        // First, try to normalize the content by adding line breaks where needed
        content = NormalizeERDContent(content);
        
        var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        var currentEntity = "";
        var inEntityDefinition = false;

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            
            // Skip the erDiagram declaration line
            if (trimmed == "erDiagram" || string.IsNullOrWhiteSpace(trimmed))
                continue;
                
            if (trimmed.Contains("{") && !inEntityDefinition)
            {
                // Extract entity name properly - just the part before the {
                currentEntity = trimmed.Split('{')[0].Trim();
                inEntityDefinition = true;
                entities.Add(new JObject
                {
                    ["name"] = currentEntity,
                    ["logicalName"] = currentEntity.ToLower().Replace(" ", "_"),
                    ["entityType"] = "Entity",
                    ["attributes"] = new JArray(),
                    ["methods"] = new JArray(),
                    ["metadata"] = new JObject
                    {
                        ["isAbstract"] = trimmed.Contains("<<abstract>>"),
                        ["isView"] = trimmed.Contains("<<view>>"),
                        ["tablespace"] = ExtractTablespace(trimmed),
                        ["engine"] = ExtractEngine(trimmed)
                    }
                });
            }
            else if (trimmed.Contains("}"))
            {
                inEntityDefinition = false;
                currentEntity = "";
            }
            else if (inEntityDefinition && !string.IsNullOrWhiteSpace(trimmed))
            {
                if (entities.Count > 0)
                {
                    var lastEntity = entities[entities.Count - 1] as JObject;
                    ParseERDField(trimmed, lastEntity);
                }
            }
        }
        return entities;
    }

    private string NormalizeERDContent(string content)
    {
        // Handle single-line ERD content by adding line breaks at logical points
        if (!content.Contains('\n') && content.Contains("erDiagram"))
        {
            // Add line breaks before entity declarations and relationship lines
            content = content.Replace("erDiagram", "erDiagram\n");
            
            // Add line breaks before entity names (look for patterns like "WORD {")
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\s+([A-Z_][A-Z0-9_]*)\s*\{", "\n$1 {");
            
            // Add line breaks before closing braces
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\s*\}\s*", "\n}\n");
            
            // Add line breaks before relationships (look for cardinality patterns)
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\s+([A-Z_][A-Z0-9_]*)\s*(\|\|--[o\|\{])", "\n$1 $2");
            
            // Clean up multiple consecutive line breaks
            content = System.Text.RegularExpressions.Regex.Replace(content, @"\n+", "\n");
        }
        
        return content;
    }

    private void ParseERDField(string fieldLine, JObject entity)
    {
        var attributes = entity["attributes"] as JArray;
        var trimmed = fieldLine.Trim();
        
        // Parse field like "string name" or "int customer_id PK"
        var parts = trimmed.Split(' ');
        if (parts.Length >= 2)
        {
            var fieldType = parts[0];
            var fieldName = parts[1];
            var constraints = parts.Length > 2 ? string.Join(" ", parts.Skip(2)) : "";
            
            attributes.Add(new JObject
            {
                ["name"] = fieldName,
                ["type"] = fieldType,
                ["constraints"] = constraints,
                ["isPrimaryKey"] = constraints.Contains("PK"),
                ["isForeignKey"] = constraints.Contains("FK"),
                ["isUnique"] = constraints.Contains("UNIQUE"),
                ["isNotNull"] = constraints.Contains("NOT NULL")
            });
        }
    }

    private void ParseComplexERDField(string fieldLine, JObject entity)
    {
        var fields = entity["fields"] as JArray;
        var constraints = entity["constraints"] as JArray;
        var indexes = entity["indexes"] as JArray;
        
        var trimmed = fieldLine.Trim();
        
        // Handle constraints
        if (trimmed.StartsWith("CONSTRAINT ") || trimmed.StartsWith("CHECK ") || trimmed.StartsWith("UNIQUE "))
        {
            constraints.Add(new JObject
            {
                ["type"] = GetConstraintType(trimmed),
                ["definition"] = trimmed,
                ["columns"] = ExtractConstraintColumns(trimmed)
            });
            return;
        }
        
        // Handle indexes
        if (trimmed.StartsWith("INDEX ") || trimmed.StartsWith("KEY "))
        {
            indexes.Add(new JObject
            {
                ["name"] = ExtractIndexName(trimmed),
                ["type"] = GetIndexType(trimmed),
                ["columns"] = ExtractIndexColumns(trimmed),
                ["isUnique"] = trimmed.Contains("UNIQUE")
            });
            return;
        }
        
        // Handle regular fields with enhanced parsing
        var parts = trimmed.Split(' ');
        if (parts.Length >= 2)
        {
            var field = new JObject
            {
                ["name"] = parts[1],
                ["type"] = parts[0],
                ["isPrimaryKey"] = trimmed.Contains("PK") || trimmed.Contains("PRIMARY KEY"),
                ["isForeignKey"] = trimmed.Contains("FK") || trimmed.Contains("FOREIGN KEY"),
                ["isUnique"] = trimmed.Contains("UK") || trimmed.Contains("UNIQUE"),
                ["isNotNull"] = trimmed.Contains("NOT NULL"),
                ["isAutoIncrement"] = trimmed.Contains("AUTO_INCREMENT") || trimmed.Contains("IDENTITY"),
                ["defaultValue"] = ExtractDefaultValue(trimmed),
                ["length"] = ExtractFieldLength(trimmed),
                ["precision"] = ExtractPrecision(trimmed),
                ["scale"] = ExtractScale(trimmed),
                ["comment"] = ExtractComment(trimmed),
                ["foreignKeyReference"] = ExtractForeignKeyReference(trimmed),
                ["checkConstraint"] = ExtractCheckConstraint(trimmed)
            };
            fields.Add(field);
        }
    }

    private string GetConstraintType(string constraintLine)
    {
        if (constraintLine.Contains("PRIMARY KEY")) return "PRIMARY_KEY";
        if (constraintLine.Contains("FOREIGN KEY")) return "FOREIGN_KEY";
        if (constraintLine.Contains("UNIQUE")) return "UNIQUE";
        if (constraintLine.Contains("CHECK")) return "CHECK";
        if (constraintLine.Contains("NOT NULL")) return "NOT_NULL";
        return "UNKNOWN";
    }

    private JArray ExtractConstraintColumns(string constraintLine)
    {
        var columns = new JArray();
        var match = System.Text.RegularExpressions.Regex.Match(constraintLine, @"\(([^)]+)\)");
        if (match.Success)
        {
            var columnList = match.Groups[1].Value.Split(',');
            foreach (var col in columnList)
            {
                columns.Add(col.Trim());
            }
        }
        return columns;
    }

    private string ExtractIndexName(string indexLine)
    {
        var parts = indexLine.Split(' ');
        return parts.Length > 1 ? parts[1] : "";
    }

    private string GetIndexType(string indexLine)
    {
        if (indexLine.Contains("BTREE")) return "BTREE";
        if (indexLine.Contains("HASH")) return "HASH";
        if (indexLine.Contains("FULLTEXT")) return "FULLTEXT";
        if (indexLine.Contains("SPATIAL")) return "SPATIAL";
        return "BTREE"; // Default
    }

    private JArray ExtractIndexColumns(string indexLine)
    {
        var columns = new JArray();
        var match = System.Text.RegularExpressions.Regex.Match(indexLine, @"\(([^)]+)\)");
        if (match.Success)
        {
            var columnList = match.Groups[1].Value.Split(',');
            foreach (var col in columnList)
            {
                columns.Add(col.Trim());
            }
        }
        return columns;
    }

    private string ExtractDefaultValue(string fieldLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(fieldLine, @"DEFAULT\s+(.+?)(?:\s|$)");
        return match.Success ? match.Groups[1].Value : null;
    }

    private int? ExtractFieldLength(string fieldLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(fieldLine, @"\((\d+)\)");
        return match.Success ? int.Parse(match.Groups[1].Value) : (int?)null;
    }

    private int? ExtractPrecision(string fieldLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(fieldLine, @"\((\d+),\d+\)");
        return match.Success ? int.Parse(match.Groups[1].Value) : (int?)null;
    }

    private int? ExtractScale(string fieldLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(fieldLine, @"\(\d+,(\d+)\)");
        return match.Success ? int.Parse(match.Groups[1].Value) : (int?)null;
    }

    private string ExtractComment(string fieldLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(fieldLine, @"COMMENT\s+['""](.+?)['""]");
        return match.Success ? match.Groups[1].Value : null;
    }

    private string ExtractForeignKeyReference(string fieldLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(fieldLine, @"REFERENCES\s+(\w+)(?:\((\w+)\))?");
        return match.Success ? $"{match.Groups[1].Value}.{match.Groups[2].Value}" : null;
    }

    private string ExtractCheckConstraint(string fieldLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(fieldLine, @"CHECK\s*\((.+?)\)");
        return match.Success ? match.Groups[1].Value : null;
    }

    private string ExtractTablespace(string entityLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(entityLine, @"TABLESPACE\s+(\w+)");
        return match.Success ? match.Groups[1].Value : null;
    }

    private string ExtractEngine(string entityLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(entityLine, @"ENGINE\s*=\s*(\w+)");
        return match.Success ? match.Groups[1].Value : null;
    }

    private JArray ExtractClassEntities(string content)
    {
        var entities = new JArray();
        var lines = content.Split('\n');

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            
            // Handle class declarations with enhanced features
            if (trimmed.StartsWith("class "))
            {
                var className = trimmed.Substring(6).Split('{')[0].Trim();
                var classType = DetermineClassType(trimmed);
                
                entities.Add(new JObject
                {
                    ["name"] = className,
                    ["type"] = classType,
                    ["methods"] = new JArray(),
                    ["fields"] = new JArray(),
                    ["constructors"] = new JArray(),
                    ["properties"] = new JArray(),
                    ["annotations"] = ExtractAnnotations(trimmed),
                    ["genericTypes"] = ExtractGenericTypes(className),
                    ["modifiers"] = ExtractClassModifiers(trimmed),
                    ["metadata"] = new JObject
                    {
                        ["isAbstract"] = trimmed.Contains("<<abstract>>") || trimmed.Contains("abstract "),
                        ["isInterface"] = trimmed.Contains("<<interface>>") || classType == "interface",
                        ["isEnum"] = trimmed.Contains("<<enum>>") || classType == "enumeration",
                        ["isStatic"] = trimmed.Contains("static "),
                        ["isFinal"] = trimmed.Contains("final ") || trimmed.Contains("sealed "),
                        ["package"] = ExtractPackage(content, className)
                    }
                });
            }
            // Handle namespace/package declarations
            else if (trimmed.StartsWith("namespace ") || trimmed.StartsWith("package "))
            {
                entities.Add(new JObject
                {
                    ["name"] = trimmed.Split(' ')[1],
                    ["type"] = "namespace",
                    ["classes"] = new JArray()
                });
            }
            // Handle interface declarations
            else if (trimmed.StartsWith("interface "))
            {
                var interfaceName = trimmed.Substring(10).Split('{')[0].Trim();
                entities.Add(new JObject
                {
                    ["name"] = interfaceName,
                    ["type"] = "interface",
                    ["methods"] = new JArray(),
                    ["properties"] = new JArray(),
                    ["genericTypes"] = ExtractGenericTypes(interfaceName)
                });
            }
            // Handle enum declarations
            else if (trimmed.StartsWith("enum "))
            {
                var enumName = trimmed.Substring(5).Split('{')[0].Trim();
                entities.Add(new JObject
                {
                    ["name"] = enumName,
                    ["type"] = "enumeration",
                    ["values"] = new JArray(),
                    ["baseType"] = ExtractEnumBaseType(trimmed)
                });
            }
            // Handle member declarations with enhanced parsing
            else if (trimmed.Contains(" : ") && entities.Count > 0)
            {
                var lastEntity = entities[entities.Count - 1] as JObject;
                ParseComplexClassMember(trimmed, lastEntity);
            }
            // Handle enum values
            else if (entities.Count > 0 && (entities[entities.Count - 1] as JObject)["type"].ToString() == "enumeration")
            {
                var lastEntity = entities[entities.Count - 1] as JObject;
                if (!string.IsNullOrWhiteSpace(trimmed) && !trimmed.Contains("{") && !trimmed.Contains("}"))
                {
                    var enumValues = lastEntity["values"] as JArray;
                    ParseEnumValue(trimmed, enumValues);
                }
            }
        }
        return entities;
    }

    private string DetermineClassType(string classDeclaration)
    {
        if (classDeclaration.Contains("<<interface>>")) return "interface";
        if (classDeclaration.Contains("<<abstract>>")) return "abstract_class";
        if (classDeclaration.Contains("<<enum>>")) return "enumeration";
        if (classDeclaration.Contains("<<utility>>")) return "utility_class";
        if (classDeclaration.Contains("<<entity>>")) return "entity_class";
        return "class";
    }

    private JArray ExtractAnnotations(string declaration)
    {
        var annotations = new JArray();
        var matches = System.Text.RegularExpressions.Regex.Matches(declaration, @"<<([^>]+)>>");
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            annotations.Add(match.Groups[1].Value);
        }
        return annotations;
    }

    private JArray ExtractGenericTypes(string typeName)
    {
        var genericTypes = new JArray();
        var match = System.Text.RegularExpressions.Regex.Match(typeName, @"<([^>]+)>");
        if (match.Success)
        {
            var types = match.Groups[1].Value.Split(',');
            foreach (var type in types)
            {
                genericTypes.Add(type.Trim());
            }
        }
        return genericTypes;
    }

    private JArray ExtractClassModifiers(string declaration)
    {
        var modifiers = new JArray();
        var modifierKeywords = new[] { "public", "private", "protected", "internal", "abstract", "static", "final", "sealed", "virtual", "override" };
        
        foreach (var modifier in modifierKeywords)
        {
            if (declaration.Contains(modifier + " "))
            {
                modifiers.Add(modifier);
            }
        }
        return modifiers;
    }

    private string ExtractPackage(string content, string className)
    {
        var lines = content.Split('\n');
        foreach (var line in lines)
        {
            if (line.Trim().StartsWith("package ") || line.Trim().StartsWith("namespace "))
            {
                return line.Trim().Split(' ')[1];
            }
        }
        return null;
    }

    private string ExtractEnumBaseType(string enumDeclaration)
    {
        var match = System.Text.RegularExpressions.Regex.Match(enumDeclaration, @":\s*(\w+)");
        return match.Success ? match.Groups[1].Value : "int";
    }

    private void ParseComplexClassMember(string memberLine, JObject classEntity)
    {
        var parts = memberLine.Split(':');
        var signature = parts[0].Trim();
        var type = parts.Length > 1 ? parts[1].Trim() : "void";
        
        var visibility = GetVisibility(signature);
        var modifiers = ExtractMemberModifiers(signature);
        var cleanSignature = CleanSignature(signature);
        
        // Determine member type
        if (memberLine.Contains("()") || memberLine.Contains("(") && memberLine.Contains(")"))
        {
            // Method or constructor
            if (cleanSignature == classEntity["name"].ToString())
            {
                // Constructor
                var constructors = classEntity["constructors"] as JArray;
                constructors.Add(new JObject
                {
                    ["signature"] = cleanSignature,
                    ["parameters"] = ExtractParameters(memberLine),
                    ["visibility"] = visibility,
                    ["modifiers"] = modifiers,
                    ["isOverloaded"] = IsOverloaded(constructors, cleanSignature)
                });
            }
            else
            {
                // Method
                var methods = classEntity["methods"] as JArray;
                methods.Add(new JObject
                {
                    ["name"] = cleanSignature.Split('(')[0],
                    ["returnType"] = type,
                    ["parameters"] = ExtractParameters(memberLine),
                    ["visibility"] = visibility,
                    ["modifiers"] = modifiers,
                    ["isOverloaded"] = IsOverloaded(methods, cleanSignature.Split('(')[0]),
                    ["genericTypes"] = ExtractGenericTypes(memberLine),
                    ["exceptions"] = ExtractExceptions(memberLine)
                });
            }
        }
        else if (memberLine.Contains("get") || memberLine.Contains("set"))
        {
            // Property
            var properties = classEntity["properties"] as JArray;
            properties.Add(new JObject
            {
                ["name"] = cleanSignature,
                ["type"] = type,
                ["visibility"] = visibility,
                ["modifiers"] = modifiers,
                ["hasGetter"] = memberLine.Contains("get"),
                ["hasSetter"] = memberLine.Contains("set"),
                ["isAutoProperty"] = memberLine.Contains("{ get; set; }")
            });
        }
        else
        {
            // Field
            var fields = classEntity["fields"] as JArray;
            fields.Add(new JObject
            {
                ["name"] = cleanSignature,
                ["type"] = type,
                ["visibility"] = visibility,
                ["modifiers"] = modifiers,
                ["isConstant"] = modifiers.Contains("const") || modifiers.Contains("final"),
                ["defaultValue"] = ExtractDefaultValue(memberLine)
            });
        }
    }

    private JArray ExtractMemberModifiers(string signature)
    {
        var modifiers = new JArray();
        var modifierKeywords = new[] { "static", "abstract", "virtual", "override", "const", "readonly", "async", "extern" };
        
        foreach (var modifier in modifierKeywords)
        {
            if (signature.Contains(modifier + " "))
            {
                modifiers.Add(modifier);
            }
        }
        return modifiers;
    }

    private string CleanSignature(string signature)
    {
        return signature.TrimStart('+', '-', '#', '~').Trim()
                       .Replace("static ", "")
                       .Replace("abstract ", "")
                       .Replace("virtual ", "")
                       .Replace("override ", "")
                       .Replace("const ", "")
                       .Replace("readonly ", "")
                       .Replace("async ", "")
                       .Replace("extern ", "");
    }

    private JArray ExtractParameters(string methodSignature)
    {
        var parameters = new JArray();
        var match = System.Text.RegularExpressions.Regex.Match(methodSignature, @"\(([^)]*)\)");
        if (match.Success && !string.IsNullOrWhiteSpace(match.Groups[1].Value))
        {
            var paramList = match.Groups[1].Value.Split(',');
            foreach (var param in paramList)
            {
                var paramParts = param.Trim().Split(' ');
                if (paramParts.Length >= 2)
                {
                    parameters.Add(new JObject
                    {
                        ["name"] = paramParts[1],
                        ["type"] = paramParts[0],
                        ["isOptional"] = param.Contains("="),
                        ["defaultValue"] = ExtractParamDefaultValue(param)
                    });
                }
            }
        }
        return parameters;
    }

    private bool IsOverloaded(JArray memberArray, string memberName)
    {
        int count = 0;
        foreach (var member in memberArray)
        {
            var name = member["name"]?.ToString() ?? member["signature"]?.ToString();
            if (name != null && name.StartsWith(memberName))
            {
                count++;
            }
        }
        return count > 0;
    }

    private JArray ExtractExceptions(string methodSignature)
    {
        var exceptions = new JArray();
        var match = System.Text.RegularExpressions.Regex.Match(methodSignature, @"throws\s+([^{]+)");
        if (match.Success)
        {
            var exceptionList = match.Groups[1].Value.Split(',');
            foreach (var exception in exceptionList)
            {
                exceptions.Add(exception.Trim());
            }
        }
        return exceptions;
    }

    private string ExtractParamDefaultValue(string parameter)
    {
        var match = System.Text.RegularExpressions.Regex.Match(parameter, @"=\s*(.+)");
        return match.Success ? match.Groups[1].Value.Trim() : null;
    }

    private void ParseEnumValue(string enumLine, JArray enumValues)
    {
        var trimmed = enumLine.Trim().TrimEnd(',');
        var parts = trimmed.Split('=');
        
        enumValues.Add(new JObject
        {
            ["name"] = parts[0].Trim(),
            ["value"] = parts.Length > 1 ? parts[1].Trim() : null,
            ["comment"] = ExtractComment(enumLine)
        });
    }

    private JArray ExtractFlowchartEntities(string content)
    {
        var entities = new JArray();
        var lines = content.Split('\n');
        var nodeNames = new HashSet<string>();
        var subgraphs = new List<JObject>();
        var currentSubgraph = (JObject)null;
        var subgraphStack = new Stack<JObject>();

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            
            // Handle subgraph declarations
            if (trimmed.StartsWith("subgraph "))
            {
                var subgraphInfo = ParseSubgraphDeclaration(trimmed);
                currentSubgraph = new JObject
                {
                    ["name"] = subgraphInfo.name,
                    ["type"] = "subgraph",
                    ["title"] = subgraphInfo.title,
                    ["direction"] = subgraphInfo.direction,
                    ["nodes"] = new JArray(),
                    ["edges"] = new JArray(),
                    ["styling"] = subgraphInfo.styling,
                    ["level"] = subgraphStack.Count
                };
                
                if (subgraphStack.Count > 0)
                {
                    // Nested subgraph
                    var parentSubgraph = subgraphStack.Peek();
                    var parentNodes = parentSubgraph["nodes"] as JArray;
                    parentNodes.Add(currentSubgraph);
                }
                else
                {
                    // Top-level subgraph
                    entities.Add(currentSubgraph);
                }
                
                subgraphStack.Push(currentSubgraph);
                subgraphs.Add(currentSubgraph);
            }
            // Handle subgraph end
            else if (trimmed == "end")
            {
                if (subgraphStack.Count > 0)
                {
                    subgraphStack.Pop();
                    currentSubgraph = subgraphStack.Count > 0 ? subgraphStack.Peek() : null;
                }
            }
            // Handle node connections and declarations
            else if (trimmed.Contains("-->") || trimmed.Contains("---") || trimmed.Contains("-.->") || trimmed.Contains("==>"))
            {
                var connectionInfo = ParseFlowchartConnection(trimmed);
                
                // Add source and target nodes if not already present
                foreach (var nodeName in new[] { connectionInfo.source, connectionInfo.target })
                {
                    if (!string.IsNullOrEmpty(nodeName) && !nodeNames.Contains(nodeName))
                    {
                        nodeNames.Add(nodeName);
                        var nodeInfo = ParseNodeDeclaration(nodeName);
                        var node = new JObject
                        {
                            ["name"] = nodeInfo.name,
                            ["type"] = "node",
                            ["shape"] = nodeInfo.shape,
                            ["label"] = nodeInfo.label,
                            ["styling"] = nodeInfo.styling,
                            ["nodeType"] = nodeInfo.nodeType
                        };
                        
                        if (currentSubgraph != null)
                        {
                            var subgraphNodes = currentSubgraph["nodes"] as JArray;
                            subgraphNodes.Add(node);
                        }
                        else
                        {
                            entities.Add(node);
                        }
                    }
                }
                
                // Add edge information
                var edge = new JObject
                {
                    ["source"] = connectionInfo.source,
                    ["target"] = connectionInfo.target,
                    ["type"] = connectionInfo.type,
                    ["label"] = connectionInfo.label,
                    ["styling"] = connectionInfo.styling,
                    ["condition"] = connectionInfo.condition
                };
                
                if (currentSubgraph != null)
                {
                    var subgraphEdges = currentSubgraph["edges"] as JArray;
                    subgraphEdges.Add(edge);
                }
                else
                {
                    entities.Add(edge);
                }
            }
            // Handle standalone node declarations
            else if (!string.IsNullOrWhiteSpace(trimmed) && !trimmed.StartsWith("%") && 
                    !trimmed.StartsWith("flowchart") && !trimmed.StartsWith("graph") &&
                    !trimmed.StartsWith("subgraph") && trimmed != "end")
            {
                var nodeInfo = ParseNodeDeclaration(trimmed);
                if (!string.IsNullOrEmpty(nodeInfo.name) && !nodeNames.Contains(nodeInfo.name))
                {
                    nodeNames.Add(nodeInfo.name);
                    var node = new JObject
                    {
                        ["name"] = nodeInfo.name,
                        ["type"] = "node",
                        ["shape"] = nodeInfo.shape,
                        ["label"] = nodeInfo.label,
                        ["styling"] = nodeInfo.styling,
                        ["nodeType"] = nodeInfo.nodeType
                    };
                    
                    if (currentSubgraph != null)
                    {
                        var subgraphNodes = currentSubgraph["nodes"] as JArray;
                        subgraphNodes.Add(node);
                    }
                    else
                    {
                        entities.Add(node);
                    }
                }
            }
        }
        
        return entities;
    }

    private (string name, string title, string direction, JObject styling) ParseSubgraphDeclaration(string subgraphLine)
    {
        var parts = subgraphLine.Substring(9).Trim(); // Remove "subgraph "
        var name = "";
        var title = "";
        var direction = "TB"; // Default direction
        var styling = new JObject();
        
        // Handle different subgraph formats
        if (parts.Contains("[") && parts.Contains("]"))
        {
            // Format: subgraph id [title]
            var bracketStart = parts.IndexOf('[');
            name = parts.Substring(0, bracketStart).Trim();
            var bracketEnd = parts.IndexOf(']');
            title = parts.Substring(bracketStart + 1, bracketEnd - bracketStart - 1);
        }
        else if (parts.Contains("\""))
        {
            // Format: subgraph "title"
            var matches = System.Text.RegularExpressions.Regex.Matches(parts, "\"([^\"]+)\"");
            if (matches.Count > 0)
            {
                title = matches[0].Groups[1].Value;
                name = title.Replace(" ", "_").ToLower();
            }
        }
        else
        {
            // Simple format: subgraph id
            name = parts;
            title = parts;
        }
        
        return (name, title, direction, styling);
    }

    private (string source, string target, string type, string label, JObject styling, string condition) 
        ParseFlowchartConnection(string connectionLine)
    {
        var connectors = new[] { "-->", "---", "-.->", "==>", "-..-", "===" };
        string connector = "";
        string[] parts = null;
        
        foreach (var conn in connectors)
        {
            if (connectionLine.Contains(conn))
            {
                connector = conn;
                var splitIndex = connectionLine.IndexOf(conn);
                if (splitIndex >= 0)
                {
                    parts = new[] { connectionLine.Substring(0, splitIndex), connectionLine.Substring(splitIndex + conn.Length) };
                }
                break;
            }
        }
        
        if (parts == null || parts.Length != 2)
            return ("", "", "", "", new JObject(), "");
        
        var source = ExtractNodeName(parts[0].Trim());
        var target = ExtractNodeName(parts[1].Trim());
        var label = ExtractEdgeLabel(connectionLine);
        var condition = ExtractCondition(connectionLine);
        var styling = ExtractEdgeStyling(connectionLine);
        var type = MapConnectionType(connector);
        
        return (source, target, type, label, styling, condition);
    }

    private (string name, string shape, string label, JObject styling, string nodeType) ParseNodeDeclaration(string nodeDeclaration)
    {
        var name = "";
        var shape = "rectangle";
        var label = "";
        var styling = new JObject();
        var nodeType = "process";
        
        // Extract node name (before any shape declaration)
        name = nodeDeclaration.Split('[')[0].Split('(')[0].Split('{')[0].Split('<')[0].Trim();
        
        // Determine shape and extract label
        if (nodeDeclaration.Contains("[") && nodeDeclaration.Contains("]"))
        {
            // Rectangle
            shape = "rectangle";
            var match = System.Text.RegularExpressions.Regex.Match(nodeDeclaration, @"\[([^\]]+)\]");
            if (match.Success) label = match.Groups[1].Value;
        }
        else if (nodeDeclaration.Contains("(") && nodeDeclaration.Contains(")"))
        {
            // Circle or rounded rectangle
            if (nodeDeclaration.Contains("((") && nodeDeclaration.Contains("))"))
            {
                shape = "circle";
                var match = System.Text.RegularExpressions.Regex.Match(nodeDeclaration, @"\(\(([^)]+)\)\)");
                if (match.Success) label = match.Groups[1].Value;
            }
            else
            {
                shape = "rounded_rectangle";
                var match = System.Text.RegularExpressions.Regex.Match(nodeDeclaration, @"\(([^)]+)\)");
                if (match.Success) label = match.Groups[1].Value;
            }
        }
        else if (nodeDeclaration.Contains("{") && nodeDeclaration.Contains("}"))
        {
            // Diamond (decision)
            shape = "diamond";
            nodeType = "decision";
            var match = System.Text.RegularExpressions.Regex.Match(nodeDeclaration, @"\{([^}]+)\}");
            if (match.Success) label = match.Groups[1].Value;
        }
        else if (nodeDeclaration.Contains("[[") && nodeDeclaration.Contains("]]"))
        {
            // Subroutine
            shape = "subroutine";
            nodeType = "subroutine";
            var match = System.Text.RegularExpressions.Regex.Match(nodeDeclaration, @"\[\[([^\]]+)\]\]");
            if (match.Success) label = match.Groups[1].Value;
        }
        else if (nodeDeclaration.Contains(">") && nodeDeclaration.Contains("]"))
        {
            // Flag
            shape = "flag";
            nodeType = "flag";
            var match = System.Text.RegularExpressions.Regex.Match(nodeDeclaration, @">([^]]+)\]");
            if (match.Success) label = match.Groups[1].Value;
        }
        
        if (string.IsNullOrEmpty(label))
            label = name;
        
        return (name, shape, label, styling, nodeType);
    }

    private string ExtractNodeName(string nodePart)
    {
        return nodePart.Split('[')[0].Split('(')[0].Split('{')[0].Split('<')[0].Trim();
    }

    private string ExtractEdgeLabel(string connectionLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(connectionLine, @"\|([^|]+)\|");
        return match.Success ? match.Groups[1].Value : "";
    }

    private string ExtractCondition(string connectionLine)
    {
        var match = System.Text.RegularExpressions.Regex.Match(connectionLine, @"--\s*([^-]+)\s*-->");
        return match.Success ? match.Groups[1].Value : "";
    }

    private JObject ExtractEdgeStyling(string connectionLine)
    {
        var styling = new JObject();
        
        if (connectionLine.Contains("-."))
            styling["style"] = "dashed";
        else if (connectionLine.Contains("=="))
            styling["style"] = "thick";
        else
            styling["style"] = "solid";
            
        return styling;
    }

    private string MapConnectionType(string connector)
    {
        switch (connector)
        {
            case "-->": return "directed";
            case "---": return "undirected";
            case "-.->": return "dotted_directed";
            case "==>": return "thick_directed";
            case "-..-": return "dotted_undirected";
            case "===": return "thick_undirected";
            default: return "directed";
        }
    }

    private JArray ExtractGenericEntities(string content)
    {
        var entities = new JArray();
        var lines = content.Split('\n');

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (!string.IsNullOrEmpty(trimmed) && !trimmed.StartsWith("%") && !trimmed.StartsWith("title"))
            {
                var words = trimmed.Split(' ');
                if (words.Length > 0 && !string.IsNullOrEmpty(words[0]))
                {
                    entities.Add(new JObject
                    {
                        ["name"] = words[0],
                        ["type"] = "element"
                    });
                }
            }
        }
        return entities;
    }

    private JArray ExtractSequenceEntities(string content)
    {
        var entities = new JArray();
        var lines = content.Split('\n');
        var participants = new Dictionary<string, JObject>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            var lineNumber = i + 1;

            // Parse participants
            if (line.StartsWith("participant ") || line.StartsWith("actor "))
            {
                var parts = line.Split(new char[] { ' ' }, 2);
                var type = parts[0];
                var name = parts.Length > 1 ? parts[1] : "";
                var alias = name;

                if (parts.Length > 3 && parts[2] == "as")
                {
                    var remainingParts = new string[parts.Length - 3];
                    for (int j = 3; j < parts.Length; j++)
                    {
                        remainingParts[j - 3] = parts[j];
                    }
                    alias = string.Join(" ", remainingParts);
                }

                if (!participants.ContainsKey(name))
                {
                    participants[name] = new JObject
                    {
                        ["name"] = name,
                        ["type"] = type,
                        ["alias"] = alias,
                        ["activations"] = new JArray(),
                        ["interactions"] = new JArray(),
                        ["line"] = lineNumber
                    };
                }
            }
            // Parse messages/interactions
            else if (line.Contains("->") || line.Contains("->>") || line.Contains("-->>") || line.Contains("-x"))
            {
                var arrows = new[] { "->>", "-->>", "->", "--x", "-x" };
                string arrow = "";
                string[] parts = null;

                foreach (var arr in arrows)
                {
                    if (line.Contains(arr))
                    {
                        arrow = arr;
                        var splitIndex = line.IndexOf(arr);
                        if (splitIndex >= 0)
                        {
                            parts = new[] { line.Substring(0, splitIndex), line.Substring(splitIndex + arr.Length) };
                        }
                        break;
                    }
                }

                if (parts != null && parts.Length == 2)
                {
                    var from = parts[0].Trim();
                    var toAndMessage = parts[1].Trim();
                    var spaceIndex = toAndMessage.IndexOf(' ');
                    var to = spaceIndex > 0 ? toAndMessage.Substring(0, spaceIndex) : toAndMessage;
                    var message = spaceIndex > 0 ? toAndMessage.Substring(spaceIndex + 1) : "";

                    // Add participants if not already defined
                    foreach (var participant in new[] { from, to })
                    {
                        if (!participants.ContainsKey(participant))
                        {
                            participants[participant] = new JObject
                            {
                                ["name"] = participant,
                                ["type"] = "participant",
                                ["alias"] = participant,
                                ["activations"] = new JArray(),
                                ["interactions"] = new JArray(),
                                ["line"] = lineNumber
                            };
                        }
                    }

                    // Add interaction
                    var interaction = new JObject
                    {
                        ["from"] = from,
                        ["to"] = to,
                        ["message"] = message,
                        ["type"] = GetMessageType(arrow),
                        ["line"] = lineNumber
                    };

                    var fromInteractions = participants[from]["interactions"] as JArray;
                    fromInteractions.Add(interaction);
                }
            }
            // Parse fragments
            else if (line.StartsWith("loop ") || line.StartsWith("alt ") || line.StartsWith("opt "))
            {
                var fragmentType = line.Split(' ')[0];
                var condition = line.Substring(fragmentType.Length + 1).Trim();
                
                entities.Add(new JObject
                {
                    ["name"] = $"{fragmentType}_{lineNumber}",
                    ["type"] = "fragment",
                    ["fragmentType"] = fragmentType,
                    ["condition"] = condition,
                    ["line"] = lineNumber
                });
            }
        }

        // Convert participants to entities
        foreach (var participant in participants.Values)
        {
            entities.Add(participant);
        }

        return entities;
    }

    private string GetMessageType(string arrow)
    {
        switch (arrow)
        {
            case "->": return "sync";
            case "->>": return "async";
            case "-->>": return "response";
            case "--x": return "destroy";
            case "-x": return "destroy";
            default: return "sync";
        }
    }

    private JArray ExtractSequenceRelationships(string content)
    {
        var relationships = new JArray();
        var lines = content.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            
            if (line.Contains("->") || line.Contains("->>") || line.Contains("-->>") || line.Contains("-x"))
            {
                var arrows = new[] { "->>", "-->>", "->", "--x", "-x" };
                string arrow = "";
                string[] parts = null;

                foreach (var arr in arrows)
                {
                    if (line.Contains(arr))
                    {
                        arrow = arr;
                        var splitIndex = line.IndexOf(arr);
                        if (splitIndex >= 0)
                        {
                            parts = new[] { line.Substring(0, splitIndex), line.Substring(splitIndex + arr.Length) };
                        }
                        break;
                    }
                }

                if (parts != null && parts.Length == 2)
                {
                    var from = parts[0].Trim();
                    var toAndMessage = parts[1].Trim();
                    var spaceIndex = toAndMessage.IndexOf(' ');
                    var to = spaceIndex > 0 ? toAndMessage.Substring(0, spaceIndex) : toAndMessage;
                    var message = spaceIndex > 0 ? toAndMessage.Substring(spaceIndex + 1) : "";

                    relationships.Add(new JObject
                    {
                        ["from"] = from,
                        ["to"] = to,
                        ["type"] = GetMessageType(arrow),
                        ["message"] = message,
                        ["line"] = i + 1
                    });
                }
            }
        }

        return relationships;
    }

    private JArray ExtractERDRelationships(string content)
    {
        var relationships = new JArray();
        
        // Normalize content first to handle single-line input
        content = NormalizeERDContent(content);
        
        var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        var relationshipTypes = new[] { "||--||", "||--o{", "}o--||", "||--|{", "}|--||", "||--o|" };

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            
            // Skip entity definitions and diagram declaration
            if (trimmed == "erDiagram" || trimmed.Contains("{") || trimmed.Contains("}") || string.IsNullOrWhiteSpace(trimmed))
                continue;
                
            foreach (var relType in relationshipTypes)
            {
                if (trimmed.Contains(relType))
                {
                    var splitIndex = trimmed.IndexOf(relType);
                    if (splitIndex >= 0)
                    {
                        var leftPart = trimmed.Substring(0, splitIndex).Trim();
                        var rightPart = trimmed.Substring(splitIndex + relType.Length).Trim();
                        
                        // Extract just the entity names, handle labels like "ORDER : places"
                        var toEntity = rightPart.Contains(":") ? rightPart.Split(':')[0].Trim() : rightPart.Split(' ')[0].Trim();
                        var label = rightPart.Contains(":") ? rightPart.Split(':')[1].Trim() : "";
                        
                        if (!string.IsNullOrEmpty(leftPart) && !string.IsNullOrEmpty(toEntity))
                        {
                            relationships.Add(new JObject
                            {
                                ["from"] = leftPart,
                                ["to"] = toEntity,
                                ["type"] = MapERDRelationType(relType),
                                ["cardinality"] = relType,
                                ["label"] = label
                            });
                        }
                    }
                    break; // Found a match, no need to check other relationship types for this line
                }
            }
        }
        return relationships;
    }

    private JArray ExtractClassRelationships(string content)
    {
        var relationships = new JArray();
        var lines = content.Split('\n');
        var relationshipTypes = new[] { "-->", "..>", "--|>", "...|>" };

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            foreach (var relType in relationshipTypes)
            {
                if (trimmed.Contains(relType))
                {
                    var splitIndex = trimmed.IndexOf(relType);
                    if (splitIndex >= 0)
                    {
                        var parts = new[] { trimmed.Substring(0, splitIndex), trimmed.Substring(splitIndex + relType.Length) };
                        if (parts.Length == 2)
                        {
                            relationships.Add(new JObject
                            {
                                ["from"] = parts[0].Trim(),
                                ["to"] = parts[1].Trim(),
                                ["type"] = MapClassRelationType(relType),
                                ["notation"] = relType
                            });
                        }
                    }
                }
            }
        }
        return relationships;
    }

    private JArray ExtractFlowchartRelationships(string content)
    {
        var relationships = new JArray();
        var lines = content.Split('\n');
        var relationshipTypes = new[] { "-->", "---", "-.->", "==>" };

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            foreach (var relType in relationshipTypes)
            {
                if (trimmed.Contains(relType))
                {
                    var splitIndex = trimmed.IndexOf(relType);
                    if (splitIndex >= 0)
                    {
                        var parts = new[] { trimmed.Substring(0, splitIndex), trimmed.Substring(splitIndex + relType.Length) };
                        if (parts.Length >= 2)
                        {
                            var fromNode = parts[0].Split('[')[0].Split('(')[0].Split('{')[0].Trim();
                            var toNode = parts[1].Split('[')[0].Split('(')[0].Split('{')[0].Trim();

                            if (!string.IsNullOrEmpty(fromNode) && !string.IsNullOrEmpty(toNode))
                            {
                                relationships.Add(new JObject
                                {
                                    ["from"] = fromNode,
                                    ["to"] = toNode,
                                    ["type"] = "flow",
                                    ["notation"] = relType
                                });
                            }
                        }
                    }
                }
            }
        }
        return relationships;
    }

    public string ConvertToFormat(string content, string format, JArray entities = null, JArray relationships = null)
    {
        if (entities == null)
            entities = ExtractEntitiesForDiagram(content);

        if (relationships == null)
            relationships = new JArray();

        switch (format.ToLower())
        {
            case "jsonschema": return GenerateJsonSchema(entities, relationships);
            case "sqlddl": return GenerateSqlDdl(entities, relationships);
            case "typescript": return GenerateTypeScript(entities, relationships);
            case "csharp": return GenerateCSharp(entities, relationships);
            case "python": return GeneratePython(entities, relationships);
            case "java": return GenerateJava(entities, relationships);
            case "go": return GenerateGo(entities, relationships);
            case "rust": return GenerateRust(entities, relationships);
            case "graphql": return GenerateGraphQL(entities, relationships);
            case "react": return GenerateReact(entities, relationships);
            case "vue": return GenerateVue(entities, relationships);
            case "swift": return GenerateSwift(entities, relationships);
            case "dataverseschema": return GenerateDataverseSchema(entities, relationships);
            case "powerappsformulas": return GeneratePowerAppsFormulas(entities, relationships);
            case "postgresqlddl": return GeneratePostgreSQLDdl(entities, relationships);
            case "mysqlddl": return GenerateMySQLDdl(entities, relationships);
            case "mongooseschema": return GenerateMongooseSchema(entities, relationships);
            case "markdown": return GenerateMarkdown(entities, relationships);
            
            // Legacy format names for backward compatibility
            case "json_schema": return GenerateJsonSchema(entities, relationships);
            case "sql_ddl": return GenerateSqlDdl(entities, relationships);
            
            // Multi-format integration (legacy)
            case "plantuml": return GeneratePlantUML(entities, "flowchart");
            case "bpmn": return GenerateBPMN(entities);
            case "html": return GenerateHTML(entities, "diagram");
            case "confluence": return GenerateConfluence(entities, "diagram");
            case "notion": return GenerateNotion(entities, "diagram");
            
            default: return JsonConvert.SerializeObject(entities, Newtonsoft.Json.Formatting.Indented);
        }
    }

    public JObject ValidateMermaidSyntax(string content)
    {
        var validation = new JObject
        {
            ["isValid"] = true,
            ["errors"] = new JArray(),
            ["warnings"] = new JArray()
        };

        var lines = content.Split('\n');
        var warnings = validation["warnings"] as JArray;

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var openBrackets = line.Count(c => c == '{' || c == '(' || c == '[');
            var closeBrackets = line.Count(c => c == '}' || c == ')' || c == ']');

            if (openBrackets != closeBrackets)
                warnings.Add($"Line {i + 1}: Unmatched brackets - {line}");
        }

        return validation;
    }

    // Format Generators (Enhanced with relationship support)
    private string GenerateJsonSchema(JArray entities, JArray relationships = null)
    {
        var schema = new JObject
        {
            ["$schema"] = "http://json-schema.org/draft-07/schema#",
            ["title"] = "Generated Schema",
            ["type"] = "object", 
            ["definitions"] = new JObject()
        };

        var definitions = schema["definitions"] as JObject;

        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            var properties = new JObject();

            // Get attributes (using both "attributes" and "fields" for compatibility)
            var attributes = entityObj["attributes"] as JArray ?? entityObj["fields"] as JArray ?? new JArray();
            
            foreach (var attr in attributes)
            {
                var attrObj = attr as JObject;
                var attrName = attrObj["name"]?.ToString();
                var dataType = attrObj["dataType"]?.ToString() ?? attrObj["type"]?.ToString();
                
                properties[attrName] = new JObject
                {
                    ["type"] = MapTypeToJsonSchema(dataType),
                    ["description"] = $"{attrName} field of {entityName}"
                };
            }

            definitions[entityName] = new JObject
            {
                ["type"] = "object",
                ["properties"] = properties
            };
        }

        return JsonConvert.SerializeObject(schema, Newtonsoft.Json.Formatting.Indented);
    }

    private string MapTypeToJsonSchema(string type)
    {
        if (string.IsNullOrEmpty(type)) return "string";
        switch (type.ToLower())
        {
            case "int": case "integer": return "integer";
            case "float": case "double": case "decimal": return "number";
            case "bool": case "boolean": return "boolean";
            case "array": case "list": return "array";
            default: return "string";
        }
    }

    private string GenerateSqlDdl(JArray entities, JArray relationships = null)
    {
        var sql = new StringBuilder("-- Generated SQL DDL\n\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            sql.AppendLine($"CREATE TABLE {entityName} (");
            
            // Get attributes (using both "attributes" and "fields" for compatibility)
            var attributes = entityObj["attributes"] as JArray ?? entityObj["fields"] as JArray ?? new JArray();
            var fieldStatements = new List<string>();
            
            foreach (var attr in attributes)
            {
                var attrObj = attr as JObject;
                var attrName = attrObj["name"]?.ToString();
                var dataType = attrObj["dataType"]?.ToString() ?? attrObj["type"]?.ToString();
                var isPrimaryKey = SafeGetBool(attrObj, "isPrimaryKey");
                
                var sqlType = MapTypeToSQL(dataType);
                var constraints = isPrimaryKey ? " PRIMARY KEY" : "";
                
                fieldStatements.Add($"    {attrName} {sqlType}{constraints}");
            }
            
            if (fieldStatements.Count == 0)
            {
                fieldStatements.Add("    id INT PRIMARY KEY");
            }
            
            sql.AppendLine(string.Join(",\n", fieldStatements));
            sql.AppendLine(");");
            sql.AppendLine();
        }
        
        return sql.ToString();
    }

    private string GenerateTypeScript(JArray entities, JArray relationships = null)
    {
        var ts = new StringBuilder("// Generated TypeScript\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            // Check for inheritance
            var parentEntity = GetParentEntityName(entityName, relationships);
            var interfaceDecl = !string.IsNullOrEmpty(parentEntity) ? 
                $"interface {entityName} extends {parentEntity}" : 
                $"interface {entityName}";
                
            ts.AppendLine($"{interfaceDecl} {{");
            
            // Get attributes (using both "attributes" and "fields" for compatibility)
            var attributes = entityObj["attributes"] as JArray ?? entityObj["fields"] as JArray ?? new JArray();
            foreach (var attr in attributes)
            {
                var attrObj = attr as JObject;
                var attrName = attrObj["name"]?.ToString();
                var dataType = attrObj["dataType"]?.ToString() ?? attrObj["type"]?.ToString();
                ts.AppendLine($"  {attrName}: {MapTypeToTypeScript(dataType)};");
            }
            
            ts.AppendLine("}\n");
        }
        return ts.ToString();
    }

    private string GetParentEntityName(string entityName, JArray relationships)
    {
        if (relationships == null) return null;
        
        foreach (var rel in relationships)
        {
            var relObj = rel as JObject;
            var relType = relObj["type"]?.ToString()?.ToLower();
            var fromEntity = relObj["from"]?.ToString();
            var toEntity = relObj["to"]?.ToString();
            
            // Check for inheritance relationships
            if (relType == "inheritance" && toEntity == entityName)
            {
                return fromEntity;
            }
        }
        return null;
    }

    private bool SafeGetBool(JObject obj, string propertyName)
    {
        var token = obj[propertyName];
        if (token == null || token.Type == JTokenType.Null)
            return false;
        
        if (token.Type == JTokenType.Boolean)
            return token.ToObject<bool>();
        
        // Try to parse string values
        var stringValue = token.ToString().ToLower();
        return stringValue == "true" || stringValue == "1";
    }

    private string GenerateCSharp(JArray entities, JArray relationships = null)
    {
        var cs = new StringBuilder("// Generated C#\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            // Check for inheritance
            var parentEntity = GetParentEntity(entityName, relationships);
            var classDecl = !string.IsNullOrEmpty(parentEntity) ? 
                $"public class {entityName} : {parentEntity}" : 
                $"public class {entityName}";
                
            cs.AppendLine($"{classDecl} {{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                cs.AppendLine($"  public {MapTypeToCSharp(fieldType)} {fieldName} {{ get; set; }}");
            }
            
            cs.AppendLine("}\n");
        }
        return cs.ToString();
    }

    private string GeneratePython(JArray entities, JArray relationships = null)
    {
        var py = new StringBuilder("# Generated Python\nfrom dataclasses import dataclass\n\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            py.AppendLine("@dataclass");
            
            // Check for inheritance
            var parentEntity = GetParentEntity(entityName, relationships);
            var classDecl = !string.IsNullOrEmpty(parentEntity) ? 
                $"class {entityName}({parentEntity}):" : 
                $"class {entityName}:";
                
            py.AppendLine(classDecl);
            
            var fields = GetEntityFields(entityObj);
            if (fields.Count == 0)
            {
                py.AppendLine("  pass");
            }
            else
            {
                foreach (var field in fields)
                {
                    var fieldObj = field as JObject;
                    var fieldName = fieldObj["name"]?.ToString();
                    var fieldType = fieldObj["type"]?.ToString();
                    py.AppendLine($"  {fieldName}: {MapTypeToPython(fieldType)}");
                }
            }
            
            py.AppendLine("");
        }
        return py.ToString();
    }

    private string GenerateGraphQL(JArray entities, JArray relationships = null)
    {
        var gql = new StringBuilder("# Generated GraphQL\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            gql.AppendLine($"type {entityName} {{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                gql.AppendLine($"  {fieldName}: {MapTypeToGraphQL(fieldType)}");
            }
            
            gql.AppendLine("}\n");
        }
        return gql.ToString();
    }

    private string GenerateMarkdown(JArray entities, JArray relationships = null)
    {
        var md = new StringBuilder("# Entity Documentation\n\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            md.AppendLine($"## {entityName}\n");
            
            // Show inheritance if applicable
            var parentEntity = GetParentEntity(entityName, relationships);
            if (!string.IsNullOrEmpty(parentEntity))
            {
                md.AppendLine($"*Inherits from: {parentEntity}*\n");
            }
            
            var fields = GetEntityFields(entityObj);
            if (fields.Count > 0)
            {
                md.AppendLine("| Field | Type |");
                md.AppendLine("|-------|------|");
                foreach (var field in fields)
                {
                    var fieldObj = field as JObject;
                    var fieldName = fieldObj["name"]?.ToString();
                    var fieldType = fieldObj["type"]?.ToString();
                    md.AppendLine($"| {fieldName} | {fieldType} |");
                }
            }
            
            md.AppendLine("");
        }
        return md.ToString();
    }

    // Additional Format Generators for all 18 formats
    private string GenerateJava(JArray entities, JArray relationships = null)
    {
        var java = new StringBuilder("// Generated Java\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            var parentEntity = GetParentEntity(entityName, relationships);
            var classDecl = !string.IsNullOrEmpty(parentEntity) ? 
                $"public class {entityName} extends {parentEntity}" : 
                $"public class {entityName}";
                
            java.AppendLine($"{classDecl} {{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                java.AppendLine($"    private {MapTypeToJava(fieldType)} {fieldName};");
            }
            
            java.AppendLine("}\\n");
        }
        return java.ToString();
    }

    private string GenerateGo(JArray entities, JArray relationships = null)
    {
        var go = new StringBuilder("// Generated Go\npackage main\n\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            go.AppendLine($"type {entityName} struct {{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                go.AppendLine($"    {fieldName} {MapTypeToGo(fieldType)} `json:\"{fieldName.ToLower()}\"`");
            }
            
            go.AppendLine("}\\n");
        }
        return go.ToString();
    }

    private string GenerateRust(JArray entities, JArray relationships = null)
    {
        var rust = new StringBuilder("// Generated Rust\nuse serde::{Deserialize, Serialize};\n\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            rust.AppendLine("#[derive(Debug, Serialize, Deserialize)]");
            rust.AppendLine($"pub struct {entityName} {{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                rust.AppendLine($"    pub {fieldName}: {MapTypeToRust(fieldType)},");
            }
            
            rust.AppendLine("}\\n");
        }
        return rust.ToString();
    }

    private string GenerateReact(JArray entities, JArray relationships = null)
    {
        var react = new StringBuilder("// Generated React TypeScript\nimport React from 'react';\n\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            react.AppendLine($"interface {entityName}Props {{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                react.AppendLine($"  {fieldName}: {MapTypeToTypeScript(fieldType)};");
            }
            
            react.AppendLine("}\\n");
        }
        return react.ToString();
    }

    private string GenerateVue(JArray entities, JArray relationships = null)
    {
        var vue = new StringBuilder("// Generated Vue Composition API\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            vue.AppendLine($"export interface {entityName} {{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                vue.AppendLine($"  {fieldName}: {MapTypeToTypeScript(fieldType)}");
            }
            
            vue.AppendLine("}\\n");
        }
        return vue.ToString();
    }

    private string GenerateSwift(JArray entities, JArray relationships = null)
    {
        var swift = new StringBuilder("// Generated Swift\nimport Foundation\n\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            swift.AppendLine($"struct {entityName}: Codable {{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                swift.AppendLine($"    let {fieldName}: {MapTypeToSwift(fieldType)}");
            }
            
            swift.AppendLine("}\\n");
        }
        return swift.ToString();
    }

    private string GenerateDataverseSchema(JArray entities, JArray relationships = null)
    {
        var schema = new StringBuilder("// Generated Dataverse Schema\n");
        // Implementation would generate Dataverse table definitions
        schema.AppendLine("// Dataverse schema generation not yet implemented");
        return schema.ToString();
    }

    private string GeneratePowerAppsFormulas(JArray entities, JArray relationships = null)
    {
        var formulas = new StringBuilder("// Generated Power Apps Formulas\n");
        // Implementation would generate Power Apps collection formulas
        formulas.AppendLine("// Power Apps formula generation not yet implemented");
        return formulas.ToString();
    }

    private string GeneratePostgreSQLDdl(JArray entities, JArray relationships = null)
    {
        return GenerateSqlDdl(entities, relationships).Replace("-- Generated SQL DDL", "-- Generated PostgreSQL DDL");
    }

    private string GenerateMySQLDdl(JArray entities, JArray relationships = null)
    {
        return GenerateSqlDdl(entities, relationships).Replace("-- Generated SQL DDL", "-- Generated MySQL DDL");
    }

    private string GenerateMongooseSchema(JArray entities, JArray relationships = null)
    {
        var mongoose = new StringBuilder("// Generated Mongoose Schema\nconst mongoose = require('mongoose');\n\n");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityName = entityObj["name"]?.ToString();
            
            mongoose.AppendLine($"const {entityName}Schema = new mongoose.Schema({{");
            
            var fields = GetEntityFields(entityObj);
            foreach (var field in fields)
            {
                var fieldObj = field as JObject;
                var fieldName = fieldObj["name"]?.ToString();
                var fieldType = fieldObj["type"]?.ToString();
                mongoose.AppendLine($"  {fieldName}: {MapTypeToMongoose(fieldType)},");
            }
            
            mongoose.AppendLine("});\\n");
            mongoose.AppendLine($"module.exports = mongoose.model('{entityName}', {entityName}Schema);\\n");
        }
        return mongoose.ToString();
    }

    // Helper Methods
    private JArray GetEntityFields(JObject entity)
    {
        // Try both "fields" and "attributes" for compatibility
        var fields = entity["fields"] as JArray;
        if (fields == null)
        {
            var attributes = entity["attributes"] as JArray;
            if (attributes != null)
            {
                // Convert attributes to fields format
                fields = new JArray();
                foreach (var attr in attributes)
                {
                    var attrObj = attr as JObject;
                    fields.Add(new JObject
                    {
                        ["name"] = attrObj["name"],
                        ["type"] = attrObj["dataType"] ?? attrObj["type"],
                        ["isPrimaryKey"] = attrObj["isPrimaryKey"],
                        ["isForeignKey"] = attrObj["isForeignKey"],
                        ["isUnique"] = attrObj["isUnique"]
                    });
                }
            }
        }
        return fields ?? new JArray();
    }

    private string GetParentEntity(string entityName, JArray relationships)
    {
        if (relationships == null) return null;
        
        foreach (var rel in relationships)
        {
            var relObj = rel as JObject;
            var relType = relObj["type"]?.ToString().ToLower();
            var fromEntity = relObj["from"]?.ToString();
            var toEntity = relObj["to"]?.ToString();
            
            // Check for inheritance relationships
            if (relType == "inheritance" && toEntity == entityName)
            {
                return fromEntity;
            }
        }
        return null;
    }

    private JArray GetRequiredFields(JArray fields)
    {
        var required = new JArray();
        foreach (var field in fields)
        {
            var fieldObj = field as JObject;
            var isPrimaryKey = SafeGetBool(fieldObj, "isPrimaryKey");
            
            if (isPrimaryKey)
            {
                required.Add(fieldObj["name"]);
            }
        }
        return required;
    }

    private string GetVisibility(string signature)
    {
        if (signature.StartsWith("+")) return "public";
        if (signature.StartsWith("-")) return "private";
        if (signature.StartsWith("#")) return "protected";
        return "public";
    }

    private string MapERDRelationType(string cardinality)
    {
        switch (cardinality)
        {
            case "||--||": return "one-to-one";
            case "||--o{": return "one-to-many";
            case "}o--||": return "many-to-one";
            case "||--|{": return "one-to-many-required";
            default: return "unknown";
        }
    }

    private string MapClassRelationType(string notation)
    {
        switch (notation)
        {
            case "-->": return "association";
            case "..>": return "dependency";
            case "--|>": return "inheritance";
            case "...|>": return "realization";
            default: return "unknown";
        }
    }

    private int CountEntities(string content)
    {
        return content.Split('\n').Count(line => line.Trim().Contains("{") || line.Trim().StartsWith("class "));
    }

    // Type Mapping Methods
    private string MapTypeToSQL(string type)
    {
        if (string.IsNullOrEmpty(type)) return "VARCHAR(255)";
        switch (type.ToLower())
        {
            case "int": case "integer": return "INT";
            case "float": case "double": return "DECIMAL(10,2)";
            case "bool": case "boolean": return "BOOLEAN";
            case "date": return "DATE";
            case "datetime": return "DATETIME";
            default: return "VARCHAR(255)";
        }
    }

    private string MapTypeToTypeScript(string type)
    {
        if (string.IsNullOrEmpty(type)) return "string";
        switch (type.ToLower())
        {
            case "int": case "integer": case "float": case "double": return "number";
            case "bool": case "boolean": return "boolean";
            case "array": case "list": return "any[]";
            default: return "string";
        }
    }

    private string MapTypeToCSharp(string type)
    {
        if (string.IsNullOrEmpty(type)) return "string";
        switch (type.ToLower())
        {
            case "int": case "integer": return "int";
            case "float": return "float";
            case "double": return "double";
            case "bool": case "boolean": return "bool";
            case "date": return "DateTime";
            default: return "string";
        }
    }

    private string MapTypeToPython(string type)
    {
        if (string.IsNullOrEmpty(type)) return "str";
        switch (type.ToLower())
        {
            case "int": case "integer": return "int";
            case "float": case "double": return "float";
            case "bool": case "boolean": return "bool";
            case "array": case "list": return "List[Any]";
            default: return "str";
        }
    }

    private string MapTypeToGraphQL(string type)
    {
        if (string.IsNullOrEmpty(type)) return "String";
        switch (type.ToLower())
        {
            case "int": case "integer": return "Int";
            case "float": case "double": return "Float";
            case "bool": case "boolean": return "Boolean";
            case "array": case "list": return "[String]";
            default: return "String";
        }
    }

    // Multi-format generators
    private string GeneratePlantUML(JArray entities, string diagramType)
    {
        var sb = new StringBuilder();
        
        switch (diagramType)
        {
            case "classDiagram":
                sb.AppendLine("@startuml");
                foreach (var entity in entities)
                {
                    var entityObj = entity as JObject;
                    if (entityObj["type"].ToString() == "class")
                    {
                        sb.AppendLine($"class {entityObj["name"]} {{");
                        var fields = entityObj["fields"] as JArray;
                        if (fields != null)
                        {
                            foreach (var field in fields)
                            {
                                var fieldObj = field as JObject;
                                var visibility = GetPlantUMLVisibility(fieldObj["visibility"]?.ToString());
                                sb.AppendLine($"  {visibility}{fieldObj["name"]} : {fieldObj["type"]}");
                            }
                        }
                        sb.AppendLine("}");
                    }
                }
                sb.AppendLine("@enduml");
                break;
                
            case "sequenceDiagram":
                sb.AppendLine("@startuml");
                foreach (var entity in entities)
                {
                    var entityObj = entity as JObject;
                    if (entityObj["type"].ToString() == "participant")
                    {
                        sb.AppendLine($"participant {entityObj["name"]}");
                    }
                }
                sb.AppendLine("@enduml");
                break;
                
            default:
                sb.AppendLine("@startuml");
                sb.AppendLine("' Converted from Mermaid");
                sb.AppendLine("@enduml");
                break;
        }
        
        return sb.ToString();
    }

    private string GenerateBPMN(JArray entities)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<definitions xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\">");
        sb.AppendLine("  <process id=\"process1\" isExecutable=\"false\">");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            var entityType = entityObj["type"]?.ToString();
            
            if (entityType == "node")
            {
                var nodeType = entityObj["nodeType"]?.ToString() ?? "task";
                sb.AppendLine($"    <{GetBPMNElementType(nodeType)} id=\"{entityObj["name"]}\" name=\"{entityObj["label"] ?? entityObj["name"]}\"/>");
            }
        }
        
        sb.AppendLine("  </process>");
        sb.AppendLine("</definitions>");
        return sb.ToString();
    }

    private string GenerateHTML(JArray entities, string diagramType)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("  <title>Mermaid Diagram Export</title>");
        sb.AppendLine("  <style>");
        sb.AppendLine("    body { font-family: Arial, sans-serif; margin: 20px; }");
        sb.AppendLine("    .entity { border: 1px solid #ccc; margin: 10px; padding: 10px; }");
        sb.AppendLine("    .relationship { color: #666; font-style: italic; }");
        sb.AppendLine("  </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine($"  <h1>{diagramType} Export</h1>");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            sb.AppendLine("  <div class=\"entity\">");
            sb.AppendLine($"    <h3>{entityObj["name"]}</h3>");
            sb.AppendLine($"    <p>Type: {entityObj["type"]}</p>");
            sb.AppendLine("  </div>");
        }
        
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

    private string GenerateConfluence(JArray entities, string diagramType)
    {
        var sb = new StringBuilder();
        sb.AppendLine("h1. Mermaid Diagram Export");
        sb.AppendLine("");
        sb.AppendLine($"*Diagram Type:* {diagramType}");
        sb.AppendLine("");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            sb.AppendLine($"h2. {entityObj["name"]}");
            sb.AppendLine($"*Type:* {entityObj["type"]}");
            sb.AppendLine("");
        }
        
        return sb.ToString();
    }

    private string GenerateNotion(JArray entities, string diagramType)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Mermaid Diagram Export");
        sb.AppendLine("");
        sb.AppendLine($"**Diagram Type:** {diagramType}");
        sb.AppendLine("");
        
        foreach (var entity in entities)
        {
            var entityObj = entity as JObject;
            sb.AppendLine($"## {entityObj["name"]}");
            sb.AppendLine($"- **Type:** {entityObj["type"]}");
            sb.AppendLine("");
        }
        
        return sb.ToString();
    }

    private string GetPlantUMLVisibility(string visibility)
    {
        switch (visibility?.ToLower())
        {
            case "private": return "-";
            case "protected": return "#";
            case "public": return "+";
            default: return "";
        }
    }

    private string GetBPMNElementType(string nodeType)
    {
        switch (nodeType?.ToLower())
        {
            case "decision": return "exclusiveGateway";
            case "process": return "task";
            case "start": return "startEvent";
            case "end": return "endEvent";
            default: return "task";
        }
    }

    // Additional Type Mapping Methods for all formats
    private string MapTypeToJava(string type)
    {
        if (string.IsNullOrEmpty(type)) return "String";
        switch (type.ToLower())
        {
            case "int": case "integer": return "Integer";
            case "float": return "Float";
            case "double": return "Double";
            case "bool": case "boolean": return "Boolean";
            case "date": return "Date";
            default: return "String";
        }
    }

    private string MapTypeToGo(string type)
    {
        if (string.IsNullOrEmpty(type)) return "string";
        switch (type.ToLower())
        {
            case "int": case "integer": return "int";
            case "float": case "double": return "float64";
            case "bool": case "boolean": return "bool";
            case "date": return "time.Time";
            default: return "string";
        }
    }

    private string MapTypeToRust(string type)
    {
        if (string.IsNullOrEmpty(type)) return "String";
        switch (type.ToLower())
        {
            case "int": case "integer": return "i32";
            case "float": return "f32";
            case "double": return "f64";
            case "bool": case "boolean": return "bool";
            case "date": return "chrono::DateTime<chrono::Utc>";
            default: return "String";
        }
    }

    private string MapTypeToSwift(string type)
    {
        if (string.IsNullOrEmpty(type)) return "String";
        switch (type.ToLower())
        {
            case "int": case "integer": return "Int";
            case "float": case "double": return "Double";
            case "bool": case "boolean": return "Bool";
            case "date": return "Date";
            default: return "String";
        }
    }

    private string MapTypeToMongoose(string type)
    {
        if (string.IsNullOrEmpty(type)) return "String";
        switch (type.ToLower())
        {
            case "int": case "integer": return "Number";
            case "float": case "double": return "Number";
            case "bool": case "boolean": return "Boolean";
            case "date": return "Date";
            default: return "String";
        }
    }
}
