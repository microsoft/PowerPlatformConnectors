public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        JObject currentUser = await GetUser("https://api.calendly.com/users/me");
        String organization = (String) currentUser["current_organization"];
        var requestContentString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestContentJson = JObject.Parse(requestContentString);
        requestContentJson.Add("organization", organization);
        this.Context.Request.Content = CreateJsonContent(requestContentJson.ToString());

        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }

    private async Task<JObject> GetUser(String uri)
    {
        String uuid = this.ParseUuid(uri);
        HttpRequestMessage reqMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, "https://api.calendly.com/users/" + uuid);
        reqMessage.Headers.Add("Authorization", this.Context.Request.Headers.Authorization.ToString());
        HttpResponseMessage userResponse = await this.Context.SendAsync(reqMessage, this.CancellationToken);

        var contentAsString = await userResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        return (JObject) JObject.Parse(contentAsString)["resource"];
    }

    private String ParseUuid(String uri)
    {
        return uri.Substring(uri.LastIndexOf('/') + 1);
    }
}
