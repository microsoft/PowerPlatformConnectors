public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Operations that return the status of a request
        string[] statusOperations =
            {
             "GetDocumentClassificationRequestStatus",
             "GetFieldExtractionRequestStatus",
             "GetMlcRequestStatus",
             "GetLanguageClassificationRequestStatus",
             "GetOcrRequestStatus"
            };

        // Operations that accept file IDs and return request IDs
        string[] asyncOperations =
            {
             "CreateFieldExtractionRequest",
             "CreateDocumentClassificationRequest",
             "CreateMlcRequest",
             "CreateLanguageClassificationRequest",
             "CreateOcrRequest"
            };

        // If this is one of the async requests, update the body to use an array containing a single
        // file ID instead of a bare file ID
        if (asyncOperations.Contains(this.Context.OperationId))
        {
             uploadSingle();
        }

        addBearerPrefix();
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // Add a boolean "is_finished" value for convenience
        if (statusOperations.Contains(this.Context.OperationId))
        {
            response = await addFinished(response).ConfigureAwait(continueOnCapturedContext: false);
        }

        // Fix up response to return just one request ID
        if (asyncOperations.Contains(this.Context.OperationId))
        {
            response = await returnSingle(response).ConfigureAwait(continueOnCapturedContext: false);
        }

        // Coalesce null to empty array when there are no extractions
        if (this.Context.OperationId == "GetFieldExtractionRequestResults")
        {
            response = await coalesceNullExtractions(response).ConfigureAwait(continueOnCapturedContext: false);
        }

        return response;
    }

    /// <summary>
    /// This method prepends "Bearer " to the Authorization header value if it is not already there.
    /// </summary>
    private void addBearerPrefix(){
        // Get the first (and presumably only) Authorization header.
        var token = this.Context.Request.Headers.Authorization.ToString();

        if (!token.StartsWith("Bearer ")) {
            this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    /// <summary>
    /// This method adds a boolean is_finished value to the response's JSON content, which is true
    /// if the request is either "complete" or "failed". This allows the flow designer to easily
    /// check for both states in a "Do until" block.
    /// </summary>
    private async Task<HttpResponseMessage> addFinished(HttpResponseMessage response) {
        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            var result = JObject.Parse(responseString);

            result.Add("is_finished", result["status"].ToString() == "complete" || result["status"].ToString() == "failed");
            response.Content = CreateJsonContent(result.ToString());
        }
        return response;
    }

    /// <summary>
    /// This method simplifies the API response by replacing the JSON content with just the first element of the
    /// `file_ids` array. This is useful since our requests only act on one file anyways, so we an avoid adding
    /// redundant "Apply to each" blocks in the Power Automate Flow.
    /// For example a response with body:
    /// <example>
    /// <code>
    /// {file_ids: [{request_id: "abc", ...}]}
    /// </code>
    /// is updated to:
    /// <code>
    /// {request_id: "abc", ...}
    /// </code>
    /// </example>
    /// </summary>
    private async Task<HttpResponseMessage> returnSingle(HttpResponseMessage response) {
        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            var result = JObject.Parse(responseString);
            var newResult = result["file_ids"][0];

            response.Content = CreateJsonContent(newResult.ToString());
        }
        return response;
    }

    /// <summary>
    /// This method converts the incoming request into a single-element array, as expected by the DocAI API.
    /// For example a request with body:
    /// <example>
    /// <code>
    /// {file_id: "1234"}
    /// </code>
    /// is updated to:
    /// <code>
    /// {file_ids: ["1234"]}
    /// </code>
    /// </example>
    /// </summary>
    private async void uploadSingle() {
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var request = JObject.Parse(contentAsString);

        request["file_ids"] = new JArray(request["file_id"].ToString());

        this.Context.Request.Content = new StringContent(request.ToString());
    }

    /// <summary>
    /// This method coalesces the value of `extractions` from `null` to `[]`.
    /// </summary>
    private async Task<HttpResponseMessage> coalesceNullExtractions(HttpResponseMessage response) {
        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            var responseJson = JToken.Parse(responseString);
            var results = responseJson["results"];
            foreach (var res in results)
            {
                var token = res["extractions"];
                if (token == null || token.Type == JTokenType.Null) {
                        res["extractions"] = JToken.Parse("[]");
                };
            }

            response.Content = CreateJsonContent(responseJson.ToString());
        }
        return response;
    }
}