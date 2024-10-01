public class Script : ScriptBase
{
  public override async Task<HttpResponseMessage> ExecuteAsync()
  {
    HttpRequestMessage Request = this.Context.Request;
    Uri modifiedUri = this.Context.Request.RequestUri;

    JObject json = JObject.Parse(await Request.Content.ReadAsStringAsync());

    JArray members = (JArray)json["members@odata.bind"];
    for (int i = 0; i < members.Count; i++)
    {
        members[i] = "https://graph.microsoft.com/v1.0/directoryObjects/" + members[i];
    }

    var modifiedRequest = new HttpRequestMessage(Request.Method, modifiedUri)
    {
      Content = new StringContent(json.ToString(), Encoding.UTF8, Request.Content.Headers.ContentType.MediaType)
    };

    // Retrieve the access token from the Authorization header of the original request
    string accessToken = Request.Headers.Authorization.Parameter;
    modifiedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    var response = await this.Context.SendAsync(modifiedRequest, this.CancellationToken).ConfigureAwait(false);
    return response;
  }
}