public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try{
           
            if (this.Context.OperationId == "ListRelationships" || this.Context.OperationId == "ListIncomingRelationships" || this.Context.OperationId == "ListModels"){
                var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
                uriBuilder.Query = uriBuilder.Query.ToString().Replace("%25", "%");
                this.Context.Request.RequestUri = uriBuilder.Uri;
                HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JObject.Parse(responseString);
                if(response.IsSuccessStatusCode){
                    var _newResult = new JObject
                    {
                        ["value"] = result["value"],
                        ["nextLink"] = result["nextLink"],
                        ["continuationToken"] = GetContinuationToken(result["nextLink"].ToString())
                    }; 
                    response.Content = CreateJsonContent(_newResult.ToString());
                    return response;
                }else{
                    response.Content = CreateJsonContent(responseString);
                    return response;
                }
            }
            if (this.Context.Request.Method == HttpMethod.Get){
                HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var newResult = new JObject
                {
                    ["result"] = responseString,
                };
                response.Content = CreateJsonContent(newResult.ToString());
                return response;

            }else if (this.Context.OperationId == "QueryTwins"){
                var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                requestContentAsString = Regex.Unescape(requestContentAsString);
                this.Context.Request.Content = CreateJsonContent(requestContentAsString);

                HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JObject.Parse(responseString);

                if(response.IsSuccessStatusCode){
                    var _newResult = new JObject
                    {
                        ["value"] = result["value"].ToString(),
                        ["continuationToken"] = result["continuationToken"]
                    }; 
                    response.Content = CreateJsonContent(_newResult.ToString());
                    return response;
                }else{
                    response.Content = CreateJsonContent(responseString);
                    return response;
                }
               
            }else if(this.Context.Request.Method == HttpMethod.Put || this.Context.Request.Method == HttpMethod.Post){
                var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                var contentAsJson = JObject.Parse(contentAsString);
                var textToSend = (string)contentAsJson["value"];
                this.Context.Request.Content = CreateJsonContent(textToSend.ToString());
                var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

                if(this.Context.OperationId == "AddTwin"){
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var newResult = new JObject
                    {
                        ["result"] = responseString,
                    };
                    response.Content = CreateJsonContent(newResult.ToString());
                }
                return response;
            }else{
                var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
                return response;
            }
        }
        catch (ConnectorException ex)
        {
            var response = new HttpResponseMessage(ex.StatusCode)
            {
                Content = CreateJsonContent("error: "+ ex.Message)
            };
            return response;
        }
    }

    string GetContinuationToken(string nextLink)
    {   
        if(nextLink == "") return nextLink;
        return nextLink.Split(new string[]{"continuationToken=", "&api-version"}, 3, StringSplitOptions.None)[1];
    }

    public class ConnectorException : Exception
    {
        public ConnectorException(
                HttpStatusCode statusCode,
                string message,
                Exception innerException = null)
                : base(
                                message,
                                innerException)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

        public override string ToString()
        {
            var error = new StringBuilder($"ConnectorException: Status code={this.StatusCode}, Message='{this.Message}'");
            var inner = this.InnerException;
            var level = 0;
            while (inner != null && level < 10)
            {
                level += 1;
                error.AppendLine($"Inner exception {level}: {inner.Message}");
                inner = inner.InnerException;
            }

            error.AppendLine($"Stack trace: {this.StackTrace}");
            return error.ToString();
        }
    }
}