public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            var client_id = this.Context.Request.Headers.GetValues("client_id").FirstOrDefault();
            var client_secret = this.Context.Request.Headers.GetValues("client_secret").FirstOrDefault();
            var grant_type = "client_credentials";
            var hostname = this.Context.Request.Headers.GetValues("host_name").FirstOrDefault();
            var oauthAccessToken = await GetAuthToken(client_id, client_secret, grant_type, hostname);
            this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oauthAccessToken);
            RemoveConnectionParamsFromRequestHeaders();
			var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
        }
        catch (Exception ex)
        {
            var errorMessage = ex.Message.ToString();
            return CreateErrorMessage(HttpStatusCode.Unauthorized, errorMessage);
        }
    }

    private async Task<string> GetAuthToken(string client_id, string client_secret, string grant_type, string hostname)
    {
        var authTokenUrl = $"https://{hostname}/o/oauth2/token";
        var requestBody = $"client_id={client_id}&grant_type={grant_type}&client_secret={client_secret}";

        var accessTokenRequest = new HttpRequestMessage(HttpMethod.Post, authTokenUrl)
        {
            Content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded")
        };
		
		JObject errorResponseBody = new JObject();
        HttpResponseMessage accessTokenResponse;
        try
        {
            accessTokenResponse = await this.Context.SendAsync(accessTokenRequest, this.CancellationToken).ConfigureAwait(false);
            if (!accessTokenResponse.IsSuccessStatusCode)
            {
                errorResponseBody = await GetResponseContentJson(accessTokenResponse).ConfigureAwait(false);
                accessTokenResponse = CreateErrorMessage(accessTokenResponse.StatusCode, errorResponseBody["error"]["error_description"].ToString());
            }
        }
        catch (Exception ex)
        {
            accessTokenResponse = CreateErrorMessage(HttpStatusCode.Unauthorized, "Failed to get token, please check the connection parameters");
        }

        if (!accessTokenResponse.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get the auth token. Status code: {accessTokenResponse.StatusCode}{errorResponseBody}");
        }

        var responseBody = await accessTokenResponse.Content.ReadAsStringAsync();
        var responseJson = JObject.Parse(responseBody);

        return responseJson["access_token"].ToString();
    }

    private async Task<JObject> GetResponseContentJson(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JObject.Parse(responseBody);
    }

     private HttpResponseMessage CreateErrorMessage(HttpStatusCode status, string message)
    {
        var response = new HttpResponseMessage(status);
        var jsonObject = new JObject();
        jsonObject.Add("status", status.ToString());
        jsonObject.Add("code", (int)status);

        if (message != null)
        {
            jsonObject.Add("message", message);
        }

        response.Content = CreateJsonContent(jsonObject.ToString());
        return response;
    }

    private void RemoveConnectionParamsFromRequestHeaders()
    {
        this.Context.Request.Headers.Remove("client_id");
        this.Context.Request.Headers.Remove("client_secret");
        this.Context.Request.Headers.Remove("host_name");
    }

    
}
