public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Logger?.LogInformation("HDX connector request started. Method: {Method}, Path: {Path}", this.Context.Request.Method, this.Context.Request.RequestUri?.AbsolutePath);

        var appName = GetHeaderValue("app_name")?.Trim();
        var email = GetHeaderValue("email")?.Trim();

        string appIdentifier = null;
        if (!string.IsNullOrWhiteSpace(appName) && !string.IsNullOrWhiteSpace(email))
        {
            appIdentifier = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{appName}:{email}"));
            this.Context.Logger?.LogInformation("HDX app_identifier computed from connection parameters.");
        }
        else
        {
            appIdentifier = GetHeaderValue("X-HDX-HAPI-APP-IDENTIFIER")?.Trim();
            if (!string.IsNullOrWhiteSpace(appIdentifier))
            {
                this.Context.Logger?.LogInformation("HDX app_identifier reused from existing request header.");
            }
        }

        if (!string.IsNullOrWhiteSpace(appIdentifier))
        {
            this.Context.Request.Headers.Remove("X-HDX-HAPI-APP-IDENTIFIER");
            this.Context.Request.Headers.TryAddWithoutValidation("X-HDX-HAPI-APP-IDENTIFIER", appIdentifier);
            this.Context.Logger?.LogInformation("HDX app_identifier injected into header.");
        }
        else
        {
            this.Context.Logger?.LogWarning("HDX request missing app_name/email connection values. app_identifier header was not injected.");
        }

        this.Context.Request.Headers.Remove("app_name");
        this.Context.Request.Headers.Remove("email");

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        this.Context.Logger?.LogInformation("HDX connector response received. StatusCode: {StatusCode}", (int)response.StatusCode);
        return response;
    }

    private string GetHeaderValue(string headerName)
    {
        if (!this.Context.Request.Headers.Contains(headerName))
        {
            return null;
        }

        var headerValues = this.Context.Request.Headers.GetValues(headerName);
        return headerValues.FirstOrDefault();
    }

}
