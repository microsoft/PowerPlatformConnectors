using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // get logged in environment id
        string envId = (string)this.Context.Request.Headers.GetValues("x-ms-environment-id").FirstOrDefault();
        if (string.IsNullOrWhiteSpace(envId))
        {
            throw new ConnectorException(HttpStatusCode.InternalServerError, "Environment Id should not be null.");
        }

        // get client region
        var region = (string)this.Context.Request.Headers.GetValues("x-ms-client-region").FirstOrDefault();
        if (string.IsNullOrWhiteSpace(region))
        {
            throw new ConnectorException(HttpStatusCode.InternalServerError, "Could not retrieve client region.");
        }
        
        if (string.IsNullOrWhiteSpace(this.Context.Request.RequestUri.PathAndQuery))
        {
            throw new ConnectorException(HttpStatusCode.InternalServerError, "Context RequestUri is null.");
        }

        // Handle get card description dynamic call when user does not select card from drop down
        if (this.Context.OperationId == "GetCardDescription")
        {
            // get card id
            var cardId = this.GetCardIdFromRequestUri(this.Context.Request.RequestUri.PathAndQuery);

            // if it is not a valid guid, we will not call backend and return empty card definition response
            // if it is a valid guid, then proceed to call the backend and return actual response
            if (!Guid.TryParse(cardId, out Guid parsedGuid))
            {
                // create empty response for seamless user experience during flow creation
                JObject emptyGetCardDescriptionObject = new JObject(
                    new JProperty("id", ""),
                    new JProperty("environmentId", ""),
                    new JProperty("name", ""),
                    new JProperty("author", ""),
                    new JProperty("connections", new JObject()),
                    new JProperty("inputs", new JObject(new JProperty("type", "object"), new JProperty("properties", new JObject()))),
                    new JProperty("output", new JObject(new JProperty("type", "object")))
                    );

                HttpResponseMessage emptyGetCardDescriptionResponse = new HttpResponseMessage();
                emptyGetCardDescriptionResponse.StatusCode = HttpStatusCode.OK;
                emptyGetCardDescriptionResponse.Content = CreateJsonContent(emptyGetCardDescriptionObject.ToString());
                return emptyGetCardDescriptionResponse;
            }
        }

        // update request uri
        this.Context.Request.RequestUri = this.GetRequestUri(envId, region);

        // send request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (this.Context.OperationId == "CreateCardInstance")
            {
                // currently to create the card instance we need to add additional activities api call to integrate the variables
                bool isBotUri = true;
                // get URI for bot activity
                Uri botUri = this.GetRequestUri(envId, region, isBotUri);
                HttpResponseMessage botResponse = await CallBotActivitiesAsync(responseString, botUri);

                if (botResponse.IsSuccessStatusCode)
                {
                    var botResponseString = await botResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var body = JObject.Parse(botResponseString);
                    var cardBody = body["value"] as JObject;
                    // add card type to card json
                    cardBody.Add("CardType", "PowerAppsCard");

                    //wrap the result in Card key 
                    var cardResult = new JObject
                    {
                        ["Card"] = cardBody
                    };

                    response.Content = CreateJsonContent(cardResult.ToString());
                }
            }
            else
            {
                response.Content = CreateJsonContent(responseString);
            }
        }
        return response;
    }

    private string GetCardIdFromRequestUri(string path)
    {
        // get card id
        string[] paths = path.Split('/').Skip(2).ToArray();
        int cardIndex = Array.IndexOf(paths, "cards");
        return paths[cardIndex + 1];
    }

    private string GetInstanceIdFromRequestUri(string path)
    {
        // get instance id
        string[] paths = path.Split('/').Skip(2).ToArray();
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
        string requestUri = this.Context.Request.RequestUri.PathAndQuery;
        string prefixPath = "cards";

        switch (operationId)
        {
            case "SearchCards":
                return prefixPath + "/cards";
            case "CreateCardInstance":
                return prefixPath + $"/cards/{this.GetCardIdFromRequestUri(requestUri)}/instances";
            case "GetCardInstance":
                return prefixPath + $"/cards/{this.GetCardIdFromRequestUri(requestUri)}/instances/{this.GetInstanceIdFromRequestUri(requestUri)}";
            case "GetCardDescription":
                return prefixPath + $"/cards/{this.GetCardIdFromRequestUri(requestUri)}";
            case "BotProcess":
                return prefixPath + $"/activities";
            default:
                return prefixPath;
        }
    }

    private Uri GetRequestUri(string envId, string region, bool isBotUri = false)
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
        string operation = isBotUri ? "BotProcess" : this.Context.OperationId;
        var uriBuilder = new UriBuilder("https", newHostUrl, -1, this.GetPath(operation));
        uriBuilder.Query = (string)this.Context.Request.Headers.GetValues("x-ms-api-version").FirstOrDefault();
        return uriBuilder.Uri;
    }

    private string GetRequestHeaderValue(string headerName)
    {
        IEnumerable<string> headerValues = this.Context.Request.Headers.GetValues(headerName);
        var headerValue = headerValues.FirstOrDefault();
        this.Context.Logger.LogInformation(headerValue);
        return (string)headerValue;
    }

    private async Task<HttpResponseMessage> CallBotActivitiesAsync(string responseString, Uri botUri)
    {
        // create a new request to call bot activity
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, botUri);

        // add all the headers from original request
        foreach (var header in this.Context.Request.Headers)
        {
            httpRequest.Headers.Add(header.Key, header.Value);
        }

        JObject body = JObject.Parse(responseString);
        JObject card = body["card"] as JObject;
        JObject action = new JObject();

        if (card.ContainsKey("refresh"))
        {
            JObject refresh = card["refresh"] as JObject;
            if (refresh.ContainsKey("action"))
            {
                action = card["refresh"]["action"] as JObject;
            }
            else
            {
                throw new Exception($"EXPECTED500: Could not find action in refresh property in card JSON.");
            }
        }
        else
        {
            throw new Exception($"EXPECTED500: Could not find refresh property in card JSON.");
        }

        // create bot request payload
        JObject _botRequestPayload = new JObject
        {
            ["serviceUrl"] = "http://blank",
            ["id"] = Guid.NewGuid(),
            ["type"] = "invoke",
            ["channelId"] = "powerautomate",
            ["name"] = "adaptiveCard/action",
            ["locale"] = Thread.CurrentThread.CurrentCulture.ToString(),
            ["localTimeZone"] = TimeZone.CurrentTimeZone.ToString(),
            ["value"] = new JObject
            {
                ["action"] = action,
                ["authentication"] = new JObject
                {
                    ["token"] = GetRequestHeaderValue("Authorization"),
                    ["id"] = body["cardId"].ToString()
                }
            }
        };

        // get original request to find input variables
        var requestString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        JObject requestContent = JObject.Parse(requestString);

        if (requestContent.ContainsKey("inputs"))
        {
            var inputs = requestContent["inputs"] as JObject;
            if (inputs != null)
            {
                if (_botRequestPayload.ContainsKey("value"))
                {
                    JObject value = _botRequestPayload["value"] as JObject;
                    if (value.ContainsKey("action"))
                    {
                        JObject valueAction = _botRequestPayload["value"]["action"] as JObject;
                        if (valueAction.ContainsKey("data"))
                        {
                            // merge inputs to data
                            JObject data = valueAction["data"] as JObject;
                            data.Merge(inputs);
                        }
                        else
                        {
                            throw new Exception($"EXPECTED500: Could not find data in action property in card JSON.");
                        }
                    }
                    else
                    {
                        throw new Exception($"EXPECTED500: Could not find action in value property in card JSON.");
                    }
                }
                else
                {
                    throw new Exception($"EXPECTED500: Could not find value property in bot request payload.");
                }
            }
        }

        httpRequest.Content = CreateJsonContent(_botRequestPayload.ToString());
        HttpResponseMessage response = await this.Context.SendAsync(httpRequest, this.CancellationToken).ConfigureAwait(false);
        return response;
    }

    public class ConnectorException : Exception
    {
        public ConnectorException(
            HttpStatusCode statusCode,
            string message,
            Exception innerException = null)
            : base(
                    message,
                    innerException)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

        public override string ToString()
        {
            var error = new StringBuilder($"ConnectorException: Status code={this.StatusCode}, Message='{this.Message}'");
            var inner = this.InnerException;
            var level = 0;
            while (inner != null && level < 10)
            {
                level += 1;
                error.AppendLine($"Inner exception {level}: {inner.Message}");
                inner = inner.InnerException;
            }

            error.AppendLine($"Stack trace: {this.StackTrace}");
            return error.ToString();
        }
    }
}
