public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        return await this.HandleForwardOperation().ConfigureAwait(false);
    }
    private async Task<HttpResponseMessage> HandleForwardOperation()
    {
		if(this.Context.OperationId == "PortalList"){
			return await this.HandlePortalList().ConfigureAwait(false);
		}
        var dict = new Dictionary<string, string>();
		JObject jObj = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync()); 
	
		foreach (JProperty property in jObj.Properties())
		{
			dict.Add(property.Name, jObj[property.Name].ToString());
		}
		this.Context.Request.Content = new FormUrlEncodedContent(dict);
		
		Context.Request.RequestUri = new System.Uri(string.Format(this.getRequestUrl()));		//	get URL based on user's Data Center
		HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.IsSuccessStatusCode)
		{
			var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			response.Content = CreateJsonContent(responseString);
		}
		return response;
	}

    private async Task<HttpResponseMessage> HandlePortalList()
    {
	    Context.Request.RequestUri = new System.Uri(string.Format(this.getRequestUrl()));
		HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		return response;
	}
	
	private string getRequestUrl(){
        var uriString = Context.Request.RequestUri.ToString();
		if(uriString.Contains("sdpondemand.manageengine.com.au/")){
          uriString =  uriString.Replace("sdpondemand.manageengine.com.au/", "servicedeskplus.net.au/");
        }
		else if(uriString.Contains("sdpondemand.manageengine.com.cn/")){
          uriString =  uriString.Replace("sdpondemand.manageengine.com.cn/", "servicedeskplus.cn/");
        }
        else if(uriString.Contains("sdpondemand.manageengine.jp/")){
          uriString =  uriString.Replace("sdpondemand.manageengine.jp/", "servicedeskplus.jp/");
        }
	    return uriString;
	}
}