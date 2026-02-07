public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Get the Authorization header value (which contains "Bearer {access_token}")
        if (this.Context.Request.Headers.TryGetValues("Authorization", out var authHeaderValues))
        {
            var authHeader = authHeaderValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader))
            {
                // Remove the "Authorization" header
                this.Context.Request.Headers.Remove("Authorization");
                
                // Extract just the token (remove "Bearer " prefix if present)
                var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) 
                    ? authHeader.Substring(7) 
                    : authHeader;
                
                // Add the token to "X-Auth-Token" header WITHOUT "Bearer " prefix
                this.Context.Request.Headers.Add("X-Auth-Token", token);
            }
        }

        // Send the request to the backend API
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }
}
