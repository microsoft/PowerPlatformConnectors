public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var userResponse = await GetCalendlyUser();
        if (!userResponse.IsSuccessStatusCode)
        {
            // Surface the error response in the caller
            return userResponse;
        }

        var content = await userResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        var userData = JObject.Parse(content)["resource"];
        var organization = userData["current_organization"];

        var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestJson = JObject.Parse(requestContent);
        requestJson.Add("organization", organization);
        this.Context.Request.Content = CreateJsonContent(requestJson.ToString());

        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }

    private async Task<HttpResponseMessage> GetCalendlyUser()
    {
        var uri = $"{this.GetBaseUri()}users/me";
        var requestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, uri);
        requestMessage.Headers.Add("Authorization", this.Context.Request.Headers.Authorization.ToString());
        return await this.Context.SendAsync(requestMessage, this.CancellationToken);
    }

    // This custom code is currently only enabled for the CreateWebhookSubscription trigger, so
    // the base URI can be quickly found by removing the last URI segment of the request URI, e.g.
    //   https://api.calendly.com/webhook_subscriptions => https://api.calendly.com/
    //   https://ngrok-url/api/v2/webhook_subscriptions => https://ngrok-url/api/v2/
    private string GetBaseUri()
    {
        var uri = this.Context.Request.RequestUri.AbsoluteUri;
        return uri.Substring(0, uri.LastIndexOf('/') + 1);
    }
}