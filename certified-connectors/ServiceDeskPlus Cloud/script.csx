public class Script : ScriptBase
{
	public override async Task<HttpResponseMessage> ExecuteAsync()
	{
		return await this.HandleForwardOperation().ConfigureAwait(false);
	}
	
	private async Task<HttpResponseMessage> HandleForwardOperation(){
		switch (this.Context.OperationId) {
			case "PortalList":
				return await this.HandlePortalList().ConfigureAwait(false);
			case "InvokeAPI":
				return await this.HandleInvokeAPI().ConfigureAwait(false);
			case "CreateRequest":
			case "UpdateRequest":
				return await this.HandleRequestAction().ConfigureAwait(false);
		}
		return await this.HandleActions().ConfigureAwait(false);
	}

	private async Task<HttpResponseMessage> HandlePortalList(){									//	Handling to get Portal List
		Context.Request.RequestUri = new System.Uri(string.Format(this.getRequestUrl()));
		HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		return response;
	}

	private async Task<HttpResponseMessage> HandleRequestAction(){								//	Handling for requester's mail id and name in request action, and to execute action api call.
		var dict = new Dictionary<string, string>();
		JObject jObj = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync()); 
		foreach (JProperty property in jObj.Properties())
		{
			JObject inputObj = JObject.Parse(jObj[property.Name].ToString());
			JObject requestObj = (JObject)inputObj["request"];
			JObject requesterObj = (JObject)requestObj["requester"];
			if(requesterObj["email_id"] != null && requesterObj["email_id"].ToString() != ""){
				requesterObj.Remove("name");
			}
			else{
				requesterObj.Remove("email_id");
			}
			dict.Add(property.Name, inputObj.ToString());
		}
		this.Context.Request.Content = new FormUrlEncodedContent(dict);
		Context.Request.RequestUri = new System.Uri(string.Format(this.getRequestUrl()));
		HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.IsSuccessStatusCode)
		{
			var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			response.Content = CreateJsonContent(responseString);
		}
		return response;
	}

	private async Task<HttpResponseMessage> HandleActions(){								// Common handling to execute action api calls
		var dict = new Dictionary<string, string>();
		JObject jObj = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync()); 
		foreach (JProperty property in jObj.Properties())
		{
			dict.Add(property.Name, jObj[property.Name].ToString());
		
		}
		this.Context.Request.Content = new FormUrlEncodedContent(dict);
		Context.Request.RequestUri = new System.Uri(string.Format(this.getRequestUrl()));
		HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (response.IsSuccessStatusCode)
		{
			var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			response.Content = CreateJsonContent(responseString);
		}
		return response;
	}

	private async Task<HttpResponseMessage> HandleInvokeAPI(){									//	handling for Invoke Api action
		Uri uri = this.Context.Request.RequestUri;
		var query = HttpUtility.ParseQueryString(uri.Query);
		var apiMethod = HttpMethod.Get;
		var updatedURL = Uri.UnescapeDataString(this.getRequestUrl());
		var apiUrl = updatedURL.Substring(0, updatedURL.IndexOf("?"));
		var dict = new Dictionary<string, string>();
		var body =  await this.Context.Request.Content.ReadAsStringAsync();
		switch (query["method"].ToString()){
			case "GET":{
				apiMethod = HttpMethod.Get;
				var qParams = "";
				if(body != null && body != ""){
					JArray jArr = JArray.Parse(body);
					for(var i=0;i<jArr.Count;i++){
						var str = jArr[i].ToString();
						var index = str.IndexOf("=");
						var keyParam = str.Substring(0, index);
						var paramValue = str.Substring(index+1);
						qParams += "&" + keyParam + "=" + HttpUtility.UrlEncode(paramValue);
					}
					if(qParams != ""){
						apiUrl += "?" + qParams.Remove(0,1);
					}
				}
				break;
			}
			case "POST":{
				apiMethod = HttpMethod.Post;
				var isFormDataParam = false;
				var bodyContent = "";
				if(body != null && body != ""){
					JArray jArr = JArray.Parse(body);
					for(var i=0;i<jArr.Count;i++){
						bodyContent = jArr[i].ToString();
						var index = bodyContent.IndexOf("=");
						if(index != -1){
							var keyParam = bodyContent.Substring(0, index);
							var paramValue = bodyContent.Substring(index+1);
							dict.Add(keyParam, paramValue);
							isFormDataParam = true;
						}
					}
					if(isFormDataParam){
						this.Context.Request.Content = new FormUrlEncodedContent(dict);
					}
					else{
						this.Context.Request.Content = CreateJsonContent(bodyContent);
					}
				}
				break;
			}
			case "PUT":{
				apiMethod = HttpMethod.Put;
				var isFormDataParam = false;
				var bodyContent = "";
				if(body != null && body != ""){
					JArray jArr = JArray.Parse(body);
					for(var i=0;i<jArr.Count;i++){
						bodyContent = jArr[i].ToString();
						var index = bodyContent.IndexOf("=");
						if(index != -1){
							var keyParam = bodyContent.Substring(0, index);
							var paramValue = bodyContent.Substring(index+1);
							dict.Add(keyParam, paramValue);
							isFormDataParam = true;
						}
					}
					if(isFormDataParam){
						this.Context.Request.Content = new FormUrlEncodedContent(dict);
					}
					else{
						this.Context.Request.Content = CreateJsonContent(bodyContent);
					}
				}
				break;
			}
		}
		Context.Request.RequestUri = new System.Uri(string.Format(apiUrl));
		Context.Request.Method = apiMethod;
		HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
		var responseJson = JObject.Parse(responseString);
		if(responseJson["response_status"].GetType().ToString().Contains("Newtonsoft.Json.Linq.JArray")){
			JArray responseStatus = (JArray)responseJson["response_status"];
			responseJson["response_status"] = responseStatus[0];
		}
		response.Content = CreateJsonContent(responseJson.ToString());
		return response;
	}

	private string getRequestUrl(){																//	get URL based on user's Data Center
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
		else if(uriString.Contains("sdpondemand.manageengine.uk/")){
		  uriString =  uriString.Replace("sdpondemand.manageengine.uk/", "servicedeskplus.uk/");
		}
		else if(uriString.Contains("sdpondemand.manageenginecloud.ca/")){
		  uriString =  uriString.Replace("sdpondemand.manageenginecloud.ca/", "servicedeskplus.ca/");
		}
		return uriString;
	}
}