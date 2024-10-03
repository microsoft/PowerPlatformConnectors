
public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // get current request Uri
        var strRequestUri = this.Context.Request.RequestUri.AbsoluteUri;
        var strRequestUriDecode = (HttpUtility.UrlDecode(strRequestUri));
        this.Context.Request.RequestUri = new Uri(strRequestUriDecode);

        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }
}