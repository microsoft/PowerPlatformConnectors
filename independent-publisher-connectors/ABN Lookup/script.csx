using Microsoft.Extensions.Logging;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Logger?.LogInformation($@"Executing Operation '{this.Context.OperationId}'.");
        var rawResponse = await this.Context?.SendAsync(this.Context.Request, this.CancellationToken); //.ConfigureAwait(continueOnCapturedContext: false)
        try
        {
            rawResponse.EnsureSuccessStatusCode();
            var content = await rawResponse.Content.ReadAsStringAsync(); //.ConfigureAwait(continueOnCapturedContext: false)
            if (content.EndsWith(")") == true) // likely jsonp
            {
                var i = content.IndexOf('(');
                if (i > 0)
                {
                    var callbackName = content.Substring(0, i);
                    if (content.StartsWith(callbackName + "(") == true)
                    {
                        //this.Context.Logger?.LogInformation($@"Received JSONP response with callback '{callbackName}'.");
                        var startLength = callbackName.Length + 1;
                        var json2 = content.Substring(startLength, content.Length - startLength - 1);
                        var response = new HttpResponseMessage(rawResponse.StatusCode)
                        {
                            Content = CreateJsonContent(json2),
                        };
                        rawResponse.Dispose(); // dispose original response
                        return response;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError($@"Unexpected Error Executing Operation '{this.Context.OperationId}'. Error: {ex.Message}");
        }
        return rawResponse;
    }
}
