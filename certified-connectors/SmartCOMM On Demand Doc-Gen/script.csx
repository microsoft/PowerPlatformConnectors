public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        await this.UpdateRequest().ConfigureAwait(false);
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
    }

    private async Task UpdateRequest()
    {
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var contentAsJson = JObject.Parse(contentAsString);
        var transactionData = (string)contentAsJson["transactionData"];
        var transactionDataBytes = System.Text.Encoding.UTF8.GetBytes(transactionData);
        contentAsJson["transactionData"] = System.Convert.ToBase64String(transactionDataBytes);
        this.Context.Request.Content = CreateJsonContent(contentAsJson.ToString());
    }
}