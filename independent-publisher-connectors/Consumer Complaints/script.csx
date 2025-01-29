public class Script : ScriptBase
{
    // Define a mapping of operation IDs to their respective query parameters requiring special handling
    private readonly Dictionary<string, string[]> specialHandlingMap = new Dictionary<string, string[]>
    {
        { "SearchConsumerComplaints", new[] { "company", "company_public_response", "company_response", "consumer_consent_provided", "consumer_disputed", "has_narrative", "issue", "product", "state", "submitted_via", "tags", "timely", "zip_code" } },
        { "SuggestCompanies", new[] { "company_public_response", "company_response", "consumer_consent_provided", "consumer_disputed", "has_narrative", "issue", "product", "state", "submitted_via", "tags", "timely", "zip_code" } },
        { "SuggestZipCodes", new[] { "company_public_response", "company_response", "consumer_consent_provided", "consumer_disputed", "has_narrative", "issue", "product", "state", "submitted_via", "tags", "timely", "zip_code" } },
        { "ListStateComplaints", new[] { "company", "company_public_response", "company_response", "consumer_consent_provided", "consumer_disputed", "has_narrative", "issue", "product", "state", "submitted_via", "tags", "timely", "zip_code" } },
        { "ListComplaintTrends", new[] { "company", "company_public_response", "company_response", "consumer_consent_provided", "consumer_disputed", "focus", "has_narrative", "issue", "product", "state", "submitted_via", "tags", "timely", "zip_code" } }
    };

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Extract the operationId from the context
        var operationId = this.Context.OperationId;

        // Check if the operationId requires special handling and get the corresponding query parameters
        if (specialHandlingMap.TryGetValue(operationId, out var queryParamNames))
        {
            // Manipulate the request query string for specific parameters
            var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            foreach (var paramName in queryParamNames)
            {
                if (query.AllKeys.Contains(paramName))
                {
                    var values = query[paramName].Split(',').Select(value => value.Trim()).ToArray();
                    // Remove the original comma-separated query parameter
                    query.Remove(paramName);
                    // Add each value as a separate query parameter
                    foreach (var value in values)
                    {
                        query.Add(paramName, value);
                    }
                }
            }

            // Update the request URI with the new query string
            var uriBuilder = new UriBuilder(this.Context.Request.RequestUri)
            {
                Query = query.ToString()
            };
            this.Context.Request.RequestUri = uriBuilder.Uri;
        }

        // Forward the potentially modified request
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
    }
}