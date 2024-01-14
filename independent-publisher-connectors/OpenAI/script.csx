public class script: ScriptBase
{

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        //Add "Bearer" to Header if not already there
        var authHeaderValue = this.Context.Request.Headers.GetValues(name: "Authorization").FirstOrDefault();
        if (!(authHeaderValue.Contains("Bearer")))
        {
            this.Context.Request.Headers.Remove(name: "Authorization");
            this.Context.Request.Headers.Add(name: "Authorization", value: "Bearer "+authHeaderValue);
        }

        //handle different Operations
        switch (this.Context.OperationId) {
            case "ChatCompletion":
                return await this.HandleChatCompletion().ConfigureAwait(false);
                break;
            default:
                var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                return response;
                break;
        }            
    }

    private async Task<HttpResponseMessage> HandleChatCompletion()
    {
        // Manipulate the request data as applicable before setting it back
        var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestContentAsJson = JObject.Parse(requestContentAsString);

        //overrides messages, if you want to use the vision functionality in Power Apps
        if (requestContentAsString.Contains("\"messagesAsString\":"))
        {
            JArray messages_array = JArray.Parse(requestContentAsJson["messagesAsString"].ToString());
            requestContentAsJson["messages"] = JToken.FromObject(messages_array);
            requestContentAsJson.Remove("messagesAsString");
        }

        if (requestContentAsString.Contains("\"tools\":"))
        {
            JArray tools_array = JArray.Parse(requestContentAsJson["tools"].ToString());
            requestContentAsJson["tools"] = JToken.FromObject(tools_array);
        }

        this.Context.Request.Content = CreateJsonContent(requestContentAsJson.ToString());
        
        // do the API call
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            
            // write first_choice and first_content as object so it's easier to process in the Power Platform
            var result = JObject.Parse(responseString);
            result["first_choice"] = result["choices"].First;

            if (responseString.Contains("\"content\":"))
            {
            result["first_content"] = result.SelectToken("choices[0].message.content").ToString();
            }
            
            response.Content = CreateJsonContent(result.ToString());
        }

        return response;
    }

}
