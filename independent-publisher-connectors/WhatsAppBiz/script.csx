public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Get the original request URL
        var originalUrl = this.Context.Request.RequestUri.ToString();
        
        // The WhatsApp API uses a single endpoint: /{phoneNumberId}/messages
        // Our connector uses virtual paths like /messages/template, /messages/image, etc.
        // We need to rewrite these to the actual endpoint
        
        string[] virtualPaths = new string[] 
        {
            "/messages/template",
            "/messages/image",
            "/messages/document",
            "/messages/location",
            "/messages/interactive",
            "/messages/status"
        };
        
        foreach (var virtualPath in virtualPaths)
        {
            if (originalUrl.Contains(virtualPath))
            {
                // Replace the virtual path with the actual endpoint
                var newUrl = originalUrl.Replace(virtualPath, "/messages");
                this.Context.Request.RequestUri = new Uri(newUrl);
                break;
            }
        }
        
        // Send the request to WhatsApp API
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
        
        return response;
    }
}
