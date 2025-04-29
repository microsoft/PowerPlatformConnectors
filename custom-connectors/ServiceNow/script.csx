public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken);

        if (this.Context.OperationId == "GetCatalogItem")
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseContent);
            var resultCatalogItem = responseJson["result"];

            if (resultCatalogItem != null)
            {
                var schema = GetVariablesSchema(resultCatalogItem);
                if (schema != null)
                {
                    resultCatalogItem["variablesSchema"] = schema;
                    response.Content = CreateJsonContent(JsonConvert.SerializeObject(
                        responseJson,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }
                    ));
                }
            }
        }

        return response;
    }

    private JObject GetVariablesSchema(JToken resultCatalogItem)
    {
        var propertiesSchema = new JObject();
        var requiredProperties = new JArray();
        var schema = new JObject()
        {
            ["type"] = "object",
            ["properties"] = propertiesSchema,
            ["required"] = requiredProperties
        };
        
        var variables = resultCatalogItem?["variables"];
        if (variables != null)
        {
            foreach (var variable in variables)
            {
                var children = variable["children"];

                if (children != null && children.Count() > 0)
                {
                    foreach (var child in children)
                    {
                        HandleVariable(propertiesSchema, requiredProperties, child);
                    }
                }
                else
                {
                    HandleVariable(propertiesSchema, requiredProperties, variable);
                }
            }
        }

        return schema;
    }

    private void HandleVariable(JObject propertiesSchema, JArray requiredProperties, JToken variable)
    {
        var isActive = variable["active"]?.Value<bool>() ?? true; // Default to true if not specified
        if (!isActive) return; // Skip inactive variables

        var propertyName = variable["name"]?.ToString();
        if (!string.IsNullOrEmpty(propertyName))
        {
            propertiesSchema[propertyName] = GenerateSchema(variable);
            if (variable["mandatory"]?.Value<bool>() == true)
            {
                requiredProperties.Add(propertyName);
            }
        }
    }


    private JObject GenerateSchema(JToken variable)
    {
        var variableSchema = new JObject()
        {
            ["type"] = "string",
            ["title"] = variable["label"]?.ToString() ?? variable["name"]?.ToString(),
            ["default"] = variable["value"]?.ToString() ?? string.Empty,
        };

        var choices = variable["choices"];
        if (choices != null && choices.Count() > 0)
        {
            variableSchema["enum"] = new JArray(choices.Select(c => c["value"]?.ToString()));
        }

        if (variable["friendly_type"]?.Value<string>()?.Equals("reference") == true && variable["reference"] != null)
        {
            variableSchema["x-ms-dynamic-values"] = new JObject
            {
                ["operationId"] = "GetRows",
                ["parameters"] = new JObject
                {
                    { "sys_id", variable["reference"] },
                    { "sysparm_limit", 200 }
                },
                ["value-collection"] = "result",
                ["value-title"] = "name",
                ["value-path"] = "sys_id"
            };
        }

        return variableSchema;
    }
}
