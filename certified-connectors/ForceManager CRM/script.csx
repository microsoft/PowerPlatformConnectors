public class Script : ScriptBase
{

    string public_key = "public_key";
    string private_key = "private_key";

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var accessTokenResponse = await this.getAccessTokenResponse().ConfigureAwait(false);
        if (accessTokenResponse.IsSuccessStatusCode)
        {
            HttpResponseMessage response = await this.invokeAction(accessTokenResponse).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                return createErrorMessage(response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        else
        {
            return accessTokenResponse;
        }
    }

    private async Task<HttpResponseMessage> getAccessTokenResponse()
    {
        var accessTokenRequest = createAccessTokenRequest();
        HttpResponseMessage accessTokenResponse = null;
        try
        {
            accessTokenResponse = await this.Context.SendAsync(accessTokenRequest, this.CancellationToken).ConfigureAwait(false);
            if (!accessTokenResponse.IsSuccessStatusCode)
            {
                JObject responseBody = await this.getResponseContentJson(accessTokenResponse).ConfigureAwait(false);
                accessTokenResponse = createErrorMessage(accessTokenResponse.StatusCode, responseBody.ToString());
            }
        }
        catch (Exception ex)
        {
            accessTokenResponse = createErrorMessage(HttpStatusCode.Unauthorized, "Connection to ForceManager server failed, please check the connection parameters");
        }
        return accessTokenResponse;
    }

    private async Task<JObject> getResponseContentJson(HttpResponseMessage response)
    {
        string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonContent = JObject.Parse(content);
        return jsonContent;
    }

    private async Task<string> getAccessToken(HttpResponseMessage accessTokenResponse)
    {
        var jsonContent = await getResponseContentJson(accessTokenResponse).ConfigureAwait(false);
        return jsonContent["token"].ToString();
    }

    private string getRequestHeaderValue(string headerName)
    {
        IEnumerable<string> headerValues = this.Context.Request.Headers.GetValues(headerName);
        var headerValue = headerValues.FirstOrDefault();
        return (string)headerValue;
    }


    private HttpRequestMessage createAccessTokenRequest()
    {

        var accessTokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.forcemanager.com/api/v4/login");
        accessTokenRequest.Content = new StringContent("{\"username\":\"" + getRequestHeaderValue(public_key) + "\",\"password\":\"" + getRequestHeaderValue(private_key) + "\"}",
                                    Encoding.UTF8,
                                    "application/json");

        return accessTokenRequest;
    }

    private void removeConnectionParamsFromRequestHeaders()
    {
        this.Context.Request.Headers.Remove(public_key);
        this.Context.Request.Headers.Remove(private_key);
    }

    private void addAuthorizationHeaderToRequest(string accessToken)
    {
        this.Context.Request.Headers.Add("X-Session-Key", accessToken);
    }

    private string NullifyEmptyValue(JToken item)
    {
        if (item == null) return null;
        string itemAsString = item.ToString();
        if (!string.IsNullOrEmpty(itemAsString.Trim()))
        {
            return itemAsString;
        }
        return null;
    }

    private async Task<JArray> withoutDeletedItems(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        JArray newResponseAsJsonArray = new JArray();
        HttpResponseMessage response = await this.Context.SendAsync(request, cancellationToken).ConfigureAwait(false);
        string responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        JArray lastResponseAsJsonArray = JArray.Parse(responseAsString);
        foreach (var item in lastResponseAsJsonArray)
        {
            if (item["deleted"] == null || !(bool)item["deleted"])
            {
                newResponseAsJsonArray.Add(item);
            }
        }
        return newResponseAsJsonArray;
    }

    private async Task<HttpResponseMessage> invokeAction(HttpResponseMessage accessTokenResponse)
    {
        string accessToken = await this.getAccessToken(accessTokenResponse).ConfigureAwait(false);
        this.removeConnectionParamsFromRequestHeaders();
        this.addAuthorizationHeaderToRequest(accessToken);

        var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        bool getFullList = Convert.ToBoolean(query.Get("getFullList"));

        HttpResponseMessage response;
        HttpResponseMessage newResponse = new HttpResponseMessage();
        string responseAsString;
        HttpRequestMessage newRequest = this.Context.Request;
        var newResponseAsJson = new JObject();

        switch (this.Context.OperationId)
        {
            case "CreateAccount":
            case "UpdateAccount":
            case "CreateContact":
            case "UpdateContact":
            case "CreateOpportunity":
            case "UpdateOpportunity":
            case "CreateSalesOrder":
            case "UpdateSalesOrder":
            case "CreateSalesOrderLine":
            case "UpdateSalesOrderLine":
                var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                var contentAsJson = JObject.Parse(contentAsString);

                if (contentAsJson["customFields"] != null)
                {
                    var keysToChange = contentAsJson["customFields"].Children<JProperty>().ToList();

                    foreach (var key in keysToChange)
                    {
                        contentAsJson[key.Name] = key.Value;
                        key.Remove();
                    }

                    ((JObject)contentAsJson).Remove("customFields");
                }

                this.Context.Request.Content = CreateJsonContent(contentAsJson.ToString());

                newResponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
                break;
            case "CreateAccountWebhook":
            case "UpdateAccountWebhook":
            case "CreateContactWebhook":
            case "UpdateContactWebhook":
            case "CreateOpportunityWebhook":
            case "UpdateOpportunityWebhook":
            case "CreateSalesOrderWebhook":
            case "UpdateSalesOrderWebhook":
                newRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.forcemanager.com/api/v4/hooks");
                foreach (var header in this.Context.Request.Headers)
                {
                    newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                newRequest.Content = this.Context.Request.Content;

                response = await this.Context.SendAsync(newRequest, this.CancellationToken).ConfigureAwait(false);
                responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var responseAsJson = JObject.Parse(responseAsString);
                newResponseAsJson = responseAsJson;
                newResponseAsJson.Add("location", $"https://api.forcemanager.com/api/v4/hooks/{responseAsJson["id"]}");
                newResponse.Content = CreateJsonContent(newResponseAsJson.ToString());
                break;
            case "ListAccounts":
            case "ListContacts":
            case "ListOpportunities":
            case "ListActivities":
            case "ListUsers":
            case "ListCalendar":
            case "ListEmails":
            case "ListCalls":
            case "ListDocuments":
            case "ListProducts":
            case "ListSalesOrders":
            case "ListSalesOrdersLines":
            case "ListSales":
                newResponse.Content = CreateJsonContent((await withoutDeletedItems(this.Context.Request, this.CancellationToken)).ToString());
                break;
            case "GetCustomFieldsSchema":
                var customFieldSchemaContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                var customFieldSchemaContentAsJson = new JObject();
                customFieldSchemaContentAsJson.Add("entity", customFieldSchemaContentAsString.Trim('"'));
                this.Context.Request.Content = CreateJsonContent(customFieldSchemaContentAsJson.ToString());

                response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
                responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                // var responseAsJson = JObject.Parse(responseAsString);

                var fieldProcessor = new FieldProcessor();
                var (customFields, requiredFields) = fieldProcessor.DynamicFields(responseAsString, "create");
                if (customFields.Count > 0)
                {
                    newResponseAsJson.Add("data", new JObject
                    {
                        ["type"] = "object",
                        // ["required"] = requiredFields,
                        ["properties"] = (JObject)JToken.FromObject(customFields)
                    });
                }
                else
                {
                    Dictionary<string, ApiProperty> missingProperties = new Dictionary<string, ApiProperty>();

                    var apiProperty = new ApiProperty();
                    apiProperty.type = "string";
                    apiProperty.format = "string";
                    apiProperty.title = "Custom Field Placeholder";
                    apiProperty.description = "This is just a workaround to support the case in which no custom fields exist " +
                        "for the specific entity in ForceManager. Since the dynamic schema of Power Automate expects some data, " +
                        " we return this useless internal property to avoid UI issue in Power Automate modules.";
                    apiProperty.xmssummary = "Custom Field Placeholder";
                    apiProperty.xmsvisibility = "internal";

                    missingProperties.Add("Z_NoCustomFieldsPresent", apiProperty);
                    newResponseAsJson.Add("data", new JObject
                    {
                        ["type"] = "object",
                        ["properties"] = (JObject)JToken.FromObject(missingProperties)
                    });
                }
                newResponse.Content = CreateJsonContent(newResponseAsJson.ToString());
                break;
            case "ListCountries":
            case "ListBranches":
            case "ListCurrencies":
            case "ListTimezones":
            case "ListAccountTypes":
            case "ListAccountStatuses":
            case "ListAccountSegments":
            case "ListContactTypes":
            case "ListOpportunityTypes":
            case "ListOpportunityStatuses":
            case "ListActivityTypes":
            case "ListSalesOrderStatuses":
                var morePages = true;
                JArray newResponseAsJsonArray = new JArray();
                int page = 0;
                JArray lastResponseAsJsonArray;
                while (morePages)
                {
                    response = await this.Context.SendAsync(newRequest, this.CancellationToken).ConfigureAwait(false);
                    responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    lastResponseAsJsonArray = JArray.Parse(responseAsString);
                    foreach (var item in lastResponseAsJsonArray.ToList())
                    {
                        if ((bool)item["deleted"])
                        {
                            item.Remove();
                        }
                        else
                        {
                            item["description"] = (NullifyEmptyValue(item["descriptionEN"]) ?? NullifyEmptyValue(item["descriptionUS"]) ??
                                NullifyEmptyValue(item["descriptionES"]) ?? NullifyEmptyValue(item["descriptionIT"]) ??
                                NullifyEmptyValue(item["descriptionDE"]) ?? NullifyEmptyValue(item["descriptionFR"]) ??
                                NullifyEmptyValue(item["descriptionBR"]) ?? NullifyEmptyValue(item["descriptionDK"]) ??
                                NullifyEmptyValue(item["descriptionPT"]) ?? NullifyEmptyValue(item["descriptionRU"]));
                        }
                    }
                    newResponseAsJsonArray.Merge(lastResponseAsJsonArray);
                    if (getFullList && lastResponseAsJsonArray.Count > 0)
                    {
                        newRequest = new HttpRequestMessage(HttpMethod.Get, this.Context.Request.RequestUri);
                        foreach (var header in this.Context.Request.Headers)
                        {
                            newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                        }
                        var uriBuilder = new UriBuilder(newRequest.RequestUri);
                        page = page + 1;
                        query["page"] = $"{page}";
                        uriBuilder.Query = query.ToString();
                        newRequest.RequestUri = new Uri(uriBuilder.ToString());
                        System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        morePages = false;
                    }
                }
                newResponse.Content = CreateJsonContent(newResponseAsJsonArray.ToString());
                break;
            default:
                newResponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
                break;
        }

        return newResponse;
    }

    private HttpResponseMessage createErrorMessage(HttpStatusCode status, string message)
    {
        HttpResponseMessage response = new HttpResponseMessage(status);
        var jsonObject = new JObject();
        jsonObject.Add("error", status.ToString());
        jsonObject.Add("code", (int)status);

        if (message != null)
        {
            jsonObject.Add("message", message);
        }
        response.Content = CreateJsonContent(jsonObject.ToString());
        return response;
    }

    public class FieldProcessor
    {
        public (Dictionary<string, ApiProperty>, JArray) DynamicFields(string jsonFields, string action)
        {
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            };
            var fields = JsonConvert.DeserializeObject<List<FieldItem>>(jsonFields, settings);

            Dictionary<string, ApiProperty> apiProperties = new Dictionary<string, ApiProperty>();
            JArray requiredFields = new JArray();

            if (fields == null) return (apiProperties, requiredFields);

            foreach (var item in fields)
            {
                if (string.IsNullOrEmpty(item.Key) || !item.Key.StartsWith("Z_"))
                    continue;

                var apiProperty = new ApiProperty();


                if (item.Type == "unicode" && !string.IsNullOrEmpty(item.Choices) && !(item.List == true))
                {
                    apiProperty.type = "array";
                    apiProperty.items = new JObject
                    {
                        ["type"] = "integer"
                    };
                }
                else
                {
                    apiProperty.type = ConvertType(item.Type);
                }
                apiProperty.format = ConvertFormat(item.Type);

                apiProperty.title = item.Label;
                apiProperty.description = item.Label;
                apiProperty.xmssummary = item.Label;

                var required = (item.Required_Via_Api && action == "create");

                // apiProperty.required = required;
                apiProperty.xmsvisibility = required ? "important" : "advanced";

                if (required)
                {
                    requiredFields.Add(item.Key);
                }

                apiProperties.Add(item.Key, apiProperty);
            }

            return (apiProperties, requiredFields);
        }

        private string ConvertType(string type)
        {
            switch (type)
            {
                case "bool": return "boolean";
                case "datetime":
                case "text":
                case "unicode": return "string";
                case "decimal":
                case "decima": return "number";
                case "int": return "integer";
                default: return "object";
            }
        }

        private string ConvertFormat(string type)
        {
            switch (type)
            {
                case "bool": return null;
                case "datetime": return "date-time";
                case "text":
                case "unicode": return null;
                case "decimal": return "float";
                case "decima": return "float";
                case "int": return "int32";
                default: return null;
            }
        }

        private List<ApiOption> ParseChoices(string choices)
        {
            var options = new List<ApiOption>();
            var cleanedChoices = choices.Replace("\"", "").Replace("{", "").Replace("}", "");
            var splitChoices = cleanedChoices.Split(',');

            foreach (var choice in splitChoices)
            {
                var items = choice.Split(':');
                if (items.Length == 2)
                {
                    options.Add(new ApiOption { label = items[1], value = items[0] });
                }
            }

            return options;
        }
    }

    public class FieldItem
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public string Help_Text { get; set; }
        public bool Required { get; set; }
        public bool Required_Via_Api { get; set; }
        public string Type { get; set; }
        public string Choices { get; set; }
        public bool? List { get; set; }
    }

    public class ApiProperty
    {
        public string type { get; set; }
        public string format { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        [JsonProperty("x-ms-summary")]
        public string xmssummary { get; set; }
        [JsonProperty("x-ms-visibility")]
        public string xmsvisibility { get; set; }
        // public bool required { get; set; }
        // public List<ApiProperty> spec { get; set; }
        // [JsonProperty("enum")]
        // public List<ApiOption> options { get; set; }
        public JObject items { get; set; }
    }

    public class ApiOption
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    public class OpenApiSchema
    {
        public string type { get; set; }
        public Dictionary<string, ApiProperty> properties { get; set; }
    }
}
