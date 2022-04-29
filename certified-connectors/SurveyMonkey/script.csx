public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var check = await this.UpdateRequest().ConfigureAwait(false);
        if (check == false)
        {
            var triggerResponse = new HttpResponseMessage(HttpStatusCode.Accepted);
            triggerResponse.Headers.Location = SetLocationHeader(string.Empty, string.Empty);
            return triggerResponse;
        }
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            await this.UpdateResponse(response).ConfigureAwait(false);
        }
        return response;
    }

    private Uri SetLocationHeader(string triggerState, string excludeIds)
    {
        var locationUriBuilder = new UriBuilder(this.Context.OriginalRequestUri);
        var query = HttpUtility.ParseQueryString(this.Context.OriginalRequestUri.Query);
        string newTriggerState = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        if (!String.IsNullOrEmpty(triggerState))
        {
            newTriggerState = triggerState;
        }
        query["triggerstate"] = newTriggerState;
        query["triggerstate_exclude_ids"] = excludeIds;
        locationUriBuilder.Query = query.ToString();
        return locationUriBuilder.Uri;
    }

    private string SetBody(string content)
    {
        string body = string.Empty;
        if (!String.IsNullOrEmpty(content))
        {
            JObject modifiedRequestBody = null;
            JObject requestBody = JObject.Parse(content);

            switch (this.Context.OperationId.ToLower())
            {
                case "sendmessage":
                    if (requestBody["scheduled_date"] != null)
                    {
                        string scheduled_date = (string)requestBody["scheduled_date"];
                        if (!string.IsNullOrWhiteSpace(scheduled_date))
                        {
                            modifiedRequestBody = JObject.Parse("{ 'scheduled_date' : '" + scheduled_date + "' }");
                        }
                    }
                    break;

                default:
                    return body;
            }
            body = (modifiedRequestBody == null) ? "{}" : modifiedRequestBody.ToString();
        }
        else
        {
            switch (this.Context.OperationId.ToLower())
            {
                case "sendmessage":
                    body = "{}";
                    break;
            }
        }
        return body;
    }

    public JArray GetNewItems(JArray items)
    {
        var originalQuery = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        var triggerState = originalQuery.Get("triggerstate");
        if (items != null && triggerState != null)
        {
            var listOfOperations = new List<string>() { "OnNewResponseAddedCollector", "OnNewResponseAddedSurvey" };

            if (listOfOperations.Contains(this.Context.OperationId, StringComparer.OrdinalIgnoreCase))
            {
                string serializedExcludeIds = string.Empty;
                var triggerStateExcludes = originalQuery.Get("triggerstate_exclude_ids");
                if (triggerStateExcludes != null)
                {
                    serializedExcludeIds = triggerStateExcludes;
                }
                if (serializedExcludeIds != string.Empty)
                {
                    var excludeIds = new HashSet<string>(serializedExcludeIds.Split(new char[] { ',' }));
                    items = new JArray(items.Where(x => !excludeIds.Contains(x["id"].ToString())));
                }
                items = (items?.Count > 0 ? items : new JArray());
            }
        }
        return items;
    }

    public string SetTriggerState(JArray newItems, string triggerState, string date)
    {
        DateTime newTriggerState = DateTime.MinValue;
        string excludeIds = string.Empty;
        var listOfOperations = new List<string>() { "OnNewResponseAddedCollector", "OnNewResponseAddedSurvey" };

        if (listOfOperations.Contains(this.Context.OperationId, StringComparer.OrdinalIgnoreCase) && newItems != null)
        {
            if (newItems.Count > 0)
            {
                newTriggerState = newItems.Max(x => DateTime.Parse(x["date_modified"].ToString()));
                newTriggerState = newTriggerState.AddSeconds(1);
                /* now get the ids of any responses with modified date in the same minute as the next trigger date so we can exclude them from the next run */
                excludeIds = string.Join(",", newItems.Where(x => DateTime.Parse(x["date_modified"].ToString()).ToString("yyyy-MM-ddTHH:mmZ") == newTriggerState.ToString("yyyy-MM-ddTHH:mmZ")).Select(y => y["id"].ToString()));
            }
        }

        if ((newItems == null || newItems.Count == 0) && !String.IsNullOrEmpty(triggerState) && newTriggerState == DateTime.MinValue)
        {
            newTriggerState = DateTime.Parse(triggerState);
        }
        else if (newTriggerState == DateTime.MinValue)
        {
            newTriggerState = DateTime.Parse(date);
        }

        return newTriggerState.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "|" + excludeIds;
    }

    private async Task<bool> UpdateRequest()
    {
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        string triggerState = query.Get("triggerstate");
        if (!string.IsNullOrEmpty(triggerState))
        {
            // initial trigger state to get existing items
            triggerState = DateTime.Parse(triggerState).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        if ("OnSurveyCreated".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
            "OnNewResponseAddedCollector".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
            "OnNewResponseAddedSurvey".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
            "OnSurveyCollectorCreated".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var segments = this.Context.Request.RequestUri.Segments.ToList();
            var removeTrigger = segments.Find(x => x.StartsWith("trigger"));
            uriBuilder.Path = uriBuilder.Path.Replace(removeTrigger, string.Empty);
            if (!string.IsNullOrEmpty(triggerState))
            {
                if ("OnSurveyCreated".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
                "OnNewResponseAddedCollector".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
                "OnNewResponseAddedSurvey".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
                {
                    query["start_modified_at"] = triggerState;
                }
                else if ("OnSurveyCollectorCreated".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
                {
                    query["start_date"] = triggerState;
                }
                else
                {
                    query["since"] = triggerState;
                }
            }
            else
            {
                return false;
            }
            query.Remove("triggerstate");
            query.Remove("triggerstate_exclude_ids");
        }
        else
        {
            var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            string body = SetBody(content);
            this.Context.Request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        }
        query.Remove("surveyId");
        uriBuilder.Query = query.ToString();
        this.Context.Request.RequestUri = uriBuilder.Uri;

        return true;
    }

    private async Task UpdateResponse(HttpResponseMessage response)
    {
        var query = HttpUtility.ParseQueryString(this.Context.OriginalRequestUri.Query);
        var triggerState = query.Get("triggerstate");
        if (!String.IsNullOrEmpty(triggerState))
        {
            triggerState = DateTime.Parse(triggerState).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
        var excludeIds = string.Empty;

        if ("OnSurveyCreated".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
            "OnNewResponseAddedCollector".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
            "OnNewResponseAddedSurvey".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
            "OnSurveyCollectorCreated".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(60));
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var body = JObject.Parse(content);
            var newItems = body.SelectToken("data") as JArray;

            if ("OnNewResponseAddedCollector".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
                "OnNewResponseAddedSurvey".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
            {
                newItems = GetNewItems(newItems);
                var triggerStates = SetTriggerState(newItems, triggerState, response.Headers?.Date.ToString());
                triggerState = triggerStates.Split('|')[0];
                excludeIds = triggerStates.Split('|')[1];
            }
            if (newItems != null && newItems.HasValues)
            {
                response.Content = new StringContent(newItems.ToString(), Encoding.UTF8, "application/json");
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Content = null;
                response.StatusCode = HttpStatusCode.Accepted;
            }
            response.Headers.Location = SetLocationHeader(triggerState, excludeIds);
        }
    }
}