public class Script : ScriptBase
{
    private static readonly HashSet<string> LegacyAuthOperations = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "GetCaseFileDetails",
        "DownloadDocument"
    };

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.Request.Headers.TryGetValues("Authorization", out var authHeaderValues))
        {
            var authHeader = authHeaderValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader))
            {
                var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? authHeader.Substring(7)
                    : authHeader;

                this.Context.Request.Headers.Remove("Authorization");
                this.Context.Request.Headers.Add("X-Auth-Token", token);

                if (LegacyAuthOperations.Contains(this.Context.OperationId))
                {
                    this.Context.Request.Headers.TryAddWithoutValidation("Authorization", "JWT");
                }
            }
        }

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }
}
