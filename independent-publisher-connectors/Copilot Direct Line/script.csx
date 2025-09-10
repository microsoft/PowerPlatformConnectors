using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    // Configuration constants for hybrid operations
    private const int DEFAULT_POLL_DELAY_MS = 500;
    private const int DEFAULT_MAX_ATTEMPTS = 100; // 50 seconds max wait
    private const int WORKFLOW_MAX_ATTEMPTS = 240; // 2 minutes max wait for workflows
    
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Handle new hybrid operations first
        switch (this.Context.OperationId)
        {
            case "StartConversationWithActivity":
                return await HandleStartConversationWithActivity();
                
            case "SendActivityAndWaitForResponse":
                return await HandleSendActivityAndWaitForResponse();
                
            case "ExecuteMultiAgentWorkflow":
                return await HandleMultiAgentWorkflow();
                
            case "CallAgentSync":
                // This operation is routed to Azure Function via policy template
                return await ForwardToAzureFunction();
        }
        
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

    // =============================================================================
    // HYBRID OPERATIONS - New v1.1 Functionality
    // =============================================================================
    
    /// <summary>
    /// Enhanced version - starts conversation and waits for bot response
    /// </summary>
    private async Task<HttpResponseMessage> HandleStartConversationWithActivity()
    {
        try
        {
            // Get required parameters
            var requestContent = await this.Context.Request.Content.ReadAsStringAsync();
            var requestBody = JObject.Parse(requestContent);
            
            var text = requestBody["text"]?.ToString();
            var from = requestBody["from"]?.ToString() ?? "user";
            
            if (string.IsNullOrEmpty(text))
                return CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter 'text' is required");
            
            // Step 1: Get Direct Line token using Troy's original logic
            var token = await GetDirectLineTokenFromOriginalLogic();
            if (token == null)
                return CreateErrorResponse(HttpStatusCode.Unauthorized, "Failed to obtain Direct Line token");
            
            // Step 2: Start conversation
            var conversationId = await StartConversation(token);
            if (conversationId == null)
                return CreateErrorResponse(HttpStatusCode.BadRequest, "Failed to start conversation");
            
            // Step 3: Send initial message
            var activity = new JObject
            {
                ["type"] = "message",
                ["text"] = text,
                ["from"] = new JObject { ["id"] = from }
            };
            
            var (sendSuccess, activityId, sendError) = await SendActivity(conversationId, activity, token);
            if (!sendSuccess)
                return CreateErrorResponse(HttpStatusCode.BadRequest, $"Send error: {sendError}");
            
            // Step 4: Wait for bot response
            var activitiesResponse = await WaitForBotResponse(conversationId, token, null, DEFAULT_MAX_ATTEMPTS);
            if (activitiesResponse == null)
                return CreateErrorResponse(HttpStatusCode.RequestTimeout, "Timeout waiting for bot response");
            
            // Step 5: Enhanced response with metadata
            var enhancedResponse = EnhanceActivitySet(activitiesResponse, conversationId, token);
            enhancedResponse["metadata"] = new JObject
            {
                ["operationType"] = "StartConversationWithActivity",
                ["responseSource"] = "DirectLine",
                ["connectorType"] = "hybrid",
                ["version"] = "1.1"
            };
            
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(enhancedResponse.ToString())
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error in StartConversationWithActivity: {ex.Message}");
        }
    }

    /// <summary>
    /// Send activity to existing conversation and wait for response
    /// </summary>
    private async Task<HttpResponseMessage> HandleSendActivityAndWaitForResponse()
    {
        try
        {
            // Extract conversation ID from path
            var conversationId = ExtractConversationIdFromPath();
            if (string.IsNullOrEmpty(conversationId))
                return CreateErrorResponse(HttpStatusCode.BadRequest, "Conversation ID not found in path");

            // Get token from request
            var token = await GetDirectLineTokenFromOriginalLogic();
            if (string.IsNullOrEmpty(token))
                return CreateErrorResponse(HttpStatusCode.Unauthorized, "Direct Line token required");

            // Get activity from request body
            var requestContent = await this.Context.Request.Content.ReadAsStringAsync();
            var activity = JObject.Parse(requestContent);

            // Send activity
            var (sendSuccess, activityId, sendError) = await SendActivity(conversationId, activity, token);
            if (!sendSuccess)
                return CreateErrorResponse(HttpStatusCode.BadRequest, $"Send error: {sendError}");

            // Wait for bot response
            var activitiesResponse = await WaitForBotResponse(conversationId, token, activityId, DEFAULT_MAX_ATTEMPTS);
            if (activitiesResponse == null)
                return CreateErrorResponse(HttpStatusCode.RequestTimeout, "Timeout waiting for bot response");

            var enhancedResponse = EnhanceActivitySet(activitiesResponse, conversationId, token);
            enhancedResponse["metadata"] = new JObject
            {
                ["operationType"] = "SendActivityAndWaitForResponse",
                ["responseSource"] = "DirectLine",
                ["connectorType"] = "hybrid",
                ["version"] = "1.1"
            };
            
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(enhancedResponse.ToString())
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(HttpStatusCode.InternalServerError, $"Error in SendActivityAndWaitForResponse: {ex.Message}");
        }
    }

    /// <summary>
    /// Executes multi-agent workflow orchestration
    /// </summary>
    private async Task<HttpResponseMessage> HandleMultiAgentWorkflow()
    {
        try
        {
            var workflowRequest = await this.Context.Request.Content.ReadAsStringAsync();
            var workflowData = JObject.Parse(workflowRequest);
            
            var agents = workflowData["agents"] as JArray;
            var userMessage = workflowData["userMessage"]?.ToString();
            var workflowType = workflowData["workflowType"]?.ToString() ?? "sequential";
            
            if (agents == null || agents.Count == 0)
                return CreateErrorResponse(HttpStatusCode.BadRequest, "At least one agent is required");
            
            if (string.IsNullOrEmpty(userMessage))
                return CreateErrorResponse(HttpStatusCode.BadRequest, "userMessage is required");
            
            var results = await ExecuteMultiAgentWorkflow(agents, userMessage, workflowType);
            
            var response = new JObject
            {
                ["workflowId"] = Guid.NewGuid().ToString(),
                ["results"] = results,
                ["timestamp"] = DateTime.UtcNow.ToString("O"),
                ["status"] = "completed",
                ["metadata"] = new JObject
                {
                    ["operationType"] = "ExecuteMultiAgentWorkflow",
                    ["workflowType"] = workflowType,
                    ["agentCount"] = agents.Count,
                    ["version"] = "1.1"
                }
            };
            
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(response.ToString())
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(HttpStatusCode.InternalServerError, $"Workflow error: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Forwards Agent SDK calls to Azure Function (via policy template)
    /// </summary>
    private async Task<HttpResponseMessage> ForwardToAzureFunction()
    {
        try
        {
            // This method is called when policy template routing fails
            // In normal operation, Agent SDK calls are routed directly to Azure Function
            return CreateErrorResponse(HttpStatusCode.BadGateway, 
                "Agent SDK calls should be routed to Azure Function via policy template. Check connector configuration.");
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(HttpStatusCode.InternalServerError, $"Azure Function routing error: {ex.Message}");
        }
    }
    
    private async Task<string> GetDirectLineTokenFromOriginalLogic()
    {
        try
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
                return null;
            }

            var authRequest = new HttpRequestMessage(HttpMethod.Post, generateURL);
            authRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secretHeader);

            HttpResponseMessage authResponse = await this.Context.SendAsync(authRequest, this.CancellationToken);

            if (!authResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var responseString = await authResponse.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            if (jsonResponse.TryGetValue("token", out JToken token))
            {
                return token.ToString();
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    private async Task<string> StartConversation(string token)
    {
        try
        {
            var startConversationUrl = "https://directline.botframework.com/v3/directline/conversations";
            var startConversationRequest = new HttpRequestMessage(HttpMethod.Post, startConversationUrl);
            startConversationRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            HttpResponseMessage startConversationResponse = await this.Context.SendAsync(startConversationRequest, this.CancellationToken);
            if (!startConversationResponse.IsSuccessStatusCode)
            {
                return null;
            }
            
            var conversationResponseString = await startConversationResponse.Content.ReadAsStringAsync();
            var conversationJson = JObject.Parse(conversationResponseString);
            
            return conversationJson["conversationId"]?.ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    private async Task<(bool success, string activityId, string error)> SendActivity(string conversationId, JToken activity, string token)
    {
        try
        {
            var sendActivityUrl = $"https://directline.botframework.com/v3/directline/conversations/{conversationId}/activities";
            var sendActivityRequest = new HttpRequestMessage(HttpMethod.Post, sendActivityUrl);
            sendActivityRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            sendActivityRequest.Content = new StringContent(
                activity.ToString(),
                System.Text.Encoding.UTF8,
                "application/json");
            
            var sendActivityResponse = await this.Context.SendAsync(sendActivityRequest, this.CancellationToken);
            if (!sendActivityResponse.IsSuccessStatusCode)
            {
                return (false, null, $"Failed to send activity: {sendActivityResponse.StatusCode}");
            }
            
            var responseContent = await sendActivityResponse.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseContent);
            var activityId = responseJson["id"]?.ToString();
            
            return (true, activityId, null);
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }
    
    private async Task<JObject> WaitForBotResponse(string conversationId, string token, string activityId, int maxAttempts)
    {
        try
        {
            for (int attempts = 0; attempts < maxAttempts; attempts++)
            {
                await Task.Delay(DEFAULT_POLL_DELAY_MS);
                
                var getActivitiesUrl = $"https://directline.botframework.com/v3/directline/conversations/{conversationId}/activities";
                var getActivitiesRequest = new HttpRequestMessage(HttpMethod.Get, getActivitiesUrl);
                getActivitiesRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var getActivitiesResponse = await this.Context.SendAsync(getActivitiesRequest, this.CancellationToken);
                if (!getActivitiesResponse.IsSuccessStatusCode)
                {
                    continue;
                }
                
                var activitiesJson = await getActivitiesResponse.Content.ReadAsStringAsync();
                var activitiesResponse = JObject.Parse(activitiesJson);
                
                if (activitiesResponse["activities"] != null && 
                    activitiesResponse["activities"].Type == JTokenType.Array &&
                    activitiesResponse["activities"].Count() >= 2)
                {
                    return activitiesResponse;
                }
            }
            
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    private JObject EnhanceActivitySet(JObject activitiesResponse, string conversationId, string token)
    {
        // Add enhanced properties
        activitiesResponse["token"] = token;
        activitiesResponse["conversationId"] = conversationId;
        
        // Add lastActivity
        var activities = activitiesResponse["activities"] as JArray;
        if (activities != null && activities.Count > 0)
        {
            var lastActivity = activities.Last;
            activitiesResponse["lastActivity"] = lastActivity;
        }
        
        return activitiesResponse;
    }
    
    private string ExtractConversationIdFromPath()
    {
        var segments = this.Context.Request.RequestUri.Segments;
        if (segments.Length > 4 && segments[3] == "conversations/")
        {
            var segment = segments[4];
            return segment.EndsWith("/") ? segment.TrimEnd('/') : segment;
        }
        return null;
    }
    
    private async Task<JArray> ExecuteMultiAgentWorkflow(JArray agents, string userMessage, string workflowType)
    {
        var results = new JArray();
        var currentMessage = userMessage;
        
        try
        {
            switch (workflowType.ToLower())
            {
                case "sequential":
                    // Execute agents in sequence
                    foreach (var agent in agents)
                    {
                        var result = await CallSingleAgent(agent, currentMessage);
                        results.Add(result);
                        
                        // Use this agent's response as input for next agent
                        var response = result["response"]?.ToString();
                        if (!string.IsNullOrEmpty(response))
                        {
                            currentMessage = response;
                        }
                    }
                    break;
                    
                case "parallel":
                    // Execute all agents with original message
                    var tasks = agents.Select(agent => CallSingleAgent(agent, userMessage)).ToArray();
                    var parallelResults = await Task.WhenAll(tasks);
                    
                    foreach (var result in parallelResults)
                    {
                        results.Add(result);
                    }
                    break;
                    
                default:
                    throw new ArgumentException($"Unknown workflow type: {workflowType}");
            }
        }
        catch (Exception ex)
        {
            var errorResult = new JObject
            {
                ["agentId"] = "workflow-error",
                ["error"] = ex.Message,
                ["timestamp"] = DateTime.UtcNow.ToString("O")
            };
            results.Add(errorResult);
        }
        
        return results;
    }
    
    private async Task<JObject> CallSingleAgent(JToken agentConfig, string message)
    {
        try
        {
            var agentId = agentConfig["agentId"]?.ToString();
            var directLineSecret = agentConfig["directLineSecret"]?.ToString();
            
            var result = new JObject
            {
                ["agentId"] = agentId,
                ["timestamp"] = DateTime.UtcNow.ToString("O")
            };
            
            if (string.IsNullOrEmpty(directLineSecret))
            {
                result["success"] = false;
                result["error"] = "Direct Line secret is required";
                return result;
            }
            
            // Get token
            var token = await GetDirectLineTokenFromOriginalLogic();
            if (string.IsNullOrEmpty(token))
            {
                result["success"] = false;
                result["error"] = "Failed to get Direct Line token";
                return result;
            }
            
            // Start conversation
            var conversationId = await StartConversation(token);
            if (string.IsNullOrEmpty(conversationId))
            {
                result["success"] = false;
                result["error"] = "Failed to start conversation";
                return result;
            }
            
            // Send message
            var activity = new JObject
            {
                ["type"] = "message",
                ["text"] = message,
                ["from"] = new JObject { ["id"] = "workflow" }
            };
            
            var (sendSuccess, activityId, sendError) = await SendActivity(conversationId, activity, token);
            if (!sendSuccess)
            {
                result["success"] = false;
                result["error"] = sendError;
                return result;
            }
            
            // Wait for response
            var activitiesResponse = await WaitForBotResponse(conversationId, token, activityId, DEFAULT_MAX_ATTEMPTS);
            if (activitiesResponse == null)
            {
                result["success"] = false;
                result["error"] = "Timeout waiting for response";
                return result;
            }
            
            // Extract bot response
            var activities = activitiesResponse["activities"] as JArray;
            string botResponse = null;
            if (activities != null)
            {
                for (int i = activities.Count - 1; i >= 0; i--)
                {
                    var activity = activities[i];
                    if (activity["from"]?["role"]?.Value<string>() != "user" &&
                        activity["type"]?.Value<string>() == "message")
                    {
                        botResponse = activity["text"]?.Value<string>();
                        break;
                    }
                }
            }
            
            result["success"] = true;
            result["response"] = botResponse;
            result["conversationId"] = conversationId;
            
            return result;
        }
        catch (Exception ex)
        {
            return new JObject
            {
                ["agentId"] = agentConfig["agentId"]?.ToString() ?? "unknown",
                ["success"] = false,
                ["error"] = ex.Message,
                ["timestamp"] = DateTime.UtcNow.ToString("O")
            };
        }
    }
    
    private StringContent CreateJsonContent(string json)
    {
        return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    }
    
    private HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string message)
    {
        var error = new JObject
        {
            ["error"] = message,
            ["timestamp"] = DateTime.UtcNow.ToString("O")
        };
        
        return new HttpResponseMessage(statusCode)
        {
            Content = CreateJsonContent(error.ToString())
        };
    }
}
