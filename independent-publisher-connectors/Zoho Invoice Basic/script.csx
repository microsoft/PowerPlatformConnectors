public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var _logger = this.Context.Logger;

        // Set the authentication URL
        string authURL = "https://accounts.zoho.com/oauth/v2/token";

        // Get the Client ID and Client Secret from the Basic Authentication header
        if (!this.Context.Request.Headers.TryGetValues("Authorization", out var authHeaders))
        {
            _logger.LogError("Authorization header is missing.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Authorization header is missing.")
            };
        }

        var authorizationHeader = authHeaders.First();
        var credentials = authorizationHeader.Split(' ')[1];
        var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
        var CLIENT_ID = decodedCredentials.Split(':')[0];
        var CLIENT_SECRET = decodedCredentials.Split(':')[1];

        // Get the organization ID
        if (!this.Context.Request.Headers.TryGetValues("X-com-zoho-invoice-organizationid", out var orgHeaders))
        {
            _logger.LogError("X-com-zoho-invoice-organizationid header is missing.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("X-com-zoho-invoice-organizationid header is missing.")
            };
        }

        var zohoInvoiceOrgId = "org" + orgHeaders.First();

        // Construct the query string manually
        var queryString = $"grant_type=client_credentials&client_id={Uri.EscapeDataString(CLIENT_ID)}&client_secret={Uri.EscapeDataString(CLIENT_SECRET)}&scope=ZohoInvoice.fullaccess.all&soid=ZohoInvoice.{Uri.EscapeDataString(zohoInvoiceOrgId)}";
        _logger.LogInformation("Constructed query string: {0}", queryString);

        Uri authUrl = new Uri($"{authURL}?{queryString}");
        _logger.LogInformation("Constructed URL: {0}", authUrl);

        HttpRequestMessage authRequest = new HttpRequestMessage(HttpMethod.Post, authUrl);
        _logger.LogInformation("Sending request: Method: {0}, Uri: {1}", authRequest.Method, authRequest.RequestUri);

        HttpResponseMessage authResponse = await this.Context.SendAsync(authRequest, this.CancellationToken);
        _logger.LogInformation("Response: StatusCode: {0}, ReasonPhrase: {1}", authResponse.StatusCode, authResponse.ReasonPhrase);

        // Check if the response content is empty
        var responseString = await authResponse.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(responseString))
        {
            _logger.LogError("The response content is empty.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("The response content is empty.")
            };
        }

        // Parse the JSON response
        JObject jsonResponse;
        try
        {
            jsonResponse = JObject.Parse(responseString);
        }
        catch (JsonReaderException ex)
        {
            _logger.LogError("Error parsing JSON response: {0}", ex.Message);
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error parsing JSON response.")
            };
        }

        if (!jsonResponse.TryGetValue("access_token", out JToken accessToken))
        {
            _logger.LogError("Access token not found in the response.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Access token not found in the response.")
            };
        }

        var ACCESS_TOKEN = accessToken.ToString();
        _logger.LogInformation("Access token retrieved: {0}", ACCESS_TOKEN);

        // Set JWT token
        var newAuthHeader = "Zoho-oauthtoken " + ACCESS_TOKEN;
        this.Context.Request.Headers.Authorization = AuthenticationHeaderValue.Parse(newAuthHeader);
        _logger.LogInformation("New Authorization header set: {0}", newAuthHeader);

        // Remove the X-com-zoho-invoice-organizationid header before sending the action request
        if (this.Context.Request.Headers.Contains("X-com-zoho-invoice-organizationid"))
        {
            this.Context.Request.Headers.Remove("X-com-zoho-invoice-organizationid");
            _logger.LogInformation("Removed header: X-com-zoho-invoice-organizationid");
        }

        // Remove the Origin header before sending the action request
        if (this.Context.Request.Headers.Contains("Origin"))
        {
            this.Context.Request.Headers.Remove("Origin");
            _logger.LogInformation("Removed header: Origin");
        }

        // Log the headers of the action request
        _logger.LogInformation("Action Request: Method: {0}, Uri: {1}", this.Context.Request.Method, this.Context.Request.RequestUri);
        foreach (var header in this.Context.Request.Headers)
        {
            _logger.LogInformation("Action Request Header: {0}: {1}", header.Key, string.Join(", ", header.Value));
        }

        // Send action request
        var actionResponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken);

        return actionResponse;
    }
}