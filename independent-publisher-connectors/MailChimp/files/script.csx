public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Get the API key from the Authorization header
        string apiKey = null;
        
        if (this.Context.Request.Headers.Authorization != null)
        {
            var authHeader = this.Context.Request.Headers.Authorization;
            
            if (authHeader.Scheme?.Equals("Bearer", StringComparison.OrdinalIgnoreCase) == true)
            {
                apiKey = authHeader.Parameter;
            }
            else if (authHeader.Scheme?.Equals("Basic", StringComparison.OrdinalIgnoreCase) == true)
            {
                // Decode Basic auth - format is base64(username:password)
                var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                var parts = decoded.Split(':');
                if (parts.Length >= 2)
                {
                    apiKey = parts[1]; // Password is the API key
                }
            }
            else
            {
                // Might just be the raw key
                apiKey = authHeader.ToString();
            }
        }
        
        if (string.IsNullOrEmpty(apiKey))
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent("{\"error\": \"API key not found in Authorization header\"}");
            return response;
        }
        
        // Extract datacenter from API key
        // API key format: abc123def456-us21
        var keyParts = apiKey.Split('-');
        if (keyParts.Length < 2)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent("{\"error\": \"Invalid API key format. Expected format: key-datacenter (e.g., abc123-us21)\"}");
            return response;
        }
        
        var datacenter = keyParts[keyParts.Length - 1]; // Get the last part after the dash
        
        // Rewrite the URL to use the correct datacenter
        var originalUri = this.Context.Request.RequestUri;
        var newUri = new UriBuilder(originalUri)
        {
            Host = $"{datacenter}.api.mailchimp.com"
        }.Uri;
        
        this.Context.Request.RequestUri = newUri;
        
        // Set the Authorization header as Basic auth (Mailchimp expects this)
        var basicAuth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"anystring:{apiKey}"));
        this.Context.Request.Headers.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuth);
        
        // Forward the request
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
    }
}
