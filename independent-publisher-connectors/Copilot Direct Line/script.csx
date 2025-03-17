public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        string ACCESS_TOKEN = null;
        string conversationId = null;
        
        if (this.Context.Request.RequestUri != null && 
            this.Context.Request.RequestUri.Segments.Length > 4 && 
            this.Context.Request.RequestUri.Segments[3] == "conversations/")
        {
            string pathSegment = this.Context.Request.RequestUri.Segments[4];
            if (!pathSegment.EndsWith("/"))
            {
                conversationId = pathSegment;
            }
            else
            {
                conversationId = pathSegment.TrimEnd('/');
            }
        }
        
        if (this.Context.Request.RequestUri != null && this.Context.Request.RequestUri.Query != null)
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            string tokenParam = queryParams["token"];
            if (!string.IsNullOrEmpty(tokenParam))
            {
                ACCESS_TOKEN = tokenParam;
            }
        }
        
        if (string.IsNullOrEmpty(ACCESS_TOKEN))
        {
            var generateURL = "https://directline.botframework.com/v3/directline/tokens/generate";
            
            string secretHeader = null;
            IEnumerable<string> headerValues;
            if (this.Context.Request.Headers.TryGetValues("secret", out headerValues))
            {
                secretHeader = headerValues.FirstOrDefault();
            }
            
            if (string.IsNullOrEmpty(secretHeader))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) 
                { 
                    Content = new StringContent("Missing secret header") 
                };
            }

            var authRequest = new HttpRequestMessage(HttpMethod.Post, generateURL);
            authRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secretHeader);
            
            HttpResponseMessage authResponse = await this.Context.SendAsync(authRequest, this.CancellationToken);
            if (!authResponse.IsSuccessStatusCode)
            {
                return authResponse;
            }

            var responseString = await authResponse.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);
            
            if (jsonResponse.TryGetValue("token", out JToken token))
            {
                ACCESS_TOKEN = token.ToString();
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Failed to obtain access token")
                };
            }
        }

        this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
        
        HttpResponseMessage actionResponse;
        if (this.Context.OperationId == "Conversations_StartActivity")
        {
            string requestContent = await this.Context.Request.Content.ReadAsStringAsync();
            JObject requestBody = JObject.Parse(requestContent);
            
            if (!requestBody.TryGetValue("activity", out JToken activity))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Activity is required in request body")
                };
            }
            
            var startConversationUrl = "https://directline.botframework.com/v3/directline/conversations";
            var startConversationRequest = new HttpRequestMessage(HttpMethod.Post, startConversationUrl);
            startConversationRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
            
            HttpResponseMessage startConversationResponse = await this.Context.SendAsync(startConversationRequest, this.CancellationToken);
            if (!startConversationResponse.IsSuccessStatusCode)
            {
                return startConversationResponse;
            }
            
            var conversationResponseString = await startConversationResponse.Content.ReadAsStringAsync();
            var conversationJson = JObject.Parse(conversationResponseString);
            
            conversationId = conversationJson["conversationId"].ToString();
            
            var sendActivityUrl = $"https://directline.botframework.com/v3/directline/conversations/{conversationId}/activities?token={ACCESS_TOKEN}";
            var sendActivityRequest = new HttpRequestMessage(HttpMethod.Post, sendActivityUrl);
            sendActivityRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
            sendActivityRequest.Content = new StringContent(
                activity.ToString(),
                System.Text.Encoding.UTF8,
                "application/json");
            
            var sendActivityResponse = await this.Context.SendAsync(sendActivityRequest, this.CancellationToken);
            if (!sendActivityResponse.IsSuccessStatusCode)
            {
                return sendActivityResponse;
            }
            
            const int maxAttempts = 100;
            const int pollDelayMs = 500;
            int attempts = 0;
            JObject activitiesResponse = null;
            
            do {
                attempts++;
                
                await Task.Delay(pollDelayMs);
                
                var getActivitiesUrl = $"https://directline.botframework.com/v3/directline/conversations/{conversationId}/activities?token={ACCESS_TOKEN}";
                var getActivitiesRequest = new HttpRequestMessage(HttpMethod.Get, getActivitiesUrl);
                getActivitiesRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
                
                var getActivitiesResponse = await this.Context.SendAsync(getActivitiesRequest, this.CancellationToken);
                if (!getActivitiesResponse.IsSuccessStatusCode)
                {
                    return getActivitiesResponse;
                }
                
                var activitiesJson = await getActivitiesResponse.Content.ReadAsStringAsync();
                activitiesResponse = JObject.Parse(activitiesJson);
                
                if (activitiesResponse["activities"] != null && 
                    activitiesResponse["activities"].Type == JTokenType.Array &&
                    activitiesResponse["activities"].Count() >= 2)
                {
                    break;
                }
                
            } while (attempts < maxAttempts);
            
            if (activitiesResponse != null)
            {
                activitiesResponse["token"] = ACCESS_TOKEN;
                activitiesResponse["conversationId"] = conversationId;
                
                actionResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        activitiesResponse.ToString(),
                        System.Text.Encoding.UTF8,
                        "application/json")
                };
            }
            else
            {
                actionResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout)
                {
                    Content = new StringContent("Timed out waiting for bot response")
                };
            }
        }
        else if (this.Context.OperationId == "Conversations_SendActivityResponse") 
        {
            string requestContent = await this.Context.Request.Content.ReadAsStringAsync();

            var originalRequestUri = this.Context.Request.RequestUri;
            var uriBuilder = new UriBuilder(originalRequestUri);
            uriBuilder.Path = uriBuilder.Path.Replace("activitiesResponse", "activities");
            var sendActivityRequest = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri);
            sendActivityRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
            sendActivityRequest.Content = new StringContent(
                requestContent,
                System.Text.Encoding.UTF8,
                "application/json"
            );
            
            var sendActivityResponse = await this.Context.SendAsync(sendActivityRequest, this.CancellationToken);
            if (!sendActivityResponse.IsSuccessStatusCode)
            {
                return sendActivityResponse;
            }
            
            var activityResponseContent = await sendActivityResponse.Content.ReadAsStringAsync();
            var activityResponseJson = JObject.Parse(activityResponseContent);
            string activityId = activityResponseJson["id"].ToString();
            
            const int maxAttempts = 100;
            const int pollDelayMs = 500;
            int attempts = 0;
            JObject activitiesResponse = null;
            
            do {
                attempts++;
                
                await Task.Delay(pollDelayMs);
                
                var getActivitiesRequest = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
                getActivitiesRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
                
                var getActivitiesResponse = await this.Context.SendAsync(getActivitiesRequest, this.CancellationToken);
                if (!getActivitiesResponse.IsSuccessStatusCode)
                {
                    return getActivitiesResponse;
                }
                
                var activitiesJson = await getActivitiesResponse.Content.ReadAsStringAsync();
                activitiesResponse = JObject.Parse(activitiesJson);
                
                if (activitiesResponse["activities"] != null && activitiesResponse["activities"].Type == JTokenType.Array)
                {
                    var activities = activitiesResponse["activities"] as JArray;
                    foreach (var a in activities)
                    {
                        if (a["replyToId"] != null && a["replyToId"].ToString() == activityId)
                        {
                            break;
                        }
                    }
                }
                
            } while (attempts < maxAttempts);
            
            if (activitiesResponse != null)
            {
                activitiesResponse["token"] = ACCESS_TOKEN;
                activitiesResponse["conversationId"] = conversationId;
                
                // Add lastActivity - extract the last item from the activities array
                if (activitiesResponse["activities"] != null && activitiesResponse["activities"].Type == JTokenType.Array)
                {
                    var activities = activitiesResponse["activities"] as JArray;
                    if (activities.Count > 0)
                    {
                        activitiesResponse["lastActivity"] = activities.Last;
                    }
                }
                
                actionResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        activitiesResponse.ToString(),
                        System.Text.Encoding.UTF8,
                        "application/json")
                };
            }
            else
            {
                actionResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout)
                {
                    Content = new StringContent("Timed out waiting for bot response")
                };
            }
        }
        else if (this.Context.OperationId == "Conversations_StartConversation")
        {
            var startConversationUrl = "https://directline.botframework.com/v3/directline/conversations";
            var startConversationRequest = new HttpRequestMessage(HttpMethod.Post, startConversationUrl);
            startConversationRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
            
            HttpResponseMessage startConversationResponse = await this.Context.SendAsync(startConversationRequest, this.CancellationToken);
            if (!startConversationResponse.IsSuccessStatusCode)
            {
                return startConversationResponse;
            }
            
            var conversationResponseString = await startConversationResponse.Content.ReadAsStringAsync();
            var conversationJson = JObject.Parse(conversationResponseString);
            
            // Just preserve the API response directly
            actionResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    conversationResponseString,
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
        }
        else if (this.Context.OperationId == "Conversations_Upload")
        {
            actionResponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
            
            if (actionResponse.IsSuccessStatusCode && actionResponse.Content != null)
            {
                string contentType = actionResponse.Content.Headers.ContentType?.MediaType;
                if (contentType != null && contentType.Contains("application/json"))
                {
                    string responseContent = await actionResponse.Content.ReadAsStringAsync();
                    try 
                    {
                        var jsonResponse = JObject.Parse(responseContent);
                        jsonResponse["token"] = ACCESS_TOKEN;
                        jsonResponse["conversationId"] = conversationId;
                        
                        actionResponse.Content = new StringContent(
                            jsonResponse.ToString(),
                            System.Text.Encoding.UTF8,
                            "application/json");
                    }
                    catch (Exception) 
                    {
                    }
                }
            }
        }
        else
        {
            // For other operations, just pass through the request with added token
            actionResponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
            
            if (actionResponse.IsSuccessStatusCode && actionResponse.Content != null)
            {
                string contentType = actionResponse.Content.Headers.ContentType?.MediaType;
                if (contentType != null && contentType.Contains("application/json"))
                {
                    string responseContent = await actionResponse.Content.ReadAsStringAsync();
                    try 
                    {
                        var jsonResponse = JObject.Parse(responseContent);
                        jsonResponse["token"] = ACCESS_TOKEN;
                        jsonResponse["conversationId"] = conversationId;
                        
                        // If there are activities, add lastActivity
                        if (jsonResponse["activities"] != null && 
                            jsonResponse["activities"].Type == JTokenType.Array)
                        {
                            var activities = jsonResponse["activities"] as JArray;
                            if (activities.Count > 0)
                            {
                                jsonResponse["lastActivity"] = activities.Last;
                            }
                        }
                        
                        actionResponse.Content = new StringContent(
                            jsonResponse.ToString(),
                            System.Text.Encoding.UTF8,
                            "application/json");
                    }
                    catch (Exception) 
                    {
                        // If we can't parse the response as JSON, leave it as is
                    }
                }
            }
        }
        
        return actionResponse;
    }
}
