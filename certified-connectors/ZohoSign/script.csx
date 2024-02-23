public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        return await this.HandleForwardOperation().ConfigureAwait(false);
    }
    private async Task<HttpResponseMessage> HandleForwardOperation()
    {
        if(this.Context.OperationId == "InvokeAPI"){
            return await this.HandleInvokeAPI().ConfigureAwait(false);
        }
        return null;
    }
    private async Task<HttpResponseMessage> HandleInvokeAPI()
    {
        Uri uri = this.Context.Request.RequestUri;
        var query = HttpUtility.ParseQueryString(uri.Query);
        var apiMethod = HttpMethod.Get;
        switch (query["method"].ToString()) {
            case "GET":
                apiMethod = HttpMethod.Get;
                break;
            case "POST":
                apiMethod = HttpMethod.Post;
                break;
			case "PUT":
                apiMethod = HttpMethod.Put;
                break;
        }
        var updatedURL = Uri.UnescapeDataString(uri.ToString());
        var apiUrl = updatedURL.Substring(0, updatedURL.IndexOf("?"));
        Context.Request.RequestUri = new System.Uri(string.Format(apiUrl));
        Context.Request.Method = apiMethod;
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        var responseJson = JObject.Parse(responseString);
        response.Content = CreateJsonContent(responseJson.ToString());
        return response;
	}
}
