public class Script : ScriptBase
{

    string client_id = "client_id";
    string client_secret = "client_secret";
    string org_id = "org_id";
    string ims_host = "ims_host";
    string technical_account_id = "technical_account_id";
    string metascopes = "metascopes";
    string private_key = "private_key";
    string authfacadeAPIKey = "Forms_PA_Connector";
    string authfacadeUrlPrefix = "https://aemforms-authfacade";
    string authfacadeUrlSuffix = ".adobe.io/adobe/forms/authfacade/jwtexchange";
    string imsStageUrl = "ims-na1-stg1.adobelogin.com";

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var accessTokenResponse = await this.getAccessTokenResponse().ConfigureAwait(false);
        if (accessTokenResponse.IsSuccessStatusCode) {
            var response = await this.invokeAction(accessTokenResponse).ConfigureAwait(false);
            return response;
        } else {
            return accessTokenResponse;
        }
    }

    private async Task<HttpResponseMessage> getAccessTokenResponse()
    {
        var accessTokenRequest = createAccessTokenRequest();
        HttpResponseMessage accessTokenResponse = null;
        try {
            accessTokenResponse = await this.Context.SendAsync(accessTokenRequest, this.CancellationToken).ConfigureAwait(false);
            if (!accessTokenResponse.IsSuccessStatusCode) {
                JObject responseBody = await this.getResponseContentJson(accessTokenResponse).ConfigureAwait(false);
                accessTokenResponse = createErrorMessage(accessTokenResponse.StatusCode, responseBody["error"]["detail"].ToString());
            }
        } catch (Exception ex) {
            accessTokenResponse = createErrorMessage(HttpStatusCode.Unauthorized,  "Connection to AEM server failed, please check the connection parameters");
        }
        return accessTokenResponse;
    }

    private async Task<JObject> getResponseContentJson(HttpResponseMessage response) {
        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonContent = JObject.Parse(content);
        return jsonContent;
    }

    private async Task<string> getAccessToken(HttpResponseMessage accessTokenResponse) {
        var jsonContent = await getResponseContentJson(accessTokenResponse).ConfigureAwait(false);
        return jsonContent["data"]["accessToken"].ToString();
    }

    private string getRequestHeaderValue(string headerName) {
        IEnumerable<string> headerValues = this.Context.Request.Headers.GetValues(headerName);
        var headerValue = headerValues.FirstOrDefault();
        return (string)headerValue;
    }

    private String getAuthFacadeEndpoint() {
        string ims_host_url = getRequestHeaderValue(ims_host);
        if (ims_host_url.Equals(imsStageUrl)) {
            return authfacadeUrlPrefix + "-stage" + authfacadeUrlSuffix;
        }
        return authfacadeUrlPrefix + authfacadeUrlSuffix;
    }

    private HttpRequestMessage createAccessTokenRequest() {

        string authFacadeEndpoint = getAuthFacadeEndpoint();
        var accessTokenRequest = new HttpRequestMessage(HttpMethod.Post, authFacadeEndpoint);

        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(new StringContent(getRequestHeaderValue(org_id)), org_id);
        multipartContent.Add(new StringContent(getRequestHeaderValue(technical_account_id)), technical_account_id);
        multipartContent.Add(new StringContent(getRequestHeaderValue(client_id)), client_id);
        multipartContent.Add(new StringContent(getRequestHeaderValue(client_secret)), client_secret);
        multipartContent.Add(new StringContent(getRequestHeaderValue(ims_host)), ims_host);
        multipartContent.Add(new StringContent(getRequestHeaderValue(metascopes)), metascopes);
        multipartContent.Add(new StringContent(getRequestHeaderValue(private_key)), private_key);
        accessTokenRequest.Headers.Add("X-Adobe-Accept-Unsupported-API", "1");
        accessTokenRequest.Headers.Add("x-api-key", authfacadeAPIKey);
        accessTokenRequest.Content = multipartContent;

        return accessTokenRequest;
    }

    private void removeConnectionParamsFromRequestHeaders() {
        this.Context.Request.Headers.Remove(client_id);
        this.Context.Request.Headers.Remove(client_secret);
        this.Context.Request.Headers.Remove(org_id);
        this.Context.Request.Headers.Remove(ims_host);
        this.Context.Request.Headers.Remove(technical_account_id);
        this.Context.Request.Headers.Remove(metascopes);
        this.Context.Request.Headers.Remove(private_key);
    }

    private void addAuthorizationHeaderToRequest(string accessToken) {
        this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private HttpContent getStreamContent(JObject requestContentAsJson, string property) {
        if (((JToken)requestContentAsJson[property]).Type == JTokenType.String) {
            return new StringContent((string)requestContentAsJson[property]);
        } else {
            var data = ((JObject)requestContentAsJson[property])["$content"].ToString();
            byte[] byteArray = Convert.FromBase64String(data);
            MemoryStream stream = new MemoryStream( byteArray );
            StreamContent dataStream = new StreamContent(stream);
            dataStream.Headers.Add("Content-Type", "application/octet-stream");
            return dataStream;
        }
    }

    private async Task transformRequestContent() {
        var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestContentAsJson = JObject.Parse(requestContentAsString);

        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(getStreamContent(requestContentAsJson, "data"), "data");
        multipartContent.Add(getStreamContent(requestContentAsJson, "template"),"template");
        if(requestContentAsJson.ContainsKey("xci")) {
            multipartContent.Add(getStreamContent(requestContentAsJson, "xci"),"xci");
        }

        if(requestContentAsJson.ContainsKey("options")) {
            var options = requestContentAsJson["options"].ToString();
            multipartContent.Add(CreateJsonContent(options),"options");
        }

        this.Context.Request.Content = multipartContent;
    }

    private async Task<HttpResponseMessage> invokeAction(HttpResponseMessage accessTokenResponse) {
        string accessToken = await this.getAccessToken(accessTokenResponse).ConfigureAwait(false);
        this.removeConnectionParamsFromRequestHeaders();
        this.addAuthorizationHeaderToRequest(accessToken);

        await this.transformRequestContent().ConfigureAwait(false);

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
    }

    private HttpResponseMessage createErrorMessage(HttpStatusCode status, string message) {
        HttpResponseMessage response = new HttpResponseMessage(status);
        var jsonObject = new JObject();
        jsonObject.Add("status", status.ToString());
        jsonObject.Add("code", (int)status);

        if (message != null) {
            jsonObject.Add("message", message);
        }
        response.Content = CreateJsonContent(jsonObject.ToString());
        return response;
    }
}
