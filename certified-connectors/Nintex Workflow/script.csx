public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        switch (this.Context.OperationId)
        {
            case "GetWorkflowDesign":
                return await this.TransformWorkflowDesignSchema();
            case "CreateWorkflowInstance":
                return await this.AttachCallbackUrlToStartInstance();
            default:
                return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }

    private async Task<HttpResponseMessage> TransformWorkflowDesignSchema()
    {
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseBody);

            var startDataArray = (JArray)result["workflow"]["startData"];
            var schema = new JObject();
            var properties = new JObject();

            if (startDataArray.Count == 0)
            {                
                var property = new JObject
                {
                    { "type", "string" },
                    { "title", "Start data" },
                    { "readOnly", true }
                };

                schema["type"] = "array";
                schema["items"] = new JObject
                {
                    { "type", "object" },
                    { "properties", new JObject {
                        { "startData", property }
                    } },
                    { "required", new JArray() },
                };
            }
            else
            {
                foreach (JObject item in startDataArray)
                {
                    var variable = item["variable"].ToString();
                    var type = item["type"].ToString();
                    var label = item["label"].ToString();
                    var swaggerType = GetType(type);

                    var property = new JObject
                    {
                        { "type", swaggerType.type },
                        { "title", label }
                    };

                    if (!string.IsNullOrEmpty(swaggerType.format))
                    {
                        property["format"] = swaggerType.format;
                    }

                    properties.Add(variable, property);

                }

                schema["type"] = "array";
                schema["items"] = new JObject
                {
                    { "type", "object" },
                    { "properties", properties },
                    { "required", new JArray() }
                };
            }

            response.Content = CreateJsonContent(schema.ToString());
        }

        return response;
    }

    private async Task<HttpResponseMessage> AttachCallbackUrlToStartInstance()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var bodyContent = new JObject();
        JObject requestObject = null;

        if (!string.IsNullOrEmpty(content)) {
            requestObject = JObject.Parse(content);
            
            var callbackUrl = requestObject["x-ntx-callbackUrl"];
            var startData = requestObject["startData"];

            if (startData != null)
            {
                bodyContent["startData"] = startData;
            }

            if (callbackUrl != null)
            {
                bodyContent["options"] = new JObject();
                bodyContent["options"]["callbackUrl"] = callbackUrl.ToString();
            } 
                
            this.Context.Request.Content = CreateJsonContent(JsonConvert.SerializeObject(bodyContent));
        }

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private static DataType GetType(string type)
    {
        switch (type)
        {
            case "text": return new DataType { type = "string" };
            case "datetime": return new DataType { type = "string", format = "date-time" };
            case "decimal": return new DataType { type = "number" };
            case "collection": return new DataType { type = "array" };
            default:
                return new DataType { type = type };
        }
    }
}

internal class DataType
{
    public string type { get; set; }
    public string format { get; set; }
}
