public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Read the original request body as a string and parse it into a JObject
        string requestBodyString = await this.Context.Request.Content.ReadAsStringAsync();
        JObject originalRequestBody = JObject.Parse(requestBodyString);

        // Determine the command type and prepare the appropriate command value
        string commandType = originalRequestBody["cmd"]?["name"]?.ToString();
        object commandValue = DetermineCommandValue(commandType, originalRequestBody);

        if (commandValue == null)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("{\"message\": \"Invalid or missing command parameters.\"}")
            };
        }

        // Modify the request body with the determined command value
        var modifiedRequestBody = new JObject
        {
            ["device"] = originalRequestBody["device"],
            ["model"] = originalRequestBody["model"],
            ["cmd"] = new JObject
            {
                ["name"] = commandType,
                ["value"] = JToken.FromObject(commandValue)
            }
        };

        // Update the original request with the modified body
        this.Context.Request.Content = CreateJsonContent(modifiedRequestBody.ToString());

        // Send the modified request
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }

    private object DetermineCommandValue(string commandType, JObject requestBody)
    {
        switch (commandType)
        {
            case "turn":
                return requestBody["turn"]?.ToString();
            case "brightness":
                return requestBody["brightness"]?.ToObject<int>();
            case "color":
                return requestBody["color"]?.ToObject<JObject>();
            case "colorTem":
                return requestBody["colorTem"]?.ToObject<int>();
            default:
                return null;
        }
    }
}
