public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Prep
        var requestKey = this.Context.Request.Headers.GetValues("api_key").FirstOrDefault();
        var requestUrl = this.Context.Request.RequestUri.AbsoluteUri;

        // Manipulate request
        requestUrl = requestUrl.Replace("api_key", requestKey);
        this.Context.Request.RequestUri = new Uri(requestUrl);
        this.Context.Request.Headers.Remove("api_key");

        // Execute API call
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }
}
