public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Logger.LogInformation($"Scripting the {this.Context.OperationId} operation.");

        try
        {
            switch (this.Context.OperationId)
            {
                case "SendHttpRequest":
                    return await this.SendHttpRequest().ConfigureAwait(false);
            }

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = CreateJsonErrorResponse($"Unhandled operation ID '{this.Context.OperationId}'");
            return response;
        }
        catch (ConnectorException ex)
        {
            var response = new HttpResponseMessage(ex.StatusCode);
            response.Content = CreateJsonErrorResponse(ex.Message);
            return response;
        }        
    }

    private async Task<HttpResponseMessage> SendHttpRequest()
    {
        var requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

        var json = JObject.Parse(requestBody);
        var method = (string)json["method"];
        var path = (string)json["path"];
        var query = json["query"]?.ToString();
        var headers = json["headers"]?.ToString();
        var body = json["body"]?.ToString();

        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = path;

        // if any query parameters are present, copy them to the request
        if (!string.IsNullOrWhiteSpace(query))
        {
            var sb = new StringBuilder();

            var queryJson = JObject.Parse(query);
            foreach (var prop in queryJson.Properties())
            {
                // don't copy reserved query parameters
                if (!new [] {"subscription-key"}.Contains(prop.Name, StringComparer.OrdinalIgnoreCase)) 
                {
                    var value = prop.Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append("&");
                        }
                        sb.Append(prop.Name);
                        sb.Append('=');
                        sb.Append(value);
                    }
                }
            }

            if (sb.Length > 0)
            {
                this.Context.Logger.LogInformation($"Query string: {sb.ToString()}");
                uriBuilder.Query = $"?{sb.ToString()}";
            }
        }

        this.Context.Request.Method = new HttpMethod(method);
        this.Context.Request.RequestUri = uriBuilder.Uri;

        // if any headers are present, copy them to the request
        if (!string.IsNullOrWhiteSpace(headers))
        {
            var headersJson = JObject.Parse(headers);
            foreach (var prop in headersJson.Properties())
            {
                // don't copy reserved headers
                if (!new [] {"Authorization", "Bb-Api-Subscription-Key"}.Contains(prop.Name, StringComparer.OrdinalIgnoreCase)) 
                {
                    this.Context.Request.Headers.Add(prop.Name, prop.Value.ToString());
                }
            }
        }
    
        // set the request body
        if (new [] {"PUT", "POST", "PATCH"}.Contains(method, StringComparer.OrdinalIgnoreCase) && (!string.IsNullOrWhiteSpace(body)))
        {
            this.Context.Request.Content = CreateJsonContent(body);
        }

        // send the API request to the SKY API backend and return the response
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);  
    }

    // Build an error response shape consistent with other error responses from Power Automate
    private static StringContent CreateJsonErrorResponse(string message)
    {
        return CreateJsonContent(new JObject()
        {
            ["error"] = new JObject()
                {
                    ["message"] = message
                }
        }.ToString());
    }

    // implementation copied from the DocuSign connector
    public class ConnectorException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ConnectorException(HttpStatusCode statusCode, string message, Exception innerException = null) : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }

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