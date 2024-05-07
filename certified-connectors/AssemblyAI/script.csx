public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        if (this.Context.OperationId == "WordSearch")
        {
            return await this.HandleForwardAndTransformOperation().ConfigureAwait(false);
        }

        // Handle an invalid operation ID
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }

    private async Task<HttpResponseMessage> HandleForwardAndTransformOperation()
    {
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
        if (response.IsSuccessStatusCode)
        {
            // the WordSearch endpoint returns the wrong content type, causing Power Automate to throw an error
            // this corrects the content type to application/json
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        return response;
    }
}