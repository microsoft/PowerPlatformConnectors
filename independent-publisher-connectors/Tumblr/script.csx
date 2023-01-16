public class script: ScriptBase
{

public override async Task < HttpResponseMessage > ExecuteAsync()
{
    return await this.ConvertAndTransformOperation().ConfigureAwait(false);

    // Handle an invalid operation ID
    HttpResponseMessage response = new HttpResponseMessage(
        HttpStatusCode.BadRequest
    );
    response.Content = CreateJsonContent(
        $"Unknown operation ID '{this.Context.OperationId}'"
    );
    return response;
}

private async Task < HttpResponseMessage > ConvertAndTransformOperation()
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
 
        var result = JObject.Parse(responseString)["response"]; 

        response.Content = CreateJsonContent(result.ToString());
    }
     return response;
}

}