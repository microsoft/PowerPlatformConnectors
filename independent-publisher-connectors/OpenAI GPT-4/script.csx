public class script: ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
    // Check which operation ID was used
    if (this.Context.OperationId == "CompletionPost" || this.Context.OperationId == "ChatPost") 
    {
        return await this.ConvertAndTransformOperation().ConfigureAwait(false);
    }

    // Handle an invalid operation ID
    HttpResponseMessage response = new HttpResponseMessage(
        HttpStatusCode.BadRequest
    );
    response.Content = CreateJsonContent(
        $"Unknown operation ID '{this.Context.OperationId}'"
    );
    return response;
    }

    private async Task<HttpResponseMessage> ConvertAndTransformOperation()
    {
    // Use the context to forward/send an HTTP request
    HttpResponseMessage response = await this.Context.SendAsync(
        this.Context.Request,
        this.CancellationToken
    ).ConfigureAwait(continueOnCapturedContext: false);

    // Do the transformation if the response was successful
    if (response.IsSuccessStatusCode)
    {
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(
        continueOnCapturedContext: false
        );

        // Example case: response string is some JSON object
        var result = JObject.Parse(responseString); 

        // Wrap the original JSON object into a new JSON object
        var newResult = new JObject();
        if (this.Context.OperationId == "CompletionPost") {
            newResult.Add("first_completion", result["choices"][0]["text"]);
        }
        if (this.Context.OperationId == "ChatPost") {
            newResult.Add("first_content", result["choices"][0]["message"]["content"]);
        }
        
        newResult.Merge(result, new JsonMergeSettings {
            // union array values together to avoid duplicates
            MergeArrayHandling = MergeArrayHandling.Concat
        });

        response.Content = CreateJsonContent(newResult.ToString());
    }
        return response;
    }
}
