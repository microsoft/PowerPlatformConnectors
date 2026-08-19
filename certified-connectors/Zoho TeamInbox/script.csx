public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if(this.Context.OperationId == "Get_Teams"){
            HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode) {
                string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                JObject result = JObject.Parse(responseString);
                JObject respObj = JObject.Parse("{}");
                JArray dataArray = (JArray)result["data"];
                JArray teamArray = new JArray();
                if(dataArray != null)
                {
                    for(int i=0; i<dataArray.Count;i++)
                    {
                        JObject dataElement = (JObject)dataArray[i];
                        if((int)dataElement["team_type"] == 2){
                            dataElement.Remove("settings");
                            dataElement.Remove("channels");
                            teamArray.Add(dataElement);
                        }
                    }
                    respObj.Add(new JProperty("teams",teamArray));
                    response.Content = CreateJsonContent(respObj.ToString());
                    return response;
                }
                response.Content = CreateJsonContent(respObj.ToString());
                return response;
            }
            else{
                return response;
            }
        }
        else if(this.Context.OperationId == "Get_From_Address"){
            var query = System.Web.HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            HttpRequestMessage request = this.Context.Request;
            var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
            string inbox_id = query["inbox_id"];
            query.Remove("inbox_id");
            query.Remove("team_id");
            query.Remove("workspace_id");
            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;
            HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode) {
                string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            
                JObject result = JObject.Parse(responseString);
                JObject respObj = JObject.Parse("{}");
                JObject dataObj = (JObject)result["data"];
                JArray emailArray = new JArray();
                if(dataObj != null)
                {
                    JArray channelArr = (JArray)dataObj["outgoing_channels"];
                    for(int i=0; i<channelArr.Count;i++)
                    {
                        JObject channelElement = (JObject)channelArr[i];
                        JArray inboxArr = (JArray)channelElement["accessible_inboxes"];
                        string[] inboxIdArr = inboxArr.ToObject<string[]>();
                        JObject emailObj = new JObject();
                    

                        if((bool)channelElement["is_enabled"] && (bool)channelElement["is_verified"] && inboxIdArr.Contains(inbox_id)){
                            emailObj.Add(new JProperty("email",(string)channelElement["source"]));
                            emailObj.Add(new JProperty("name",(string)channelElement["name"]));
                            emailArray.Add(emailObj);
                        }
                    
                    }
                }
                respObj.Add(new JProperty("from_address",emailArray));
                response.Content = CreateJsonContent(respObj.ToString());
                return response;
            }
            else{
                return response;
            }
        }
        else if(this.Context.OperationId == "Search_Mail"){
            var query = System.Web.HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            HttpRequestMessage request = this.Context.Request;
            JObject queryObj = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
            var formData = new Dictionary<string, string>{
                                { "query", queryObj.ToString() }
                            };
            request.Content = new FormUrlEncodedContent(formData);
            HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
           
            return response;
        }
        HttpResponseMessage response1 = new HttpResponseMessage(HttpStatusCode.OK);
        response1.Content = CreateJsonContent("{\"message\": \"Hello World\"}");
        return response1;
    }
}
