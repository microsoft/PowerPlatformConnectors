public class Script : ScriptBase
{
    // Define a mapping of operation IDs to their respective query parameters requiring special handling
    private readonly Dictionary<string, string[]> specialHandlingMap = new Dictionary<string, string[]>
    {
        // operationIds with their query parameters to handle
        { "GetFacilities", new[] { "facilityIds", "services[]", "bbox[]" } }, // Example operationId and parameters
        { "GetFacilityServicesById", new[] { "serviceIds" } },
        { "GetNearbyFacilities", new[] { "services[]" } }
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