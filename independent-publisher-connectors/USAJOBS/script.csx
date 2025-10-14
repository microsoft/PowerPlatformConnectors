public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Log the request details
        LogRequestDetails();

        // Check if the operation ID starts with "Search"
        if (this.Context.OperationId.StartsWith("Search", StringComparison.OrdinalIgnoreCase))
        {
            // Remove the "/search" portion from the path
            var requestUri = this.Context.Request.RequestUri.ToString().Replace("/search", "");
            this.Context.Request.RequestUri = new Uri(requestUri);

            // Send the request and get the response
            var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

            return await HandleTransformCodelistResponse(response).ConfigureAwait(false);
        }
        else
        {
            // Send the request without modification if the operation ID does not start with "Search"
            return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        }
    }

    private void LogRequestDetails()
    {
        // Log the request method and URI
        this.Context.Logger.LogInformation($"Request Method: {this.Context.Request.Method}");
        this.Context.Logger.LogInformation($"Request URI: {this.Context.Request.RequestUri}");

        // Log the request headers
        foreach (var header in this.Context.Request.Headers)
        {
            this.Context.Logger.LogInformation($"Header: {header.Key} = {string.Join(", ", header.Value)}");
        }

        // Log the request content
        var requestBody = this.Context.Request.Content.ReadAsStringAsync().Result;
        this.Context.Logger.LogInformation($"Request Body: {requestBody}");
    }

    private async Task<HttpResponseMessage> HandleTransformCodelistResponse(HttpResponseMessage response)
    {
        try
        {
            // Read the content of the response
            var contentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(contentAsString))
            {
                throw new Exception("Response content is empty");
            }

            // Parse the content as JSON object
            var contentAsJson = JObject.Parse(contentAsString);

            // Handle the special case for GeoLocCodes
            if (this.Context.Request.RequestUri.AbsolutePath.Contains("codelist/geoloccodes"))
            {
                var results = contentAsJson["CodeList"]
                    .SelectMany(cl => cl["ValidValue"])
                    .Where(vv => vv["IsDisabled"].ToString().Equals("No", StringComparison.OrdinalIgnoreCase))
                    .Select(vv => new JObject
                    {
                        ["Code"] = vv["Code"],
                        ["Value"] = GenerateValue(vv)
                    }).ToList();

                var reshapedResult = new JObject
                {
                    ["results"] = new JArray(results)
                };

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = CreateJsonContent(reshapedResult.ToString())
                };
            }
            else
            {
                // Filter and reshape the results for other cases
                var results = contentAsJson["CodeList"]
                    .SelectMany(cl => cl["ValidValue"])
                    .Where(vv => vv["IsDisabled"].ToString().Equals("No", StringComparison.OrdinalIgnoreCase))
                    .Select(vv => new JObject
                    {
                        ["Code"] = vv["Code"],
                        ["Value"] = vv["Code"] + " - " + vv["Value"]
                    }).ToList();

                // Create a new JSON object with the reshaped results
                var reshapedResult = new JObject
                {
                    ["results"] = new JArray(results)
                };

                // Create the response message
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = CreateJsonContent(reshapedResult.ToString())
                };
            }
        }
        catch (Exception ex)
        {
            // Log the error
            this.Context.Logger.LogError($"Error processing response: {ex.Message}");

            // Create an error response message
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent(new JObject
                {
                    ["error"] = $"Error processing response: {ex.Message}"
                }.ToString())
            };
        }
    }

    private string GenerateValue(JToken vv)
    {
        var parts = new List<string>
        {
            vv["Code"]?.ToString(),
            vv["Country"]?.ToString(),
            vv["CountrySubdivision"]?.ToString(),
            vv["USCounty"]?.ToString(),
            vv["City"]?.ToString()
        }.Where(part => !string.IsNullOrEmpty(part));

        return string.Join(" - ", parts);
    }
}