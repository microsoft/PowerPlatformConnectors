public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.UpdateBaseUri(GetBearerToken(this.Context.Request), this.Context.Request);
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
    }

     private void UpdateBaseUri(string bearerToken, HttpRequestMessage Request)
    {
        if (!string.IsNullOrEmpty(bearerToken))
        {
            // Update base uri
            var baseUri = this.GetBaseUri(bearerToken);
            Request.RequestUri = new Uri(new Uri(baseUri), Request.RequestUri.PathAndQuery);
        }
        if (Request.Method.ToString().ToUpper() == "PATCH"){
            Request.Method = HttpMethod.Post;
            Request.Headers.Add( "X-HTTP-Method-Override","PATCH");
        }
    }


    private string GetBearerToken(HttpRequestMessage Request)
    {
        var bearerToken = string.Empty;
        IEnumerable<string> headerValues;
        if (Request.Headers.TryGetValues("Authorization", out headerValues))
        {
            bearerToken = headerValues.FirstOrDefault();
        }
        return bearerToken;
    }

    private string GetBaseUri(string bearerToken)
    {
        var accessTokenClaimsBase64 = bearerToken.Substring(7).Split('.')[1];
        string token = accessTokenClaimsBase64.Replace('_', '/').Replace('-', '+');
		switch (token.Length % 4)
		{
			case 2: token += "=="; break;
			case 3: token += "="; break;
		}
		var encodedClaims = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var tokenClaimsJson = JObject.Parse(encodedClaims);
        var audience = (string)tokenClaimsJson["aud"];
        return "https://api." + audience;
    }
}
