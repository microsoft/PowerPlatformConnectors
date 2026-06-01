public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var dict = new Dictionary<string,string>();

        if(this.Context.OperationId == "Get_Sender_Details"){
            string url = this.Context.Request.RequestUri.LocalPath;
            string pattern = "^/api/accounts/([0-9]+)";
            Match match = Regex.Match(url,pattern);
            string accountId = match.Groups[1].Value;
            Uri originalUri = this.Context.Request.RequestUri;
            string modifiedUri = originalUri.ToString().Replace("/"+accountId, "");
            this.Context.Request.RequestUri = new Uri(modifiedUri);
            HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            JObject result = JObject.Parse(responseString);
            JObject respObj = JObject.Parse("{}");
            JArray dataArray = (JArray)result["data"];
            JArray send_mail_details = new JArray();
            JArray sendMailArr = JArray.Parse("[]");

            if(dataArray != null)
            {
               for(int i=0; i<dataArray.Count;i++)
               {
                    JObject dataElement = (JObject)dataArray[i];
                   if(dataElement != null)
                   {
                       if(dataElement["sendMailDetails"] != null)
                       {
                           send_mail_details = (JArray) dataElement["sendMailDetails"];

                           for(int mailIndex=0; mailIndex<send_mail_details.Count;mailIndex++)
                           {
                               JObject sendMailDetail = (JObject)send_mail_details[mailIndex];
                               if((bool)sendMailDetail["status"])
                               {
                                   sendMailArr.Add(sendMailDetail);
                               }
                           }
                           respObj.Add(new JProperty("sendMailDetails",sendMailArr));
                       }
                   }
                }
            }
            response.Content = CreateJsonContent(respObj.ToString());
            return response;
        }
        else if(this.Context.OperationId == "Send_Mail")
        {
            JObject sendMailObj = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
            JArray attachments = (JArray)sendMailObj["attachmentDetails"];

            if(attachments != null) {
                Task<HttpResponseMessage> attResponseMessage = uploadAllAttachments(attachments);
                HttpResponseMessage httpRespMsg = await attResponseMessage;
                if(!httpRespMsg.IsSuccessStatusCode){
                    return httpRespMsg;
                }
                string responseString = await httpRespMsg.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                JObject attRes = JObject.Parse(responseString);
                sendMailObj.Remove("attachmentDetails");
                sendMailObj.Add(new JProperty("attachments",(JArray)attRes["data"]));
            }

            this.Context.Request.Content = CreateJsonContent(sendMailObj.ToString());
            HttpResponseMessage mailRes = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            return mailRes;

        }
        else if(this.Context.OperationId == "Save_Draft")
        {
            JObject sendMailObj = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
            JArray attachments = (JArray)sendMailObj["attachmentDetails"];
            if(attachments != null) {
                Task<HttpResponseMessage> attResponseMessage = uploadAllAttachments(attachments);
                HttpResponseMessage httpRespMsg = await attResponseMessage;
                string responseString = await httpRespMsg.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                JObject attRes = JObject.Parse(responseString);

                sendMailObj.Remove("attachmentDetails");
                sendMailObj.Add(new JProperty("attachments",(JArray)attRes["data"]));

            }

            Uri originalUri = this.Context.Request.RequestUri;
            string modifiedUri = originalUri.ToString().Replace("/draft", "");
            this.Context.Request.RequestUri = new Uri(modifiedUri);

            this.Context.Request.Content = CreateJsonContent(sendMailObj.ToString());
            HttpResponseMessage mailRes = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            return mailRes;

        }
        else if(this.Context.OperationId == "Search_Mail")
        {
            JObject searchObject = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
            string searchString = "";
            foreach (var searchCriteria in searchObject)
            {
                string name = searchCriteria.Key;
                JToken value = searchCriteria.Value;
                string searchValue = (string)value;
                if(name == "fromDate" || name == "toDate"){
                    searchValue = DateTime.Parse(searchValue).ToString("dd-MMM-yyyy");
                }
                if(searchString == "")
                {
                    searchString = name +":"+searchValue;
                }
                else
                {
                    searchString = searchString + "::" + name +":"+ searchValue;
                }
            }
            if(searchString == ""){
                searchString = "entire:@";
            }
            var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
           this.Context.Request.Method = HttpMethod.Get;
           this.Context.Request.Content = new StringContent(string.Empty);
           var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
           query["searchKey"] = searchString;
           uriBuilder.Query = query.ToString();
           this.Context.Request.RequestUri = uriBuilder.Uri;
            HttpResponseMessage mailSearchRes = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            if(!mailSearchRes.IsSuccessStatusCode){
                return mailSearchRes;
            }
            string mailSearchResString = await mailSearchRes.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            JObject result = JObject.Parse(mailSearchResString);
            JObject respObj = JObject.Parse("{}");
            JArray dataArray = (JArray)result["data"];

            if(dataArray != null)
            {
               for(int i=0; i<dataArray.Count;i++)
               {
                   JObject metaData = (JObject)dataArray[i];
                   if(metaData.ContainsKey("toAddress")){
                        metaData["toAddress"] = HttpUtility.HtmlDecode((string)metaData["toAddress"]);
                    }
                   if(metaData.ContainsKey("ccAddress")){
                        metaData["ccAddress"] = HttpUtility.HtmlDecode((string)metaData["ccAddress"]);
                    }
                    dataArray[i] = metaData;
               }
               respObj["data"] = dataArray;
               mailSearchRes.Content = CreateJsonContent(respObj.ToString());
            }
            return mailSearchRes;

        }
        else if(this.Context.OperationId == "Get_Email_Content")
        {
             HttpRequestMessage mailMetaRequest = CloneHttpRequest(this.Context.Request);
            HttpResponseMessage mailContentRes = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if(mailContentRes.IsSuccessStatusCode ){

                String url = mailMetaRequest.RequestUri.ToString();
                url = url.Replace("/content","/details");
                mailMetaRequest.RequestUri = new Uri(url);
                var query = System.Web.HttpUtility.ParseQueryString(mailMetaRequest.RequestUri.Query);
                var uriBuilder = new UriBuilder(mailMetaRequest.RequestUri);
                query.Remove("includeBlockContent");
                uriBuilder.Query = query.ToString();
                mailMetaRequest.RequestUri = uriBuilder.Uri;
                mailMetaRequest.Headers.TryAddWithoutValidation("Content-Type", "aapplication/json");
                HttpResponseMessage mailMetaRes = await this.Context.SendAsync(mailMetaRequest, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                string metaResString = await mailMetaRes.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                JObject metaResObj = JObject.Parse(metaResString);
                JObject metaData = (JObject)metaResObj["data"];
                string contentResString= await mailContentRes.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                JObject contentResObj = JObject.Parse(contentResString);
                JObject contentData = (JObject)contentResObj["data"];
                metaData["content"] = contentData["content"];
                metaData["toAddress"] = HttpUtility.HtmlDecode((string)metaData["toAddress"]);
                if(metaData.ContainsKey("ccAddress")){
                    metaData["ccAddress"] = HttpUtility.HtmlDecode((string)metaData["ccAddress"]);
                }
                metaResObj["data"]=metaData;
                mailMetaRes.Content = CreateJsonContent(metaResObj.ToString());
                return mailMetaRes;
            }

            return mailContentRes;
        }
        HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
        errorResponse.Content = CreateJsonContent("{}");
        return errorResponse;
    }
    public async Task<HttpResponseMessage> uploadAllAttachments(JArray attachments)
    {
        using(HttpRequestMessage attachmentReq = CloneHttpRequest(this.Context.Request))
        {
			string url = attachmentReq.RequestUri.ToString()+"/attachments?uploadType=multipart";
            url = url.Replace("/draft","");
			attachmentReq.RequestUri = new Uri(url);
            using(MultipartFormDataContent form = new MultipartFormDataContent())
            {
                for(int attIndex=0; attIndex<attachments.Count;attIndex++)
                {
                    JObject attachment = (JObject)attachments[attIndex];
                    string attachmentName = (string)attachment["attachmentName"];
                    string attachmentContent = (string)attachment["attachmentContent"];
                    byte[] fileContent = Convert.FromBase64String(attachmentContent);
                    ByteArrayContent fileData = new ByteArrayContent(fileContent);
                    form.Add(fileData, "attach", attachmentName);
                }
                attachmentReq.Content = form;
                HttpResponseMessage response = await this.Context.SendAsync(attachmentReq, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }
        HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
        errorResponse.Content = CreateJsonContent("{}");
        return errorResponse;
    }

    public static HttpRequestMessage CloneHttpRequest(HttpRequestMessage request)
    {
        HttpRequestMessage clone = new HttpRequestMessage(request.Method, request.RequestUri);

        clone.Version = request.Version;
        foreach (KeyValuePair<string, object> prop in request.Properties)
        {
             clone.Properties.Add(prop);
        }
        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        clone.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
        return clone;
    }
}