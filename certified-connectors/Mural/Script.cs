/**
* This is the script for the Mural power automate connector.
*/
public class Script : ScriptBase
{
  #region Constants
  private const int STICKY_NOTE_PADDING = 30;
  #endregion

  #region Type
  public struct Widget
  {
    public double height;
    public double width;
    public double x;
    public double y;
  }

  public struct WidgetsResponse
  {
    public Dictionary<string, Widget> Widgets;
  }

  public struct Zone
  {
    public Dictionary<string, string> Endpoints;
    public string Id;
  }

  public struct ContentSession
  {
    public string Token;
    public Zone Zone;
    public string WorkspaceId;
  }

  public struct ContentSessionResponse {
    public ContentSession Value;
  }
  #endregion

  #region Helper Methods
  public (int, int) CalculateTopRightCoordinate(List<Widget> widgets)
  {
    if (widgets.Count == 0) return (0, 0 );

    List<double> allX = new List<double>(), allY = new List<double>();
    widgets.ForEach(widget =>
    {
      allX.Add(widget.x + widget.width);
      allY.Add(widget.y);
    });

    return ((int)Math.Floor(allX.Max()), (int)Math.Floor(allY.Min()));
  }
  #endregion

  #region Operations
  /**
  * This operation creates a new sticky note in the given mural. It places the sticky in the top
  * right of the mural unless X and Y coordinates are specified.
  **/
  private async Task<HttpResponseMessage> CreateNewStickyNote()
  {
    //Need to search the path for the mural ID. There is probably a less clunky way to do this.
    var muralId = new Regex(@"murals/([^/]*)").Match(Context.Request.RequestUri!.ToString()).Groups[1].Value;
    var host = Context.Request.RequestUri.GetLeftPart(UriPartial.Authority);

    var bodyStr = await Context.Request.Content!.ReadAsStringAsync();

    if (string.IsNullOrEmpty(bodyStr))
      return new HttpResponseMessage(HttpStatusCode.BadRequest) {
        Content = CreateJsonContent("{{\"error\":\"Empty request body.\"}}")
      };

    var body = JsonConvert.DeserializeObject<Dictionary<string, object>>(bodyStr);

    if (body == null)
      return new HttpResponseMessage(HttpStatusCode.BadRequest) {
        Content = CreateJsonContent("{{\"error\":\"Invalid request body.\"}}")
      };

    //If either x or y isn't specified but the other is, then the other will default to 0
    var hasX = body.TryGetValue("x", out var x);
    var hasY = body.TryGetValue("y", out var y);

    //We only need to make extra API calls if x and y weren't specified
    if (!hasX && !hasY) {
      var contentSessionRequest = new HttpRequestMessage
      {
        RequestUri = new Uri(String.Format("{0}/api/public/v1/murals/{1}/content-session", host, muralId)),
        Method = HttpMethod.Post
      };

      foreach(var header in Context.Request.Headers)
      {
        contentSessionRequest.Headers.Add(header.Key, header.Value);
      }

      var contentSessionResponse = await Context.SendAsync(contentSessionRequest, CancellationToken);

      if (contentSessionResponse.StatusCode != HttpStatusCode.OK)
        return contentSessionResponse;

      var responseBody = await contentSessionResponse.Content.ReadAsStringAsync();
      var session = JsonConvert.DeserializeObject<ContentSessionResponse>(responseBody);

      var uri = new Uri(String.Format("https://{0}/api/v0/content/murals/{1}/widgets",
            session.Value.Zone.Endpoints["api"],
            muralId));

      var widgetsRequest = new HttpRequestMessage
      {
        RequestUri = uri,
        Method = HttpMethod.Get,
      };

      widgetsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Value.Token);

      var widgetsResponse = await Context.SendAsync(widgetsRequest, CancellationToken);

      if (widgetsResponse.StatusCode != HttpStatusCode.OK) {
        return widgetsResponse;
      }

      var widgetsBody = await widgetsResponse.Content.ReadAsStringAsync();
      var widgets = JsonConvert.DeserializeObject<WidgetsResponse>(widgetsBody);

      var coords = CalculateTopRightCoordinate(widgets.Widgets.Values.ToList<Widget>());
      x = coords.Item1 + STICKY_NOTE_PADDING;
      y = coords.Item2;
    }

    body["x"] = x != null ? x : 0;
    body["y"] = y != null ? y : 0;

    Context.Request.Content = CreateJsonContent(JsonConvert.SerializeObject(body));
    return await Context.SendAsync(Context.Request, CancellationToken);
  }
  #endregion

  /**
  *  This is the entry point of the script.
  **/
  public override async Task<HttpResponseMessage> ExecuteAsync()
  {
    string op = Context.OperationId;

    switch (op)
    {
      case "CreateNewStickyNote":
        return await CreateNewStickyNote();
      //For any endpoint not handled by this script, forward the request to the api
      default:
        return await Context.SendAsync(Context.Request, CancellationToken).ConfigureAwait(false);
    }
  }
}
