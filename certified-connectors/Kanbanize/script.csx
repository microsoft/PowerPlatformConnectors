public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.OperationId == "CreateCard" || this.Context.OperationId == "UpdateCard")
        {
            await this.UpdateNameRequest().ConfigureAwait(false);
        }
        return await this.HandleForwardAndTransformOperation().ConfigureAwait(false);
    }

    private async Task UpdateNameRequest() {
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var contentAsJson = JObject.Parse(contentAsString);

        if(contentAsJson.ContainsKey("cfields"))
        {
            var customFieldsValue = contentAsJson["cfields"];

            if( customFieldsValue != null && !string.IsNullOrEmpty(customFieldsValue.ToString()))
            {
                try
                {
                    var cfieldsAsJson = JObject.Parse(customFieldsValue.ToString());

                    contentAsJson.Remove("cfields");
                    contentAsJson.Add("cfields", JObject.FromObject(cfieldsAsJson));                        
                }
                catch (JsonReaderException)
                {
                    //Invalid json format
                    contentAsJson.Remove("cfields");
                }
                catch (Exception) //some other exception
                {
                    contentAsJson.Remove("cfields");
                }
            } else {
                contentAsJson.Remove("cfields");
            }
        }
        
        this.Context.Request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
        this.Context.Request.Content = CreateJsonContent(contentAsJson.ToString());
    }

    private async Task<HttpResponseMessage> HandleForwardAndTransformOperation()
    {
        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }
}
