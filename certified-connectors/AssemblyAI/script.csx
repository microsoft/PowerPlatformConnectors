public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Request.Headers.Add("User-Agent", "AssemblyAI/1.0 (integration=msft-connector/1.1.0)");

        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        if (this.Context.OperationId == "WordSearch")
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
        else
        {
            HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return response;
        }
    }
}
