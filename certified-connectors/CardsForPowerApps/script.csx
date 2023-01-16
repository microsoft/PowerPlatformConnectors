public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Request.RequestUri = this.UpdateRequestUri(this.Context.Request.RequestUri.PathAndQuery);
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }
    private string GetEnvironmentId(string[] paths)
    {
        // get env id
        int envIndex = Array.IndexOf(paths, "environments");
        return paths[envIndex + 1];
    }
    private string GetCardId(string[] paths)
    {
        // get card id
        int cardIndex = Array.IndexOf(paths, "cards");
        return paths[cardIndex + 1];
    }
    private string GetInstanceId(string[] paths)
    {
        // get instance id
        int instanceIndex = Array.IndexOf(paths, "instances");
        return paths[instanceIndex + 1];
    }
    private Uri UpdateRequestUri(string path)
    {
        string[] paths = path.Split('/').Skip(2).ToArray();
        string newHostUrl = this.CreateHostUrl(paths);
        int cardIndex = Array.IndexOf(paths, "cards");
        string cid = paths[cardIndex + 1];
        var uriBuilder = new UriBuilder("https", newHostUrl, -1, this.GetPath(this.Context.OperationId, paths));
        uriBuilder.Query = "api-version=2022-03-01-preview";
        return uriBuilder.Uri;
    }
    private string CreateHostUrl(string[] paths)
    {
        string envId = GetEnvironmentId(paths);
        var normalizedEnvId = envId.ToLower().Replace("-", "");
        var hexPrefix = normalizedEnvId.Substring(0, normalizedEnvId.Length - 1);
        var hexSuffix = normalizedEnvId.Substring(normalizedEnvId.Length - 1, 1);
        //create new host
        return $"{hexPrefix}.{hexSuffix}.environment.api.preprod.powerplatform.com";
    }
    private string GetPath(string OperationId, string[] paths)
    {
        string prefixPath = "cards";
        switch (OperationId)
        {
            case "SearchCards":
                return prefixPath + "/cards";
            case "CreateCardInstance":
                return prefixPath + $"/cards/{this.GetCardId(paths)}/instances";
            case "GetCardInstance":
                return prefixPath + $"/cards/{this.GetCardId(paths)}/instances/{this.GetInstanceId(paths)}";
            case "GetCardDescription":
                return prefixPath + $"/cards/{this.GetCardId(paths)}";
            default:
                return prefixPath;
        }
    }
}