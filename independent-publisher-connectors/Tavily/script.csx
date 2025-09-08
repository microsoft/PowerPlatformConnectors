public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var authorizationHeader = "Bearer " + this.Context.Request.Headers.Authorization.ToString();
        this.Context.Request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorizationHeader);

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
    }
}