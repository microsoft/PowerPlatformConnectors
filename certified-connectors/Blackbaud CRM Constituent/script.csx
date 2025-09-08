public class Script : ScriptBase
{

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Logger.LogInformation($"Scripting the {this.Context.OperationId} operation.");

        switch (this.Context.OperationId)
        {
            case "SimpleListLoad": 
                return await this.HandleSimpleListLoadOperation().ConfigureAwait(false);
        }

        return CreateErrorResponse(HttpStatusCode.BadRequest, $"Unhandled operation ID '{this.Context.OperationId}'");
    }

    private static HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string message)
    {
        var response = new HttpResponseMessage(statusCode);
        response.Content = CreateJsonContent(new JObject() {
            ["error"] = new JObject() {
                ["message"] = message
            }
        }.ToString());

        return response;
    }    

    private async Task<string> TranslateSimpleDataListItem(string simpleDataListId, string item) {

        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = $"/crm-adnmg/simplelists/{simpleDataListId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

        // Copy the subscription key and Authorization headers from the request
        request.Headers.Authorization = this.Context.Request.Headers.Authorization;
        request.Headers.Add("Bb-Api-Subscription-Key", this.Context.Request.Headers.GetValues("Bb-Api-Subscription-Key").FirstOrDefault<string>());

        // call SimpleListLoad to get the results
        this.Context.Logger.LogInformation($"Translating URI = {request.RequestUri.ToString()}.");
        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode) {
            return null;
        }

        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        var json = JObject.Parse(responseString);
        var rows = (JArray)json["rows"];

        this.Context.Logger.LogInformation($"Looking for {item}.");
        var itemJson = rows.Where(row => (string)row["label"] == item).FirstOrDefault();
        var result = (string)itemJson["value"];

        this.Context.Logger.LogInformation($"Found translation {result}.");
        return result;
    }

    private async Task<HttpResponseMessage> HandleSimpleListLoadOperation()
    {
        // This script transforms the query string for the SimpleListLoad endpoint from
        // ?paramKey1=foo&paramValue1=bar   (shape used by Power Automate)
        // to
        // ?parameters=foo,bar              (shape used by SKY API)
        //
        // if a translation source is provided (as paramSourceType), then translate the paramValue accordingly

        // get the query from the request
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        if (query?.Count > 0) {
            var sb = new StringBuilder();

            // these named parameters are explicitly declared in the connector OpenAPI for SimpleListLoad
            for (var i = 1; i <= 2; i++) {

                // use a temp NameValueCollection obtained from HttpUtility to get the special subclass that handles encoding in ToString()
                var newQuery = HttpUtility.ParseQueryString("");

                var paramKey = query[$"paramKey{i}"];
                if (paramKey != null) {
                    var paramValue = query[$"paramValue{i}"];
                    var sourceType = query[$"paramSourceType{i}"];
                    var paramSource = query[$"paramSource{i}"];

                    if ((sourceType == "SimpleDataList") && (paramValue != null)) {
                        // translate the simple data list item label to the value.
                        paramValue = await TranslateSimpleDataListItem(paramSource, paramValue).ConfigureAwait(false);
                    }

                    newQuery["parameters"] = $"{paramKey},{paramValue}";

                    // build up the new query string
                    if (sb.Length > 0) {
                        sb.Append('&');  
                    }
                    sb.Append(newQuery.ToString());
                }
            }

            // replace the Power Automate query with the params for the backend
            if (sb.Length > 0) {
                uriBuilder.Query = sb.ToString();
            }        

            // update the request with the new properly formatted URL for the backend
            this.Context.Request.RequestUri = uriBuilder.Uri;
        }

        this.Context.Logger.LogInformation($"New URI = {this.Context.Request.RequestUri.ToString()}.");

        // send the request to the backend
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}
