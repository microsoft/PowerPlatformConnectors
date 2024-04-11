public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            var client_id = this.Context.Request.Headers.GetValues("client_id").FirstOrDefault();
            var client_secret = this.Context.Request.Headers.GetValues("client_secret").FirstOrDefault();
            var grant_type = "client_credentials";

			if(this.Context.OperationId == "TokenFromAuthWrapper"){
				createRequestBodyForTokenFromAuthWrapper(client_id,client_secret);
			}else{
				var user_name = this.Context.Request.Headers.GetValues("user_name").FirstOrDefault();
				var hostname = this.Context.Request.Headers.GetValues("new_url").FirstOrDefault();
				var authServiceAccessToken = await GetAuthTokenForServiceUrl(client_id, client_secret, grant_type, hostname);

				var impersonatedUserAccessToken = await ImpersonateUserAndGetAccessToken(client_id, client_secret, authServiceAccessToken, user_name, hostname);
				
				this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", impersonatedUserAccessToken);

				RemoveConnectionParamsFromRequestHeaders();
			}

			var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        return response;
        }
        catch (Exception ex)
        {
            var errorMessage = ex.Message.ToString();
            return CreateErrorMessage(HttpStatusCode.Unauthorized, errorMessage);
        }
    }

    private async Task<string> GetAuthTokenForServiceUrl(string client_id, string client_secret, string grant_type, string hostname)
    {
        var getAuthTokenForServiceUrl = $"https://{hostname}/auth/token";
        var getAuthTokenForServiceRequestBody = $"client_id={client_id}&grant_type={grant_type}&client_secret={client_secret}";

        var accessTokenRequest = new HttpRequestMessage(HttpMethod.Post, getAuthTokenForServiceUrl)
        {
            Content = new StringContent(getAuthTokenForServiceRequestBody, Encoding.UTF8, "application/x-www-form-urlencoded")
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
            accessTokenResponse = CreateErrorMessage(HttpStatusCode.Unauthorized, "Connection to AEM server failed, please check the connection parameters");
        }

        if (!accessTokenResponse.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get the auth token. Status code: {accessTokenResponse.StatusCode}{errorResponseBody}");
        }

        var responseBody = await accessTokenResponse.Content.ReadAsStringAsync();
        var responseJson = JObject.Parse(responseBody);

        return responseJson["access_token"].ToString();
    }

	private async Task<string> ImpersonateUserAndGetAccessToken(string client_id, string client_secret, string authServiceAccessToken, string user_name, string hostname)
	{
		var impersonateUserEndpointUrl = $"https://{hostname}/api/v3/sessionService/impersonateUser";
		var impersonateUserRequestBody = $"{{\"client_id\":\"{client_id}\",\"client_secret\":\"{client_secret}\",\"impersonator_token\":\"{authServiceAccessToken}\",\"user_name\":\"{user_name}\"}}";

		var impersonateUserRequest = new HttpRequestMessage(HttpMethod.Post, impersonateUserEndpointUrl)
		{
			Content = new StringContent(impersonateUserRequestBody, Encoding.UTF8, "application/json")
		};
        impersonateUserRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authServiceAccessToken);
		HttpResponseMessage impersonateUserResponse;
		try
		{
			impersonateUserResponse = await this.Context.SendAsync(impersonateUserRequest, this.CancellationToken).ConfigureAwait(false);
			if (!impersonateUserResponse.IsSuccessStatusCode)
			{
				var errorResponseBody = await GetResponseContentJson(impersonateUserResponse).ConfigureAwait(false);
				impersonateUserResponse = CreateErrorMessage(impersonateUserResponse.StatusCode, errorResponseBody["error"]["detail"].ToString());
				throw new Exception($"Failed to impersonate the user. Status code: {impersonateUserResponse.StatusCode}");
			}
		}
		catch (Exception ex)
		{
			impersonateUserResponse = CreateErrorMessage(HttpStatusCode.Unauthorized, "Connection to AEM server failed, please check the connection parameters");
			throw new Exception($"Failed to impersonate the user. Status code: {impersonateUserResponse.StatusCode}");
		}

		var responseBody = await impersonateUserResponse.Content.ReadAsStringAsync();
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
        this.Context.Request.Headers.Remove("hostname");
        this.Context.Request.Headers.Remove("user_name");
        this.Context.Request.Headers.Remove("new_url");
    }
	
	private void createRequestBodyForTokenFromAuthWrapper(string client_id, string client_secret){
		var queryString = this.Context.Request.RequestUri.Query;
		var queryParams = HttpUtility.ParseQueryString(queryString);
		//Hardcoding Grant Type as passowrd for this operation
		var customGrantType="password";
		var password = queryParams["password"];
		var username = queryParams["username"];
		
		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("client_id", client_id),
			new KeyValuePair<string, string>("client_secret", client_secret),
			new KeyValuePair<string, string>("grant_type", customGrantType),
			new KeyValuePair<string, string>("username", username),
			new KeyValuePair<string, string>("password", password)
		});

		// Change the request body to use application/x-www-form-urlencoded format
		this.Context.Request.Content = content;
	}
}
