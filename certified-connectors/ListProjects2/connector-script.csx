public class Script : ScriptBase
{
  public override async Task<HttpResponseMessage> ExecuteAsync()
  {
    try
    {
      this.Context.Request.Headers.TryAddWithoutValidation("incoming-client-request2", getClientType());

      var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

      return response;
    }
    catch (ConnectorException ex)
    {
      var response = new HttpResponseMessage(ex.StatusCode);
      
      return response;
    }
  }

  private string getClientType()
  {
    var userAgent = this.context.Request.Headers.GetValueOrDefault("User-Agent") ?? string.Empty;
    var xmsUserAgent = this.context.Request.Headers.GetValueOrDefault("x-ms-user-agent") ?? string.Empty;

    if (userAgent.StartsWith("PowerApps", StringComparison.OrdinalIgnoreCase) || xmsUserAgent.StartsWith("PowerApps", StringComparison.OrdinalIgnoreCase))
    {
        return "PowerApps";
    }

    if (xmsUserAgent.StartsWith("LogicAppsDesigner", StringComparison.OrdinalIgnoreCase))
    {
        return xmsUserAgent.IndexOf("flow-client", StringComparison.OrdinalIgnoreCase) > -1 ? "PowerAutomate" : "LogicApps";
    }

    if (userAgent.StartsWith("azure-logic-apps", StringComparison.OrdinalIgnoreCase))
    {
        return userAgent.IndexOf("microsoft-flow", StringComparison.OrdinalIgnoreCase) > -1 ? "PowerAutomate" : "LogicApps";
    }

    if (userAgent.StartsWith("Microsoft Flow", StringComparison.OrdinalIgnoreCase))
    {
        return "PowerAutomate";
    }
    return "Sai-Invalid"
  }
}