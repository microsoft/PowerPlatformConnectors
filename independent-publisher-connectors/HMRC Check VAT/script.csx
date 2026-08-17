public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // The Client ID and Client secret arrive as a Basic authentication header.
        var authorization = this.Context.Request.Headers.Authorization;
        if (authorization == null || !"Basic".Equals(authorization.Scheme, StringComparison.OrdinalIgnoreCase))
        {
            return CreateError(HttpStatusCode.Unauthorized, "A Client ID and Client secret are required.");
        }

        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authorization.Parameter));
        var separator = credentials.IndexOf(':');
        var clientId = credentials.Substring(0, separator);
        var clientSecret = credentials.Substring(separator + 1);

        // Exchange them for an application access token using the OAuth 2.0 client credentials grant.
        // The token endpoint shares the host of the request, so this works for both the production
        // and sandbox HMRC environments.
        var tokenUri = new Uri(this.Context.Request.RequestUri, "/oauth/token");
        using (var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenUri))
        {
            tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["scope"] = "read:vat"
            });

            using (var tokenResponse = await this.Context.SendAsync(tokenRequest, this.CancellationToken).ConfigureAwait(false))
            {
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    return CreateError(tokenResponse.StatusCode, "Could not obtain an access token from HMRC. Check the Client ID and Client secret.");
                }

                var payload = JObject.Parse(await tokenResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                var accessToken = (string)payload["access_token"];

                // Call the requested operation with the bearer token and the HMRC API version header.
                this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                this.Context.Request.Headers.Accept.Clear();
                this.Context.Request.Headers.Accept.ParseAdd("application/vnd.hmrc.2.0+json");

                // The connector runtime adds an Origin header; HMRC's gateway rejects requests that carry it.
                this.Context.Request.Headers.Remove("Origin");
            }
        }

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
    }

    private HttpResponseMessage CreateError(HttpStatusCode statusCode, string message)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = CreateJsonContent(new JObject { ["message"] = message }.ToString())
        };
    }
}
