public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Check if this is an operation we want to mess with
        switch (this.Context.OperationId) {
            case "ias_getdata_snapshot_v2": 
            case "ias_getdata_processed_v2":
            case "ias_getdata_plot_v2":
            case "ias_getdata_raw_v2":
                return await QueryAndUnpack().ConfigureAwait(false);
            default: 
                return await Query().ConfigureAwait(false);
        }
    }


    public async Task<HttpResponseMessage> Query() {
        // Send the request without modification
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }

    public async Task<HttpResponseMessage> QueryAndUnpack() {
        var origResponse = await Query().ConfigureAwait(false);

        if (origResponse.IsSuccessStatusCode) {
            var contentAsString = await origResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            JObject contentAsJson = JObject.Parse(contentAsString);

            var tag = (string)contentAsJson.Properties().First().Name;
            JObject tagObj = (JObject)contentAsJson[tag];

            // overwrite the original response content with the unpacked version
            origResponse.Content = CreateJsonContent(tagObj.ToString());

            return origResponse;
        } else {
            //if there is an error return the original response
            return origResponse;
        }
    }
}
