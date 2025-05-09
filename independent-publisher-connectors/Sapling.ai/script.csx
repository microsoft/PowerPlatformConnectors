public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Manipulate request
        var requestKey = this.Context.Request.Headers.GetValues("api_key").FirstOrDefault();
        var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestContentAsJson = JObject.Parse(requestContentAsString);

        requestContentAsJson["key"] = requestKey;
        this.Context.Request.Content = CreateJsonContent(requestContentAsJson.ToString());
        this.Context.Request.Headers.Remove("api_key");

        // Execute API call
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // Manipulate response

        return response;
    }
}
