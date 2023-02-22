public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            String matchOprId = "OnDocumentEvent";
            if (matchOprId.StartsWith(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
            {
                var wresponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
                var content = await wresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var body = ParseContentAsJObject(content, false);
                wresponse.Headers.Location = new Uri(string.Format("{0}/{1}", this.Context.OriginalRequestUri.ToString(), body.GetValue("id").ToString()));
                return wresponse;
            }


        }
        catch (Exception ex)
        {
            throw;

        }

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;

    }

    private static JObject ParseContentAsJObject(string content, bool isRequest)
    {
        JObject body;
        body = JObject.Parse(content);
        return body;
    }

}