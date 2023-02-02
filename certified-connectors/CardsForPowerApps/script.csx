using System.Linq;
using System.Threading.Tasks;
using System;
using System.Runtime.Remoting.Contexts;
using System.Net;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // get logged in environment id
        string envId = (string)this.Context.Request.Headers.GetValues("x-ms-environment-id").FirstOrDefault();
        if (string.IsNullOrWhiteSpace(envId))
        {
            throw new Exception("EXPECTED400: Environment Id null.");
        }

        // get client region
        var region = (string)this.Context.Request.Headers.GetValues("x-ms-client-region").FirstOrDefault();
        if (string.IsNullOrWhiteSpace(region))
        {
            throw new Exception("EXPECTED400:Could not retrieve client region.");
        }

        // update request uri
        this.Context.Request.RequestUri = this.UpdateRequestUri(envId, region);

        var request = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

        // send request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if (response.IsSuccessStatusCode) {
           var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
           response.Content = CreateJsonContent(responseString);
        }  
        return response;
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

    public string GetCardsEndpointSuffix(string region)
    {
        switch (region)
        {
            case "tip2":
            // Eventually, BAP test (but not PPE) environments, and thus Cards test environments, will move to Tip2.
            // when they do, we should
            // return "api.test.powerplatform.com";
            // for now, fall through
            case "tip1":
                return "api.preprod.powerplatform.com";
            case "gcc":
                return "api.gov.powerplatform.microsoft.us";
            case "gcchigh":
                return "api.high.powerplatform.microsoft.us";
            case "dod":
                return "api.appsplatform.us";
            default:
                return "api.powerplatform.com";
        }
    }

    public int GetCardsSuffixLength(string region)
    {
        switch (region)
        {
            case "tip1":
            case "tip2":
                return 1;
            default:
                return 2;
        }
    }

    private string GetPath(string operationId)
    {
        string path = this.Context.Request.RequestUri.PathAndQuery;
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new Exception("EXPECTED400: Context request path is empty.");
        }
        string[] paths = path.Split('/').Skip(2).ToArray();

        string prefixPath = "cards";
        switch (operationId)
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

    private Uri UpdateRequestUri(string envId, string region)
    {
        // normalize the env id it to be used in the host
        var normalizedEnvId = envId.ToLower().Replace("-", "");

        // get endpoint suffix
        var endpointSuffix = this.GetCardsEndpointSuffix(region);
        if (string.IsNullOrWhiteSpace(endpointSuffix))
        {
            throw new Exception("EXPECTED400: Endpoint suffix is null");
        }

        // get suffic value based on client region
        var suffixValue = this.GetCardsSuffixLength(region);
        // example URL "https://922430067018eb13b524ff9020b4959.7.environment.api.preprod.powerplatform.com/";
        if (suffixValue == null)
        {
            throw new Exception("EXPECTED400: IdSuffixLength is null.");
        }
        var idSuffixLength = (int)suffixValue;

        // get hex prefix and suffix
        var hexPrefix = normalizedEnvId.Substring(0, normalizedEnvId.Length - idSuffixLength);
        var hexSuffix = normalizedEnvId.Substring(normalizedEnvId.Length - idSuffixLength, idSuffixLength);

        if (string.IsNullOrWhiteSpace(hexPrefix) || string.IsNullOrEmpty(hexSuffix))
        {
            throw new Exception($"EXPECTED400: Could not create proper url with hex prefix '{hexPrefix}' and hex suffix '{hexSuffix}'.");
        }

        // create new host url
        var newHostUrl = $"{hexPrefix}.{hexSuffix}.environment.{endpointSuffix}";
        if (string.IsNullOrWhiteSpace(newHostUrl))
        {
            throw new Exception($"EXPECTED400: Could not create proper url with hex prefix '{hexPrefix}' and hex suffix '{hexSuffix}'.");
        }

        // build the new request uri
        var uriBuilder = new UriBuilder("https", newHostUrl, -1, this.GetPath(this.Context.OperationId));
        uriBuilder.Query = (string)this.Context.Request.Headers.GetValues("x-ms-api-version").FirstOrDefault();
        return uriBuilder.Uri;
    }
}