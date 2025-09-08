public class Script : ScriptBase
{

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Logger.LogInformation($"Scripting the {this.Context.OperationId} operation.");

        switch (this.Context.OperationId)
        {
            case "CreateMajorGivingPlan":
                return await this.HandleCreateMajorGivingPlanOperation().ConfigureAwait(false);

            case "ListMajorGivingPlanLocations":
                return await this.HandleListMajorGivingPlanLocationsOperation().ConfigureAwait(false);

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

    private async Task<JObject> GetMajorGivingProspectPlan(string planId) {

        // return the prospect plan as a JObject

        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = $"/crm-prsmg/prospectplans/{planId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

        // Copy the subscription key and Authorization headers from the request
        request.Headers.Authorization = this.Context.Request.Headers.Authorization;
        request.Headers.Add("Bb-Api-Subscription-Key", this.Context.Request.Headers.GetValues("Bb-Api-Subscription-Key").FirstOrDefault<string>());

        // call the GetProspectPlan endpoint
        this.Context.Logger.LogInformation($"Calling GetProspectPlan, URI = {request.RequestUri.ToString()}.");
        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode) {
            return null;
        }

        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        return JObject.Parse(responseString);
    }

    private async Task<HttpResponseMessage> HandleCreateMajorGivingPlanOperation()
    {
        // This script handles setting the sequence field for any secondary fundraisers listed (so we don't have to expose it in the UI)

        // first, get the object from the request
        var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestJson = JObject.Parse(requestContent);

        // if there are any secondary fundraisers, append a sequence
        var fundraisersJson = requestJson["secondary_fundraisers"] as JArray;
        if (fundraisersJson is not null) {
            var index = 0;
            foreach (var item in fundraisersJson)
            {
                item["sequence"] = index++;
            }
        }

        var newBody = requestJson;

        // send the request to SKY API
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task<HttpResponseMessage> HandleListMajorGivingPlanLocationsOperation()
    {

        // The "Address Simple Data List" is parameterized by a constituent ID, but the UpdateMajorGivingPlanStep action only has the 
        // the plan ID (which is not part of the actual SKY API endpoint, just a construct in the connector action to provide context).  
        // So we need to fetch the plan in order to get the constituent ID, and then use the constituent ID when calling SimpleListLoad.

        // get the plan ID from the request
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        var paths = uriBuilder.Path.Split('/').Skip(2).ToArray();
        var planIdString = paths[Array.IndexOf(paths, "v_prospectplans") + 1];

        // we can only build the dropdown if the plan Id is a literal value
        var isValid = Guid.TryParse(planIdString, out Guid planId);
        if (!isValid) {
            this.Context.Logger.LogInformation($"Plan identifier {planIdString} is not a guid.");
            return null;
        }
        
        // get the constituent ID from the major giving plan
        var json = await GetMajorGivingProspectPlan(planIdString).ConfigureAwait(false);
        if (json is null) {
            this.Context.Logger.LogInformation($"Plan {planIdString} was not found."); 
            return null;
        }

        // only proceed if we are able to determine the constituentId
        var constituentId = json["prospect_id"]?.ToString();
        if (string.IsNullOrWhiteSpace(constituentId)) {
            this.Context.Logger.LogInformation($"Constituent identifier was not found in the plan."); 
            return null;
        }

        // build the URL to the "Address Simple Data List"
        uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = $"/crm-adnmg/simplelists/b0cb4058-4355-431a-abdb-3e9f2be8c918";
        uriBuilder.Query = $"parameters=constituent_id,{constituentId}";

        this.Context.Request.RequestUri = uriBuilder.Uri;

        // send the request to the backend
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
