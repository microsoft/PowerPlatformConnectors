public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        addBearerPrefix();

        var response = await Context.SendAsync(Context.Request, CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }

    private void addBearerPrefix() {
        var headerValuesToken = Context.Request.Headers.GetValues("api_key");
        var accessToken = headerValuesToken.FirstOrDefault<string>();
        
        Context.Request.Headers.Remove("api_key");
        Context.Request.Headers.Remove("Authorization");

        Context.Request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
    }
}
