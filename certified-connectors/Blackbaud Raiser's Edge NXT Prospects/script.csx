public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        switch (this.Context.OperationId)
        {
            case "ListOpportunityCustomFields":
                return await this.HandleListCustomFieldOperation().ConfigureAwait(false);
        }

        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unhandled operation ID '{this.Context.OperationId}'");
        return response;
    }

    private async Task<HttpResponseMessage> HandleListCustomFieldOperation()
    {
        // make the API request to the SKY API backend
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // if the call to the backend is successful, transform the response items
        if (response.IsSuccessStatusCode)
        {
            // get the response string, convert it to a JObject, and then look at the "value" property (which will be the array of custom fields)
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            var result = JObject.Parse(responseString);
            var value = result["value"] as JArray;

            // if the array contains any items, append a new bespoke xxx_value property to each item
            if (value?.Count() > 0)
            {
                foreach (var cf in value)
                {
                    // only append the bespoke xxx_value property if a value property is present
                    if (cf.SelectToken("value") != null) {
                        var valueField = $"{cf["type"]}_value";
                        valueField = valueField.ToLowerInvariant();
                        cf[valueField] = cf["value"];
                    }
                }
            }

            // assemble the new response
            var responseJson = new JObject() {
                ["count"] = result["count"],
                ["value"] = value
            };

            // set the new response content
            response.Content = CreateJsonContent(responseJson.ToString());            
        }

        return response;
    }
}