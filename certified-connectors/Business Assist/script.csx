public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
      var url = this.Context.Request.RequestUri.AbsoluteUri;
      var subUrl = url.Substring(0, url.LastIndexOf('/'));
      var jobId = subUrl.Substring(subUrl.LastIndexOf('/') + 1);
      HttpResponseMessage response = await this.Context.SendAsync(
        this.Context.Request,
        this.CancellationToken
      ).ConfigureAwait(continueOnCapturedContext: false);

      if (response.IsSuccessStatusCode)
      {
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(
          continueOnCapturedContext: false
        );

        // Example case: response string is some JSON object
        var result = JArray.Parse(responseString); 

        // Wrap the original JSON object into a new JSON object
        var newResult = new JObject
        {
          ["OperationId"] = jobId,
          ["Status"] = 2,
          ["Results"] = result,
        };
        response.Content = CreateJsonContent(newResult.ToString());
      }
      return response;
    }
}