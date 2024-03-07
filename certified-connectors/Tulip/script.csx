public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
            return await this.HandleForwardAndTransformOperation().ConfigureAwait(false);

    }

    private async Task<HttpResponseMessage> HandleForwardAndTransformOperation()
    {
        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var TulipResponseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            var result = JObject.Parse(TulipResponseString);
            var columns = (JArray)result["columns"];
            var required = new JArray
            {
                "id",
            };
            var properties = new JObject();

            var dataTypeMapping = new JObject
                {
                    ["timestamp"] = "string",
                    ["interval"] = "number",
                    ["float"] ="number",
                    ["color"] = "object",
                    ["imageUrl"] = "string",
                    ["fileUrl"] = "string",
                    ["user"] = "string",
                    ["station"] = "string",
                    ["machine"] = "string",
                    ["tableLink"] = "string"
                };

            foreach (JObject column in columns){
                string columnUid = column.GetValue("name").ToString();
                string columnDescription = column.GetValue("label").ToString();
                string columnSummary = column.GetValue("label").ToString();

                var dataFormatObject = (JObject)column["dataType"];
                var dataFormatString = dataFormatObject.GetValue("type").ToString();
                var dataType = "string";
                if (dataTypeMapping.ContainsKey(dataFormatString) == true){
                    dataType = dataTypeMapping.GetValue(dataFormatString).ToString();
                } else{
                    dataType =  "string";
                }

                var newProperty = new JObject
                    {
                    ["type"] = dataType,
                    ["description"] = columnDescription,
                    ["x-ms-summary"] = columnDescription,
                    };
                properties.Add(columnUid, newProperty);

            }

            var newResultObject = new JObject
            {
                ["type"] = "object",
                ["properties"] = properties,
                ["required"] = required,
            };

            var newResult = new JObject
            {
                ["type"] ="array",
            };
            newResult.Add("items", newResultObject);
            
            response.Content = CreateJsonContent(newResult.ToString());
        }

        return response;
    }
}
