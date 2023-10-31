public class Script : ScriptBase
{
    public class StringContainsAdvancedFilter
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("operatorType")]
        public string OperatorType { get; private set; } = "StringContains";

        [JsonProperty("values")]
        public string[] Values { get; set; }
    }

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        if (Context.OperationId == "CreateSMSReceivedEventSubscription")
        {
            var content = await Context.Request.Content.ReadAsStringAsync();

            var json = JsonConvert.DeserializeObject<JObject>(content);

            var advancedFilters = new List<StringContainsAdvancedFilter>();

            var toNumbersCount = AddAdvanvedFilter(json, advancedFilters, "toPhoneNumbers", "data.To");
            var fromNumbersCount = AddAdvanvedFilter(json, advancedFilters, "fromPhoneNumbers", "data.From");

            if (toNumbersCount + fromNumbersCount > 25)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                        "You can only enter a total of 25 \"To/From Phone Number\" values. " +
                        "Please delete some \"To/From Phone Number\" values.")
                };
            }

            json["properties"]["filter"]["advancedFilters"] = JToken.FromObject(advancedFilters);

            Context.Request.Content = CreateJsonContent(json.ToString());

            var eventSubscriptionName = GetEventSubscriptionName(json);

            Context.Request.RequestUri = AppendEventSubscriptionNameToUriPath(
                Context.Request.RequestUri, eventSubscriptionName);

            var response = await Context.SendAsync(Context.Request, CancellationToken);
            response.Headers.Location = AppendEventSubscriptionNameToUriPath(
                Context.OriginalRequestUri, eventSubscriptionName);
            return response;
        }

        // Handle an invalid operation ID
        var invalidOpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
        {
            Content = CreateJsonContent($"Unknown operation ID '{Context.OperationId}'")
        };
        return invalidOpResponse;
    }

    private static int AddAdvanvedFilter(JObject obj, List<StringContainsAdvancedFilter> advancedFilters, string prop, string key)
    {
        if (obj.TryGetValue(prop, out var phoneNumbersJson))
        {
            var phoneNumbers = phoneNumbersJson.ToObject<string[]>();

            advancedFilters.Add(new StringContainsAdvancedFilter()
            {
                Key = key,
                Values = phoneNumbers
            });

            return phoneNumbers.Length;
        }

        return 0;
    }

    private static Uri AppendEventSubscriptionNameToUriPath(Uri uri, string eventSubscriptionName)
    {
        var newUri = new UriBuilder(uri);
        newUri.Path += $"/{eventSubscriptionName}";
        return newUri.Uri;
    }

    private static string GetEventSubscriptionName(JObject obj)
    {
        var input = (obj.GetValue("eventSubscriptionName") ?? "").ToString();
        return string.IsNullOrEmpty(input)
            ? $"acs-sms-events-connector-{Guid.NewGuid().ToString()}"
            : input;
    }
}
